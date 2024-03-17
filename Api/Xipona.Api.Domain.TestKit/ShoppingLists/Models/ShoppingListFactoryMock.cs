using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Models;

public class ShoppingListFactoryMock : Mock<IShoppingListFactory>
{
    public ShoppingListFactoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupCreateNew(IStore store, IShoppingList returnValue)
    {
        Setup(i => i.CreateNew(
                It.Is<IStore>(s => s == store)))
            .Returns(returnValue);
    }
}