using Xipona.Api.Generators.Common;

namespace Xipona.Api.Generators.Handlers;

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