using ShoppingList.Core;

namespace ShoppingList.Domain.Models
{
    public class ShoppingListId : GenericPrimitive<int>
    {
        public ShoppingListId(int id)
            : base(id)
        {
        }
    }
}