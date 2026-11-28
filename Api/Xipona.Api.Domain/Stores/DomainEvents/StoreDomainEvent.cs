using Xipona.Api.Core.DomainEventHandlers;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Stores.DomainEvents;

public record StoreDomainEvent : IDomainEvent
{
    public StoreId StoreId { get; init; }
}
