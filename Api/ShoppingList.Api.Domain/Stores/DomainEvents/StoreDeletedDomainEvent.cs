using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.DomainEvents;

public class StoreDeletedDomainEvent : IDomainEvent
{
    public StoreDeletedDomainEvent(StoreId storeId)
    {
        StoreId = storeId;
    }

    public StoreId StoreId { get; }
}