namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get
{
    public class StoreItemAvailabilityContract
    {
        public StoreItemAvailabilityContract(StoreItemStoreContract store, float price)
        {
            Store = store;
            Price = price;
        }

        public StoreItemStoreContract Store { get; }
        public float Price { get; }
    }
}