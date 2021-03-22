using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;
using System.Collections.Generic;
using System.Linq;
using StoreModels = ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions
{
    public static class StoreExtensions
    {
        public static ShoppingListStoreReadModel ToCommonStoreReadModel(this IShoppingListStore model)
        {
            return new ShoppingListStoreReadModel(model.Id, model.Name, model.IsDeleted);
        }

        public static ActiveStoreReadModel ToActiveStoreReadModel(this StoreModels.IStore model,
            IEnumerable<StoreItemReadModel> items)
        {
            return new ActiveStoreReadModel(model.Id, model.Name, items, model.Sections.Select(s => s.ToReadModel()));
        }
    }
}