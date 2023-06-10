using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Modifications;

public class ItemModificationServiceMock : Mock<IItemModificationService>
{
    public ItemModificationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupRemoveAvailabilitiesForAsync(StoreId storeId)
    {
        Setup(x => x.RemoveAvailabilitiesForAsync(storeId)).Returns(Task.CompletedTask);
    }

    public void VerifyRemoveAvailabilitiesForAsync(StoreId storeId, Times times)
    {
        Verify(x => x.RemoveAvailabilitiesForAsync(storeId), times);
    }
}