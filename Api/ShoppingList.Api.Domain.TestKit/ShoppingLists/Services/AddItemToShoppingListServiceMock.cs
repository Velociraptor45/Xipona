using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Threading;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Services
{
    public class AddItemToShoppingListServiceMock : Mock<IAddItemToShoppingListService>
    {
        public AddItemToShoppingListServiceMock(MockBehavior behavior) : base(behavior)
        {
        }

        public void VerifyAddItemToShoppingListOnce(IShoppingList shoppingList, ItemId itemId, SectionId sectionId,
            float quantity)
        {
            Verify(i => i.AddItemToShoppingList(
                shoppingList,
                itemId,
                sectionId,
                quantity,
                It.IsAny<CancellationToken>()));
        }

        public void VerifyAddItemToShoppingListOnce(IShoppingList shoppingList, TemporaryItemId temporaryItemId,
            SectionId sectionId, float quantity)
        {
            Verify(i => i.AddItemToShoppingList(
                shoppingList,
                temporaryItemId,
                sectionId,
                quantity,
                It.IsAny<CancellationToken>()));
        }
    }
}