using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public class ShoppingListFactory : IShoppingListFactory
    {
        private readonly IShoppingListStoreFactory shoppingListStoreFactory;
        private readonly IShoppingListSectionFactory shoppingListSectionFactory;

        public ShoppingListFactory(IShoppingListStoreFactory shoppingListStoreFactory,
            IShoppingListSectionFactory shoppingListSectionFactory)
        {
            this.shoppingListStoreFactory = shoppingListStoreFactory;
            this.shoppingListSectionFactory = shoppingListSectionFactory;
        }

        public IShoppingList Create(ShoppingListId id, IShoppingListStore store, IEnumerable<IShoppingListSection> sections,
            DateTime? completionDate)
        {
            return new ShoppingList(id, store, sections, completionDate);
        }

        public IShoppingList CreateNew(IStore store)
        {
            IShoppingListStore shoppingListStore = shoppingListStoreFactory.Create(store);
            var sections = store.Sections.Select(s => shoppingListSectionFactory.Create(s, Enumerable.Empty<IShoppingListItem>()));

            return CreateNew(shoppingListStore, sections, null);
        }

        public IShoppingList CreateNew(IShoppingListStore store, IEnumerable<IShoppingListSection> sections,
            DateTime? completionDate)
        {
            return new ShoppingList(new ShoppingListId(0), store, sections, completionDate);
        }
    }
}