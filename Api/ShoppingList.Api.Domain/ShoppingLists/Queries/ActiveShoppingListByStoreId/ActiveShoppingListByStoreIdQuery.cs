using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId
{
    public class ActiveShoppingListByStoreIdQuery : IQuery<ShoppingListReadModel>
    {
        public ActiveShoppingListByStoreIdQuery(StoreId storeId)
        {
            StoreId = storeId;
        }

        public StoreId StoreId { get; }
    }
}