using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public interface IShoppingListSectionFactory
    {
        IShoppingListSection Create(ShoppingListSectionId sectionId, string name, IEnumerable<IShoppingListItem> shoppingListItems, int sortingIndex, bool isDefaultSection);
    }
}