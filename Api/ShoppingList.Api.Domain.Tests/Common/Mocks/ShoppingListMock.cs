using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Mocks
{
    public class ShoppingListMock : Mock<IShoppingList>
    {
        public ShoppingListMock(IShoppingList shoppingList)
        {
            SetupId(shoppingList.Id);
        }

        public void SetupId(ShoppingListId returnValue)
        {
            Setup(i => i.Id)
                .Returns(returnValue);
        }

        public void VerifyRemoveItemOnce(ShoppingListItemId itemId)
        {
            Verify(i => i.RemoveItem(
                    It.Is<ShoppingListItemId>(id => id == itemId)),
                Times.Once);
        }
    }
}