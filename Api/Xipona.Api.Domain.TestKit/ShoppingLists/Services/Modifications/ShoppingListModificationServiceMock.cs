using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Shared;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Services.Modifications;

public class ShoppingListModificationServiceMock : Mock<IShoppingListModificationService>
{
    public ShoppingListModificationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupRemoveSectionAsync(SectionId sectionId)
    {
        Setup(m => m.RemoveSectionAsync(sectionId)).Returns(Task.CompletedTask);
    }

    public void SetupAddTemporaryItemAsync(ShoppingListId shoppingListId, ItemName itemName, QuantityType quantityType,
        QuantityInBasket quantity, Price price, SectionId sectionId, TemporaryItemId temporaryItemId)
    {
        Setup(m =>
                m.AddTemporaryItemAsync(shoppingListId, itemName, quantityType, quantity, price, sectionId, temporaryItemId))
            .Returns(Task.CompletedTask);
    }

    public void VerifyAddTemporaryItemAsync(ShoppingListId shoppingListId, ItemName itemName, QuantityType quantityType,
        QuantityInBasket quantity, Price price, SectionId sectionId, TemporaryItemId temporaryItemId, Func<Times> times)
    {
        Verify(m =>
                m.AddTemporaryItemAsync(shoppingListId, itemName, quantityType, quantity, price, sectionId, temporaryItemId),
            times);
    }

    public void SetupRemoveItemFromBasketAsync(ShoppingListId shoppingListId, OfflineTolerantItemId offlineTolerantItemId,
        ItemTypeId? itemTypeId)
    {
        Setup(m => m.RemoveItemFromBasketAsync(shoppingListId, offlineTolerantItemId, itemTypeId))
            .Returns(Task.CompletedTask);
    }

    public void VerifyRemoveItemFromBasketAsync(ShoppingListId shoppingListId, OfflineTolerantItemId offlineTolerantItemId,
        ItemTypeId? itemTypeId, Func<Times> times)
    {
        Verify(m => m.RemoveItemFromBasketAsync(shoppingListId, offlineTolerantItemId, itemTypeId), times);
    }

    public void SetupChangeItemQuantityAsync(ShoppingListId shoppingListId, OfflineTolerantItemId offlineTolerantItemId,
        ItemTypeId? itemTypeId, QuantityInBasket quantity)
    {
        Setup(m => m.ChangeItemQuantityAsync(shoppingListId, offlineTolerantItemId, itemTypeId, quantity))
            .Returns(Task.CompletedTask);
    }

    public void VerifyChangeItemQuantityAsync(ShoppingListId shoppingListId, OfflineTolerantItemId offlineTolerantItemId,
        ItemTypeId? itemTypeId, QuantityInBasket quantity, Func<Times> times)
    {
        Verify(m => m.ChangeItemQuantityAsync(shoppingListId, offlineTolerantItemId, itemTypeId, quantity), times);
    }

    public void SetupRemoveItemAsync(ShoppingListId shoppingListId, OfflineTolerantItemId offlineTolerantItemId,
        ItemTypeId? itemTypeId)
    {
        Setup(m => m.RemoveItemAsync(shoppingListId, offlineTolerantItemId, itemTypeId)).Returns(Task.CompletedTask);
    }

    public void VerifyRemoveItemAsync(ShoppingListId shoppingListId, OfflineTolerantItemId offlineTolerantItemId,
        ItemTypeId? itemTypeId, Func<Times> times)
    {
        Verify(m => m.RemoveItemAsync(shoppingListId, offlineTolerantItemId, itemTypeId), times);
    }

    public void SetupPutItemInBasketAsync(ShoppingListId shoppingListId, OfflineTolerantItemId offlineTolerantItemId,
        ItemTypeId? itemTypeId)
    {
        Setup(m => m.PutItemInBasketAsync(shoppingListId, offlineTolerantItemId, itemTypeId)).Returns(Task.CompletedTask);
    }

    public void VerifyPutItemInBasketAsync(ShoppingListId shoppingListId, OfflineTolerantItemId offlineTolerantItemId,
        ItemTypeId? itemTypeId, Func<Times> times)
    {
        Verify(m => m.PutItemInBasketAsync(shoppingListId, offlineTolerantItemId, itemTypeId), times);
    }
}