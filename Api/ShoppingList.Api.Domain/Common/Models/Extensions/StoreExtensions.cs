using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.SharedModels;
using ShoppingList.Api.Domain.Models;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions
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