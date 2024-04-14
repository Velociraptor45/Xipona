﻿using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Ports;

public class ShoppingListRepositoryMock : Mock<IShoppingListRepository>
{
    public ShoppingListRepositoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupFindActiveByAsync(ItemId itemId, IEnumerable<IShoppingList> returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                It.Is<ItemId>(id => id == itemId)))
            .Returns(Task.FromResult(returnValue));
    }

    public void SetupFindActiveByAsync(IEnumerable<IShoppingList> returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                It.IsAny<ItemId>()))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(StoreId storeId, IShoppingList? returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                It.Is<StoreId>(id => id == storeId)))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindActiveByAsync(IEnumerable<StoreId> storeIds, IEnumerable<IShoppingList> returnValue)
    {
        Setup(i => i.FindActiveByAsync(
                storeIds))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindByAsync(ShoppingListId shoppingListId, IShoppingList? returnValue)
    {
        Setup(instance => instance.FindByAsync(
                It.Is<ShoppingListId>(id => id == shoppingListId)))
            .ReturnsAsync(returnValue);
    }

    public void SetupFindByAsync(ItemTypeId itemTypeId, IEnumerable<IShoppingList> returnValue)
    {
        Setup(m => m.FindByAsync(itemTypeId))
            .ReturnsAsync(returnValue);
    }

    public void VerifyStoreAsyncOnce(IShoppingList shoppingList)
    {
        Verify(
            i => i.StoreAsync(
                It.Is<IShoppingList>(list => list == shoppingList)),
            Times.Once);
    }

    public void VerifyStoreAsyncNever()
    {
        Verify(
            i => i.StoreAsync(
                It.IsAny<IShoppingList>()),
            Times.Never);
    }

    public void VerifyStoreAsync(IShoppingList shoppingList, Func<Times> times)
    {
        Verify(
            i => i.StoreAsync(
                shoppingList),
            times);
    }

    public void SetupStoreAsync(IShoppingList shoppingList)
    {
        Setup(m => m.StoreAsync(shoppingList))
            .Returns(Task.CompletedTask);
    }

    public void SetupDeleteAsync(ShoppingListId shoppingListId)
    {
        Setup(m => m.DeleteAsync(shoppingListId)).Returns(Task.CompletedTask);
    }

    public void VerifyDeleteAsync(ShoppingListId shoppingListId, Func<Times> times)
    {
        Verify(m => m.DeleteAsync(shoppingListId), times);
    }

    public void VerifyDeleteAsyncNever()
    {
        Verify(m => m.DeleteAsync(It.IsAny<ShoppingListId>()), Times.Never);
    }
}