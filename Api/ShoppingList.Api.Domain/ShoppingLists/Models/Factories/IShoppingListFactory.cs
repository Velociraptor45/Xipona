using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public interface IShoppingListFactory
    {
        IShoppingList Create(ShoppingListId id, IShoppingListStore store, IEnumerable<IShoppingListSection> sections, DateTime? completionDate);

        IShoppingList CreateNew(IShoppingListStore store, IEnumerable<IShoppingListSection> sections, DateTime? completionDate);
        IShoppingList CreateNew(IStore store);
    }
}