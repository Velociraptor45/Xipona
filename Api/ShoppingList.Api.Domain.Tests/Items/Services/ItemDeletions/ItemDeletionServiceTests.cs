using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Deletions;
using ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.Items.Models;
using ShoppingList.Api.Domain.TestKit.Items.Ports;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Items.Services.ItemDeletions;

public class ItemDeletionServiceTests
{
    public class DeleteAsyncTests
    {
        private readonly DeleteAsyncFixture _fixture;

        public DeleteAsyncTests()
        {
            _fixture = new DeleteAsyncFixture();
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidItemId_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupItemId();
            _fixture.SetupFindingNoItem();

            var sut = _fixture.CreateSut();

            // Act
            Func<Task> func = async () => await sut.DeleteAsync(_fixture.ItemId);

            // Assert
            using (new AssertionScope())
            {
                await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemNotFound);
            }
        }

        [Theory]
        [ClassData(typeof(HandleAsyncWithShoppingListsTestData))]
        public async Task DeleteAsync_WithItemOnShoppingLists_ShouldDeleteItemAndRemoveItFromActiveShoppingLists(
            List<ShoppingListMock> shoppingListMocks)
        {
            // Arrange
            _fixture.SetupItemId();
            _fixture.SetupStoreItemMock();
            _fixture.SetupFindingItem();
            _fixture.SetupFindingShoppingList(shoppingListMocks);
            _fixture.SetupDeletingItem();
            _fixture.SetupStoringItem();
            _fixture.SetupStoringShoppingLists(shoppingListMocks);

            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteAsync(_fixture.ItemId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyDeleteStoreItemOnce();
                _fixture.VerifyStoringItemOnce();
                if (!shoppingListMocks.Any())
                {
                    _fixture.VerifyStoreNoShoppingList();
                }
                else
                {
                    _fixture.VerifyRemoveItemFromShoppingList(shoppingListMocks);
                    _fixture.VerifyStoreAllShoppingLists(shoppingListMocks);
                }
            }
        }

        private sealed class DeleteAsyncFixture : LocalFixture
        {
            public ItemId ItemId { get; private set; }
            private StoreItemMock _storeItemMock;

            public void SetupItemId()
            {
                ItemId = ItemId.New;
            }

            public void SetupStoreItemMock()
            {
                _storeItemMock = new StoreItemMock(StoreItemMother.Initial().Create(), MockBehavior.Strict);
            }

            #region Mock Setup

            public void SetupDeletingItem()
            {
                _storeItemMock.SetupDelete();
            }

            public void SetupFindingItem()
            {
                ItemRepositoryMock.SetupFindByAsync(ItemId, _storeItemMock.Object);
            }

            public void SetupFindingNoItem()
            {
                ItemRepositoryMock.SetupFindByAsync(ItemId, null);
            }

            public void SetupStoringItem()
            {
                ItemRepositoryMock.SetupStoreAsync(_storeItemMock.Object, _storeItemMock.Object);
            }

            public void SetupFindingShoppingList(List<ShoppingListMock> shoppingListMocks)
            {
                ShoppingListRepositoryMock.SetupFindActiveByAsync(_storeItemMock.Object.Id,
                    shoppingListMocks.Select(m => m.Object));
            }

            public void SetupStoringShoppingLists(List<ShoppingListMock> shoppingListMocks)
            {
                foreach (var listMock in shoppingListMocks)
                {
                    ShoppingListRepositoryMock.SetupStoreAsync(listMock.Object);
                }
            }

            #endregion Mock Setup

            #region Verify

            public void VerifyDeleteStoreItemOnce()
            {
                _storeItemMock.VerifyDeleteOnce();
            }

            public void VerifyStoringItemOnce()
            {
                ItemRepositoryMock.VerifyStoreAsyncOnce(_storeItemMock.Object);
            }

            public void VerifyStoreNoShoppingList()
            {
                ShoppingListRepositoryMock.VerifyStoreAsyncNever();
            }

            public void VerifyStoreAllShoppingLists(List<ShoppingListMock> shoppingListMocks)
            {
                foreach (var shoppingListMock in shoppingListMocks)
                {
                    ShoppingListRepositoryMock.VerifyStoreAsyncOnce(shoppingListMock.Object);
                }
            }

            public void VerifyRemoveItemFromShoppingList(List<ShoppingListMock> shoppingListMocks)
            {
                foreach (var shoppingListMock in shoppingListMocks)
                {
                    shoppingListMock.VerifyRemoveItemOnce(_storeItemMock.Object.Id);
                }
            }

            #endregion Verify
        }
    }

    private abstract class LocalFixture
    {
        protected ShoppingListRepositoryMock ShoppingListRepositoryMock;
        protected ItemRepositoryMock ItemRepositoryMock;

        protected LocalFixture()
        {
            ShoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
            ItemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
        }

        public ItemDeletionService CreateSut()
        {
            return new ItemDeletionService(
                ItemRepositoryMock.Object,
                ShoppingListRepositoryMock.Object,
                default);
        }
    }
}