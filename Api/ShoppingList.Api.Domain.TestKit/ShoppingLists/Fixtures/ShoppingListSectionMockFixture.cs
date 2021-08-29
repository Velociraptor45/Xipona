using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures
{
    public class ShoppingListSectionMockFixture
    {
        public ShoppingListSectionMock Create()
        {
            return CreateMany(1).First();
        }

        public IEnumerable<ShoppingListSectionMock> CreateMany(int count)
        {
            var sections = new ShoppingListSectionBuilder().CreateMany(3);
            return sections.Select(s => new ShoppingListSectionMock(s));
        }
    }
}