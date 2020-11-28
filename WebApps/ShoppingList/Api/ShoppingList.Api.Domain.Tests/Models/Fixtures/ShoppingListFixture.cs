using AutoFixture;
using ShoppingList.Api.Domain.Models;
using System.Collections.Generic;
using System.Linq;

using DomainModels = ShoppingList.Api.Domain.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Models.Fixtures
{
    public class ShoppingListFixture
    {
        private readonly ShoppingListItemFixture shoppingListItemFixture;
        private readonly CommonFixture commonFixture;

        public ShoppingListFixture()
        {
            shoppingListItemFixture = new ShoppingListItemFixture();
            commonFixture = new CommonFixture();
        }

        public DomainModels.ShoppingList GetShoppingList(int itemCount, IEnumerable<int> requiredItemIds = null,
            StoreId storeId = null)
        {
            List<ShoppingListItem> items = new List<ShoppingListItem>();
            List<int> requiredItemIdList = requiredItemIds?.ToList() ?? new List<int>();

            for (int i = 0; i < itemCount; i++)
            {
                if (i < requiredItemIdList.Count)
                {
                    int id = requiredItemIdList[i];
                    items.Add(shoppingListItemFixture.GetShoppingListItemWithId(id));
                    continue;
                }

                items.Add(shoppingListItemFixture.GetShoppingListItem());
            }

            var fixture = commonFixture.GetNewFixture();
            fixture.Inject(items.AsEnumerable());
            fixture.Inject(storeId ?? new StoreId(commonFixture.NextInt()));
            return fixture.Create<DomainModels.ShoppingList>();
        }
    }
}