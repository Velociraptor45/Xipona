using ProjectHermes.ShoppingList.Api.Core;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public class ShoppingListId : GenericPrimitive<int>
    {
        public ShoppingListId(int id)
            : base(id)
        {
        }
    }
}