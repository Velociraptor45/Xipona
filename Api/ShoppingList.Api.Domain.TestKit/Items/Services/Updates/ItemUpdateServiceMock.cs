using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Updates;

public class ItemUpdateServiceMock : Mock<IItemUpdateService>
{
    public ItemUpdateServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupUpdateAsync(ItemId itemId, ItemTypeId? itemTypeId, StoreId storeId, Price price)
    {
        Setup(m => m.UpdateAsync(itemId, itemTypeId, storeId, price))
            .Returns(Task.CompletedTask);
    }

    public void VerifyUpdateAsync(ItemId itemId, ItemTypeId? itemTypeId, StoreId storeId, Price price,
        Func<Times> times)
    {
        Verify(m => m.UpdateAsync(itemId, itemTypeId, storeId, price), times);
    }
}