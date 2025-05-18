using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Xipona.Api.Generators.Core.Extensions;

public static class ClassDeclarationSyntaxExtensions
{
    public static string GetNamespace(this ClassDeclarationSyntax node)
    {
        SyntaxNode? parent = node.Parent;

        while (parent != null)
        {
            switch (parent)
            {
                case NamespaceDeclarationSyntax namespaceDeclaration:
                    return namespaceDeclaration.Name.ToString();
                case FileScopedNamespaceDeclarationSyntax fileScopedNamespace:
                    return fileScopedNamespace.Name.ToString();
                default:
                    parent = parent.Parent;
                    break;
            }
        }

        throw new InvalidOperationException($"Class {node.Identifier.Text} is outside of any namespace");
    }

    public static bool NamespaceStartsWith(this ClassDeclarationSyntax node, string namespaceName)
    {
        return node.GetNamespace().StartsWith(namespaceName);
    }
}
