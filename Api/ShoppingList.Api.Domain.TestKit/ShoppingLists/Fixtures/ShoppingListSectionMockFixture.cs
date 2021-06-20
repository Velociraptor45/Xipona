using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures
{
    public class ShoppingListSectionMockFixture
    {
        private readonly ShoppingListSectionFixture shoppingListSectionFixture;
        private readonly CommonFixture commonFixture;

        public ShoppingListSectionMockFixture(ShoppingListSectionFixture shoppingListSectionFixture,
            CommonFixture commonFixture)
        {
            this.shoppingListSectionFixture = shoppingListSectionFixture;
            this.commonFixture = commonFixture;
        }

        public ShoppingListSectionMock Create()
        {
            return CreateMany(1).First();
        }

        public IEnumerable<ShoppingListSectionMock> CreateMany(int count)
        {
            var sections = shoppingListSectionFixture.CreateManyValid(count);
            return sections.Select(s => new ShoppingListSectionMock(s));
        }
    }
}