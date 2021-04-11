using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Models
{
    public class ShoppingListMock : Mock<IShoppingList>
    {
        public ShoppingListMock(IShoppingList shoppingList)
        {
            SetupId(shoppingList.Id);
            SetupStore(shoppingList.StoreId);
            SetupSections(shoppingList.Sections);
        }

        public void SetupId(ShoppingListId returnValue)
        {
            Setup(i => i.Id)
                .Returns(returnValue);
        }

        public void SetupStore(StoreId returnValue)
        {
            Setup(i => i.StoreId)
                .Returns(returnValue);
        }

        public void SetupSections(IEnumerable<IShoppingListSection> sections)
        {
            var readonlySections = sections.ToList().AsReadOnly();

            Setup(i => i.Sections)
                .Returns(readonlySections);
        }

        public void SetupFinish(DateTime completionDate, IShoppingList returnValue)
        {
            Setup(i => i.Finish(
                It.Is<DateTime>(date => date == completionDate)))
                .Returns(returnValue);
        }

        public void VerifyRemoveItemOnce(ItemId itemId)
        {
            Verify(i => i.RemoveItem(
                    It.Is<ItemId>(id => id == itemId)),
                Times.Once);
        }

        public void VerifyPutItemInBasketOnce(ItemId itemId)
        {
            Verify(i => i.PutItemInBasket(itemId), Times.Once);
        }

        public void VerifyRemoveItemFromBasketOnce(ItemId itemId)
        {
            Verify(i => i.RemoveFromBasket(itemId), Times.Once);
        }

        public void VerifyAddItemOnce(IShoppingListItem listItem, SectionId sectionId)
        {
            Verify(i => i.AddItem(
                    It.Is<IShoppingListItem>(item => item == listItem),
                    It.Is<SectionId>(id => id == sectionId)),
                Times.Once);
        }

        public void VerifyAddSectionOnce(IShoppingListSection section)
        {
            Verify(i => i.AddSection(
                    It.Is<IShoppingListSection>(s => s == section)),
                Times.Once);
        }

        public void VerifyAddSectionNever()
        {
            Verify(i => i.AddSection(
                    It.IsAny<IShoppingListSection>()),
                Times.Never);
        }

        public void VerifyFinishOnce(DateTime completionDate)
        {
            Verify(i => i.Finish(
                    It.Is<DateTime>(date => date == completionDate)),
                Times.Once);
        }

        public void VerifyChangeItemQuantityOnce(ItemId itemId, float quantity)
        {
            Verify(i => i.ChangeItemQuantity(
                    itemId,
                    quantity),
                Times.Once);
        }
    }
}