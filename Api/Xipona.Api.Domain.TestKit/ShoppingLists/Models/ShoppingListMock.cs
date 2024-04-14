﻿using ProjectHermes.Xipona.Api.Core.Services;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Shared;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Models;

public class ShoppingListMock : Mock<IShoppingList>
{
    public ShoppingListMock(IShoppingList shoppingList, MockBehavior behavior = MockBehavior.Default)
        : base(behavior)
    {
        SetupId(shoppingList.Id);
        SetupStoreId(shoppingList.StoreId);
        SetupItems(shoppingList.Items);
        SetupSections(shoppingList.Sections);
    }

    public ShoppingListItem GetRandomItem(CommonFixture commonFixture)
    {
        return commonFixture.ChooseRandom(Object.Items);
    }

    public ShoppingListItem GetRandomItem(CommonFixture commonFixture, Func<ShoppingListItem, bool> condition)
    {
        var filteredItems = Object.Items.Where(condition);

        return commonFixture.ChooseRandom(filteredItems);
    }

    #region Setup properties

    public void SetupId(ShoppingListId returnValue)
    {
        Setup(i => i.Id)
            .Returns(returnValue);
    }

    public void SetupStoreId(StoreId returnValue)
    {
        Setup(i => i.StoreId)
            .Returns(returnValue);
    }

    public void SetupItems(IReadOnlyCollection<ShoppingListItem> items)
    {
        Setup(i => i.Items)
            .Returns(items);
    }

    public void SetupSections(IEnumerable<IShoppingListSection> sections)
    {
        var readonlySections = sections.ToList().AsReadOnly();

        Setup(i => i.Sections)
            .Returns(readonlySections);
    }

    #endregion Setup properties

    #region Setup methods

    public void SetupFinish(DateTimeOffset completionDate, IDateTimeService dateTimeService, IShoppingList returnValue)
    {
        Setup(i => i.Finish(completionDate, dateTimeService))
            .Returns(returnValue);
    }

    public void SetupAddSection(IShoppingListSection section)
    {
        Setup(m => m.AddSection(section));
    }

    public void SetupAddItem(ShoppingListItem item, SectionId sectionId, bool throwIfAlreadyPresent = true)
    {
        Setup(m => m.AddItem(item, sectionId, throwIfAlreadyPresent));
    }

    #endregion Setup methods

    #region Verify methods

    public void VerifyRemoveItemOnce(ItemId itemId)
    {
        Verify(i => i.RemoveItem(itemId),
            Times.Once);
    }

    public void VerifyRemoveItem(ItemId itemId, ItemTypeId? itemTypeId, Func<Times> times)
    {
        Verify(m => m.RemoveItem(itemId, itemTypeId), times);
    }

    public void VerifyRemoveItemNever(ItemId itemId, ItemTypeId? itemTypeId)
    {
        Verify(i => i.RemoveItem(itemId, itemTypeId), Times.Never);
    }

    public void VerifyPutItemInBasketOnce(ItemId itemId)
    {
        Verify(i => i.PutItemInBasket(itemId), Times.Once);
    }

    public void VerifyPutItemInBasket(ItemId itemId, ItemTypeId? typeId, Func<Times> times)
    {
        Verify(i => i.PutItemInBasket(itemId, typeId), times);
    }

    public void VerifyPutItemInBasketNever()
    {
        Verify(i => i.PutItemInBasket(It.IsAny<ItemId>()), Times.Never);
    }

    public void VerifyPutItemInBasketWithTypeIdNever()
    {
        Verify(i => i.PutItemInBasket(It.IsAny<ItemId>(), It.IsAny<ItemTypeId?>()), Times.Never);
    }

    public void VerifyRemoveItemFromBasketOnce(ItemId itemId, ItemTypeId? itemTypeId)
    {
        Verify(i => i.RemoveFromBasket(itemId, itemTypeId), Times.Once);
    }

    public void VerifyAddItemOnce(ShoppingListItem listItem, SectionId sectionId, bool throwIfAlreadyPresent = true)
    {
        Verify(i => i.AddItem(listItem, sectionId, throwIfAlreadyPresent),
            Times.Once);
    }

    public void VerifyAddItemNever()
    {
        Verify(i => i.AddItem(
                It.IsAny<ShoppingListItem>(),
                It.IsAny<SectionId>(),
                It.IsAny<bool>()),
            Times.Never);
    }

    public void VerifyAddSectionOnce(IShoppingListSection section)
    {
        Verify(i => i.AddSection(section),
            Times.Once);
    }

    public void VerifyAddSectionNever()
    {
        Verify(i => i.AddSection(
                It.IsAny<IShoppingListSection>()),
            Times.Never);
    }

    public void VerifyFinish(DateTimeOffset completionDate, IDateTimeService dateTimeService, Func<Times> times)
    {
        Verify(i => i.Finish(completionDate, dateTimeService), times);
    }

    public void VerifyChangeItemQuantityOnce(ItemId itemId, ItemTypeId? itemTypeId, QuantityInBasket quantity)
    {
        Verify(i => i.ChangeItemQuantity(itemId, itemTypeId, quantity),
            Times.Once);
    }

    #endregion Verify methods
}