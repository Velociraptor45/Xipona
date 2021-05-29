using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services;
using System.Threading;

namespace ShoppingList.Api.Domain.TestKit.Manufacturers.Services
{
    public class ManufacturerValidationServiceMock
    {
        private readonly Mock<IManufacturerValidationService> mock;

        public ManufacturerValidationServiceMock(Mock<IManufacturerValidationService> mock)
        {
            this.mock = mock;
        }

        public ManufacturerValidationServiceMock(Fixture fixture)
        {
            mock = fixture.Freeze<Mock<IManufacturerValidationService>>();
        }

        public void VerifyValidateAsyncOnce(ManufacturerId manufacturerId)
        {
            mock.Verify(i => i.ValidateAsync(
                    manufacturerId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        public void VerifyValidateAsyncNever()
        {
            mock.Verify(i => i.ValidateAsync(
                    It.IsAny<ManufacturerId>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}