using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System.Collections.Generic;
using System.Linq;

using Models = ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures
{
    public class ShoppingListFixture
    {
        private readonly ShoppingListItemFixture shoppingListItemFixture;
        private readonly CommonFixture commonFixture;

        public ShoppingListFixture(ShoppingListItemFixture shoppingListItemFixture, CommonFixture commonFixture)
        {
            this.shoppingListItemFixture = shoppingListItemFixture;
            this.commonFixture = commonFixture;
        }

        public IShoppingList GetShoppingList(int itemCount = 3,
            IEnumerable<IShoppingListItem> additionalItems = null)
        {
            var listId = new ShoppingListId(commonFixture.NextInt());
            var storeId = new StoreId(commonFixture.NextInt());
            return GetShoppingList(listId, storeId, itemCount, additionalItems);
        }

        public IShoppingList GetShoppingList(StoreId storeId, int itemCount = 3,
            IEnumerable<IShoppingListItem> additionalItems = null)
        {
            var listId = new ShoppingListId(commonFixture.NextInt());
            return GetShoppingList(listId, storeId, itemCount, additionalItems);
        }

        public IShoppingList GetShoppingList(ShoppingListId id, StoreId storeId = null, int itemCount = 3,
            IEnumerable<IShoppingListItem> additionalItems = null)
        {
            var allItems = additionalItems?.ToList() ?? new List<IShoppingListItem>();
            var additionalItemIds = allItems.Select(i => i.Id.Actual.Value);
            var uniqueItems = GetUniqueShoppingListItems(itemCount, additionalItemIds);
            allItems.AddRange(uniqueItems);
            allItems.Shuffle();

            var fixture = commonFixture.GetNewFixture();
            fixture.Inject(id);

            if (storeId != null)
                fixture.Inject(storeId);

            fixture.Inject(allItems.AsEnumerable());
            return fixture.Create<Models.ShoppingList>();
        }

        private IEnumerable<IShoppingListItem> GetUniqueShoppingListItems(int count, IEnumerable<int> exclude)
        {
            List<int> itemIds = commonFixture.NextUniqueInts(count, exclude).ToList();
            List<IShoppingListItem> items = new List<IShoppingListItem>();

            foreach (var itemId in itemIds)
            {
                var item = shoppingListItemFixture.GetShoppingListItemWithId(itemId);
                items.Add(item);
            }
            return items;
        }
    }
}