using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public interface IShoppingListFactory
    {
        IShoppingList Create(ShoppingListId id, IStore store, IEnumerable<IShoppingListSection> sections, DateTime? completionDate);

        IShoppingList Create(IStore store, IEnumerable<IShoppingListSection> sections, DateTime? completionDate);
    }
}