using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Mocks
{
    public class ShoppingListMock : Mock<IShoppingList>
    {
        public ShoppingListMock(IShoppingList shoppingList)
        {
            SetupId(shoppingList.Id);
            SetupStore(shoppingList.Store);
        }

        public void SetupId(ShoppingListId returnValue)
        {
            Setup(i => i.Id)
                .Returns(returnValue);
        }

        public void SetupStore(IShoppingListStore returnValue)
        {
            Setup(i => i.Store)
                .Returns(returnValue);
        }

        public void SetupGetSectionsWithItemsNotInBasket(IEnumerable<IShoppingListSection> returnValue)
        {
            Setup(i => i.RemoveItemsInBasket())
                .Returns(returnValue);
        }

        public void VerifyRemoveItemOnce(ItemId itemId)
        {
            Verify(i => i.RemoveItem(
                    It.Is<ItemId>(id => id == itemId)),
                Times.Once);
        }

        public void VerifyAddItemOnce(IShoppingListItem listItem, ShoppingListSectionId sectionId)
        {
            Verify(i => i.AddItem(
                    It.Is<IShoppingListItem>(item => item == listItem),
                    It.Is<ShoppingListSectionId>(id => id == sectionId)),
                Times.Once);
        }

        public void VerifySetCompletionDateOnce(DateTime completionDate)
        {
            Verify(i => i.SetCompletionDate(
                    It.Is<DateTime>(date => date == completionDate)),
                Times.Once);
        }

        public void VerifyGetSectionsWithItemsNotInBasketOnce()
        {
            Verify(i => i.RemoveItemsInBasket(),
                Times.Once);
        }

        public void VerifyRemoveAllItemsNotInBasketOnce()
        {
            Verify(i => i.RemoveItemsNotInBasket(),
                Times.Once);
        }
    }
}