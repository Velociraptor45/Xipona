using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;

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