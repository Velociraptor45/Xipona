using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Deletions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Services.Deletions;

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