using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;

public class ShoppingListSectionMock : Mock<IShoppingListSection>
{
    public ShoppingListSectionMock(IShoppingListSection section)
    {
        SetupId(section.Id);
        SetupItems(section.Items);
        SetupOriginalRemoveItemsNotInBasket();
        SetupOriginalRemoveItemsInBasket();
    }

    public void SetupId(SectionId returnValue)
    {
        Setup(i => i.Id)
            .Returns(returnValue);
    }

    public void SetupItems(IReadOnlyCollection<IShoppingListItem> returnValue)
    {
        Setup(i => i.Items)
            .Returns(returnValue);
    }

    public void SetupContainsItem(bool returnValue)
    {
        Setup(i => i.ContainsItem(
                It.IsAny<ItemId>()))
            .Returns(returnValue);
    }

    public void SetupContainsItem(ItemId itemId, ItemTypeId? itemTypeId, bool returnValue)
    {
        Setup(i => i.ContainsItem(itemId, itemTypeId))
            .Returns(returnValue);
    }

    public void SetupOriginalRemoveItemsNotInBasket()
    {
        Setup(i => i.RemoveItemsNotInBasket())
            .Returns(Object.RemoveItemsNotInBasket());
    }

    public void SetupOriginalRemoveItemsInBasket()
    {
        Setup(i => i.RemoveItemsInBasket())
            .Returns(Object.RemoveItemsInBasket());
    }

    public void VerifyAddItemOnce(IShoppingListItem item, bool throwIfAlreadyPresent = true)
    {
        Verify(i => i.AddItem(
                It.Is<IShoppingListItem>(itm => itm == item),
                throwIfAlreadyPresent),
            Times.Once);
    }

    public void VerifyRemoveItem(ItemId id, ItemTypeId? itemTypeId, Func<Times> times)
    {
        Verify(i => i.RemoveItem(id, itemTypeId), times);
    }

    public void VerifyRemoveItemNever()
    {
        Verify(i => i.RemoveItem(
                It.IsAny<ItemId>()),
            Times.Never);
    }

    public void VerifyPutItemInBasketOnce(ItemId itemId, ItemTypeId? itemTypeId)
    {
        Verify(i => i.PutItemInBasket(itemId, itemTypeId), Times.Once);
    }

    public void VerifyPutItemInBasketNever()
    {
        Verify(i => i.PutItemInBasket(
                It.IsAny<ItemId>(),
                It.IsAny<ItemTypeId?>()),
            Times.Never);
    }

    public void VerifyRemoveItemFromBasketOnce(ItemId itemId, ItemTypeId? itemTypeId)
    {
        Verify(i => i.RemoveItemFromBasket(itemId, itemTypeId), Times.Once);
    }

    public void VerifyRemoveItemFromBasketNever()
    {
        Verify(i => i.RemoveItemFromBasket(
                It.IsAny<ItemId>(),
                It.IsAny<ItemTypeId?>()),
            Times.Never);
    }

    public void VerifyChangeItemQuantityOnce(ItemId itemId, ItemTypeId? itemTypeId, QuantityInBasket quantity)
    {
        Verify(i => i.ChangeItemQuantity(itemId, itemTypeId, quantity),
            Times.Once);
    }

    public void VerifyChangeItemQuantityNever()
    {
        Verify(i => i.ChangeItemQuantity(
                It.IsAny<ItemId>(),
                It.IsAny<ItemTypeId?>(),
                It.IsAny<QuantityInBasket>()),
            Times.Never);
    }
}