using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions
{
    public static class StoreExtensions
    {
        public static ShoppingListStoreReadModel ToShoppingListStoreReadModel(this IStore model)
        {
            return new ShoppingListStoreReadModel(model.Id, model.Name);
        }

        public static StoreItemStoreReadModel ToStoreItemStoreReadModel(this IStore model)
        {
            return new StoreItemStoreReadModel(model.Id, model.Name, model.Sections.Select(s => s.ToReadModel()));
        }

        public static StoreReadModel ToStoreReadModel(this IStore model, int itemCount)
        {
            return new StoreReadModel(model.Id, model.Name, itemCount, model.Sections.Select(s => s.ToReadModel()));
        }
    }
}