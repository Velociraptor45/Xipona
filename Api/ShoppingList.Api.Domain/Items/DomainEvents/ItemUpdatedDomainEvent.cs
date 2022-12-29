using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;

public class ItemUpdatedDomainEvent : IDomainEvent
{
    public ItemUpdatedDomainEvent(ItemId oldItemId, IItem newItem)
    {
        OldItemId = oldItemId;
        NewItem = newItem;
    }

    public ItemId OldItemId { get; }
    public IItem NewItem { get; }
}