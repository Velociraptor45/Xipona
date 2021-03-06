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

        public IStoreItemAvailability Create(IStoreItemStore store, float price, StoreItemSectionId defaultSectionId)
        {
            return new StoreItemAvailability(store, price, defaultSectionId);
        }

        public IStoreItemAvailability Create(IStore store, float price, StoreItemSectionId defaultSectionId)
        {
            var storeItemStore = storeItemStoreFactory.Create(store);

            return new StoreItemAvailability(storeItemStore, price, defaultSectionId);
        }

        public IStoreItemAvailability Create(IStore store, float price, StoreSectionId defaultSectionId)
        {
            var storeItemStore = storeItemStoreFactory.Create(store);

            return new StoreItemAvailability(storeItemStore, price, defaultSectionId.AsStoreItemSectionId());
        }
    }
}