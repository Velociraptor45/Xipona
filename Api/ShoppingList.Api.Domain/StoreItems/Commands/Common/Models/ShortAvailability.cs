using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models
{
    public class ShortAvailability
    {
        public ShortAvailability(StoreItemStoreId storeId, float price, StoreItemSectionId storeItemSectionId)
        {
            StoreId = storeId;
            Price = price;
            StoreItemSectionId = storeItemSectionId;
        }

        public StoreItemStoreId StoreId { get; }
        public float Price { get; }
        public StoreItemSectionId StoreItemSectionId { get; }
    }
}