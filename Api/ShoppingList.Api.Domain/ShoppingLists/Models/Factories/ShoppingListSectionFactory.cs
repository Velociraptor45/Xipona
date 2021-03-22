using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public class ShoppingListSectionFactory : IShoppingListSectionFactory
    {
        public IShoppingListSection Create(ShoppingListSectionId sectionId, IEnumerable<IShoppingListItem> shoppingListItems)
        {
            return new ShoppingListSection(sectionId, shoppingListItems);
        }
    }
}