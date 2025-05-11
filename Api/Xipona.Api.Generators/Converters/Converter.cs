using Xipona.Api.Generators.Common;

namespace Xipona.Api.Generators.Converters;

public readonly record struct Converter
{
    public Converter(string name, string namespaceName, TypeAnalysis sourceType, TypeAnalysis targetType)
    {
        Name = name;
        NamespaceName = namespaceName;
        SourceType = sourceType;
        TargetType = targetType;
    }

    public string Name { get; }
    public string NamespaceName { get; }
    public TypeAnalysis SourceType { get; }
    public TypeAnalysis TargetType { get; }

    public IEnumerable<string> GetAllNamespaces()
    {
        return SourceType.GetAllNamespaces().Concat(TargetType.GetAllNamespaces()).Concat([NamespaceName]);
    }
};