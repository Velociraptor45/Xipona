using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Text;
using Xipona.Api.Generators.Core.Common;
using Xipona.Api.Generators.Core.Extensions;

namespace Xipona.Api.ApplicationServices.Generators.CqrsHandlers;

public abstract class CqrsHandlerDiGeneratorBase : IIncrementalGenerator
{
    public abstract void Initialize(IncrementalGeneratorInitializationContext context);

    protected IncrementalValuesProvider<CqrsHandler> GetAllHandlers(IncrementalGeneratorInitializationContext context,
        string interfaceName)
    {
        return context.SyntaxProvider.CreateSyntaxProvider(
            (s, _) =>
            {
                if (s is not ClassDeclarationSyntax classNode)
                    return false;

                if (classNode.BaseList is null
                    || classNode.BaseList.Types.Count != 1
                    || classNode.BaseList.Types[0].Type is not GenericNameSyntax baseType
                    || baseType.Identifier.Text != interfaceName
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

                // first and second generic argument of handler interface
                var firstGenericArgument = genericArguments[0];
                var secondGenericArgument = genericArguments[1];

                return new CqrsHandler(
                    classNode.Identifier.Text,
                    classNode.GetNamespace(),
                    new TypeAnalysis(firstGenericArgument, ctx),
                    new TypeAnalysis(secondGenericArgument, ctx));
            });
    }

    protected string GetRegistrations(ImmutableArray<CqrsHandler> allHandlers, string interfaceName)
    {
        var registrationBuilder = new StringBuilder();
        foreach (var h in allHandlers)
        {
            registrationBuilder.AppendLine(
                $"services.AddTransient<{interfaceName}<{h.FirstGenericArgument}, {h.SecondGenericArgument}>, {h.NamespaceName}.{h.Name}>();");
            registrationBuilder.Append("        ");
        }

        return registrationBuilder.ToString();
    }

    protected string GetNamespaces(ImmutableArray<CqrsHandler> allHandlers, string interfaceNamespace)
    {
        var namespaces = allHandlers
            .SelectMany(qh => qh.GetAllNamespaces())
            .Union([
                "Microsoft.Extensions.DependencyInjection",
                interfaceNamespace
            ])
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        var namespaceBuilder = new StringBuilder();
        foreach (var ns in namespaces)
        {
            namespaceBuilder.AppendLine($"using {ns};");
        }

        return namespaceBuilder.ToString();
    }
}
