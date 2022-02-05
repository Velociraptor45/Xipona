using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;

namespace ShoppingList.Api.Domain.TestKit.Manufacturers.Ports;

public class ManufacturerRepositoryMock : Mock<IManufacturerRepository>
{
    public ManufacturerRepositoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupFindByAsync(ManufacturerId manufacturerId, IManufacturer returnValue)
    {
        Setup(i => i.FindByAsync(
                It.Is<ManufacturerId>(id => id == manufacturerId),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(returnValue));
    }

    public void SetupFindByAsync(IEnumerable<ManufacturerId> manufacturerIds, IEnumerable<IManufacturer> returnValue)
    {
        Setup(i => i.FindByAsync(
                It.Is<IEnumerable<ManufacturerId>>(ids => ids.SequenceEqual(manufacturerIds)),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(returnValue));
    }
}