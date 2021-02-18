using Moq;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions
{
    public static class StoreItemAvailabilityFactoryMockExtensions
    {
        public static void SetupCreate(this Mock<IStoreItemAvailabilityFactory> mock, StoreItemStoreId storeId,
            float price, IStoreItemSection section, IStoreItemAvailability returnValue)
        {
            mock.Setup(i => i.Create(
                        It.Is<StoreItemStoreId>(id => id == storeId),
                        It.Is<float>(p => p == price),
                        It.Is<IStoreItemSection>(sec => sec == section)))
                    .Returns(returnValue);
        }
    }
}