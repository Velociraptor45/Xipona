using ProjectHermes.ShoppingList.Api.Core;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public class ShoppingListItemActualId : GenericPrimitive<int>
    {
        public ShoppingListItemActualId(int id)
            : base(id)
        {
        }
    }
}