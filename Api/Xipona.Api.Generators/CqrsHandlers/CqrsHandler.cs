using Xipona.Api.Generators.Common;

namespace Xipona.Api.Generators.CqrsHandlers;

public readonly record struct CqrsHandler
{
    public CqrsHandler(string name, string namespaceName, TypeAnalysis firstGenericArgument, TypeAnalysis secondGenericArgument)
    {
        Name = name;
        NamespaceName = namespaceName;
        FirstGenericArgument = firstGenericArgument;
        SecondGenericArgument = secondGenericArgument;
    }

    public string Name { get; }
    public string NamespaceName { get; }
    public TypeAnalysis FirstGenericArgument { get; }
    public TypeAnalysis SecondGenericArgument { get; }

    public IEnumerable<string> GetAllNamespaces()
    {
        return FirstGenericArgument.GetAllNamespaces().Concat(SecondGenericArgument.GetAllNamespaces());
    }
};