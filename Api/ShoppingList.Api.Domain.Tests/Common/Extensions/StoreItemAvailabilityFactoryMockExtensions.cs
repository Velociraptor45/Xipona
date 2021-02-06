using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions
{
    public static class StoreItemAvailabilityFactoryMockExtensions
    {
        public static void SetupMultipleCreate(this Mock<IStoreItemAvailabilityFactory> mock,
            IEnumerable<IStoreItemAvailability> returnValue)
        {
            foreach (var availability in returnValue)
            {
                mock.SetupCreate(availability);
            }
        }

        public static void SetupCreate(this Mock<IStoreItemAvailabilityFactory> mock,
            IStoreItemAvailability returnValue)
        {
            mock.Setup(i => i.Create(
                        It.Is<StoreId>(id => id == returnValue.StoreId),
                        It.Is<float>(price => price == returnValue.Price),
                        It.Is<IStoreItemSection>(section => section == returnValue.DefaultSection)))
                    .Returns(returnValue);
        }
    }
}