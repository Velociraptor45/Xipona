using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem
{
    public static class ItemCreationExtensions
    {
        public static StoreItem ToStoreItem(this ItemCreation itemCreation, ItemCategory itemCategory,
            Manufacturer manufacturer)
        {
            return new StoreItem(new StoreItemId(0),
                itemCreation.Name,
                false,
                itemCreation.Comment,
                false,
                itemCreation.QuantityType,
                itemCreation.QuantityInPacket,
                itemCreation.QuantityTypeInPacket,
                itemCategory,
                manufacturer,
                itemCreation.Availabilities);
        }
    }
}