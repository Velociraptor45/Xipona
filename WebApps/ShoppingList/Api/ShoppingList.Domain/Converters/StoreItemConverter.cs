using ShoppingList.Domain.Exceptions;
using ShoppingList.Domain.Models;
using ShoppingList.Domain.Queries.ItemSearch;
using System.Linq;

namespace ShoppingList.Domain.Converters
{
    public static class StoreItemConverter
    {
        public static ShoppingListItem ToShoppingListItemDomain(this StoreItem storeItem, StoreId storeId,
            ItemCategory itemCategory, Manufacturer manufacturer, bool isInBasket, float quantity)
        {
            var availability = storeItem.Availabilities
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
                itemCategory,
                manufacturer,
                isInBasket,
                quantity);
        }

        public static ItemSearchReadModel ToItemSearchReadModel(this StoreItem storeItem, StoreId storeId,
            ItemCategory itemCategory, Manufacturer manufacturer)
        {
            var storeAvailability = storeItem.Availabilities
                .FirstOrDefault(av => av.StoreId == storeId);

            if (storeAvailability == null)
                throw new ItemAtStoreNotAvailableException(storeItem.Id, storeId);

            return new ItemSearchReadModel(storeItem.Id, storeItem.Name, storeAvailability.Price,
                manufacturer, itemCategory);
        }
    }
}