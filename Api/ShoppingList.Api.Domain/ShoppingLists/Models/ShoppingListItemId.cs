using ProjectHermes.ShoppingList.Api.Core;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public class ShoppingListItemId : GenericPrimitive<int>
    {
        public ShoppingListItemId(int id) : base(id)
        {
        }
    }
}