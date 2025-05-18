using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Text;
using Xipona.Api.Generators.Core.Common;
using Xipona.Api.Generators.Core.Extensions;

namespace Xipona.Api.Generators.Core.Converters;

public abstract class ConverterDiGeneratorBase : IIncrementalGenerator
{
    public abstract void Initialize(IncrementalGeneratorInitializationContext context);

    protected IncrementalValuesProvider<Converter[]> GetAllConverters(IncrementalGeneratorInitializationContext context,
        string interfaceName, string namespaceNameStart)
    {
        return context.SyntaxProvider.CreateSyntaxProvider(
            (s, _) =>
            {
                if (s is not ClassDeclarationSyntax classNode)
                    return false;

                if (classNode.BaseList is null
                    || classNode.BaseList.Types.Count == 0
                    || classNode.BaseList.Types.All(t =>
                        t.Type is not GenericNameSyntax baseType
                        || baseType.Identifier.Text != interfaceName
                        || baseType.TypeArgumentList.Arguments.Count != 2))
                    return false;

                if (!classNode.NamespaceStartsWith(namespaceNameStart))
                    return false;

                return true;
            },
            static (ctx, _) =>
            {
                var classNode = (ClassDeclarationSyntax)ctx.Node;

                var converters = new List<Converter>();

                foreach (var type in classNode.BaseList!.Types)
                {
                    if (type.Type is not GenericNameSyntax baseType)
                        continue;

                    var genericArguments = baseType.TypeArgumentList.Arguments;

                    // source and target type from generic argument of converter interface
                    var sourceType = genericArguments[0];
                    var targetType = genericArguments[1];

                    converters.Add(
                        new Converter(
                            classNode.Identifier.Text,
                            classNode.GetNamespace(),
                            new TypeAnalysis(sourceType, ctx),
                            new TypeAnalysis(targetType, ctx)));
                }

                return converters.ToArray();
            });
    }

    protected string GetRegistrations(ImmutableArray<Converter> allConverters, string interfaceName)
    {
        var registrationBuilder = new StringBuilder();
        foreach (var c in allConverters)
        {
            registrationBuilder.AppendLine(
                $"services.AddTransient<{interfaceName}<{c.SourceType}, {c.TargetType}>, {c.NamespaceName}.{c.Name}>();");
            registrationBuilder.Append("        ");
        }

        return registrationBuilder.ToString();
    }
}
