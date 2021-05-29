using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories
{
    public class StoreFactory : IStoreFactory
    {
        public IStore Create(StoreId id, string name, bool isDeleted, IEnumerable<IStoreSection> sections)
        {
            return new Store(id, name, isDeleted, sections);
        }
    }
}