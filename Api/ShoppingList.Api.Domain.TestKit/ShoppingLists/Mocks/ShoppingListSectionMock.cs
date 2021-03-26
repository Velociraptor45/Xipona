using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Mocks
{
    public class ShoppingListSectionMock : Mock<IShoppingListSection>
    {
        public ShoppingListSectionMock(IShoppingListSection section)
        {
            SetupId(section.Id);
            SetupItems(section.Items);
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

        public void VerifyPutItemInBasketOnce(ItemId id)
        {
            Verify(i => i.PutItemInBasket(
                    It.Is<ItemId>(itemId => itemId == id)),
                Times.Once);
        }

        public void VerifyPutItemInBasketNever()
        {
            Verify(i => i.PutItemInBasket(
                    It.IsAny<ItemId>()),
                Times.Never);
        }

        public void VerifyRemoveItemFromBasketOnce(ItemId id)
        {
            Verify(i => i.RemoveItemFromBasket(
                    It.Is<ItemId>(itemId => itemId == id)),
                Times.Once);
        }

        public void VerifyRemoveItemFromBasketNever()
        {
            Verify(i => i.RemoveItemFromBasket(
                    It.IsAny<ItemId>()),
                Times.Never);
        }

        public void VerifyChangeItemQuantityOnce(ItemId id, float quantity)
        {
            Verify(i => i.ChangeItemQuantity(
                    It.Is<ItemId>(itemId => itemId == id),
                    It.Is<float>(qnt => qnt == quantity)),
                Times.Once);
        }

        public void VerifyChangeItemQuantityNever()
        {
            Verify(i => i.ChangeItemQuantity(
                    It.IsAny<ItemId>(),
                    It.IsAny<float>()),
                Times.Never);
        }
    }
}