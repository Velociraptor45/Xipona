using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public class ShoppingListFactory : IShoppingListFactory
    {
        public IShoppingList Create(ShoppingListId id, IStore store, IEnumerable<IShoppingListSection> sections,
            DateTime? completionDate)
        {
            return new ShoppingList(id, store, sections, completionDate);
        }

        public IShoppingList Create(IStore store, IEnumerable<IShoppingListSection> sections,
            DateTime? completionDate)
        {
            return new ShoppingList(new ShoppingListId(0), store, sections, completionDate);
        }
    }
}