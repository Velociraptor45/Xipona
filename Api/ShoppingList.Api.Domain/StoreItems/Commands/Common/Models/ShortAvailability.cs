using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models
{
    public class ShortAvailability
    {
        public ShortAvailability(StoreId storeId, float price, StoreItemSectionId storeItemSectionId)
        {
            StoreId = storeId;
            Price = price;
            StoreItemSectionId = storeItemSectionId;
        }

        public StoreId StoreId { get; }
        public float Price { get; }
        public StoreItemSectionId StoreItemSectionId { get; }
    }
}