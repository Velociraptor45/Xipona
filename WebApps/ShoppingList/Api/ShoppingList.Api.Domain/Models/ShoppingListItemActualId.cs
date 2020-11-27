using ShoppingList.Api.Core;

namespace ShoppingList.Api.Domain.Models
{
    public class ShoppingListItemActualId : GenericPrimitive<int>
    {
        public ShoppingListItemActualId(int id)
            : base(id)
        {
        }
    }
}