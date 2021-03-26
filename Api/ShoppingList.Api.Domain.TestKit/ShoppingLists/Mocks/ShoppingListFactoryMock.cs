using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Mocks
{
    public class ShoppingListFactoryMock
    {
        private readonly Mock<IShoppingListFactory> mock;

        public ShoppingListFactoryMock(Mock<IShoppingListFactory> mock)
        {
            this.mock = mock;
        }

        public ShoppingListFactoryMock(Fixture fixture)
        {
            mock = fixture.Freeze<Mock<IShoppingListFactory>>();
        }

        public void SetupCreateNew(IStore store, IShoppingList returnValue)
        {
            mock.Setup(i => i.CreateNew(
                    It.Is<IStore>(s => s == store)))
                .Returns(returnValue);
        }
    }
}