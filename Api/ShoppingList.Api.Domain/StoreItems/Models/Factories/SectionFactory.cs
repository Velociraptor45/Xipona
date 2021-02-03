using ProjectHermes.ShoppingList.Api.Domain.Sections.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public class SectionFactory : ISectionFactory
    {
        public ISection Create(SectionId sectionId, string name, IEnumerable<IShoppingListItem> shoppingListItems,
            int sortingIndex, bool isDefaultSection)
        {
            return new Section(sectionId, name, shoppingListItems, sortingIndex, isDefaultSection);
        }
    }
}