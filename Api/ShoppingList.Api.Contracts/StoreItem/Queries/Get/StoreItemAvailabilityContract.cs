namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get
{
    public class StoreItemAvailabilityContract
    {
        public StoreItemAvailabilityContract(int storeId, float price)
        {
            StoreId = storeId;
            Price = price;
        }

        public int StoreId { get; }
        public float Price { get; }
    }
}