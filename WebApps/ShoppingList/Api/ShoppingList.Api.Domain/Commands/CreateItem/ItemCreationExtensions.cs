using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Commands.CreateItem
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
                itemCreation.QuantityInPacketType,
                itemCategory,
                manufacturer,
                itemCreation.Availabilities);
        }
    }
}