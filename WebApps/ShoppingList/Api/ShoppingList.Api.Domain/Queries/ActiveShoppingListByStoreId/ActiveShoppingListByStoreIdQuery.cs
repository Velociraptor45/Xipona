using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.SharedModels;

namespace ShoppingList.Api.Domain.Queries.ActiveShoppingListByStoreId
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