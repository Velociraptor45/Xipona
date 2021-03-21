using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.TestKit.Manufacturers.Mocks
{
    public class ManufacturerRepositoryMock
    {
        private readonly Mock<IManufacturerRepository> mock;

        public ManufacturerRepositoryMock(Mock<IManufacturerRepository> mock)
        {
            this.mock = mock;
        }

        public ManufacturerRepositoryMock(Fixture fixture)
        {
            mock = fixture.Freeze<Mock<IManufacturerRepository>>();
        }

        public void SetupFindByAsync(ManufacturerId manufacturerId, IManufacturer returnValue)
        {
            mock
                .Setup(i => i.FindByAsync(
                    It.Is<ManufacturerId>(id => id == manufacturerId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnValue));
        }
    }
}