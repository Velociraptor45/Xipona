using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ShoppingList.Api.Domain.TestKit.Items.Ports;

public class ItemRepositoryMock : Mock<IItemRepository>
{
    public ItemRepositoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupFindByAsync(ItemId itemId, IItem? returnValue)
    {
        Setup(i => i.FindByAsync(
                It.Is<ItemId>(id => id == itemId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindByAsync(IEnumerable<ItemId> itemIds, IEnumerable<IItem> returnValue)
    {
        Setup(i => i.FindByAsync(
                It.Is<IEnumerable<ItemId>>(ids => ids.SequenceEqual(itemIds)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindByAsync(TemporaryItemId temporaryItemId, IItem? returnValue)
    {
        Setup(i => i.FindByAsync(
                It.Is<TemporaryItemId>(id => id == temporaryItemId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(ItemCategoryId itemCategoryId, IEnumerable<IItem> returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                It.Is<ItemCategoryId>(id => id == itemCategoryId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(string name, StoreId storeId, IEnumerable<IItem> returnValue)
    {
        Setup(m => m.FindActiveByAsync(name, storeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void VerifyFindByAsync(TemporaryItemId temporaryItemId)
    {
        Verify(
            i => i.FindByAsync(
                It.Is<TemporaryItemId>(id => id == temporaryItemId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    public void VerifyStoreAsyncOnce(IItem storeItem)
    {
        Verify(
            i => i.StoreAsync(
                It.Is<IItem>(item => item == storeItem),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    public void VerifyStoreAsyncNever()
    {
        Verify(
            i => i.StoreAsync(
                It.IsAny<IItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    public void SetupStoreAsync(IItem item, IItem returnValue)
    {
        Setup(m => m.StoreAsync(item, It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void VerifyStoreAsync(IItem item, Func<Times> times)
    {
        Verify(m => m.StoreAsync(item, It.IsAny<CancellationToken>()), times);
    }
}