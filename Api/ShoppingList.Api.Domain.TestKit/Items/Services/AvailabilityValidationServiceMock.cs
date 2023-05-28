using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Validations;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services;

public class AvailabilityValidationServiceMock : Mock<IAvailabilityValidationService>
{
    public AvailabilityValidationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void VerifyValidateOnce(IEnumerable<IItemAvailability> availabilities)
    {
        Verify(i => i.ValidateAsync(
                availabilities),
            Times.Once);
    }

    public void SetupValidateAsync(IEnumerable<IItemAvailability> availabilities)
    {
        Setup(m => m.ValidateAsync(
                It.Is<IEnumerable<IItemAvailability>>(avs => avs.IsEquivalentTo(availabilities))))
            .Returns(Task.CompletedTask);
    }
}