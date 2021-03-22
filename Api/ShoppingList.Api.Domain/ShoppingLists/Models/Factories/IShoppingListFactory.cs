using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public interface IShoppingListFactory
    {
        IShoppingList Create(ShoppingListId id, StoreId storeId, DateTime? completionDate,
            IEnumerable<IShoppingListSection> sections);

        IShoppingList CreateNew(StoreId storeId, IEnumerable<IShoppingListSection> sections);
    }
}