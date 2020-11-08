using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Commands.UpdateItem
{
    public static class ItemUpdateExtensions
    {
        public static StoreItem ToStoreItem(this ItemUpdate itemUpdate, ItemCategory itemCategory,
            Manufacturer manufacturer)
        {
            return new StoreItem(new StoreItemId(0),
                itemUpdate.Name, false, itemUpdate.Comment, false, itemUpdate.QuantityType,
                itemUpdate.QuantityInPacket, itemUpdate.QuantityTypeInPacket, itemCategory, manufacturer,
                itemUpdate.Availabilities);
        }
    }
}