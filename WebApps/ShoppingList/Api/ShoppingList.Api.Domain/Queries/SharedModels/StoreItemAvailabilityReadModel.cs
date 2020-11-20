using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Queries.SharedModels
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