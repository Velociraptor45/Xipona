using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Mocks
{
    public class StoreItemFactoryMock
    {
        private readonly Mock<IStoreItemFactory> mock;

        public StoreItemFactoryMock(Mock<IStoreItemFactory> mock)
        {
            this.mock = mock;
        }

        public StoreItemFactoryMock(Fixture fixture)
        {
            mock = fixture.Freeze<Mock<IStoreItemFactory>>();
        }

        public void SetupCreate(TemporaryItemCreation temporaryItemCreation, IStoreItem returnValue)
        {
            mock
                .Setup(i => i.Create(
                    It.Is<TemporaryItemCreation>(obj => obj == temporaryItemCreation)))
                .Returns(returnValue);
        }

        public void SetupCreate(ItemCreation itemCreation, IStoreItem returnValue)
        {
            mock
                .Setup(i => i.Create(
                    It.Is<ItemCreation>(c => c == itemCreation)))
                .Returns(returnValue);
        }
    }
}