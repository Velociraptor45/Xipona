using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Services.Queries;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Stores.Services.Queries;

public class StoreQueryServiceMock : Mock<IStoreQueryService>
{
    public StoreQueryServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupGetActiveAsync(IEnumerable<IStore> returnValue)
    {
        Setup(m => m.GetActiveAsync())
            .ReturnsAsync(returnValue);
    }

    public void SetupGetActiveAsync(StoreId storeId, IStore returnValue)
    {
        Setup(m => m.GetActiveAsync(storeId))
            .ReturnsAsync(returnValue);
    }
}