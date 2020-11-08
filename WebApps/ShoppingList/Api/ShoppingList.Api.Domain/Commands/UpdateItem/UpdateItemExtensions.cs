using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Commands.UpdateItem
{
    public static class UpdateItemExtensions
    {
        public static StoreItem ToStoreItem(this ItemUpdate itemUpdate, ItemCategory itemCategory,
            Manufacturer manufacturer)
        {
            return new StoreItem(itemUpdate.Id, itemUpdate.Name, itemUpdate.IsDeleted, itemUpdate.Comment,
                itemUpdate.IsTemporary, itemUpdate.QuantityType, itemUpdate.QuantityInPacket,
                itemUpdate.QuantityTypeInPacket, itemCategory, manufacturer, itemUpdate.Availabilities);
        }
    }
}