using Moq;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;
using System.Collections.Generic;
using System.Threading;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Services
{
    public class AvailabilityValidationServiceMock : Mock<IAvailabilityValidationService>
    {
        public AvailabilityValidationServiceMock(MockBehavior behavior) : base(behavior)
        {
        }

        public void VerifyValidateOnce(IEnumerable<IStoreItemAvailability> availabilities)
        {
            Verify(i => i.ValidateAsync(
                    availabilities,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}