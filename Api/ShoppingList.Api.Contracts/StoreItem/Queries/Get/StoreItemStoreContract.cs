using System.Collections.Generic;
using System.Linq;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get
{
    public class StoreItemStoreContract
    {
        public StoreItemStoreContract(int id, string name, IEnumerable<StoreSectionContract> sections)
        {
            Id = id;
            Name = name;
            Sections = sections.ToList().AsReadOnly();
        }

        public int Id { get; }
        public string Name { get; }
        public IReadOnlyCollection<StoreSectionContract> Sections { get; }
    }
}