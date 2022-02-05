using System.Collections.Generic;
using System.Linq;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;

public class ShoppingListSectionMockFixture
{
    public ShoppingListSectionMock Create()
    {
        return CreateMany(1).First();
    }

    public IEnumerable<ShoppingListSectionMock> CreateMany(int count)
    {
        var sections = new ShoppingListSectionBuilder().CreateMany(count);
        return sections.Select(s => new ShoppingListSectionMock(s));
    }
}