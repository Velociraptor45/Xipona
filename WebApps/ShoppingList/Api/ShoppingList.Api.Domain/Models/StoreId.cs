using ShoppingList.Api.Core;

namespace ShoppingList.Api.Domain.Models
{
    public class StoreId : GenericPrimitive<int>
    {
        public StoreId(int id)
            : base(id)
        {
        }
    }
}