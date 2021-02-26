using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;

namespace ShoppingList.Api.Domain.TestKit.Shared.Mocks
{
    public class StoreItemAvailabilityFactoryMock
    {
        private readonly Mock<IStoreItemAvailabilityFactory> mock;

        public StoreItemAvailabilityFactoryMock(Mock<IStoreItemAvailabilityFactory> mock)
        {
            this.mock = mock;
        }

        public StoreItemAvailabilityFactoryMock(Fixture fixture)
        {
            mock = fixture.Freeze<Mock<IStoreItemAvailabilityFactory>>();
        }

        public void SetupCreate(IStoreItemStore store, float price, IStoreItemSection section,
            IStoreItemAvailability returnValue)
        {
            mock.Setup(i => i.Create(
                        It.Is<IStoreItemStore>(s => s == store),
                        It.Is<float>(p => p == price),
                        It.Is<IStoreItemSection>(sec => sec == section)))
                    .Returns(returnValue);
        }

        public void SetupCreate(IStore store, float price, IStoreItemSection section,
            IStoreItemAvailability returnValue)
        {
            mock.Setup(i => i.Create(
                        It.Is<IStore>(s => s == store),
                        It.Is<float>(p => p == price),
                        It.Is<IStoreItemSection>(sec => sec == section)))
                    .Returns(returnValue);
        }

        public void SetupCreate(IStore store, float price, IStoreSection section, IStoreItemAvailability returnValue)
        {
            mock.Setup(i => i.Create(
                        It.Is<IStore>(s => s == store),
                        It.Is<float>(p => p == price),
                        It.Is<IStoreSection>(sec => sec == section)))
                    .Returns(returnValue);
        }

        public void VerifyCreateOnce(IStore store, float price, IStoreItemSection section)
        {
            mock.Verify(i => i.Create(
                    It.Is<IStore>(s => s == store),
                    It.Is<float>(p => p == price),
                    It.Is<IStoreItemSection>(s => s == section)),
                Times.Once);
        }

        public void VerifyCreateOnce(IStore store, float price, IStoreSection section)
        {
            mock.Verify(i => i.Create(
                    It.Is<IStore>(s => s == store),
                    It.Is<float>(p => p == price),
                    It.Is<IStoreSection>(s => s == section)),
                Times.Once);
        }
    }
}