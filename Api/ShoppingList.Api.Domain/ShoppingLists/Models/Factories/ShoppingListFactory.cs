using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public class ShoppingListFactory : IShoppingListFactory
    {
        public IShoppingList Create(ShoppingListId id, IStore store, IEnumerable<IShoppingListItem> items,
            DateTime? completionDate)
        {
            return new ShoppingList(id, store, items, completionDate);
        }
    }
}