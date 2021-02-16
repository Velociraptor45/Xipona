using ProjectHermes.ShoppingList.Api.Core;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Model
{
    public class StoreId : GenericPrimitive<int>
    {
        public StoreId(int value) : base(value)
        {
        }
    }
}