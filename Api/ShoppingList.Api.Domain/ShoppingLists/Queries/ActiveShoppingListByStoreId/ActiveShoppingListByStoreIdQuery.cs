using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId
{
    public class ActiveShoppingListByStoreIdQuery : IQuery<ShoppingListReadModel>
    {
        public ActiveShoppingListByStoreIdQuery(ShoppingListStoreId storeId)
        {
            StoreId = storeId;
        }

        public ShoppingListStoreId StoreId { get; }
    }
}