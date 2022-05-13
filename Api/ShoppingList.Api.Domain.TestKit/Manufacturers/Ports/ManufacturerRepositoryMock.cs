using FluentAssertions.Common;
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
            .ReturnsAsync(returnValue);
    }

    public void SetupFindByAsync(IEnumerable<ManufacturerId> manufacturerIds, IEnumerable<IManufacturer> returnValue)
    {
        Setup(i => i.FindByAsync(
                It.Is<IEnumerable<ManufacturerId>>(ids => ids.IsSameOrEqualTo(manufacturerIds)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupStoreAsync(IManufacturer manufacturer, IManufacturer returnValue)
    {
        Setup(m => m.StoreAsync(manufacturer, It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void VerifyStoreAsync(IManufacturer manufacturer, Func<Times> times)
    {
        Verify(m => m.StoreAsync(manufacturer, It.IsAny<CancellationToken>()), times);
    }
}