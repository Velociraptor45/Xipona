using ShoppingList.Core;

namespace ShoppingList.Domain.Models
{
    public class StoreId : GenericPrimitive<int>
    {
        public StoreId(int id)
            : base(id)
        {
        }
    }
}