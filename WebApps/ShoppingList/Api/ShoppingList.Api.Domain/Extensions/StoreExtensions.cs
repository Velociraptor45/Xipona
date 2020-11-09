using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.AllActiveStores;
using ShoppingList.Api.Domain.Queries.SharedModels;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.Extensions
{
    public static class StoreExtensions
    {
        public static StoreReadModel ToStoreReadModel(this Store model)
        {
            return new StoreReadModel(model.Id, model.Name, model.IsDeleted);
        }

        public static ActiveStoreReadModel ToActiveStoreReadModel(this Store model, IEnumerable<StoreItem> items)
        {
            return new ActiveStoreReadModel(model.Id, model.Name, items);
        }
    }
}