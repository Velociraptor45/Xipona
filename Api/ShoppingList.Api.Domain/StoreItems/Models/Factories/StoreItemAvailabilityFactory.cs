using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public class StoreItemAvailabilityFactory : IStoreItemAvailabilityFactory
    {
        private readonly IStoreItemStoreFactory storeItemStoreFactory;

        public StoreItemAvailabilityFactory(IStoreItemStoreFactory storeItemStoreFactory)
        {
            this.storeItemStoreFactory = storeItemStoreFactory;
        }

        public IStoreItemAvailability Create(IStoreItemStore store, float price, IStoreItemSection defaultSection)
        {
            return new StoreItemAvailability(store, price, defaultSection);
        }

        public IStoreItemAvailability Create(IStore store, float price, IStoreItemSection defaultSection)
        {
            var storeItemStore = storeItemStoreFactory.Create(store);

            return new StoreItemAvailability(storeItemStore, price, defaultSection);
        }
    }
}