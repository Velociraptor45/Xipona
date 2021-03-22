using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public class ShoppingListFactory : IShoppingListFactory
    {
        public IShoppingList Create(ShoppingListId id, StoreId storeId, DateTime? completionDate,
            IEnumerable<IShoppingListSection> sections)
        {
            return new ShoppingList(id, storeId, completionDate, sections);
        }

        public IShoppingList CreateNew(StoreId storeId, IEnumerable<IShoppingListSection> sections)
        {
            return new ShoppingList(new ShoppingListId(0), storeId, null, sections);
        }
    }
}