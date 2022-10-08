using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Services.Queries;

public class StoreQueryServiceMock : Mock<IStoreQueryService>
{
    public StoreQueryServiceMock(MockBehavior behavior) : base(behavior)
    {
        
    }

    public void SetupGetActiveAsync(IEnumerable<StoreReadModel> returnValue)
    {
        Setup(m => m.GetActiveAsync())
            .ReturnsAsync(returnValue);
    }
}
