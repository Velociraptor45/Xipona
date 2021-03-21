using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class StoreItemStore : IStoreItemStore
    {
        public StoreItemStore(StoreItemStoreId id, string name, IEnumerable<IStoreItemSection> sections)
        {
            Id = id;
            Name = name;
            Sections = sections.ToList().AsReadOnly();
        }

        public StoreItemStoreId Id { get; }
        public string Name { get; }
        public IReadOnlyCollection<IStoreItemSection> Sections { get; }
    }
}