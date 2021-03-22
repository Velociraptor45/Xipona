using ProjectHermes.ShoppingList.Api.Core;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models
{
    public class StoreId : GenericPrimitive<int>
    {
        public StoreId(int value) : base(value)
        {
        }

        public StoreItemStoreId AsStoreItemStoreId()
        {
            return new StoreItemStoreId(Value);
        }

        public ShoppingListStoreId AsShoppingListStoreId()
        {
            return new ShoppingListStoreId(Value);
        }
    }
}