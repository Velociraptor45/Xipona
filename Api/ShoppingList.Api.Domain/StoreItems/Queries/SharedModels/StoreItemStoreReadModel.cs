using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels
{
    public class StoreItemStoreReadModel
    {
        public StoreItemStoreReadModel(StoreItemStoreId id, string name, IEnumerable<StoreItemSectionReadModel> sections)
        {
            Id = id;
            Name = name;
            Sections = sections.ToList().AsReadOnly();
        }

        public StoreItemStoreId Id { get; }
        public string Name { get; }
        public IReadOnlyCollection<StoreItemSectionReadModel> Sections { get; }
    }
}