using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Services.Deletions;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Stores.Services.Deletions;

public class StoreDeletionServiceMock : Mock<IStoreDeletionService>
{
    public StoreDeletionServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupDeleteAsync(StoreId storeId)
    {
        Setup(x => x.DeleteAsync(storeId)).Returns(Task.CompletedTask);
    }

    public void VerifyDeleteAsync(StoreId storeId, Func<Times> times)
    {
        Verify(x => x.DeleteAsync(storeId), times);
    }
}