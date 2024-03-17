using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Models;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Fixtures;

public class ShoppingListMockFixture
{
    public ShoppingListMock Create()
    {
        return CreateMany(1).First();
    }

    public IEnumerable<ShoppingListMock> CreateMany(int amount)
    {
        var shoppingLists = new ShoppingListBuilder().CreateMany(amount);
        return shoppingLists.Select(list => new ShoppingListMock(list));
    }
}