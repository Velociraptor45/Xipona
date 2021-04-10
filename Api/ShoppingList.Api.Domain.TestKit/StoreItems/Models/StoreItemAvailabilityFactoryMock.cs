using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Models
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

        public void SetupCreate(StoreId storeId, float price, SectionId sectionId,
            IStoreItemAvailability returnValue)
        {
            mock.Setup(i => i.Create(
                        It.Is<StoreId>(id => id == storeId),
                        It.Is<float>(p => p == price),
                        It.Is<SectionId>(id => id == sectionId)))
                    .Returns(returnValue);
        }

        public void VerifyCreateOnce(StoreId storeId, float price, SectionId sectionId)
        {
            mock.Verify(i => i.Create(
                    It.Is<StoreId>(id => id == storeId),
                    It.Is<float>(p => p == price),
                    It.Is<SectionId>(id => id == sectionId)),
                Times.Once);
        }
    }
}