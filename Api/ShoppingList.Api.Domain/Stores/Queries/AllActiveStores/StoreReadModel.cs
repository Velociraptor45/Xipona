using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores
{
    public class StoreReadModel
    {
        public StoreReadModel(StoreId id, string name, IEnumerable<StoreItemReadModel> items,
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