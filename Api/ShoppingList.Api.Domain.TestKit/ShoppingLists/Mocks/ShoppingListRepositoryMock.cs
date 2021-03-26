using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Mocks
{
    public class ShoppingListRepositoryMock
    {
        private readonly Mock<IShoppingListRepository> mock;

        public ShoppingListRepositoryMock(Mock<IShoppingListRepository> mock)
        {
            this.mock = mock;
        }

        public ShoppingListRepositoryMock(Fixture fixture)
        {
            mock = fixture.Freeze<Mock<IShoppingListRepository>>();
        }

        public void SetupFindActiveByAsync(ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.ItemId storeItemId,
            IEnumerable<IShoppingList> returnValue)
        {
            mock
                .Setup(i => i.FindActiveByAsync(
                    It.Is<ItemId>(id => id == storeItemId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnValue));
        }

        public void SetupFindActiveByAsync(
            IEnumerable<IShoppingList> returnValue)
        {
            mock
                .Setup(i => i.FindActiveByAsync(
                    It.IsAny<ItemId>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue);
        }

        public void SetupFindActiveByAsync(StoreId storeId, IShoppingList returnValue)
        {
            mock
                .Setup(i => i.FindActiveByAsync(
                    It.Is<StoreId>(id => id == storeId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnValue));
        }

        public void SetupFindByAsync(ShoppingListId shoppingListId,
            IShoppingList returnValue)
        {
            mock
                .Setup(instance => instance.FindByAsync(
                    It.Is<ShoppingListId>(id => id == shoppingListId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IShoppingList>(returnValue));
        }

        public void VerifyStoreAsyncOnce(IShoppingList shoppingList)
        {
            mock.Verify(
                i => i.StoreAsync(
                    It.Is<IShoppingList>(list => list == shoppingList),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        public void VerifyStoreAsyncNever()
        {
            mock.Verify(
                i => i.StoreAsync(
                    It.IsAny<IShoppingList>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}