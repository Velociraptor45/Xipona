using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.SharedModels;
using System.Collections.Generic;

using CommonModels = ProjectHermes.ShoppingList.Api.Domain.Common.ReadModels;
using StoreModels = ProjectHermes.ShoppingList.Api.Domain.Stores.Model;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions
{
    public static class StoreExtensions
    {
        public static CommonModels.StoreReadModel ToCommonStoreReadModel(this IStore model)
        {
            return new CommonModels.StoreReadModel(model.Id, model.Name, model.IsDeleted);
        }

        public static StoreReadModel ToStoreReadModel(this StoreModels.IStore model)
        {
            return new StoreReadModel(model.Id, model.Name, model.IsDeleted);
        }

        public static ActiveStoreReadModel ToActiveStoreReadModel(this StoreModels.IStore model,
            IEnumerable<StoreItemReadModel> items)
        {
            return new ActiveStoreReadModel(model.Id, model.Name, items);
        }
    }
}