using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Text;
using Xipona.Api.Generators.Core.Common;

namespace Xipona.Api.ApplicationServices.Generators.Dispatchers;

public abstract class DispatcherGeneratorBase : IIncrementalGenerator
{
    public abstract void Initialize(IncrementalGeneratorInitializationContext context);

    protected IncrementalValuesProvider<Handler> GetAllHandlers(IncrementalGeneratorInitializationContext context,
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

                if (classNode.BaseList!.Types[0].Type is not GenericNameSyntax baseType)
                    throw new InvalidOperationException("Base type is not a generic type.");

                var genericArguments = baseType.TypeArgumentList.Arguments;
                var firstGenericArgument = genericArguments[0];
                var secondGenericArgument = genericArguments[1];

                return new Handler(
                    new TypeAnalysis(firstGenericArgument, ctx),
                    new TypeAnalysis(secondGenericArgument, ctx));
            });
    }

    protected string GetDictEntries(ImmutableArray<Handler> allHandlers, string handlerInterfaceName)
    {
        var builder = new StringBuilder();

        foreach (var handler in allHandlers)
        {
            builder.AppendLine(
                $"{{ typeof({handler.FirstArgumentType}), typeof({handlerInterfaceName}<{handler.FirstArgumentType}, {handler.ReturnType}>) }},");
            builder.Append("        ");
        }

        return builder.ToString();
    }
}
