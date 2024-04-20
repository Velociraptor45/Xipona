using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Validations;
using ProjectHermes.Xipona.Api.TestTools.Extensions;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services;

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