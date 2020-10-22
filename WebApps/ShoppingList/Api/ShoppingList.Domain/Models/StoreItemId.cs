using ShoppingList.Core;

namespace ShoppingList.Domain.Models
{
    public class StoreItemId : GenericPrimitive<int>
    {
        public StoreItemId(int id)
            : base(id)
        {
        }
    }
}