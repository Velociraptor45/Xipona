using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Mocks
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

        public void VerifyRemoveItemOnce(ShoppingListItemId itemId)
        {
            Verify(i => i.RemoveItem(
                    It.Is<ShoppingListItemId>(id => id == itemId)),
                Times.Once);
        }

        public void VerifyAddItemOnce(IShoppingListItem listItem, ShoppingListSectionId sectionId)
        {
            Verify(i => i.AddItem(
                    It.Is<IShoppingListItem>(item => item == listItem),
                    It.Is<ShoppingListSectionId>(id => id == sectionId)),
                Times.Once);
        }
    }
}