using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions
{
    public static class ManufacturerRepositoryMockExtensions
    {
        public static void SetupFindByAsync(this Mock<IManufacturerRepository> mock, ManufacturerId manufacturerId,
            IManufacturer returnValue)
        {
            mock
                .Setup(i => i.FindByAsync(
                    It.Is<ManufacturerId>(id => id == manufacturerId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnValue));
        }
    }
}