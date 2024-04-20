﻿using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.AddItems;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Services;

public class AddItemToShoppingListServiceMock : Mock<IAddItemToShoppingListService>
{
    public AddItemToShoppingListServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupAddItemAsync(IShoppingList shoppingList,
        ItemId itemId, SectionId? sectionId, QuantityInBasket quantity)
    {
        Setup(m => m.AddItemAsync(
                shoppingList,
                itemId,
                sectionId,
                quantity))
            .Returns(Task.CompletedTask);
    }

    public void SetupAddItemWithTypeAsync(IShoppingList shoppingList, IItem item,
        ItemTypeId typeId, SectionId? sectionId, QuantityInBasket quantity)
    {
        Setup(m => m.AddItemWithTypeAsync(
                shoppingList,
                item,
                typeId,
                sectionId,
                quantity))
            .Returns(Task.CompletedTask);
    }

    public void SetupAddItemWithTypeAsync(ShoppingListId shoppingListId, ItemId itemId,
        ItemTypeId typeId, SectionId? sectionId, QuantityInBasket quantity)
    {
        Setup(m => m.AddItemWithTypeAsync(
                shoppingListId,
                itemId,
                typeId,
                sectionId,
                quantity))
            .Returns(Task.CompletedTask);
    }

    public void SetupAddAsync(IEnumerable<ItemToShoppingListAddition> itemsToAdd)
    {
        Setup(m => m.AddAsync(
                itemsToAdd))
            .Returns(Task.CompletedTask);
    }

    public void SetupAddAsync(ShoppingListId shoppingListId, ItemId itemId, SectionId? sectionId,
        QuantityInBasket quantity)
    {
        Setup(m => m.AddAsync(shoppingListId, itemId, sectionId, quantity))
            .Returns(Task.CompletedTask);
    }

    public void VerifyAddItemWithTypeAsync(IShoppingList shoppingList, IItem item,
        ItemTypeId typeId, SectionId? sectionId, QuantityInBasket quantity, Func<Times> times)
    {
        Verify(m => m.AddItemWithTypeAsync(
                shoppingList,
                item,
                typeId,
                sectionId,
                quantity),
            times);
    }

    public void VerifyAddItemWithTypeAsync(ShoppingListId shoppingListId, ItemId itemId,
        ItemTypeId typeId, SectionId? sectionId, QuantityInBasket quantity, Func<Times> times)
    {
        Verify(m => m.AddItemWithTypeAsync(
                shoppingListId,
                itemId,
                typeId,
                sectionId,
                quantity),
            times);
    }

    public void VerifyAddItemAsyncOnce(IShoppingList shoppingList, ItemId itemId, SectionId? sectionId,
        QuantityInBasket quantity)
    {
        Verify(i => i.AddItemAsync(
                shoppingList,
                itemId,
                sectionId,
                quantity),
            Times.Once);
    }

    public void VerifyAddAsync(IEnumerable<ItemToShoppingListAddition> itemsToAdd, Func<Times> times)
    {
        Verify(m => m.AddAsync(itemsToAdd), times);
    }

    public void VerifyAddAsync(ShoppingListId shoppingListId, ItemId itemId, SectionId? sectionId,
        QuantityInBasket quantity, Func<Times> times)
    {
        Verify(m => m.AddAsync(shoppingListId, itemId, sectionId, quantity), times);
    }
}