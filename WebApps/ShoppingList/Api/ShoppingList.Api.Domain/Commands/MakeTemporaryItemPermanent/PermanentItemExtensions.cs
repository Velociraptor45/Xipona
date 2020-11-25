using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Commands.MakeTemporaryItemPermanent
{
    public static class PermanentItemExtensions
    {
        public static StoreItem ToStoreItem(this PermanentItem permanentItem, ItemCategory itemCategory,
            Manufacturer manufacturer)
        {
            return new StoreItem(
                permanentItem.Id,
                permanentItem.Name,
                false,
                permanentItem.Comment,
                false,
                permanentItem.QuantityType,
                permanentItem.QuantityInPacket,
                permanentItem.QuantityTypeInPacket,
                itemCategory,
                manufacturer,
                permanentItem.Availabilities);
        }
    }
}