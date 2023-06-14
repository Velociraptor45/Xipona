using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Ports;

public class StoreRepositoryMock : Mock<IStoreRepository>
{
    public StoreRepositoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupGetAsync(IEnumerable<IStore> returnValue)
    {
        Setup(i => i.GetActiveAsync())
            .ReturnsAsync(returnValue);
    }

    public void SetupFindByAsync(StoreId storeId, IStore? returnValue)
    {
        Setup(i => i.FindByAsync(
                storeId))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(IEnumerable<StoreId> storeIds, IEnumerable<IStore> returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                It.Is<IEnumerable<StoreId>>(ids => ids.SequenceEqual(storeIds))))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(StoreId storeId, IStore? returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                storeId))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(SectionId sectionId, IStore? returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                sectionId))
            .ReturnsAsync(returnValue);
    }

    public void SetupStoreAsync(IStore store, IStore returnValue)
    {
        Setup(m => m.StoreAsync(store))
            .ReturnsAsync(returnValue);
    }

    public void VerifyFindActiveByAsyncOnce(StoreId storeId)
    {
        Verify(i => i.FindActiveByAsync(
                storeId),
            Times.Once);
    }

    public void VerifyFindByAsyncOnce(StoreId storeId)
    {
        Verify(i => i.FindByAsync(
                storeId),
            Times.Once);
    }

    public void VerifyStoreAsync(IStore store, Func<Times> times)
    {
        Verify(m => m.StoreAsync(store), times);
    }
}