using Moq;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions
{
    public static class StoreItemAvailabilityFactoryMockExtensions
    {
        public static void SetupCreate(this Mock<IStoreItemAvailabilityFactory> mock, IStoreItemStore store,
            float price, IStoreItemSection section, IStoreItemAvailability returnValue)
        {
            mock.Setup(i => i.Create(
                        It.Is<IStoreItemStore>(s => s == store),
                        It.Is<float>(p => p == price),
                        It.Is<IStoreItemSection>(sec => sec == section)))
                    .Returns(returnValue);
        }

        public static void SetupCreate(this Mock<IStoreItemAvailabilityFactory> mock, IStore store,
            float price, IStoreItemSection section, IStoreItemAvailability returnValue)
        {
            mock.Setup(i => i.Create(
                        It.Is<IStore>(s => s == store),
                        It.Is<float>(p => p == price),
                        It.Is<IStoreItemSection>(sec => sec == section)))
                    .Returns(returnValue);
        }

        public static void VerifyCreateOnce(this Mock<IStoreItemAvailabilityFactory> mock, IStore store,
            float price, IStoreItemSection section)
        {
            mock.Verify(i => i.Create(
                    It.Is<IStore>(s => s == store),
                    It.Is<float>(p => p == price),
                    It.Is<IStoreItemSection>(s => s == section)),
                Times.Once);
        }
    }
}