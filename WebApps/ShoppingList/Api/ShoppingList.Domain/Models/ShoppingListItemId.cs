using ShoppingList.Core;

namespace ShoppingList.Domain.Models
{
    public class ShoppingListItemId : GenericPrimitive<int>
    {
        public ShoppingListItemId(int id)
            : base(id)
        {
        }
    }
}