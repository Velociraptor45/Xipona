using ShoppingList.Domain.Models;

namespace ShoppingList.Domain.Converters
{
    public static class StoreItemConverter
    {
        public static ShoppingListItem ToShoppingListItemDomain(this StoreItem storeItem,
            ItemCategory itemCategory, Manufacturer manufacturer, bool isInBasket, float quantity)
        {
            return new ShoppingListItem(storeItem.Id.ToShoppingListItemId(),
                storeItem.Name,
                storeItem.IsDeleted,
                storeItem.Comment,
                storeItem.IsTemporary,
                storeItem.Price,
                storeItem.QuantityType,
                storeItem.QuantityInPacket,
                storeItem.QuantityTypeInPacket,
                itemCategory,
                manufacturer,
                isInBasket,
                quantity);
        }
    }
}