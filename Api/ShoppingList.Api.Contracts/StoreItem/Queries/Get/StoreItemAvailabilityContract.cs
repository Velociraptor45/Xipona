using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get
{
    public class StoreItemAvailabilityContract
    {
        public StoreItemAvailabilityContract(StoreItemStoreContract store, float price,
            StoreSectionContract defaultSection)
        {
            Store = store;
            Price = price;
            DefaultSection = defaultSection;
        }

        public StoreItemStoreContract Store { get; }
        public float Price { get; }
        public StoreSectionContract DefaultSection { get; }
    }
}