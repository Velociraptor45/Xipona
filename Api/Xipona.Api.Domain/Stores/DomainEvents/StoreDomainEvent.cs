using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Stores.DomainEvents;

public record StoreDomainEvent : IDomainEvent
{
    public StoreId StoreId { get; init; }
}
