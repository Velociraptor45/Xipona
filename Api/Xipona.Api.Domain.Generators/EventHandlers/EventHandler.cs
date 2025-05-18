using Xipona.Api.Generators.Core.Common;

namespace Xipona.Api.Domain.Generators.EventHandlers;

public readonly record struct EventHandler
{
    public EventHandler(string name, string namespaceName, TypeAnalysis eventType)
    {
        Name = name;
        NamespaceName = namespaceName;
        EventType = eventType;
    }

    public string Name { get; }
    public string NamespaceName { get; }
    public TypeAnalysis EventType { get; }
};