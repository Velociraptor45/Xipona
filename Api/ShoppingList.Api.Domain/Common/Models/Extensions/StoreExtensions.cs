using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.SharedModels;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions
{
    public static class StoreExtensions
    {
        public static StoreReadModel ToStoreReadModel(this IStore model)
        {
            return new StoreReadModel(model.Id, model.Name, model.IsDeleted);
        }

        public static ActiveStoreReadModel ToActiveStoreReadModel(this IStore model,
            IEnumerable<StoreItemReadModel> items)
        {
            return new ActiveStoreReadModel(model.Id, model.Name, items);
        }
    }
}