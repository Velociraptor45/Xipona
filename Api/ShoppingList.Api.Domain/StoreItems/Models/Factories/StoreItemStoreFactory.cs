using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public class StoreItemStoreFactory : IStoreItemStoreFactory
    {
        private readonly IStoreItemSectionFactory storeItemSectionFactory;

        public StoreItemStoreFactory(IStoreItemSectionFactory storeItemSectionFactory)
        {
            this.storeItemSectionFactory = storeItemSectionFactory;
        }

        public IStoreItemStore Create(IStore store)
        {
            var sections = store.Sections.Select(s => storeItemSectionFactory.Create(s));

            return new StoreItemStore(store.Id.AsStoreItemStoreId(), store.Name, sections);
        }
    }
}