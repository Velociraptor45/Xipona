﻿using Moq.Language.Flow;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemModifications;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.TemporaryItems;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Validation;
using ShoppingList.Api.Core.TestKit.Extensions;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Models;

public class StoreItemMock : Mock<IStoreItem>
{
    public StoreItemMock(IStoreItem storeItem, MockBehavior behavior) : base(behavior)
    {
        SetupId(storeItem.Id);
        SetupIsTemporary(storeItem.IsTemporary);
        SetupIsDeleted(storeItem.IsDeleted);
        SetupAvailabilities(storeItem.Availabilities);
        SetupItemTypes(storeItem.ItemTypes);
        SetupHasItemTypes(storeItem.HasItemTypes);
        SetupItemCategoryId(storeItem.ItemCategoryId);
        SetupManufacturerId(storeItem.ManufacturerId);
    }

    public bool ModifyWithTypeCalled { get; set; }

    private void SetupItemTypes(IReadOnlyCollection<IItemType> itemTypes)
    {
        Setup(i => i.ItemTypes)
            .Returns(itemTypes);
    }

    private void SetupHasItemTypes(bool hasItemTypes)
    {
        Setup(i => i.HasItemTypes)
            .Returns(hasItemTypes);
    }

    public ISetup<IStoreItem, IReadOnlyCollection<IItemType>> SetupItemTypes()
    {
        return Setup(i => i.ItemTypes);
    }

    public void SetupId(ItemId returnValue)
    {
        Setup(i => i.Id)
            .Returns(returnValue);
    }

    public void SetupIsTemporary(bool returnValue)
    {
        Setup(i => i.IsTemporary)
            .Returns(returnValue);
    }

    public void SetupIsDeleted(bool returnValue)
    {
        Setup(i => i.IsDeleted)
            .Returns(returnValue);
    }

    public void SetupItemCategoryId(ItemCategoryId? itemCategoryId)
    {
        Setup(i => i.ItemCategoryId)
            .Returns(itemCategoryId);
    }

    public void SetupManufacturerId(ManufacturerId? manufacturerId)
    {
        Setup(i => i.ManufacturerId)
            .Returns(manufacturerId);
    }

    public void SetupAvailabilities(IEnumerable<IStoreItemAvailability> returnValue)
    {
        Setup(i => i.Availabilities)
            .Returns(returnValue.ToList().AsReadOnly());
    }

    public void SetupModifyAsync(ItemWithTypesModification modification, IValidator validator)
    {
        Setup(m => m.ModifyAsync(modification, validator))
            .Callback(() => ModifyWithTypeCalled = true)
            .Returns(Task.CompletedTask);
    }

    public void SetupDelete()
    {
        Setup(i => i.Delete());
    }

    #region Verify

    public void VerifyDeleteOnce()
    {
        Verify(i => i.Delete(), Times.Once);
    }

    public void VerifyDeleteNever()
    {
        Verify(i => i.Delete(), Times.Never);
    }

    public void VerifyMakePermanentOnce(PermanentItem permanentItem,
        IEnumerable<IStoreItemAvailability> availabilities)
    {
        Verify(
            i => i.MakePermanent(
                It.Is<PermanentItem>(pi => pi == permanentItem),
                It.Is<IEnumerable<IStoreItemAvailability>>(list => list.SequenceEqual(availabilities))),
            Times.Once);
    }

    public void VerifyModifyAsync(ItemWithTypesModification modification, IValidator validator, Func<Times> times)
    {
        Verify(m => m.ModifyAsync(modification, validator), times);
    }

    #endregion Verify

    public void SetupMakePermanent(PermanentItem permanentItem, IEnumerable<IStoreItemAvailability> availabilities)
    {
        Setup(i => i.MakePermanent(permanentItem,
            It.Is<IEnumerable<IStoreItemAvailability>>(avs => avs.IsEquivalentTo(availabilities))));
    }
}