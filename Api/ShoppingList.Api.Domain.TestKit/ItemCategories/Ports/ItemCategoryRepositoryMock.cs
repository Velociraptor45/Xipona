using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.ItemCategories.Ports;

public class ItemCategoryRepositoryMock : Mock<IItemCategoryRepository>
{
    public ItemCategoryRepositoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupFindByAsync(ItemCategoryId itemCategoryId, IItemCategory? returnValue)
    {
        Setup(i => i.FindByAsync(
                It.Is<ItemCategoryId>(id => id == itemCategoryId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindByAsync(IEnumerable<ItemCategoryId> itemCategoryIds, IEnumerable<IItemCategory> returnValue)
    {
        Setup(i => i.FindByAsync(
                It.Is<IEnumerable<ItemCategoryId>>(ids => ids.SequenceEqual(itemCategoryIds)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(ItemCategoryId itemCategoryId, IItemCategory? returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                itemCategoryId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void VerifyStoreAsyncOnce(IItemCategory itemCategory)
    {
        Verify(i => i.StoreAsync(
                It.Is<IItemCategory>(cat => cat == itemCategory),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    public void SetupStoreAsync(IItemCategory itemCategory, IItemCategory returnValue)
    {
        Setup(m => m.StoreAsync(itemCategory, It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void VerifyStoreAsync(IItemCategory itemCategory, Func<Times> times)
    {
        Verify(m => m.StoreAsync(itemCategory, It.IsAny<CancellationToken>()), times);
    }

    public void SetupFindByAsync(string searchInput, bool includeDeleted, int? limit, IEnumerable<IItemCategory> returnValue)
    {
        Setup(m => m.FindByAsync(searchInput, includeDeleted, limit, It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }
}