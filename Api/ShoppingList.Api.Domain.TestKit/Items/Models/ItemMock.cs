using Moq.Language.Flow;
using ProjectHermes.ShoppingList.Api.Core.Services;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.TemporaryItems;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;

public class ItemMock : Mock<IItem>
{
    public ItemMock(IItem item, MockBehavior behavior) : base(behavior)
    {
        SetupId(item.Id);
        SetupIsTemporary(item.IsTemporary);
        SetupIsDeleted(item.IsDeleted);
        SetupAvailabilities(item.Availabilities);
        SetupItemTypes(item.ItemTypes);
        SetupHasItemTypes(item.HasItemTypes);
        SetupItemCategoryId(item.ItemCategoryId);
        SetupManufacturerId(item.ManufacturerId);
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

    public ISetup<IItem, IReadOnlyCollection<IItemType>> SetupItemTypes()
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

    public void SetupAvailabilities(IEnumerable<ItemAvailability> returnValue)
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

    public void SetupMakePermanent(PermanentItem permanentItem, IEnumerable<ItemAvailability> availabilities)
    {
        Setup(i => i.MakePermanent(permanentItem,
            It.Is<IEnumerable<ItemAvailability>>(avs => avs.IsEquivalentTo(availabilities))));
    }

    public void SetupUpdate(StoreId storeId, ItemTypeId? itemTypeId, Price price, IDateTimeService dateTimeService,
        IItem returnValue)
    {
        Setup(m => m.Update(storeId, itemTypeId, price, dateTimeService))
            .Returns(returnValue);
    }

    public void SetupUpdateAsync(ItemUpdate update, IValidator validator, IDateTimeService dateTimeService,
        IItem returnValue)
    {
        Setup(m => m.UpdateAsync(update, validator, dateTimeService))
            .ReturnsAsync(returnValue);
    }

    public void SetupUpdateAsync(ItemWithTypesUpdate update, IValidator validator, IDateTimeService dateTimeService,
        IItem returnValue)
    {
        Setup(m => m.UpdateAsync(update, validator, dateTimeService))
            .ReturnsAsync(returnValue);
    }

    public void SetupTransferToDefaultSection(SectionId oldSectionId, SectionId newSectionId)
    {
        Setup(m => m.TransferToDefaultSection(oldSectionId, newSectionId));
    }

    public void SetupRemoveAvailabilitiesFor(StoreId storeId)
    {
        Setup(m => m.RemoveAvailabilitiesFor(storeId));
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
        IEnumerable<ItemAvailability> availabilities)
    {
        Verify(
            i => i.MakePermanent(
                It.Is<PermanentItem>(pi => pi == permanentItem),
                It.Is<IEnumerable<ItemAvailability>>(list => list.SequenceEqual(availabilities))),
            Times.Once);
    }

    public void VerifyModifyAsync(ItemWithTypesModification modification, IValidator validator, Func<Times> times)
    {
        Verify(m => m.ModifyAsync(modification, validator), times);
    }

    public void VerifyUpdate(StoreId storeId, ItemTypeId? itemTypeId, Price price, IDateTimeService dateTimeService,
        Func<Times> times)
    {
        Verify(m => m.Update(storeId, itemTypeId, price, dateTimeService), times);
    }

    public void VerifyUpdateAsync(ItemUpdate update, IValidator validator, IDateTimeService dateTimeService,
        Func<Times> times)
    {
        Verify(m => m.UpdateAsync(update, validator, dateTimeService), times);
    }

    public void VerifyUpdateAsync(ItemWithTypesUpdate update, IValidator validator, IDateTimeService dateTimeService,
        Func<Times> times)
    {
        Verify(m => m.UpdateAsync(update, validator, dateTimeService), times);
    }

    public void VerifyTransferToDefaultSection(SectionId oldSectionId, SectionId newSectionId, Func<Times> times)
    {
        Verify(m => m.TransferToDefaultSection(oldSectionId, newSectionId), times);
    }

    public void VerifyRemoveAvailabilitiesFor(StoreId storeId, Func<Times> times)
    {
        Verify(m => m.RemoveAvailabilitiesFor(storeId), times);
    }

    #endregion Verify
}