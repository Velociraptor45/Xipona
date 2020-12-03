using ShoppingList.Api.Core;

namespace ShoppingList.Api.Domain.Models
{
    public class StoreItemActualId : GenericPrimitive<int>
    {
        public StoreItemActualId(int id)
            : base(id)
        {
        }
    }
}