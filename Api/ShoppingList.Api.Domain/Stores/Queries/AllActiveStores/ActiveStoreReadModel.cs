using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores
{
    public class ActiveStoreReadModel
    {
        private readonly IEnumerable<StoreItemReadModel> items;

        public ActiveStoreReadModel(StoreId id, string name, IEnumerable<StoreItemReadModel> items)
        {
            Id = id;
            Name = name;
            this.items = items;
        }

        public StoreId Id { get; }
        public string Name { get; }

        public IReadOnlyCollection<StoreItemReadModel> Items => items.ToList().AsReadOnly();
    }
}