using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CodeGeneration.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Laconic.CodeGen
{
    public class RecordsGenerator : ICodeGenerator
    {
        public RecordsGenerator(AttributeData attributeData)
        {
        }

        public Task<SyntaxList<MemberDeclarationSyntax>> GenerateAsync(
            TransformationContext context,
            IProgress<Diagnostic> progress,
            CancellationToken cancellationToken)
        {
            var result = new List<MemberDeclarationSyntax>();

            var recordContainer = (InterfaceDeclarationSyntax) context.ProcessingNode;
            var isPublic = recordContainer.Modifiers.Any(x => x.Kind() == SyntaxKind.PublicKeyword);
            
            foreach (MethodDeclarationSyntax method in recordContainer.Members) {
                var parameters = method.ParameterList.Parameters;
                var classForMethod = ClassDeclaration(method.Identifier)
                    .WithLeadingTrivia(ParseLeadingTrivia("#nullable enable"))
                    .AddModifiers(isPublic ? new[] {Token(SyntaxKind.PublicKeyword)} : new SyntaxToken[0])
                    .AddModifiers(Token(SyntaxKind.PartialKeyword))
                    .AddBaseListTypes(SimpleBaseType(ParseTypeName($"System.IEquatable<{method.Identifier.Text}>")))
                    .AddPropertiesFromParameters(parameters)
                    .AddConstructor(parameters)
                    .AddWithMethod(parameters)
                    .AddDeconstructMethod(parameters)
                    .AddEqualityMethods(parameters)
                    .AddGetHashCode(parameters);

                result.Add(classForMethod);
            }

            return Task.FromResult(List(result));
        }
    }

    public class UnionGenerator : ICodeGenerator
    {
        public UnionGenerator(AttributeData attributeData)
        {
        }

        public Task<SyntaxList<MemberDeclarationSyntax>> GenerateAsync(
            TransformationContext context,
            IProgress<Diagnostic> progress,
            CancellationToken cancellationToken)
        {
            var result = new List<MemberDeclarationSyntax>();

            var unionContainer = (InterfaceDeclarationSyntax) context.ProcessingNode;
            var unionInterfaceName = unionContainer.Identifier.ValueText.Trim('_');
            var isPublic = unionContainer.Modifiers.Any(x => x.Kind() == SyntaxKind.PublicKeyword);
            result.Add(InterfaceDeclaration(unionInterfaceName).AddModifiers(Token(SyntaxKind.PublicKeyword)));

            foreach (MethodDeclarationSyntax method in unionContainer.Members) {
                var parameters = method.ParameterList.Parameters;
                var classForMethod = ClassDeclaration(method.Identifier)
                    .WithLeadingTrivia(ParseLeadingTrivia("#nullable enable"))
                    .AddModifiers(isPublic ? new[] {Token(SyntaxKind.PublicKeyword)} : new SyntaxToken[0])
                    .AddModifiers(Token(SyntaxKind.PartialKeyword))
                    .AddBaseListTypes(SimpleBaseType(IdentifierName(unionInterfaceName)));

                if (method.ReturnType is IdentifierNameSyntax name && name.Identifier.Text == "record") {
                    classForMethod = classForMethod
                        .AddBaseListTypes(SimpleBaseType(ParseTypeName($"System.IEquatable<{method.Identifier.Text}>")))
                        .AddEqualityMethods(parameters)
                        .AddPropertiesFromParameters(parameters)
                        .AddConstructor(parameters)
                        .AddDeconstructMethod(parameters)
                        .AddGetHashCode(parameters)
                        .AddWithMethod(parameters);
                }

                result.Add(classForMethod);
            }

            return Task.FromResult(List(result));
        }
    }

    static class Utils
    {
        static (string TitleCase, string camelCase) GetNames(string source) =>
            (Char.ToUpper(source[0]) + source.Substring(1),
                Char.ToLower(source[0]) + source.Substring(1));

        public static ClassDeclarationSyntax AddPropertiesFromParameters(
            this ClassDeclarationSyntax cls,
            SeparatedSyntaxList<ParameterSyntax> parameters)
        {
            foreach (var parameter in parameters) {
                var names = GetNames(parameter.Identifier.ValueText);
                var prop = PropertyDeclaration(parameter.Type, names.TitleCase)
                    .AddModifiers(Token(SyntaxKind.PublicKeyword))
                    .AddAccessorListAccessors(
                        AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)));
                cls = cls.AddMembers(prop);
            }

            return cls;
        }

        public static ClassDeclarationSyntax AddConstructor(
            this ClassDeclarationSyntax cls,
            SeparatedSyntaxList<ParameterSyntax> parameters)
        {
            if (parameters.Count == 0)
                return cls;

            var assignments = parameters
                .Select(x => GetNames(x.Identifier.ValueText))
                .Select(x => ParseStatement($"{x.TitleCase} = {x.camelCase};"));
            var ctor = ConstructorDeclaration(cls.Identifier)
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .WithParameterList(ParameterList(parameters))
                .WithBody(Block(assignments.ToArray()));

            return cls.AddMembers(ctor);
        }

        public static ClassDeclarationSyntax AddWithMethod(
            this ClassDeclarationSyntax cls,
            SeparatedSyntaxList<ParameterSyntax> parameters)
        {
            if (parameters.Count == 0)
                return cls;

            var ctorCallArguments = parameters.Select(x => GetNames(x.Identifier.ValueText)).Select(p =>
                Argument(
                    BinaryExpression(
                        SyntaxKind.CoalesceExpression,
                        IdentifierName(p.camelCase),
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            ThisExpression(),
                            IdentifierName(p.TitleCase)))));

            var objCreation = ObjectCreationExpression(
                IdentifierName(cls.Identifier),
                ArgumentList(SeparatedList(ctorCallArguments.ToArray())),
                null
            );

            var nullableParams = parameters.Select(p =>
                Parameter(p.Identifier)
                    .WithType(NullableType(p.Type))
                    .WithDefault(EqualsValueClause(LiteralExpression(SyntaxKind.NullLiteralExpression)))
            );

            var withMethod = MethodDeclaration(IdentifierName(cls.Identifier), "With")
                .WithParameterList(ParameterList(SeparatedList(nullableParams.ToArray())))
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .WithExpressionBody(ArrowExpressionClause(objCreation))
                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

            return cls.AddMembers(withMethod);
        }

        public static ClassDeclarationSyntax AddDeconstructMethod(
            this ClassDeclarationSyntax cls,
            SeparatedSyntaxList<ParameterSyntax> parameters)
        {
            if (parameters.Count <= 1)
                return cls;

            var outParams = parameters.Select(p =>
                Parameter(p.Identifier)
                    .WithType(p.Type)
                    .WithModifiers(TokenList(Token(SyntaxKind.OutKeyword)))
            ).ToArray();

            var assignments = parameters.Select(x => GetNames(x.Identifier.ValueText)).Select(p =>
                ExpressionStatement(
                    AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        IdentifierName(p.camelCase),
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            ThisExpression(),
                            IdentifierName(p.TitleCase))
                    ))
            ).ToArray();

            var deconstructMethod = MethodDeclaration(PredefinedType(Token(SyntaxKind.VoidKeyword)), "Deconstruct")
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                .WithParameterList(ParameterList(SeparatedList(outParams)))
                .WithBody(Block(assignments));

            return cls.AddMembers(deconstructMethod);
        }

        public static ClassDeclarationSyntax AddEqualityMethods(
            this ClassDeclarationSyntax cls,
            SeparatedSyntaxList<ParameterSyntax> parameters)
        {
            var typedCompare = parameters.Count switch {
                0 => new[] { "true" },
                _ => parameters.Select(x => new {x.Type, Names = GetNames(x.Identifier.Text)})
                .Select(x =>
                    x.Type is ArrayTypeSyntax arr
                        ? $"System.Linq.Enumerable.SequenceEqual({x.Names.TitleCase}, other.{x.Names.TitleCase})"
                        : $"{x.Names.TitleCase}.Equals(other.{x.Names.TitleCase})")
                .ToArray()
            };

            var typedEqualsMethod = ParseMemberDeclaration($"public bool Equals({cls.Identifier.Text} other) => "
                                                           + String.Join(" && ", typedCompare) + ";");

            var untypedEqualsMethod = ParseMemberDeclaration(
                "public override bool Equals(object other) {"
                + $"\nif (other is {cls.Identifier.Text} typed)"
                + "\n{"
                + "\n    return typed.Equals(this);"
                + "}"
                + "return false;"
                + "}"
            );

            var equalsOperator = ParseMemberDeclaration(
                $"public static bool operator == ({cls.Identifier.Text} lhs, {cls.Identifier.Text} rhs) => " +
                "lhs.Equals(rhs);"
            );

            var notEqualsOperator = ParseMemberDeclaration(
                $"public static bool operator != ({cls.Identifier.Text} lhs, {cls.Identifier.Text} rhs) => " +
                "!lhs.Equals(rhs);"
            );

            return cls.AddMembers(typedEqualsMethod, untypedEqualsMethod, equalsOperator, notEqualsOperator);
        }

        public static ClassDeclarationSyntax AddGetHashCode(
            this ClassDeclarationSyntax cls,
            SeparatedSyntaxList<ParameterSyntax> parameters)
        {
            if (parameters.Count == 0)
                return cls.AddMembers(ParseMemberDeclaration(
                    $"public override int GetHashCode() => \"{cls.Identifier.Text}\".GetHashCode();"));
            
            var head = GetNames(parameters.First().Identifier.Text);
            var tail = parameters.Skip(1).Select(x => GetNames(x.Identifier.Text));

            var tailLines = tail.Select(x => $"hashCode = (hashCode * 397) ^ {x.TitleCase}.GetHashCode();\n").ToArray();

            var method = ParseMemberDeclaration(
                "public override int GetHashCode()"
                + "\n{"
                + "\n    unchecked {"
                + $"\n        var hashCode = {head.TitleCase}.GetHashCode();\n"
                + $"\n      " + String.Join("", tailLines)
                + "\n         return hashCode;"
                + "\n    }"
                + "}"
            );

            return cls.AddMembers(method);
        }
    }
}