using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;

namespace ShoppingList.Api.Domain.TestKit.Stores.Ports;

public class StoreRepositoryMock : Mock<IStoreRepository>
{
    public StoreRepositoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupGetAsync(IEnumerable<IStore> returnValue)
    {
        Setup(i => i.GetAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindByAsync(StoreId storeId, IStore returnValue)
    {
        Setup(i => i.FindByAsync(
                storeId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindByAsync(IEnumerable<StoreId> storeIds, IEnumerable<IStore> returnValue)
    {
        Setup(i => i.FindByAsync(
                It.Is<IEnumerable<StoreId>>(ids => ids.SequenceEqual(storeIds)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(StoreId storeId, IStore returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                storeId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void VerifyFindActiveByAsyncOnce(StoreId storeId)
    {
        Verify(i => i.FindActiveByAsync(
                storeId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    public void VerifyFindByAsyncOnce(StoreId storeId)
    {
        Verify(i => i.FindByAsync(
                storeId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}