using ProjectHermes.ShoppingList.Api.Core;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public class ShoppingListStoreId : GenericPrimitive<int>
    {
        public ShoppingListStoreId(int id)
            : base(id)
        {
        }

        public StoreItemStoreId ToStoreItemStoreId()
        {
            return new StoreItemStoreId(Value);
        }
    }
}