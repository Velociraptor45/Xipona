using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;

public class ShoppingListRepositoryMock : Mock<IShoppingListRepository>
{
    public ShoppingListRepositoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupFindActiveByAsync(ItemId storeItemId,
        IEnumerable<IShoppingList> returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                It.Is<ItemId>(id => id == storeItemId),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(returnValue));
    }

    public void SetupFindActiveByAsync(
        IEnumerable<IShoppingList> returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                It.IsAny<ItemId>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(StoreId storeId, IShoppingList returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                It.Is<StoreId>(id => id == storeId),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(returnValue));
    }

    public void SetupFindByAsync(ShoppingListId shoppingListId,
        IShoppingList returnValue)
    {
        Setup(instance => instance.FindByAsync(
                It.Is<ShoppingListId>(id => id == shoppingListId),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(returnValue));
    }

    public void SetupFindByAsync(ItemTypeId itemTypeId, IEnumerable<IShoppingList> returnValue)
    {
        Setup(m => m.FindByAsync(itemTypeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void VerifyStoreAsyncOnce(IShoppingList shoppingList)
    {
        Verify(
            i => i.StoreAsync(
                It.Is<IShoppingList>(list => list == shoppingList),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    public void VerifyStoreAsyncNever()
    {
        Verify(
            i => i.StoreAsync(
                It.IsAny<IShoppingList>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    public void VerifyStoreAsync(IShoppingList shoppingList, Func<Times> times)
    {
        Verify(
            i => i.StoreAsync(
                shoppingList,
                It.IsAny<CancellationToken>()),
            times);
    }

    public void SetupStoreAsync(IShoppingList shoppingList)
    {
        Setup(m => m.StoreAsync(shoppingList, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }
}