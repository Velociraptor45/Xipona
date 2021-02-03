using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Sections.Models;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public interface IShoppingListFactory
    {
        IShoppingList Create(ShoppingListId id, IStore store, IEnumerable<ISection> sections, DateTime? completionDate);

        IShoppingList Create(IStore store, IEnumerable<ISection> sections, DateTime? completionDate);
    }
}