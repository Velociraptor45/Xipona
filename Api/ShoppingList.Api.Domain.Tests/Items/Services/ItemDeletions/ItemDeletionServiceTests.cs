using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Deletions;
using ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.Items.Models;
using ShoppingList.Api.Domain.TestKit.Items.Ports;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.TestTools.Exceptions;

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
            _fixture.SetupItemMock();
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
                _fixture.VerifyDeleteItemOnce();
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
            private ItemMock? _itemMock;

            public void SetupItemId()
            {
                ItemId = ItemId.New;
            }

            public void SetupItemMock()
            {
                _itemMock = new ItemMock(ItemMother.Initial().Create(), MockBehavior.Strict);
            }

            #region Mock Setup

            public void SetupDeletingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemMock);
                _itemMock.SetupDelete();
            }

            public void SetupFindingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemMock);
                ItemRepositoryMock.SetupFindByAsync(ItemId, _itemMock.Object);
            }

            public void SetupFindingNoItem()
            {
                ItemRepositoryMock.SetupFindByAsync(ItemId, null);
            }

            public void SetupStoringItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemMock);
                ItemRepositoryMock.SetupStoreAsync(_itemMock.Object, _itemMock.Object);
            }

            public void SetupFindingShoppingList(List<ShoppingListMock> shoppingListMocks)
            {
                TestPropertyNotSetException.ThrowIfNull(_itemMock);
                ShoppingListRepositoryMock.SetupFindActiveByAsync(_itemMock.Object.Id,
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

            public void VerifyDeleteItemOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemMock);
                _itemMock.VerifyDeleteOnce();
            }

            public void VerifyStoringItemOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemMock);
                ItemRepositoryMock.VerifyStoreAsyncOnce(_itemMock.Object);
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
                TestPropertyNotSetException.ThrowIfNull(_itemMock);

                foreach (var shoppingListMock in shoppingListMocks)
                {
                    shoppingListMock.VerifyRemoveItemOnce(_itemMock.Object.Id);
                }
            }

            #endregion Verify
        }
    }

    private abstract class LocalFixture
    {
        protected readonly ShoppingListRepositoryMock ShoppingListRepositoryMock;
        protected readonly ItemRepositoryMock ItemRepositoryMock;

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