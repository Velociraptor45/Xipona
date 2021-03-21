using ProjectHermes.ShoppingList.Api.Core;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public class ShoppingListSectionId : GenericPrimitive<int>
    {
        public ShoppingListSectionId(int value) : base(value)
        {
        }
    }
}