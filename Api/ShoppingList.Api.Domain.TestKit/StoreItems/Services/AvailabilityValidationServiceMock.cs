using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Validations;
using ShoppingList.Api.TestTools.Extensions;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Services;

public class AvailabilityValidationServiceMock : Mock<IAvailabilityValidationService>
{
    public AvailabilityValidationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void VerifyValidateOnce(IEnumerable<IItemAvailability> availabilities)
    {
        Verify(i => i.ValidateAsync(
                availabilities,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    public void SetupValidateAsync(IEnumerable<IItemAvailability> availabilities)
    {
        Setup(m => m.ValidateAsync(
                It.Is<IEnumerable<IItemAvailability>>(avs => avs.IsEquivalentTo(availabilities)),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }
}