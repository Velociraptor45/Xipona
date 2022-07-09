using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Items.Services.ItemDeletions;

public class HandleAsyncWithShoppingListsTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var shoppingListMockFixture = new ShoppingListMockFixture();

        yield return new object[]
        {
            shoppingListMockFixture.Create().ToMonoList()
        };
        yield return new object[]
        {
            shoppingListMockFixture.CreateMany(3).ToList()
        };
        yield return new object[]
        {
            new List<ShoppingListMock>()
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}