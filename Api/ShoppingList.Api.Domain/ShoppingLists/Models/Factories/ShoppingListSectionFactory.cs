using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public class ShoppingListSectionFactory : IShoppingListSectionFactory
    {
        public IShoppingListSection Create(ShoppingListSectionId sectionId, string name, IEnumerable<IShoppingListItem> shoppingListItems,
            int sortingIndex, bool isDefaultSection)
        {
            return new ShoppingListSection(sectionId, name, shoppingListItems, sortingIndex, isDefaultSection);
        }
    }
}