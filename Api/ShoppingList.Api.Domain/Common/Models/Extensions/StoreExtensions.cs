using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;
using System.Collections.Generic;
using System.Linq;
using StoreModels = ProjectHermes.ShoppingList.Api.Domain.Stores.Model;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions
{
    public static class StoreExtensions
    {
        public static StoreReadModel ToCommonStoreReadModel(this IShoppingListStore model)
        {
            return new StoreReadModel(model.Id, model.Name, model.IsDeleted);
        }

        public static Stores.Queries.SharedModels.StoreReadModel ToStoreReadModel(this StoreModels.IStore model)
        {
            return new Stores.Queries.SharedModels.StoreReadModel(model.Id, model.Name, model.IsDeleted);
        }

        public static ActiveStoreReadModel ToActiveStoreReadModel(this StoreModels.IStore model,
            IEnumerable<StoreItemReadModel> items)
        {
            return new ActiveStoreReadModel(model.Id, model.Name, items, model.Sections.Select(s => s.ToReadModel()));
        }
    }
}