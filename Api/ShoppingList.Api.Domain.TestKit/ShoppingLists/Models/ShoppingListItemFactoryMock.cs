using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Models
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

        public void SetupCreate(ItemId itemId, ItemTypeId? itemTypeId, bool isInBasket, float quantity,
            IShoppingListItem returnValue)
        {
            mock
                .Setup(instance => instance.Create(
                    itemId,
                    itemTypeId,
                    isInBasket,
                    quantity))
                .Returns(returnValue);
        }

        public void VerifyCreateOnce(ItemId itemId, ItemTypeId? itemTypeId, bool isInBasket, float quantity)
        {
            mock
                .Verify(i => i.Create(
                        itemId,
                        itemTypeId,
                        isInBasket,
                        quantity),
                    Times.Once);
        }
    }
}