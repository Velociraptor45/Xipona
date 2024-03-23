using ProjectHermes.Xipona.Api.Core.TestKit.Services;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Shared;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Services;
using ProjectHermes.Xipona.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Stores.Ports;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

using DomainModels = ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Tests.ShoppingLists.Services.ShoppingListModifications;

public class ShoppingListModificationServiceTests
{
    public class ChangeItemQuantityAsyncTests
    {
        private readonly ChangeItemQuantityAsyncFixture _fixture;

        public ChangeItemQuantityAsyncTests()
        {
            _fixture = new ChangeItemQuantityAsyncFixture();
        }

        [Fact]
        public async Task ChangeItemQuantityAsync_WithInvalidShoppingListId_ShouldThrowDomainException()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupTemporaryItemId();
            _fixture.SetupShoppingListId();
            _fixture.SetupActualItemId();
            _fixture.SetupItemTypeId();
            _fixture.SetupQuantity();
            _fixture.SetupShoppingListRepositoryFindingNoList();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OfflineTolerantItemId);

            // Act
            Func<Task> function = async () => await sut.ChangeItemQuantityAsync(_fixture.ShoppingListId,
                _fixture.OfflineTolerantItemId, _fixture.ItemTypeId, _fixture.Quantity);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ShoppingListNotFound);
            }
        }

        [Fact]
        public async Task ChangeItemQuantityAsync_WithInvalidOfflineId_ShouldThrowDomainException()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupTemporaryItemId();
            _fixture.SetupShoppingListId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupQuantity();

            _fixture.SetupShoppingListMock();

            _fixture.SetupShoppingListRepositoryFindBy();
            _fixture.SetupNotFindingTemporaryItem();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OfflineTolerantItemId);

            // Act
            Func<Task> function = async () => await sut.ChangeItemQuantityAsync(_fixture.ShoppingListId,
                _fixture.OfflineTolerantItemId, _fixture.ItemTypeId, _fixture.Quantity);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound);
            }
        }

        [Fact]
        public async Task ChangeItemQuantityAsync_WithActualId_ShouldChangeItemQuantityAndStoreIt()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupShoppingListId();
            _fixture.SetupActualItemId();
            _fixture.SetupItemTypeId();
            _fixture.SetupQuantity();
            _fixture.SetupShoppingListMock();
            _fixture.SetupShoppingListRepositoryFindBy();
            _fixture.SetupStoringShoppingList();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OfflineTolerantItemId);

            // Act
            await sut.ChangeItemQuantityAsync(_fixture.ShoppingListId, _fixture.OfflineTolerantItemId,
                _fixture.ItemTypeId, _fixture.Quantity);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyChangingItemQuantity();
                _fixture.VerifyStoreAsync();
            }
        }

        [Fact]
        public async Task ChangeItemQuantityAsync_WithOfflineId_ShouldChangeItemQuantityAndStoreIt()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupTemporaryItemId();
            _fixture.SetupShoppingListId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupQuantity();
            _fixture.SetupShoppingListMock();
            _fixture.SetupItem();

            _fixture.SetupShoppingListRepositoryFindBy();
            _fixture.SetupFindingTemporaryItem();
            _fixture.SetupStoringShoppingList();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OfflineTolerantItemId);

            // Act
            await sut.ChangeItemQuantityAsync(_fixture.ShoppingListId, _fixture.OfflineTolerantItemId,
                _fixture.ItemTypeId, _fixture.Quantity);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyChangeItemQuantityOnce();
                _fixture.VerifyStoreAsync();
            }
        }

        private sealed class ChangeItemQuantityAsyncFixture : ShoppingListModificationServiceFixture
        {
            private ShoppingListMock? _shoppingListMock;
            private IItem? _item;

            public ShoppingListId ShoppingListId { get; private set; }
            public ItemTypeId? ItemTypeId { get; private set; }
            public OfflineTolerantItemId? OfflineTolerantItemId { get; private set; }
            public QuantityInBasket Quantity { get; private set; }

            public void SetupActualItemId()
            {
                OfflineTolerantItemId = OfflineTolerantItemId.FromActualId(Guid.NewGuid());
            }

            public void SetupTemporaryItemId()
            {
                OfflineTolerantItemId = OfflineTolerantItemId.FromOfflineId(Guid.NewGuid());
            }

            public void SetupShoppingListId()
            {
                ShoppingListId = ShoppingListId.New;
            }

            public void SetupItemTypeId()
            {
                ItemTypeId = DomainModels.ItemTypeId.New;
            }

            public void SetupItemTypeIdNull()
            {
                ItemTypeId = null;
            }

            public void SetupQuantity()
            {
                Quantity = new QuantityInBasketBuilder().Create();
            }

            public void SetupShoppingListMock()
            {
                _shoppingListMock = new ShoppingListMock(ShoppingListMother.Sections(2).Create());
            }

            public void SetupItem()
            {
                _item = ItemMother.Initial().Create();
            }

            #region Fixture Setup

            public void SetupShoppingListRepositoryFindBy()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, _shoppingListMock.Object);
            }

            public void SetupShoppingListRepositoryFindingNoList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, null);
            }

            public void SetupStoringShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                ShoppingListRepositoryMock.SetupStoreAsync(_shoppingListMock.Object);
            }

            public void SetupFindingTemporaryItem()
            {
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId);
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId.OfflineId);
                var itemId = new TemporaryItemId(OfflineTolerantItemId.OfflineId.Value);
                ItemRepositoryMock.SetupFindActiveByAsync(itemId, _item);
            }

            public void SetupNotFindingTemporaryItem()
            {
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId);
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId.OfflineId);
                var itemId = new TemporaryItemId(OfflineTolerantItemId.OfflineId.Value);
                ItemRepositoryMock.SetupFindActiveByAsync(itemId, null);
            }

            #endregion Fixture Setup

            #region Verify

            public void VerifyStoreAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                ShoppingListRepositoryMock.VerifyStoreAsyncOnce(_shoppingListMock.Object);
            }

            public void VerifyChangeItemQuantityOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                _shoppingListMock.VerifyChangeItemQuantityOnce(_item.Id, ItemTypeId, Quantity);
            }

            public void VerifyChangingItemQuantity()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId);
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId.ActualId);
                _shoppingListMock.VerifyChangeItemQuantityOnce(
                    new ItemId(OfflineTolerantItemId.ActualId.Value),
                    ItemTypeId,
                    Quantity);
            }

            #endregion Verify
        }
    }

    public class RemoveItemAsyncTests
    {
        private readonly RemoveItemAsyncFixture _fixture;

        public RemoveItemAsyncTests()
        {
            _fixture = new RemoveItemAsyncFixture();
        }

        [Fact]
        public async Task RemoveItemAsync_WithInvalidShoppingListId_ShouldThrowDomainException()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupShoppingListId();
            _fixture.SetupTemporaryItemId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupFindingNoShoppingList();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OfflineTolerantItemId);

            // Act
            Func<Task> function = async () => await sut.RemoveItemAsync(_fixture.ShoppingListId,
                _fixture.OfflineTolerantItemId, _fixture.ItemTypeId);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ShoppingListNotFound);
            }
        }

        [Fact]
        public async Task RemoveItemAsync_WithInvalidActualItemId_ShouldThrowDomainException()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupShoppingListId();
            _fixture.SetupActualItemId();
            _fixture.SetupItemTypeId();

            _fixture.SetupShoppingListMock();
            _fixture.SetupFindingShoppingList();
            _fixture.SetupFindingNoItemByActualId();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OfflineTolerantItemId);

            // Act
            Func<Task> function = async () => await sut.RemoveItemAsync(_fixture.ShoppingListId,
                _fixture.OfflineTolerantItemId, _fixture.ItemTypeId);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemNotFound);
            }
        }

        [Fact]
        public async Task RemoveItemAsync_WithInvalidOfflineId_ShouldThrowDomainException()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupShoppingListId();
            _fixture.SetupTemporaryItemId();
            _fixture.SetupItemTypeIdNull();

            _fixture.SetupShoppingListMock();
            _fixture.SetupFindingShoppingList();
            _fixture.SetupFindingNoItemByTemporaryId();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OfflineTolerantItemId);

            // Act
            Func<Task> function = async () => await sut.RemoveItemAsync(_fixture.ShoppingListId,
                _fixture.OfflineTolerantItemId, _fixture.ItemTypeId);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemNotFound);
            }
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        public async Task RemoveItemAsync_WithValidActualId_ShouldRemoveItemFromBasket(bool isActualItemId, bool isTemporaryItem)
        {
            // Arrange
            var sut = _fixture.CreateSut();

            if (isTemporaryItem)
            {
                _fixture.SetupTemporaryItemMock();
                _fixture.SetupDeletingItem();
                _fixture.SetupStoringItem();
            }
            else
            {
                _fixture.SetupItemMock();
            }

            if (isActualItemId)
            {
                _fixture.SetupActualItemId();
                _fixture.SetupItemTypeId();
                _fixture.SetupFindingItemByActualId();
            }
            else
            {
                _fixture.SetupTemporaryItemId();
                _fixture.SetupItemTypeIdNull();
                _fixture.SetupFindingItemByTemporaryId();
            }

            _fixture.SetupShoppingListId();
            _fixture.SetupShoppingListMock();
            _fixture.SetupFindingShoppingList();
            _fixture.SetupStoringShoppingList();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OfflineTolerantItemId);

            // Act
            await sut.RemoveItemAsync(_fixture.ShoppingListId, _fixture.OfflineTolerantItemId, _fixture.ItemTypeId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyRemoveItemOnce();
                _fixture.VerifyStoreShoppingListOnce();

                if (isTemporaryItem)
                {
                    _fixture.VerifyDeleteItemOnce();
                    _fixture.VerifyItemOnce();
                }
                else
                {
                    _fixture.VerifyDeleteItemNever();
                    _fixture.VerifyItemNever();
                }
            }
        }

        private sealed class RemoveItemAsyncFixture : ShoppingListModificationServiceFixture
        {
            public ShoppingListId ShoppingListId { get; private set; }
            public ItemTypeId? ItemTypeId { get; private set; }
            public OfflineTolerantItemId? OfflineTolerantItemId { get; private set; }
            private ItemMock? _itemMock;
            private ShoppingListMock? _shoppingListMock;

            public void SetupActualItemId()
            {
                OfflineTolerantItemId = OfflineTolerantItemId.FromActualId(Guid.NewGuid());
            }

            public void SetupTemporaryItemId()
            {
                OfflineTolerantItemId = OfflineTolerantItemId.FromOfflineId(Guid.NewGuid());
            }

            public void SetupShoppingListId()
            {
                ShoppingListId = ShoppingListId.New;
            }

            public void SetupItemTypeId()
            {
                ItemTypeId = DomainModels.ItemTypeId.New;
            }

            public void SetupItemTypeIdNull()
            {
                ItemTypeId = null;
            }

            public void SetupTemporaryItemMock()
            {
                var item = ItemMother.InitialTemporary().Create();
                _itemMock = new ItemMock(item, MockBehavior.Strict);
            }

            public void SetupItemMock()
            {
                var item = ItemMother.Initial().Create();
                _itemMock = new ItemMock(item, MockBehavior.Strict);
            }

            public void SetupShoppingListMock()
            {
                _shoppingListMock = new ShoppingListMock(ShoppingListMother.Sections(3).Create());
            }

            #region Mock Setup

            public void SetupDeletingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemMock);
                _itemMock.SetupDelete();
            }

            public void SetupFindingItemByActualId()
            {
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId);
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId.ActualId);
                TestPropertyNotSetException.ThrowIfNull(_itemMock);
                ItemRepositoryMock.SetupFindActiveByAsync(new ItemId(OfflineTolerantItemId.ActualId.Value),
                    _itemMock.Object);
            }

            public void SetupFindingNoItemByActualId()
            {
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId);
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId.ActualId);
                ItemRepositoryMock.SetupFindActiveByAsync(new ItemId(OfflineTolerantItemId.ActualId.Value), null);
            }

            public void SetupFindingItemByTemporaryId()
            {
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId);
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId.OfflineId);
                TestPropertyNotSetException.ThrowIfNull(_itemMock);
                ItemRepositoryMock.SetupFindActiveByAsync(new TemporaryItemId(OfflineTolerantItemId.OfflineId.Value),
                    _itemMock.Object);
            }

            public void SetupFindingNoItemByTemporaryId()
            {
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId);
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId.OfflineId);
                ItemRepositoryMock.SetupFindActiveByAsync(new TemporaryItemId(OfflineTolerantItemId.OfflineId.Value),
                    null);
            }

            public void SetupStoringItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemMock);
                ItemRepositoryMock.SetupStoreAsync(_itemMock.Object, _itemMock.Object);
            }

            public void SetupFindingShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, _shoppingListMock.Object);
            }

            public void SetupFindingNoShoppingList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, null);
            }

            public void SetupStoringShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                ShoppingListRepositoryMock.SetupStoreAsync(_shoppingListMock.Object);
            }

            #endregion Mock Setup

            #region Verify

            public void VerifyRemoveItemOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                TestPropertyNotSetException.ThrowIfNull(_itemMock);
                _shoppingListMock.VerifyRemoveItem(_itemMock.Object.Id, ItemTypeId, Times.Once);
            }

            public void VerifyDeleteItemOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemMock);
                _itemMock.VerifyDeleteOnce();
            }

            public void VerifyDeleteItemNever()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemMock);
                _itemMock.VerifyDeleteNever();
            }

            public void VerifyItemOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemMock);
                ItemRepositoryMock.VerifyStoreAsyncOnce(_itemMock.Object);
            }

            public void VerifyItemNever()
            {
                ItemRepositoryMock.VerifyStoreAsyncNever();
            }

            public void VerifyStoreShoppingListOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                ShoppingListRepositoryMock.VerifyStoreAsyncOnce(_shoppingListMock.Object);
            }

            #endregion Verify
        }
    }

    public class RemoveItemFromBasketAsyncTests
    {
        private readonly RemoveItemFromBasketAsyncFixture _fixture;

        public RemoveItemFromBasketAsyncTests()
        {
            _fixture = new RemoveItemFromBasketAsyncFixture();
        }

        [Fact]
        public async Task RemoveItemFromBasketAsync_WithInvalidOfflineId_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupShoppingListId();
            _fixture.SetupTemporaryItemId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupShoppingListMock();
            _fixture.SetupFindingShoppingList();
            _fixture.SetupFindingNoItemByOfflineId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OfflineTolerantItemId);

            // Act
            Func<Task> function = async () => await sut.RemoveItemFromBasketAsync(_fixture.ShoppingListId,
                _fixture.OfflineTolerantItemId, _fixture.ItemTypeId);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemNotFound);
            }
        }

        [Fact]
        public async Task RemoveItemFromBasketAsync_WithInvalidListId_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupShoppingListId();
            _fixture.SetupTemporaryItemId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupFindingNoShoppingList();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OfflineTolerantItemId);

            // Act
            Func<Task> function = async () => await sut.RemoveItemFromBasketAsync(_fixture.ShoppingListId,
                _fixture.OfflineTolerantItemId, _fixture.ItemTypeId);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ShoppingListNotFound);
            }
        }

        #region WithActualId

        [Fact]
        public async Task RemoveItemFromBasketAsync_WithActualId_ShouldRemoveItemFromBasket()
        {
            // Arrange
            _fixture.SetupWithActualId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OfflineTolerantItemId);

            // Act
            await sut.RemoveItemFromBasketAsync(_fixture.ShoppingListId, _fixture.OfflineTolerantItemId,
                _fixture.ItemTypeId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyRemoveItemFromBasketWithCommandActualId();
            }
        }

        [Fact]
        public async Task RemoveItemFromBasketAsync_WithActualId_ShouldStoreShoppingList()
        {
            // Arrange
            _fixture.SetupWithActualId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OfflineTolerantItemId);

            // Act
            await sut.RemoveItemFromBasketAsync(_fixture.ShoppingListId, _fixture.OfflineTolerantItemId,
                _fixture.ItemTypeId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyStoringShoppingList();
            }
        }

        #endregion WithActualId

        #region WithValidOfflineId

        [Fact]
        public async Task RemoveItemFromBasketAsync_WithValidOfflineId_ShouldRemoveItemFromBasket()
        {
            // Arrange
            _fixture.SetupWithValidOfflineId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OfflineTolerantItemId);

            // Act
            await sut.RemoveItemFromBasketAsync(_fixture.ShoppingListId, _fixture.OfflineTolerantItemId,
                _fixture.ItemTypeId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyRemoveItemFromBasketWithItemId();
            }
        }

        [Fact]
        public async Task RemoveItemFromBasketAsync_WithValidOfflineId_ShouldStoreShoppingList()
        {
            // Arrange
            _fixture.SetupWithValidOfflineId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OfflineTolerantItemId);

            // Act
            await sut.RemoveItemFromBasketAsync(_fixture.ShoppingListId, _fixture.OfflineTolerantItemId,
                _fixture.ItemTypeId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyStoringShoppingList();
            }
        }

        #endregion WithValidOfflineId

        private sealed class RemoveItemFromBasketAsyncFixture : ShoppingListModificationServiceFixture
        {
            private IItem? _item;
            private ShoppingListMock? _shoppingListMock;

            public ShoppingListId ShoppingListId { get; private set; }
            public ItemTypeId? ItemTypeId { get; private set; }
            public OfflineTolerantItemId? OfflineTolerantItemId { get; private set; }

            public void SetupActualItemId()
            {
                OfflineTolerantItemId = OfflineTolerantItemId.FromActualId(Guid.NewGuid());
            }

            public void SetupTemporaryItemId()
            {
                OfflineTolerantItemId = OfflineTolerantItemId.FromOfflineId(Guid.NewGuid());
            }

            public void SetupShoppingListId()
            {
                ShoppingListId = ShoppingListId.New;
            }

            public void SetupItemTypeId()
            {
                ItemTypeId = DomainModels.ItemTypeId.New;
            }

            public void SetupItemTypeIdNull()
            {
                ItemTypeId = null;
            }

            public void SetupItem()
            {
                _item = ItemMother.Initial().Create();
            }

            public void SetupShoppingListMock()
            {
                _shoppingListMock = new ShoppingListMock(ShoppingListMother.Sections(3).Create());
            }

            #region Mock Setup

            public void SetupFindingShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, _shoppingListMock.Object);
            }

            public void SetupFindingNoShoppingList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, null);
            }

            public void SetupStoringShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                ShoppingListRepositoryMock.SetupStoreAsync(_shoppingListMock.Object);
            }

            public void SetupFindingItemByOfflineId()
            {
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId);
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId.OfflineId);
                var tempId = new TemporaryItemId(OfflineTolerantItemId.OfflineId.Value);
                ItemRepositoryMock.SetupFindActiveByAsync(tempId, _item);
            }

            public void SetupFindingNoItemByOfflineId()
            {
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId);
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId.OfflineId);
                var tempId = new TemporaryItemId(OfflineTolerantItemId.OfflineId.Value);
                ItemRepositoryMock.SetupFindActiveByAsync(tempId, null);
            }

            #endregion Mock Setup

            #region Verify

            public void VerifyRemoveItemFromBasketWithItemId()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                TestPropertyNotSetException.ThrowIfNull(_item);
                _shoppingListMock.VerifyRemoveItemFromBasketOnce(_item.Id, ItemTypeId);
            }

            public void VerifyRemoveItemFromBasketWithCommandActualId()
            {
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId);
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId.ActualId);
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                _shoppingListMock.VerifyRemoveItemFromBasketOnce(
                    new ItemId(OfflineTolerantItemId.ActualId.Value),
                    ItemTypeId);
            }

            public void VerifyStoringShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                ShoppingListRepositoryMock.VerifyStoreAsyncOnce(_shoppingListMock.Object);
            }

            #endregion Verify

            #region Aggregates

            public void SetupWithValidOfflineId()
            {
                SetupShoppingListId();
                SetupTemporaryItemId();
                SetupItemTypeIdNull();
                SetupItem();
                SetupShoppingListMock();
                SetupFindingShoppingList();
                SetupFindingItemByOfflineId();
                SetupStoringShoppingList();
            }

            public void SetupWithActualId()
            {
                SetupShoppingListId();
                SetupActualItemId();
                SetupItemTypeId();
                SetupShoppingListMock();
                SetupFindingShoppingList();
                SetupStoringShoppingList();
            }

            #endregion Aggregates
        }
    }

    public class PutItemInBasketAsyncTests
    {
        private readonly PutItemInBasketAsyncFixture _fixture;

        public PutItemInBasketAsyncTests()
        {
            _fixture = new PutItemInBasketAsyncFixture();
        }

        [Fact]
        public async Task PutItemInBasketAsync_WithInvalidShoppingListId_ShouldThrowDomainException()
        {
            // Arrange
            var handler = _fixture.CreateSut();
            _fixture.SetupShoppingListId();
            _fixture.SetupTemporaryItemId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupShoppingListRepositoryFindingNoList();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OfflineTolerantItemId);

            // Act
            Func<Task> function = async () => await handler.PutItemInBasketAsync(_fixture.ShoppingListId,
                _fixture.OfflineTolerantItemId, _fixture.ItemTypeId);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ShoppingListNotFound);
            }
        }

        [Fact]
        public async Task PutItemInBasketAsync_WithInvalidOfflineId_ShouldThrowDomainException()
        {
            // Arrange
            var handler = _fixture.CreateSut();
            _fixture.SetupShoppingListId();
            _fixture.SetupTemporaryItemId();
            _fixture.SetupItemTypeIdNull();

            _fixture.SetupShoppingListMock();

            _fixture.SetupShoppingListRepositoryFindBy();
            _fixture.SetupItemRepositoryFindingNoItem();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OfflineTolerantItemId);

            // Act
            Func<Task> function = async () => await handler.PutItemInBasketAsync(_fixture.ShoppingListId,
                _fixture.OfflineTolerantItemId, _fixture.ItemTypeId);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound);
            }
        }

        [Fact]
        public async Task PutItemInBasketAsync_WithActualId_ShouldPutItemInBasket()
        {
            // Arrange
            var handler = _fixture.CreateSut();
            _fixture.SetupShoppingListId();
            _fixture.SetupActualItemId();
            _fixture.SetupItemTypeId();

            _fixture.SetupShoppingListMock();
            _fixture.SetupShoppingListRepositoryFindBy();
            _fixture.SetupStoringShoppingList();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OfflineTolerantItemId);

            // Act
            await handler.PutItemInBasketAsync(_fixture.ShoppingListId, _fixture.OfflineTolerantItemId,
                _fixture.ItemTypeId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyPutItemInBasketWithOfflineIdOnce();
                _fixture.VerifyStoreAsync();
            }
        }

        [Fact]
        public async Task PutItemInBasketAsync_WithValidOfflineId_ShouldPutItemInBasket()
        {
            // Arrange
            var handler = _fixture.CreateSut();
            _fixture.SetupShoppingListId();
            _fixture.SetupTemporaryItemId();
            _fixture.SetupItemTypeIdNull();

            _fixture.SetupItem();
            _fixture.SetupShoppingListMock();

            _fixture.SetupShoppingListRepositoryFindBy();
            _fixture.SetupItemRepositoryFindBy();
            _fixture.SetupStoringShoppingList();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OfflineTolerantItemId);

            // Act
            await handler.PutItemInBasketAsync(_fixture.ShoppingListId, _fixture.OfflineTolerantItemId,
                _fixture.ItemTypeId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyItemRepositoryFindByWithTemporaryItemId();
                _fixture.VerifyPutItemInBasketOnce();
                _fixture.VerifyStoreAsync();
            }
        }

        private sealed class PutItemInBasketAsyncFixture : ShoppingListModificationServiceFixture
        {
            private ShoppingListMock? _shoppingListMock;
            private IItem? _item;

            public ShoppingListId ShoppingListId { get; private set; }
            public ItemTypeId? ItemTypeId { get; private set; }
            public OfflineTolerantItemId? OfflineTolerantItemId { get; private set; }

            public void SetupActualItemId()
            {
                OfflineTolerantItemId = OfflineTolerantItemId.FromActualId(Guid.NewGuid());
            }

            public void SetupTemporaryItemId()
            {
                OfflineTolerantItemId = OfflineTolerantItemId.FromOfflineId(Guid.NewGuid());
            }

            public void SetupShoppingListId()
            {
                ShoppingListId = ShoppingListId.New;
            }

            public void SetupItemTypeId()
            {
                ItemTypeId = DomainModels.ItemTypeId.New;
            }

            public void SetupItemTypeIdNull()
            {
                ItemTypeId = null;
            }

            public void SetupShoppingListMock()
            {
                _shoppingListMock = new ShoppingListMock(ShoppingListMother.Sections(2).Create());
            }

            public void SetupItem()
            {
                _item = ItemMother.Initial().Create();
            }

            #region Fixture Setup

            public void SetupShoppingListRepositoryFindBy()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, _shoppingListMock.Object);
            }

            public void SetupShoppingListRepositoryFindingNoList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, null);
            }

            public void SetupStoringShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                ShoppingListRepositoryMock.SetupStoreAsync(_shoppingListMock.Object);
            }

            public void SetupItemRepositoryFindBy()
            {
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId);
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId.OfflineId);
                var itemId = new TemporaryItemId(OfflineTolerantItemId.OfflineId.Value);
                ItemRepositoryMock.SetupFindActiveByAsync(itemId, _item);
            }

            public void SetupItemRepositoryFindingNoItem()
            {
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId);
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId.OfflineId);
                var itemId = new TemporaryItemId(OfflineTolerantItemId.OfflineId.Value);
                ItemRepositoryMock.SetupFindActiveByAsync(itemId, null);
            }

            #endregion Fixture Setup

            #region Verify

            public void VerifyStoreAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                ShoppingListRepositoryMock.VerifyStoreAsyncOnce(_shoppingListMock.Object);
            }

            public void VerifyPutItemInBasketOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                _shoppingListMock.VerifyPutItemInBasket(_item.Id, ItemTypeId, Times.Once);
            }

            public void VerifyPutItemInBasketWithOfflineIdOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId);
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId.ActualId);
                _shoppingListMock.VerifyPutItemInBasket(new ItemId(OfflineTolerantItemId.ActualId.Value),
                    ItemTypeId, Times.Once);
            }

            public void VerifyItemRepositoryFindByWithTemporaryItemId()
            {
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId);
                TestPropertyNotSetException.ThrowIfNull(OfflineTolerantItemId.OfflineId);
                var itemId = new TemporaryItemId(OfflineTolerantItemId.OfflineId.Value);
                ItemRepositoryMock.VerifyFindByAsync(itemId);
            }

            #endregion Verify
        }
    }

    public class FinishAsyncTests
    {
        private readonly FinishAsyncFixture _fixture;

        public FinishAsyncTests()
        {
            _fixture = new FinishAsyncFixture();
        }

        [Fact]
        public async Task HandleAsync_WithInvalidShoppingListId_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupShoppingListId();
            _fixture.SetupCompletionDate();
            _fixture.SetupNotFindingShoppingList();

            var sut = _fixture.CreateSut();

            // Act
            Func<Task> function = async () =>
                await sut.FinishAsync(_fixture.ShoppingListId, _fixture.CompletionDate);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ShoppingListNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidData_ShouldFinishShoppingList()
        {
            // Arrange
            _fixture.SetupShoppingListId();
            _fixture.SetupCompletionDate();
            _fixture.SetupShoppingListMock();
            _fixture.SetupCreatedShoppingListMock();
            _fixture.SetupFindingShoppingList();
            _fixture.SetupFinishingShoppingList();
            _fixture.SetupStoringShoppingListMock();
            _fixture.SetupStoringCreatedShoppingListMock();

            var sut = _fixture.CreateSut();

            // Act
            await sut.FinishAsync(_fixture.ShoppingListId, _fixture.CompletionDate);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyFinishingShoppingList();
                _fixture.VerifyStoringShoppingList();
                _fixture.VerifyStoringCreatedShoppingList();
            }
        }

        private sealed class FinishAsyncFixture : ShoppingListModificationServiceFixture
        {
            private ShoppingListMock? _shoppingListMock;
            private ShoppingListMock? _createdShoppingListMock;

            public ShoppingListId ShoppingListId { get; private set; }

            public DateTimeOffset CompletionDate { get; private set; }

            public void SetupShoppingListId()
            {
                ShoppingListId = ShoppingListId.New;
            }

            public void SetupCompletionDate()
            {
                CompletionDate = DateTimeOffset.UtcNow;
            }

            public void SetupShoppingListMock()
            {
                _shoppingListMock =
                    new ShoppingListMock(ShoppingListMother.Initial().Create(), MockBehavior.Strict);
            }

            public void SetupCreatedShoppingListMock()
            {
                _createdShoppingListMock =
                    new ShoppingListMock(ShoppingListMother.Initial().Create(), MockBehavior.Strict);
            }

            public void SetupFindingShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, _shoppingListMock.Object);
            }

            public void SetupNotFindingShoppingList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, null);
            }

            public void SetupFinishingShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_createdShoppingListMock);
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                _shoppingListMock.SetupFinish(CompletionDate, DateTimeServiceMock.Object, _createdShoppingListMock.Object);
            }

            public void SetupStoringShoppingListMock()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                ShoppingListRepositoryMock.SetupStoreAsync(_shoppingListMock.Object);
            }

            public void SetupStoringCreatedShoppingListMock()
            {
                TestPropertyNotSetException.ThrowIfNull(_createdShoppingListMock);
                ShoppingListRepositoryMock.SetupStoreAsync(_createdShoppingListMock.Object);
            }

            public void VerifyFinishingShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                _shoppingListMock.VerifyFinish(CompletionDate, DateTimeServiceMock.Object, Times.Once);
            }

            public void VerifyStoringShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingListMock);
                ShoppingListRepositoryMock.VerifyStoreAsync(_shoppingListMock.Object, Times.Once);
            }

            public void VerifyStoringCreatedShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_createdShoppingListMock);
                ShoppingListRepositoryMock.VerifyStoreAsync(_createdShoppingListMock.Object, Times.Once);
            }
        }
    }

    public class RemoveSectionAsyncTests
    {
        private readonly RemoveSectionAsyncFixture _fixture;

        public RemoveSectionAsyncTests()
        {
            _fixture = new RemoveSectionAsyncFixture();
        }

        [Fact]
        public async Task RemoveSectionAsync_WithNotFindingStore_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupSectionId();
            _fixture.SetupNotFindingStore();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.SectionId);

            // Act
            var func = async () => await sut.RemoveSectionAsync(_fixture.SectionId.Value);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.StoreNotFound);
        }

        [Fact]
        public async Task RemoveSectionAsync_WithNotFindingShoppingList_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupSectionId();
            _fixture.SetupStore();
            _fixture.SetupFindingStore();
            _fixture.SetupNotFindingShoppingList();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.SectionId);

            // Act
            var func = async () => await sut.RemoveSectionAsync(_fixture.SectionId.Value);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ShoppingListNotFound);
        }

        private sealed class RemoveSectionAsyncFixture : ShoppingListModificationServiceFixture
        {
            private StoreMock? _storeMock;
            public SectionId? SectionId { get; private set; }

            public void SetupSectionId()
            {
                SectionId = Domain.Stores.Models.SectionId.New;
            }

            public void SetupStore()
            {
                _storeMock = new StoreMock(MockBehavior.Strict, new StoreBuilder().Create());
            }

            public void SetupNotFindingStore()
            {
                TestPropertyNotSetException.ThrowIfNull(SectionId);

                StoreRepositoryMock.SetupFindActiveByAsync(SectionId.Value, null);
            }

            public void SetupFindingStore()
            {
                TestPropertyNotSetException.ThrowIfNull(SectionId);
                TestPropertyNotSetException.ThrowIfNull(_storeMock);

                StoreRepositoryMock.SetupFindActiveByAsync(SectionId.Value, _storeMock.Object);
            }

            public void SetupNotFindingShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_storeMock);

                ShoppingListRepositoryMock.SetupFindActiveByAsync(_storeMock.Object.Id, null);
            }
        }
    }

    private abstract class ShoppingListModificationServiceFixture
    {
        protected readonly AddItemToShoppingListServiceMock AddItemToShoppingListServiceMock = new(MockBehavior.Strict);
        protected readonly ShoppingListRepositoryMock ShoppingListRepositoryMock = new(MockBehavior.Strict);
        protected readonly ItemRepositoryMock ItemRepositoryMock = new(MockBehavior.Strict);
        protected readonly StoreRepositoryMock StoreRepositoryMock = new(MockBehavior.Strict);
        protected readonly ShoppingListSectionFactoryMock ShoppingListSectionFactoryMock = new(MockBehavior.Strict);
        protected readonly ItemFactoryMock ItemFactoryMock = new(MockBehavior.Strict);
        protected readonly DateTimeServiceMock DateTimeServiceMock = new(MockBehavior.Strict);

        public ShoppingListModificationService CreateSut()
        {
            return new ShoppingListModificationService(
                AddItemToShoppingListServiceMock.Object,
                ShoppingListRepositoryMock.Object,
                ItemRepositoryMock.Object,
                StoreRepositoryMock.Object,
                ShoppingListSectionFactoryMock.Object,
                ItemFactoryMock.Object,
                DateTimeServiceMock.Object);
        }
    }
}