using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;

namespace ProjectHermes.Xipona.Api.Domain.Common.Models;

public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void PublishDomainEvent(IDomainEvent domainEvent)
    {
        var transformedDomainEvent = OnBeforeAddingDomainEvent(domainEvent);
        _domainEvents.Add(transformedDomainEvent);
    }

    protected virtual IDomainEvent OnBeforeAddingDomainEvent(IDomainEvent domainEvent)
    {
        return domainEvent;
    }

    protected void PublishDomainEvents(IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            PublishDomainEvent(domainEvent);
        }
    }

    public async Task DispatchDomainEvents(IDomainEventDispatcher dispatcher)
    {
        foreach (var domainEvent in _domainEvents)
        {
            await dispatcher.DispatchAsync(domainEvent);
        }

        _domainEvents.Clear();
    }

    public void EnrichWithRowVersion(byte[] rowVersion)
    {
        if (RowVersion.Length > 0)
            throw new InvalidOperationException("Row version already exists.");

        RowVersion = rowVersion;
    }
}