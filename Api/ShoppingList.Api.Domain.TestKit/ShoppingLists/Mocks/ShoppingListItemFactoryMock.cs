using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Mocks
{
    public class ShoppingListItemFactoryMock
    {
        private readonly Mock<IShoppingListItemFactory> mock;

        public ShoppingListItemFactoryMock(Mock<IShoppingListItemFactory> mock)
        {
            this.mock = mock;
        }

        public ShoppingListItemFactoryMock(Fixture fixture)
        {
            mock = fixture.Freeze<Mock<IShoppingListItemFactory>>();
        }

        public void SetupCreate(ItemId itemId, float price, bool isInBasket, float quantity,
            IShoppingListItem returnValue)
        {
            mock
                .Setup(instance => instance.Create(
                    It.Is<ItemId>(id => id == itemId),
                    It.Is<bool>(b => b == isInBasket),
                    It.Is<float>(q => q == quantity)))
                .Returns(returnValue);
        }

        public void VerifyCreateOnce(ItemId itemId, float price, bool isInBasket, float quantity)
        {
            mock
                .Verify(i => i.Create(
                        It.Is<ItemId>(id => id == itemId),
                        It.Is<bool>(inBasket => inBasket == isInBasket),
                        It.Is<float>(q => q == quantity)),
                    Times.Once);
        }
    }
}