using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels
{
    public class StoreItemAvailabilityReadModel
    {
        public StoreItemAvailabilityReadModel(StoreId storeId, float price)
        {
            StoreId = storeId;
            Price = price;
        }

        public StoreId StoreId { get; }
        public float Price { get; }
    }
}