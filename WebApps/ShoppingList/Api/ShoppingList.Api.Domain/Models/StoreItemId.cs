using ShoppingList.Api.Core;

namespace ShoppingList.Api.Domain.Models
{
    public class StoreItemId : GenericPrimitive<int>
    {
        public StoreItemId(int id)
            : base(id)
        {
        }
    }
}