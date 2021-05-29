using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Threading;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Services
{
    public class AddItemToShoppingListServiceMock
    {
        private readonly Mock<IAddItemToShoppingListService> mock;

        public AddItemToShoppingListServiceMock(Fixture fixture)
        {
            mock = fixture.Freeze<Mock<IAddItemToShoppingListService>>();
        }

        public void VerifyAddItemToShoppingListOnce(IShoppingList shoppingList, ItemId itemId, SectionId sectionId,
            float quantity)
        {
            mock.Verify(i => i.AddItemToShoppingList(
                shoppingList,
                itemId,
                sectionId,
                quantity,
                It.IsAny<CancellationToken>()));
        }

        public void VerifyAddItemToShoppingListOnce(IShoppingList shoppingList, TemporaryItemId temporaryItemId,
            SectionId sectionId, float quantity)
        {
            mock.Verify(i => i.AddItemToShoppingList(
                shoppingList,
                temporaryItemId,
                sectionId,
                quantity,
                It.IsAny<CancellationToken>()));
        }
    }
}