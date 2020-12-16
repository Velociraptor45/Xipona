using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.UpdateItem
{
    public static class ItemUpdateExtensions
    {
        public static IStoreItem ToStoreItem(this ItemUpdate itemUpdate, IItemCategory itemCategory,
            IManufacturer manufacturer, IStoreItem predecessor)
        {
            return new StoreItem(new StoreItemId(0),
                itemUpdate.Name, false, itemUpdate.Comment, false, itemUpdate.QuantityType,
                itemUpdate.QuantityInPacket, itemUpdate.QuantityTypeInPacket, itemCategory, manufacturer,
                itemUpdate.Availabilities, predecessor);
        }
    }
}