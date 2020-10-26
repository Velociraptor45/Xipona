using ShoppingList.Api.Core;

namespace ShoppingList.Api.Domain.Models
{
    public class ShoppingListItemId : GenericPrimitive<int>
    {
        public ShoppingListItemId(int id)
            : base(id)
        {
        }
    }
}