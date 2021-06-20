using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores
{
    public class StoreReadModel
    {
        public StoreReadModel(StoreId id, string name, int itemCount,
            IEnumerable<StoreSectionReadModel> sections)
        {
            Id = id;
            Name = name;
            ItemCount = itemCount;
            Sections = sections.ToList().AsReadOnly();
        }

        public StoreId Id { get; }
        public string Name { get; }
        public int ItemCount { get; }
        public IReadOnlyCollection<StoreSectionReadModel> Sections { get; }
    }
}