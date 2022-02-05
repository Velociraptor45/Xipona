using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Ports;

public class ItemRepositoryMock : Mock<IItemRepository>
{
    public ItemRepositoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupFindByAsync(ItemId itemId, IStoreItem returnValue)
    {
        Setup(i => i.FindByAsync(
                It.Is<ItemId>(id => id == itemId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindByAsync(IEnumerable<ItemId> itemIds, IEnumerable<IStoreItem> returnValue)
    {
        Setup(i => i.FindByAsync(
                It.Is<IEnumerable<ItemId>>(ids => ids.SequenceEqual(itemIds)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindByAsync(TemporaryItemId temporaryItemId, IStoreItem returnValue)
    {
        Setup(i => i.FindByAsync(
                It.Is<TemporaryItemId>(id => id == temporaryItemId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(ItemCategoryId itemCategoryId, IEnumerable<IStoreItem> returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                It.Is<ItemCategoryId>(id => id == itemCategoryId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void VerifyFindByAsync(ItemId storeItemId)
    {
        Verify(
            i => i.FindByAsync(
                It.Is<ItemId>(id => id == storeItemId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    public void VerifyFindByAsync(TemporaryItemId temporaryItemId)
    {
        Verify(
            i => i.FindByAsync(
                It.Is<TemporaryItemId>(id => id == temporaryItemId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    public void VerifyStoreAsyncOnce(IStoreItem storeItem)
    {
        Verify(
            i => i.StoreAsync(
                It.Is<IStoreItem>(item => item == storeItem),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    public void VerifyStoreAsyncNever()
    {
        Verify(
            i => i.StoreAsync(
                It.IsAny<IStoreItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    public void SetupStoreAsync(IStoreItem item, IStoreItem returnValue)
    {
        Setup(m => m.StoreAsync(item, It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void VerifyStoreAsync(IStoreItem item, Func<Times> times)
    {
        Verify(m => m.StoreAsync(item, It.IsAny<CancellationToken>()), times);
    }

    public void SetupFindActiveByAsync(string name, StoreId storeId, IEnumerable<IStoreItem> returnValue)
    {
        Setup(m => m.FindActiveByAsync(name, storeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }
}