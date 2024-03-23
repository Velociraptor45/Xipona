using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;
using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.DomainEvents;

public record ItemDomainEvent : IDomainEvent
{
    public ItemId ItemId { get; init; }
}