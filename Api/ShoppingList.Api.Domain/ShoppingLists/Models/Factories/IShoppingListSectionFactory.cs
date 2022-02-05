using System.Collections.Generic;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;

public interface IShoppingListSectionFactory
{
    IShoppingListSection Create(SectionId sectionId, IEnumerable<IShoppingListItem> shoppingListItems);
    IShoppingListSection CreateEmpty(IStoreSection storeSection);
    IShoppingListSection CreateEmpty(SectionId sectionId);
}