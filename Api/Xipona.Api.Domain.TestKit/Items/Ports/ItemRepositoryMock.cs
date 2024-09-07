using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.TestTools.Extensions;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Items.Ports;

public class ItemRepositoryMock : Mock<IItemRepository>
{
    public ItemRepositoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupFindByAsync(IEnumerable<ItemId> itemIds, IEnumerable<IItem> returnValue)
    {
        Setup(i => i.FindByAsync(
                It.Is<IEnumerable<ItemId>>(ids => ids.SequenceEqual(itemIds))))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(IEnumerable<ItemId> itemIds, IEnumerable<IItem> returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                It.Is<IEnumerable<ItemId>>(ids => ids.IsEquivalentTo(itemIds))))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(TemporaryItemId temporaryItemId, IItem? returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                temporaryItemId))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(ItemId itemId, IItem? returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                itemId))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(ItemCategoryId itemCategoryId, IEnumerable<IItem> returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                It.Is<ItemCategoryId>(id => id == itemCategoryId)))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(string name, StoreId storeId, IEnumerable<ItemId> excludedItemIds, int? limit,
        IEnumerable<IItem> returnValue)
    {
        Setup(m => m.FindActiveByAsync(name, storeId, excludedItemIds, limit))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(SectionId sectionId, IEnumerable<IItem> returnValue)
    {
        Setup(m => m.FindActiveByAsync(sectionId)).ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(StoreId storeId, IEnumerable<IItem> returnValue)
    {
        Setup(m => m.FindActiveByAsync(storeId)).ReturnsAsync(returnValue);
    }

    public void VerifyFindByAsync(TemporaryItemId temporaryItemId)
    {
        Verify(
            i => i.FindActiveByAsync(
                It.Is<TemporaryItemId>(id => id == temporaryItemId)),
            Times.Once);
    }

    public void VerifyStoreAsyncOnce(IItem item)
    {
        Verify(
            i => i.StoreAsync(
                It.Is<IItem>(it => it == item)),
            Times.Once);
    }

    public void VerifyStoreAsyncNever()
    {
        Verify(
            i => i.StoreAsync(
                It.IsAny<IItem>()),
            Times.Never);
    }

    public void SetupStoreAsync(IItem item, IItem returnValue)
    {
        Setup(m => m.StoreAsync(item))
            .ReturnsAsync(returnValue);
    }

    public void VerifyStoreAsync(IItem item, Func<Times> times)
    {
        Verify(m => m.StoreAsync(item), times);
    }

    public void SetupGetTotalCountByAsync(string searchInput, int returnValue)
    {
        Setup(m => m.GetTotalCountByAsync(searchInput)).ReturnsAsync(returnValue);
    }
}