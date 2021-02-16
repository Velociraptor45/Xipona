using ProjectHermes.ShoppingList.Api.Core;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models
{
    public class StoreId : GenericPrimitive<int>
    {
        public StoreId(int id)
            : base(id)
        {
        }

        public StoreItemStoreId ToStoreItemStoreId()
        {
            return new StoreItemStoreId(Value);
        }
    }
}