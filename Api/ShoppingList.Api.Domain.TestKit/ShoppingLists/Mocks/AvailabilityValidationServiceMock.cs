using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;
using System.Collections.Generic;
using System.Threading;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Mocks
{
    public class AvailabilityValidationServiceMock
    {
        private readonly Mock<IAvailabilityValidationService> mock;

        public AvailabilityValidationServiceMock(Mock<IAvailabilityValidationService> mock)
        {
            this.mock = mock;
        }

        public AvailabilityValidationServiceMock(Fixture fixture)
        {
            mock = fixture.Freeze<Mock<IAvailabilityValidationService>>();
        }

        public void VerifyValidateOnce(IEnumerable<IStoreItemAvailability> availabilities)
        {
            mock.Verify(i => i.Validate(
                    availabilities,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}