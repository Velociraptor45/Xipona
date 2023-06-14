using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Deletions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Ports;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

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
        public async Task DeleteAsync_WithInvalidItemId_ShouldNotThrow()
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
                await func.Should().NotThrowAsync();
            }
        }

        [Fact]
        public async Task DeleteAsync_WithItemOnShoppingLists_ShouldDeleteItem()
        {
            // Arrange
            _fixture.SetupItemId();
            _fixture.SetupItemMock();
            _fixture.SetupFindingItem();
            _fixture.SetupDeletingItem();
            _fixture.SetupStoringItem();

            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteAsync(_fixture.ItemId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyDeleteItemOnce();
                _fixture.VerifyStoringItemOnce();
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
            ItemRepositoryMock.SetupFindActiveByAsync(ItemId, _itemMock.Object);
        }

        public void SetupFindingNoItem()
        {
            ItemRepositoryMock.SetupFindActiveByAsync(ItemId, null);
        }

        public void SetupStoringItem()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemMock);
            ItemRepositoryMock.SetupStoreAsync(_itemMock.Object, _itemMock.Object);
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

        #endregion Verify
    }

    private abstract class LocalFixture
    {
        protected readonly ItemRepositoryMock ItemRepositoryMock;

        protected LocalFixture()
        {
            ItemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
        }

        public ItemDeletionService CreateSut()
        {
            return new ItemDeletionService(
                _ => ItemRepositoryMock.Object,
                default);
        }
    }
}