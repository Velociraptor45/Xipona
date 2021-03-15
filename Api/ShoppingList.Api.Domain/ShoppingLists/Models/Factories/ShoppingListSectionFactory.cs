using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
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

        public IShoppingListSection Create(IStoreSection storeSection, IEnumerable<IShoppingListItem> items)
        {
            return new ShoppingListSection(storeSection.Id.AsShoppingListSectionId(), storeSection.Name, items,
                storeSection.SortingIndex, storeSection.IsDefaultSection);
        }
    }
}