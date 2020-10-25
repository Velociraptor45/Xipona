using ShoppingList.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Domain.Queries.AllActiveStores
{
    public class ActiveStoreReadModel
    {
        private readonly IEnumerable<StoreItem> items;

        public ActiveStoreReadModel(StoreId id, string name, IEnumerable<StoreItem> items)
        {
            Id = id;
            Name = name;
            this.items = items;
        }

        public StoreId Id { get; }
        public string Name { get; }

        public IReadOnlyCollection<StoreItem> Items => items.ToList().AsReadOnly();
    }
}