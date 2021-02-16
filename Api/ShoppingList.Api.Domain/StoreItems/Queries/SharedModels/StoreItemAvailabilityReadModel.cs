using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels
{
    public class StoreItemAvailabilityReadModel
    {
        public StoreItemAvailabilityReadModel(StoreItemStoreId storeId, float price)
        {
            StoreId = storeId;
            Price = price;
        }

        public StoreItemStoreId StoreId { get; }
        public float Price { get; }
    }
}