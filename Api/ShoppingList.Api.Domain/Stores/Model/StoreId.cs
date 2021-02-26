using ProjectHermes.ShoppingList.Api.Core;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Model
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
    }
}