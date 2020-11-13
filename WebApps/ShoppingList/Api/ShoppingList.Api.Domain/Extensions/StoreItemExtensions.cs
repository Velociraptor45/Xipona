using ShoppingList.Api.Domain.Exceptions;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.ItemFilterResults;
using ShoppingList.Api.Domain.Queries.ItemSearch;
using System.Linq;

namespace ShoppingList.Api.Domain.Extensions
{
    public static class StoreItemExtensions
    {
        public static ShoppingListItem ToShoppingListItemDomain(this StoreItem storeItem, StoreId storeId,
            bool isInBasket, float quantity)
        {
            StoreItemAvailability availability = storeItem.Availabilities
                .FirstOrDefault(availability => availability.StoreId == storeId);
            if (availability == null)
                throw new ItemAtStoreNotAvailableException(storeItem.Id, storeId);

            return new ShoppingListItem(storeItem.Id.ToShoppingListItemId(),
                storeItem.Name,
                storeItem.IsDeleted,
                storeItem.Comment,
                storeItem.IsTemporary,
                availability.Price,
                storeItem.QuantityType,
                storeItem.QuantityInPacket,
                storeItem.QuantityTypeInPacket,
                storeItem.ItemCategory,
                storeItem.Manufacturer,
                isInBasket,
                quantity);
        }

        public static ItemSearchReadModel ToItemSearchReadModel(this StoreItem storeItem, StoreId storeId)
        {
            StoreItemAvailability storeAvailability = storeItem.Availabilities
                .FirstOrDefault(av => av.StoreId == storeId);

            if (storeAvailability == null)
                throw new ItemAtStoreNotAvailableException(storeItem.Id, storeId);

            return new ItemSearchReadModel(storeItem.Id, storeItem.Name, storeAvailability.Price,
                storeItem.Manufacturer, storeItem.ItemCategory);
        }

        public static ItemFilterResultReadModel ToItemFilterResultReadModel(this StoreItem model)
        {
            return new ItemFilterResultReadModel(model.Id, model.Name);
        }
    }
}