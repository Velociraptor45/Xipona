namespace Xipona.Api.Generators.Common;

public readonly record struct TypeAnalysis
{
    public TypeAnalysis(string typeName, string typeNamespace, IEnumerable<TypeAnalysis> genericArguments)
    {
        TypeName = typeName;
        TypeNamespace = typeNamespace;
        GenericArguments = genericArguments.ToList();
    }

    public string TypeName { get; }
    public string TypeNamespace { get; }
    public IReadOnlyCollection<TypeAnalysis> GenericArguments { get; }

    public IEnumerable<string> GetAllNamespaces()
    {
        return new[] { TypeNamespace }
            .Concat(GenericArguments.SelectMany(arg => arg.GetAllNamespaces()));
    }
}