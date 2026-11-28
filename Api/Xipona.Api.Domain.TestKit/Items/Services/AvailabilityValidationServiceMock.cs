using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Validations;
using Xipona.Api.TestTools.Extensions;

namespace Xipona.Api.Domain.TestKit.Items.Services;

public class AvailabilityValidationServiceMock : Mock<IAvailabilityValidationService>
{
    public AvailabilityValidationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void VerifyValidateOnce(IEnumerable<ItemAvailability> availabilities)
    {
        Verify(i => i.ValidateAsync(
                availabilities),
            Times.Once);
    }

    public void SetupValidateAsync(IEnumerable<ItemAvailability> availabilities)
    {
        Setup(m => m.ValidateAsync(
                It.Is<IEnumerable<ItemAvailability>>(avs => avs.IsEquivalentTo(availabilities))))
            .Returns(Task.CompletedTask);
    }
}