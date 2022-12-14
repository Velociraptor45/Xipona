using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;

public class ItemDeletedDomainEvent : IDomainEvent
{
    public ItemDeletedDomainEvent(ItemId itemId)
    {
        ItemId = itemId;
    }

    public ItemId ItemId { get; }
}