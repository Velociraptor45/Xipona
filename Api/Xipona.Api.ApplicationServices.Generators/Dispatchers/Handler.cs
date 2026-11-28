using Xipona.Api.Generators.Core.Common;

namespace Xipona.Api.ApplicationServices.Generators.Dispatchers;

public readonly record struct Handler
{
    public Handler(TypeAnalysis firstArgumentType, TypeAnalysis returnType)
    {
        FirstArgumentType = firstArgumentType;
        ReturnType = returnType;
    }

    public TypeAnalysis FirstArgumentType { get; }
    public TypeAnalysis ReturnType { get; }
}
