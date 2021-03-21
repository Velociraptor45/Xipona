using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Mocks
{
    public class ShoppingListSectionMock : Mock<IShoppingListSection>
    {
        public ShoppingListSectionMock(IShoppingListSection section)
        {
            SetupId(section.Id);
            SetupName(section.Name);
            SetupSortingIndex(section.SortingIndex);
            SetupItems(section.ShoppingListItems);
            SetupIsDefaultSection(section.IsDefaultSection);
        }

        public void SetupId(ShoppingListSectionId returnValue)
        {
            Setup(i => i.Id)
                .Returns(returnValue);
        }

        public void SetupName(string returnValue)
        {
            Setup(i => i.Name)
                .Returns(returnValue);
        }

        public void SetupSortingIndex(int returnValue)
        {
            Setup(i => i.SortingIndex)
                .Returns(returnValue);
        }

        public void SetupItems(IReadOnlyCollection<IShoppingListItem> returnValue)
        {
            Setup(i => i.ShoppingListItems)
                .Returns(returnValue);
        }

        public void SetupIsDefaultSection(bool returnValue)
        {
            Setup(i => i.IsDefaultSection)
                .Returns(returnValue);
        }

        public void SetupContainsItem(bool returnValue)
        {
            Setup(i => i.ContainsItem(
                    It.IsAny<ShoppingListItemId>()))
                .Returns(returnValue);
        }

        public void SetupContainsItem(ShoppingListItemId itemId, bool returnValue)
        {
            Setup(i => i.ContainsItem(
                    It.Is<ShoppingListItemId>(id => id == itemId)))
                .Returns(returnValue);
        }

        public void VerifyAddItemOnce(IShoppingListItem item)
        {
            Verify(i => i.AddItem(
                    It.Is<IShoppingListItem>(itm => itm == item)),
                Times.Once);
        }

        public void VerifyRemoveItemOnce(ShoppingListItemId id)
        {
            Verify(i => i.RemoveItem(
                    It.Is<ShoppingListItemId>(itemId => itemId == id)),
                Times.Once);
        }

        public void VerifyRemoveItemNever()
        {
            Verify(i => i.RemoveItem(
                    It.IsAny<ShoppingListItemId>()),
                Times.Never);
        }

        public void VerifyPutItemInBasketOnce(ShoppingListItemId id)
        {
            Verify(i => i.PutItemInBasket(
                    It.Is<ShoppingListItemId>(itemId => itemId == id)),
                Times.Once);
        }

        public void VerifyPutItemInBasketNever()
        {
            Verify(i => i.PutItemInBasket(
                    It.IsAny<ShoppingListItemId>()),
                Times.Never);
        }

        public void VerifyRemoveItemFromBasketOnce(ShoppingListItemId id)
        {
            Verify(i => i.RemoveItemFromBasket(
                    It.Is<ShoppingListItemId>(itemId => itemId == id)),
                Times.Once);
        }

        public void VerifyRemoveItemFromBasketNever()
        {
            Verify(i => i.RemoveItemFromBasket(
                    It.IsAny<ShoppingListItemId>()),
                Times.Never);
        }

        public void VerifyChangeItemQuantityOnce(ShoppingListItemId id, float quantity)
        {
            Verify(i => i.ChangeItemQuantity(
                    It.Is<ShoppingListItemId>(itemId => itemId == id),
                    It.Is<float>(qnt => qnt == quantity)),
                Times.Once);
        }

        public void VerifyChangeItemQuantityNever()
        {
            Verify(i => i.ChangeItemQuantity(
                    It.IsAny<ShoppingListItemId>(),
                    It.IsAny<float>()),
                Times.Never);
        }
    }
}