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

namespace Laconic.CodeGeneration
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
                        .AddGetHashCode(parameters, true)
                        .AddWithMethod(parameters);
                }

                result.Add(classForMethod);
            }

            return Task.FromResult(List(result));
        }
    }

    public class SignalsGenerator : ICodeGenerator
    {
        public SignalsGenerator(AttributeData attributeData)
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
                    .AddBaseListTypes(
                        SimpleBaseType(IdentifierName("Signal")),
                        SimpleBaseType(IdentifierName(unionInterfaceName)))
                    .AddPropertiesFromParameters(parameters);

                classForMethod = AddSignalConstructor(classForMethod, parameters);
                    
                result.Add(classForMethod);
            }

            return Task.FromResult(List(result));
        }
        
        
        static ClassDeclarationSyntax AddSignalConstructor(
            ClassDeclarationSyntax cls,
            SeparatedSyntaxList<ParameterSyntax> parameters)
        {
            var assignments = parameters
                .Select(x => Utils.GetNames(x.Identifier.ValueText))
                .Select(x => ParseStatement($"{x.TitleCase} = {x.camelCase};"));
            
            var ctor = ConstructorDeclaration(cls.Identifier)
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .WithParameterList(ParameterList(parameters))
                .WithBody(Block(assignments.ToArray()));

            var args = parameters.Take(2).Select(x => x.Identifier.ValueText).ToArray();
            if (args.Length == 0)
                args = new[] {"null"};

            ctor = ctor.WithInitializer(ConstructorInitializer(
                SyntaxKind.BaseConstructorInitializer,
                ParseArgumentList("(" + String.Join(", ", args) + ")"))
            );

            return cls.AddMembers(ctor);
        }
    }
}