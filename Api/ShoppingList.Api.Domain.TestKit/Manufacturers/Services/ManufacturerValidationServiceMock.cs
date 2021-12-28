using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services;
using System.Threading;

namespace ShoppingList.Api.Domain.TestKit.Manufacturers.Services
{
    public class ManufacturerValidationServiceMock : Mock<IManufacturerValidationService>
    {
        public ManufacturerValidationServiceMock(MockBehavior behavior) : base(behavior)
        {
        }

        public void VerifyValidateAsyncOnce(ManufacturerId manufacturerId)
        {
            Verify(i => i.ValidateAsync(
                    manufacturerId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        public void VerifyValidateAsyncNever()
        {
            Verify(i => i.ValidateAsync(
                    It.IsAny<ManufacturerId>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}