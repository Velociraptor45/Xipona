using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Mocks;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures
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
            var shoppingLists = shoppingListFixture.CreateManyValid(amount);
            return shoppingLists.Select(list => new ShoppingListMock(list));
        }
    }
}