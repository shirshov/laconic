using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Laconic.CodeGeneration
{
    static class Utils
    {
        public static (string TitleCase, string camelCase) GetNames(string source) =>
            (Char.ToUpper(source[0]) + source.Substring(1),
                Char.ToLower(source[0]) + source.Substring(1));

        public static ClassDeclarationSyntax AddPropertiesFromParameters(
            this ClassDeclarationSyntax cls,
            SeparatedSyntaxList<ParameterSyntax> parameters)
        {
            foreach (var parameter in parameters) {
                var names = GetNames(parameter.Identifier.ValueText);
                var prop = SyntaxFactory.PropertyDeclaration(parameter.Type, names.TitleCase)
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddAccessorListAccessors(
                        SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));
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
                .Select(x => SyntaxFactory.ParseStatement($"{x.TitleCase} = {x.camelCase};"));
            var ctor = SyntaxFactory.ConstructorDeclaration(cls.Identifier)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithParameterList(SyntaxFactory.ParameterList(parameters))
                .WithBody(SyntaxFactory.Block(assignments.ToArray()));

            return cls.AddMembers(ctor);
        }

        public static ClassDeclarationSyntax AddWithMethod(
            this ClassDeclarationSyntax cls,
            SeparatedSyntaxList<ParameterSyntax> parameters)
        {
            if (parameters.Count == 0)
                return cls;

            var ctorCallArguments = parameters.Select(x => GetNames(x.Identifier.ValueText)).Select(p =>
                SyntaxFactory.Argument(
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.CoalesceExpression,
                        SyntaxFactory.IdentifierName(p.camelCase),
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ThisExpression(),
                            SyntaxFactory.IdentifierName(p.TitleCase)))));

            var objCreation = SyntaxFactory.ObjectCreationExpression(
                SyntaxFactory.IdentifierName(cls.Identifier),
                SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(ctorCallArguments.ToArray())),
                null
            );

            var nullableParams = parameters.Select(p =>
                SyntaxFactory.Parameter(p.Identifier)
                    .WithType(SyntaxFactory.NullableType(p.Type))
                    .WithDefault(SyntaxFactory.EqualsValueClause(SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)))
            );

            var withMethod = SyntaxFactory.MethodDeclaration(SyntaxFactory.IdentifierName(cls.Identifier), "With")
                .WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(nullableParams.ToArray())))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithExpressionBody(SyntaxFactory.ArrowExpressionClause(objCreation))
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

            return cls.AddMembers(withMethod);
        }

        public static ClassDeclarationSyntax AddDeconstructMethod(
            this ClassDeclarationSyntax cls,
            SeparatedSyntaxList<ParameterSyntax> parameters)
        {
            if (parameters.Count <= 1)
                return cls;

            var outParams = parameters.Select(p =>
                SyntaxFactory.Parameter(p.Identifier)
                    .WithType(p.Type)
                    .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.OutKeyword)))
            ).ToArray();

            var assignments = parameters.Select(x => GetNames(x.Identifier.ValueText)).Select(p =>
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.IdentifierName(p.camelCase),
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ThisExpression(),
                            SyntaxFactory.IdentifierName(p.TitleCase))
                    ))
            ).ToArray();

            var deconstructMethod = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)), "Deconstruct")
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                .WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(outParams)))
                .WithBody(SyntaxFactory.Block(assignments));

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

            var typedEqualsMethod = SyntaxFactory.ParseMemberDeclaration($"public bool Equals({cls.Identifier.Text} other) => "
                                                                         + String.Join(" && ", typedCompare) + ";");

            var untypedEqualsMethod = SyntaxFactory.ParseMemberDeclaration(
                "public override bool Equals(object other) {"
                + $"\nif (other is {cls.Identifier.Text} typed)"
                + "\n{"
                + "\n    return typed.Equals(this);"
                + "}"
                + "return false;"
                + "}"
            );

            var equalsOperator = SyntaxFactory.ParseMemberDeclaration(
                $"public static bool operator == ({cls.Identifier.Text} lhs, {cls.Identifier.Text} rhs) => " +
                "lhs.Equals(rhs);"
            );

            var notEqualsOperator = SyntaxFactory.ParseMemberDeclaration(
                $"public static bool operator != ({cls.Identifier.Text} lhs, {cls.Identifier.Text} rhs) => " +
                "!lhs.Equals(rhs);"
            );

            return cls.AddMembers(typedEqualsMethod, untypedEqualsMethod, equalsOperator, notEqualsOperator);
        }

        public static ClassDeclarationSyntax AddGetHashCode(
            this ClassDeclarationSyntax cls,
            SeparatedSyntaxList<ParameterSyntax> parameters, bool useClassName = false)
        {
            if (parameters.Count == 0) {
                var exp = useClassName ? $"typeof({cls.Identifier.Text})" : "base";
                Console.WriteLine(exp);
                return cls.AddMembers(SyntaxFactory.ParseMemberDeclaration(
                    $"public override int GetHashCode() => {exp}.GetHashCode();"));
            }
            
            var head = GetNames(parameters.First().Identifier.Text);
            var tail = parameters.Skip(1).Select(x => GetNames(x.Identifier.Text));

            var tailLines = tail.Select(x => $"hashCode = (hashCode * 397) ^ {x.TitleCase}.GetHashCode();\n").ToArray();

            var method = SyntaxFactory.ParseMemberDeclaration(
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