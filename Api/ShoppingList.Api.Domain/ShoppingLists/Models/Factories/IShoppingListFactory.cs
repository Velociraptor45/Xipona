using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public interface IShoppingListFactory
    {
        IShoppingList Create(ShoppingListId id, IShoppingListStore store, IEnumerable<IShoppingListSection> sections, DateTime? completionDate);

        IShoppingList Create(IShoppingListStore store, IEnumerable<IShoppingListSection> sections, DateTime? completionDate);
    }
}