using ShoppingList.Api.Core;

namespace ShoppingList.Api.Domain.Models
{
    public class ShoppingListId : GenericPrimitive<int>
    {
        public ShoppingListId(int id)
            : base(id)
        {
        }
    }
}