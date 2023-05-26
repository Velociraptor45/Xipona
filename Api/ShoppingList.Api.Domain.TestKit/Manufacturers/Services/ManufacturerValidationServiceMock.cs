using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Manufacturers.Services;

public class ManufacturerValidationServiceMock : Mock<IManufacturerValidationService>
{
    public ManufacturerValidationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void VerifyValidateAsyncOnce(ManufacturerId manufacturerId)
    {
        Verify(i => i.ValidateAsync(
                manufacturerId),
            Times.Once);
    }

    public void VerifyValidateAsyncNever()
    {
        Verify(i => i.ValidateAsync(
                It.IsAny<ManufacturerId>()),
            Times.Never);
    }

    public void SetupValidateAsync(ManufacturerId manufacturerId)
    {
        Setup(m => m.ValidateAsync(manufacturerId))
            .Returns(Task.CompletedTask);
    }
}