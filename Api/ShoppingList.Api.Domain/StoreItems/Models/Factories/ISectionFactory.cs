using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public interface ISectionFactory
    {
        IShoppingListSection Create(ShoppingListSectionId sectionId, string name, IEnumerable<IShoppingListItem> shoppingListItems, int sortingIndex, bool isDefaultSection);
    }
}