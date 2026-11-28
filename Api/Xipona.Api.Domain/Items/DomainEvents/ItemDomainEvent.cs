using Xipona.Api.Core.DomainEventHandlers;
using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.DomainEvents;

public record ItemDomainEvent : IDomainEvent
{
    public ItemId ItemId { get; init; }
}