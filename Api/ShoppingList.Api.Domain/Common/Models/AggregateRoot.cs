using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models;

public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void PublishDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
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