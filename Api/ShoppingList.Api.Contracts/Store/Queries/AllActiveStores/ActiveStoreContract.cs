using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores
{
    public class ActiveStoreContract
    {
        public ActiveStoreContract(int id, string name, int itemCount, IEnumerable<StoreSectionContract> sections)
        {
            Id = id;
            Name = name;
            ItemCount = itemCount;
            Sections = sections.ToList().AsReadOnly();
        }

        public int Id { get; }
        public string Name { get; }
        public int ItemCount { get; }
        public IReadOnlyCollection<StoreSectionContract> Sections { get; }
    }
}