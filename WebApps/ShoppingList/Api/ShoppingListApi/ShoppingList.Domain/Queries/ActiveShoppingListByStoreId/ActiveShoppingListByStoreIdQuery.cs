using ShoppingList.Domain.Models;
using ShoppingList.Domain.Queries.SharedModels;

namespace ShoppingList.Domain.Queries.ActiveShoppingListByStoreId
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