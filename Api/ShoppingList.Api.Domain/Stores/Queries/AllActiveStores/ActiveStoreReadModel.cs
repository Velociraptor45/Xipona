using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores
{
    public class ActiveStoreReadModel
    {
        public ActiveStoreReadModel(StoreId id, string name, IEnumerable<StoreItemReadModel> items,
            IEnumerable<StoreSectionReadModel> sections)
        {
            Id = id;
            Name = name;
            Items = items.ToList().AsReadOnly();
            Sections = sections.ToList().AsReadOnly();
        }

        public StoreId Id { get; }
        public string Name { get; }
        public IReadOnlyCollection<StoreSectionReadModel> Sections { get; }

        public IReadOnlyCollection<StoreItemReadModel> Items { get; }
    }
}