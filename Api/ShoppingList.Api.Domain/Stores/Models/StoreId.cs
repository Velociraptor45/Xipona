using ProjectHermes.ShoppingList.Api.Core;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models
{
    public class StoreId : GenericPrimitive<int>
    {
        public StoreId(int value) : base(value)
        {
        }
    }
}