using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using Xipona.Api.Generators.Common;

namespace Xipona.Api.Generators.Handlers;

[Generator]
public class HandlerDiGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var queryHandlers = context.SyntaxProvider.CreateSyntaxProvider(
            static (s, _) =>
            {
                if (s is not ClassDeclarationSyntax classNode)
                    return false;

                if (classNode.BaseList is null
                    || classNode.BaseList.Types.Count != 1
                    || classNode.BaseList.Types[0].Type is not GenericNameSyntax baseType
                    || baseType.Identifier.Text != "IQueryHandler"
                    || baseType.TypeArgumentList.Arguments.Count != 2)
                    return false;

                return true;
            },
            static (ctx, _) =>
            {
                var classNode = (ClassDeclarationSyntax)ctx.Node;

                if (classNode.BaseList?.Types[0].Type is not GenericNameSyntax baseType)
                    throw new InvalidOperationException("Base type is not a generic type.");

                var genericArguments = baseType.TypeArgumentList.Arguments;

                // Get the first and second generic arguments
                var firstGenericArgument = genericArguments[0];
                var secondGenericArgument = genericArguments[1];

                return new Handler(
                    classNode.Identifier.Text,
                    new TypeAnalysis(
                        firstGenericArgument.ToString(),
                        GetNamespace(ctx, firstGenericArgument),
                        GetGenericTypeArguments(ctx, firstGenericArgument)),
                    new TypeAnalysis(
                        secondGenericArgument.ToString(),
                        GetNamespace(ctx, secondGenericArgument),
                        GetGenericTypeArguments(ctx, secondGenericArgument)));


                string GetNamespace(GeneratorSyntaxContext ctx, TypeSyntax type)
                {
                    var symbol = ctx.SemanticModel.GetSymbolInfo(type).Symbol as INamedTypeSymbol;

                    if (symbol is null)
                        throw new InvalidOperationException($"Unable to resolve the symbol for type {type}");

                    return symbol.ContainingNamespace.ToDisplayString();
                }

                IEnumerable<TypeAnalysis> GetGenericTypeArguments(GeneratorSyntaxContext ctx, TypeSyntax type)
                {
                    if (type is not GenericNameSyntax genericType)
                        yield break;

                    var genericArguments = genericType.TypeArgumentList.Arguments;

                    foreach (var genericArgument in genericArguments)
                    {
                        yield return new TypeAnalysis(
                            genericArgument.ToString(),
                            GetNamespace(ctx, genericArgument),
                            GetGenericTypeArguments(ctx, genericArgument));
                    }
                }
            });

        context.RegisterSourceOutput(queryHandlers.Collect(), (ctx, allQueryHandlers) =>
        {
            if (allQueryHandlers.Length == 0)
                return;

            var registrationBuilder = new StringBuilder();
            foreach (var qh in allQueryHandlers)
            {
                registrationBuilder.AppendLine(
                    $"services.AddTransient<IQueryHandler<{qh.FirstGenericArgument.TypeName}, {qh.SecondGenericArgument.TypeName}>, {qh.Name}>();");
                registrationBuilder.Append("        ");
            }

            var namespaces = allQueryHandlers
                .SelectMany(qh => qh.GetAllNamespaces())
                .Union([
                        "Microsoft.Extensions.DependencyInjection",
                        "ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries"
                    ])
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            var namespaceBuilder = new StringBuilder();
            foreach (var ns in namespaces)
            {
                namespaceBuilder.AppendLine($"using {ns};");
                registrationBuilder.Append("        ");
            }

            var src = $$"""
                        {{namespaceBuilder}}

                        namespace ProjectHermes.Xipona.Api.ApplicationServices;

                        public static class GeneratedServiceCollectionExtensions
                        {
                            public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
                            {
                                {{registrationBuilder}}
                                
                                return services;
                            }
                        }
                        """;

            ctx.AddSource("GeneratedServiceCollectionExtensions.g.cs", SourceText.From(src, Encoding.UTF8));
        });
    }
}

public readonly record struct Handler
{
    public Handler(string name, TypeAnalysis firstGenericArgument, TypeAnalysis secondGenericArgument)
    {
        Name = name;
        FirstGenericArgument = firstGenericArgument;
        SecondGenericArgument = secondGenericArgument;
    }

    public string Name { get; }
    public TypeAnalysis FirstGenericArgument { get; }
    public TypeAnalysis SecondGenericArgument { get; }

    public IEnumerable<string> GetAllNamespaces()
    {
        return FirstGenericArgument.GetAllNamespaces().Concat(SecondGenericArgument.GetAllNamespaces());
    }
};