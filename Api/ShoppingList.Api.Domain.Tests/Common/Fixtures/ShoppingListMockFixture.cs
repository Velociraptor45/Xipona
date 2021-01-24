using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Mocks;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures
{
    public class ShoppingListMockFixture
    {
        private readonly CommonFixture commonFixture;
        private readonly ShoppingListFixture shoppingListFixture;

        public ShoppingListMockFixture(CommonFixture commonFixture, ShoppingListFixture shoppingListFixture)
        {
            this.commonFixture = commonFixture;
            this.shoppingListFixture = shoppingListFixture;
        }

        public ShoppingListMock Create()
        {
            return CreateMany(1).First();
        }

        public IEnumerable<ShoppingListMock> CreateMany(int amount)
        {
            var uniqueIds = commonFixture.NextUniqueInts(amount);

            foreach (var id in uniqueIds)
            {
                var shoppingList = shoppingListFixture.GetShoppingList(new ShoppingListId(id));
                yield return new ShoppingListMock(shoppingList);
            }
        }
    }
}