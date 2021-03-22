using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Mocks
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

        public void SetupCreate(IStoreItemStore store, float price, StoreItemSectionId sectionId,
            IStoreItemAvailability returnValue)
        {
            mock.Setup(i => i.Create(
                        It.Is<IStoreItemStore>(s => s == store),
                        It.Is<float>(p => p == price),
                        It.Is<StoreItemSectionId>(id => id == sectionId)))
                    .Returns(returnValue);
        }

        public void SetupCreate(IStore store, float price, StoreItemSectionId sectionId,
            IStoreItemAvailability returnValue)
        {
            mock.Setup(i => i.Create(
                        It.Is<IStore>(s => s == store),
                        It.Is<float>(p => p == price),
                        It.Is<StoreItemSectionId>(id => id == sectionId)))
                    .Returns(returnValue);
        }

        public void SetupCreate(IStore store, float price, SectionId sectionId, IStoreItemAvailability returnValue)
        {
            mock.Setup(i => i.Create(
                        It.Is<IStore>(s => s == store),
                        It.Is<float>(p => p == price),
                        It.Is<SectionId>(id => id == sectionId)))
                    .Returns(returnValue);
        }

        public void VerifyCreateOnce(IStore store, float price, StoreItemSectionId sectionId)
        {
            mock.Verify(i => i.Create(
                    It.Is<IStore>(s => s == store),
                    It.Is<float>(p => p == price),
                    It.Is<StoreItemSectionId>(id => id == sectionId)),
                Times.Once);
        }

        public void VerifyCreateOnce(IStore store, float price, SectionId sectionId)
        {
            mock.Verify(i => i.Create(
                    It.Is<IStore>(s => s == store),
                    It.Is<float>(p => p == price),
                    It.Is<SectionId>(id => id == sectionId)),
                Times.Once);
        }
    }
}