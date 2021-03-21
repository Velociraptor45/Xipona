using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get
{
    public class StoreItemStoreContract
    {
        public StoreItemStoreContract(int id, string name, IEnumerable<StoreItemSectionContract> sections)
        {
            Id = id;
            Name = name;
            Sections = sections.ToList().AsReadOnly();
        }

        public int Id { get; }
        public string Name { get; }
        public IReadOnlyCollection<StoreItemSectionContract> Sections { get; }
    }
}