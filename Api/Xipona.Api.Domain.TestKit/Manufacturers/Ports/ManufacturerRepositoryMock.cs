﻿using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Ports;
using ProjectHermes.Xipona.Api.TestTools.Extensions;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Manufacturers.Ports;

public class ManufacturerRepositoryMock : Mock<IManufacturerRepository>
{
    public ManufacturerRepositoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupFindByAsync(ManufacturerId manufacturerId, IManufacturer? returnValue)
    {
        Setup(i => i.FindByAsync(
                manufacturerId))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(ManufacturerId manufacturerId, IManufacturer? returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                manufacturerId))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindByAsync(IEnumerable<ManufacturerId> manufacturerIds, IEnumerable<IManufacturer> returnValue)
    {
        Setup(i => i.FindByAsync(
                It.Is<IEnumerable<ManufacturerId>>(ids => ids.IsEquivalentTo(manufacturerIds))))
            .ReturnsAsync(returnValue);
    }

    public void SetupStoreAsync(IManufacturer manufacturer, IManufacturer returnValue)
    {
        Setup(m => m.StoreAsync(manufacturer))
            .ReturnsAsync(returnValue);
    }

    public void VerifyStoreAsync(IManufacturer manufacturer, Func<Times> times)
    {
        Verify(m => m.StoreAsync(manufacturer), times);
    }
}