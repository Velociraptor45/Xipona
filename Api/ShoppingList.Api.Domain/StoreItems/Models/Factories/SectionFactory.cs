using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public class SectionFactory : ISectionFactory
    {
        public IShoppingListSection Create(ShoppingListSectionId sectionId, string name, IEnumerable<IShoppingListItem> shoppingListItems,
            int sortingIndex, bool isDefaultSection)
        {
            return new ShoppingListSection(sectionId, name, shoppingListItems, sortingIndex, isDefaultSection);
        }
    }
}