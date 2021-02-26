using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public class StoreItemAvailabilityFactory : IStoreItemAvailabilityFactory
    {
        private readonly IStoreItemStoreFactory storeItemStoreFactory;
        private readonly IStoreItemSectionFactory storeItemSectionFactory;

        public StoreItemAvailabilityFactory(IStoreItemStoreFactory storeItemStoreFactory,
            IStoreItemSectionFactory storeItemSectionFactory)
        {
            this.storeItemStoreFactory = storeItemStoreFactory;
            this.storeItemSectionFactory = storeItemSectionFactory;
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

        public IStoreItemAvailability Create(IStore store, float price, IStoreSection defaultSection)
        {
            var storeItemStore = storeItemStoreFactory.Create(store);
            var section = storeItemSectionFactory.Create(defaultSection);

            return new StoreItemAvailability(storeItemStore, price, section);
        }
    }
}