using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Deletions;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Services.Deletions;

public class ShoppingListDeletionServiceMock : Mock<IShoppingListDeletionService>
{
    public ShoppingListDeletionServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupHardDeleteForStoreAsync(StoreId storeId)
    {
        Setup(x => x.HardDeleteForStoreAsync(storeId))
            .Returns(Task.CompletedTask);
    }

    public void VerifyHardDeleteForStoreAsync(StoreId storeId, Func<Times> times)
    {
        Verify(x => x.HardDeleteForStoreAsync(storeId), times);
    }
}