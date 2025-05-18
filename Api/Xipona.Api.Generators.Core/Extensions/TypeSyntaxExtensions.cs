using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xipona.Api.Generators.Core.Common;

namespace Xipona.Api.Generators.Core.Extensions;

public static class TypeSyntaxExtensions
{
    public static string GetName(this TypeSyntax type, GeneratorSyntaxContext ctx)
    {
        if (ctx.SemanticModel.GetSymbolInfo(type).Symbol is not INamedTypeSymbol symbol)
            throw new InvalidOperationException($"Unable to resolve the symbol for type {type}");

        return symbol.Name;
    }

    public static string GetNamespace(this TypeSyntax type, GeneratorSyntaxContext ctx)
    {
        if (ctx.SemanticModel.GetSymbolInfo(type).Symbol is not INamedTypeSymbol symbol)
            throw new InvalidOperationException($"Unable to resolve the symbol for type {type}");

        return symbol.ContainingNamespace.ToDisplayString();
    }

    public static IEnumerable<TypeAnalysis> GetGenericTypeArguments(this TypeSyntax type, GeneratorSyntaxContext ctx)
    {
        if (type is not GenericNameSyntax genericType)
            yield break;

        var genericArguments = genericType.TypeArgumentList.Arguments;

        foreach (var genericArgument in genericArguments)
        {
            yield return new TypeAnalysis(genericArgument, ctx);
        }
    }
}
