namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels
{
    public class StoreItemAvailabilityReadModel
    {
        public StoreItemAvailabilityReadModel(StoreItemStoreReadModel store, float price)
        {
            Store = store;
            Price = price;
        }

        public StoreItemStoreReadModel Store { get; }
        public float Price { get; }
    }
}