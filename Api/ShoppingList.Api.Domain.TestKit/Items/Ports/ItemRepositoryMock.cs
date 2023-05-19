using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Ports;

public class ItemRepositoryMock : Mock<IItemRepository>
{
    public ItemRepositoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupFindByAsync(IEnumerable<ItemId> itemIds, IEnumerable<IItem> returnValue)
    {
        Setup(i => i.FindByAsync(
                It.Is<IEnumerable<ItemId>>(ids => ids.SequenceEqual(itemIds)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(IEnumerable<ItemId> itemIds, IEnumerable<IItem> returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                It.Is<IEnumerable<ItemId>>(ids => ids.IsEquivalentTo(itemIds)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(TemporaryItemId temporaryItemId, IItem? returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                temporaryItemId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(ItemId itemId, IItem? returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                itemId,
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

    public void SetupFindActiveByAsync(string name, StoreId storeId, IEnumerable<ItemId> excludedItemIds, int? limit,
        IEnumerable<IItem> returnValue)
    {
        Setup(m => m.FindActiveByAsync(name, storeId, excludedItemIds, limit, It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(SectionId sectionId, IEnumerable<IItem> returnValue)
    {
        Setup(m => m.FindActiveByAsync(sectionId, It.IsAny<CancellationToken>())).ReturnsAsync(returnValue);
    }

    public void VerifyFindByAsync(TemporaryItemId temporaryItemId)
    {
        Verify(
            i => i.FindActiveByAsync(
                It.Is<TemporaryItemId>(id => id == temporaryItemId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    public void VerifyStoreAsyncOnce(IItem item)
    {
        Verify(
            i => i.StoreAsync(
                It.Is<IItem>(it => it == item),
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