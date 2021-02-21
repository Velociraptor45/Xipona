namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get
{
    public class StoreItemAvailabilityContract
    {
        public StoreItemAvailabilityContract(StoreItemStoreContract store, float price,
            StoreItemSectionContract defaultSection)
        {
            Store = store;
            Price = price;
            DefaultSection = defaultSection;
        }

        public StoreItemStoreContract Store { get; }
        public float Price { get; }
        public StoreItemSectionContract DefaultSection { get; }
    }
}