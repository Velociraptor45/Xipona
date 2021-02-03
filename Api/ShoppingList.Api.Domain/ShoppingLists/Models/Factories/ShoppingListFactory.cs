using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Sections.Models;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public class ShoppingListFactory : IShoppingListFactory
    {
        public IShoppingList Create(ShoppingListId id, IStore store, IEnumerable<ISection> sections,
            DateTime? completionDate)
        {
            return new ShoppingList(id, store, sections, completionDate);
        }

        public IShoppingList Create(IStore store, IEnumerable<ISection> sections,
            DateTime? completionDate)
        {
            return new ShoppingList(new ShoppingListId(0), store, sections, completionDate);
        }
    }
}