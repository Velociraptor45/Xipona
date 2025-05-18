using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Text;
using Xipona.Api.Generators.Common;
using Xipona.Api.Generators.Extensions;

namespace Xipona.Api.Generators.EventHandlers;

public abstract class EventHandlerDiGeneratorBase : IIncrementalGenerator
{
    public abstract void Initialize(IncrementalGeneratorInitializationContext context);

    protected IncrementalValuesProvider<EventHandler> GetAllHandlers(IncrementalGeneratorInitializationContext context,
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
                    || baseType.TypeArgumentList.Arguments.Count != 1)
                    return false;

                return true;
            },
            static (ctx, _) =>
            {
                var classNode = (ClassDeclarationSyntax)ctx.Node;

                if (classNode.BaseList?.Types[0].Type is not GenericNameSyntax baseType)
                    throw new InvalidOperationException("Base type is not a generic type.");

                var genericArguments = baseType.TypeArgumentList.Arguments;
                var genericArgument = genericArguments[0];

                return new EventHandler(
                    classNode.Identifier.Text,
                    classNode.GetNamespace(),
                    new TypeAnalysis(genericArgument, ctx));
            });
    }

    protected string GetRegistrations(ImmutableArray<EventHandler> allHandlers, string interfaceName)
    {
        var registrationBuilder = new StringBuilder();
        foreach (var h in allHandlers)
        {
            registrationBuilder.AppendLine(
                $"services.AddTransient<{interfaceName}<{h.EventType}>, {h.NamespaceName}.{h.Name}>();");
            registrationBuilder.Append("        ");
        }

        return registrationBuilder.ToString();
    }
}
