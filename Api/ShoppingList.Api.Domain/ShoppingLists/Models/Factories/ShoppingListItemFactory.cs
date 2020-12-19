using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public class ShoppingListItemFactory : IShoppingListItemFactory
    {
        public IShoppingListItem Create(ShoppingListItemId id, string name, bool isDeleted, string comment,
            bool isTemporary, float pricePerQuantity, QuantityType quantityType, float quantityInPacket,
            QuantityTypeInPacket quantityTypeInPacket, IItemCategory itemCategory, IManufacturer manufacturer,
            bool isInBasket, float quantity)
        {
            return new ShoppingListItem(
                id,
                name,
                isDeleted,
                comment,
                isTemporary,
                pricePerQuantity,
                quantityType,
                quantityInPacket,
                quantityTypeInPacket,
                itemCategory,
                manufacturer,
                isInBasket,
                quantity);
        }

        public IShoppingListItem Create(IStoreItem storeItem, float price, bool isInBasket, float quantity)
        {
            return new ShoppingListItem(storeItem.Id.ToShoppingListItemId(),
                storeItem.Name,
                storeItem.IsDeleted,
                storeItem.Comment,
                storeItem.IsTemporary,
                price,
                storeItem.QuantityType,
                storeItem.QuantityInPacket,
                storeItem.QuantityTypeInPacket,
                storeItem.ItemCategory,
                storeItem.Manufacturer,
                isInBasket,
                quantity);
        }
    }
}