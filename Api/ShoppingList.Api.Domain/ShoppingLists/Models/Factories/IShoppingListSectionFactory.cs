using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public interface IShoppingListSectionFactory
    {
        IShoppingListSection Create(SectionId sectionId, IEnumerable<IShoppingListItem> shoppingListItems);
        IShoppingListSection CreateEmpty(IStoreSection storeSection);
    }
}