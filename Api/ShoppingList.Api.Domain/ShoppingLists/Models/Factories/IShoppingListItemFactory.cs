using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public interface IShoppingListItemFactory
    {
        IShoppingListItem Create(ShoppingListItemId id, string name, bool isDeleted, string comment, bool isTemporary,
            float pricePerQuantity, QuantityType quantityType, float quantityInPacket,
            QuantityTypeInPacket quantityTypeInPacket, IItemCategory itemCategory, IManufacturer manufacturer,
            bool isInBasket, float quantity);
        IShoppingListItem Create(IStoreItem storeItem, float price, bool isInBasket, float quantity);
    }
}