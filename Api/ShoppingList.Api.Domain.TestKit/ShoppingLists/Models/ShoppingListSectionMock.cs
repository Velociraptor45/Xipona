using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Models
{
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

        public void SetupContainsItem(ItemId itemId, bool returnValue)
        {
            Setup(i => i.ContainsItem(
                    It.Is<ItemId>(id => id == itemId)))
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

        public void VerifyAddItemOnce(IShoppingListItem item)
        {
            Verify(i => i.AddItem(
                    It.Is<IShoppingListItem>(itm => itm == item)),
                Times.Once);
        }

        public void VerifyRemoveItemOnce(ItemId id)
        {
            Verify(i => i.RemoveItem(
                    It.Is<ItemId>(itemId => itemId == id)),
                Times.Once);
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

        public void VerifyChangeItemQuantityOnce(ItemId itemId, ItemTypeId? itemTypeId, float quantity)
        {
            Verify(i => i.ChangeItemQuantity(itemId, itemTypeId, quantity),
                Times.Once);
        }

        public void VerifyChangeItemQuantityNever()
        {
            Verify(i => i.ChangeItemQuantity(
                    It.IsAny<ItemId>(),
                    It.IsAny<ItemTypeId?>(),
                    It.IsAny<float>()),
                Times.Never);
        }
    }
}