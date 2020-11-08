using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Commands.ChangeItem
{
    public static class ChangeItemExtensions
    {
        public static StoreItem ToStoreItem(this ItemChange itemChange, ItemCategory itemCategory,
            Manufacturer manufacturer)
        {
            return new StoreItem(itemChange.Id, itemChange.Name, itemChange.IsDeleted, itemChange.Comment,
                itemChange.IsTemporary, itemChange.QuantityType, itemChange.QuantityInPacket,
                itemChange.QuantityTypeInPacket, itemCategory, manufacturer, itemChange.Availabilities);
        }
    }
}