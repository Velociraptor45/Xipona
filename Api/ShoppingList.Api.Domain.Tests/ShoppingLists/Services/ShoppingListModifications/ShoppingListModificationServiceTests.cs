using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Services.ShoppingListModifications;

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
        public async Task ChangeItemQuantityAsync_WithOfflineTollerantItemIdNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var sut = _fixture.CreateSut();

            // Act
            Func<Task> function = async () => await sut.ChangeItemQuantityAsync(_fixture.ShoppingListId,
                null, _fixture.ItemTypeId, _fixture.Quantity);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ArgumentNullException>().WithMessage("*offlineTolerantItemId*");
            }
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
            _fixture.SetupStoreItem();

            _fixture.SetupShoppingListRepositoryFindBy();
            _fixture.SetupFindingTemporaryItem();
            _fixture.SetupStoringShoppingList();

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

        private sealed class ChangeItemQuantityAsyncFixture : LocalFixture
        {
            private ShoppingListMock _shoppingListMock;
            private IStoreItem _storeItem;

            public ShoppingListId ShoppingListId { get; private set; }
            public ItemTypeId? ItemTypeId { get; private set; }
            public OfflineTolerantItemId OfflineTolerantItemId { get; private set; }
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
                ItemTypeId = Domain.StoreItems.Models.ItemTypeId.New;
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

            public void SetupStoreItem()
            {
                _storeItem = StoreItemMother.Initial().Create();
            }

            #region Fixture Setup

            public void SetupShoppingListRepositoryFindBy()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, _shoppingListMock.Object);
            }

            public void SetupShoppingListRepositoryFindingNoList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, null);
            }

            public void SetupStoringShoppingList()
            {
                ShoppingListRepositoryMock.SetupStoreAsync(_shoppingListMock.Object);
            }

            public void SetupFindingTemporaryItem()
            {
                var itemId = new TemporaryItemId(OfflineTolerantItemId.OfflineId.Value);
                ItemRepositoryMock.SetupFindByAsync(itemId, _storeItem);
            }

            public void SetupNotFindingTemporaryItem()
            {
                var itemId = new TemporaryItemId(OfflineTolerantItemId.OfflineId.Value);
                ItemRepositoryMock.SetupFindByAsync(itemId, null);
            }

            #endregion Fixture Setup

            #region Verify

            public void VerifyStoreAsync()
            {
                ShoppingListRepositoryMock.VerifyStoreAsyncOnce(_shoppingListMock.Object);
            }

            public void VerifyChangeItemQuantityOnce()
            {
                _shoppingListMock.VerifyChangeItemQuantityOnce(_storeItem.Id, ItemTypeId, Quantity);
            }

            public void VerifyChangingItemQuantity()
            {
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
        public async Task RemoveItemAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupShoppingListId();
            _fixture.SetupItemTypeId();

            // Act
            Func<Task> function = async () =>
                await sut.RemoveItemAsync(_fixture.ShoppingListId, null, _fixture.ItemTypeId);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ArgumentNullException>();
            }
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
                    _fixture.VerifyStoreItemOnce();
                }
                else
                {
                    _fixture.VerifyDeleteItemNever();
                    _fixture.VerifyStoreItemNever();
                }
            }
        }

        private sealed class RemoveItemAsyncFixture : LocalFixture
        {
            public ShoppingListId ShoppingListId { get; private set; }
            public ItemTypeId? ItemTypeId { get; private set; }
            public OfflineTolerantItemId OfflineTolerantItemId { get; private set; }
            private StoreItemMock _itemMock;
            private ShoppingListMock _shoppingListMock;

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
                ItemTypeId = Domain.StoreItems.Models.ItemTypeId.New;
            }

            public void SetupItemTypeIdNull()
            {
                ItemTypeId = null;
            }

            public void SetupTemporaryItemMock()
            {
                var item = StoreItemMother.InitialTemporary().Create();
                _itemMock = new StoreItemMock(item, MockBehavior.Strict);
            }

            public void SetupItemMock()
            {
                var item = StoreItemMother.Initial().Create();
                _itemMock = new StoreItemMock(item, MockBehavior.Strict);
            }

            public void SetupShoppingListMock()
            {
                _shoppingListMock = new ShoppingListMock(ShoppingListMother.Sections(3).Create());
            }

            #region Mock Setup

            public void SetupDeletingItem()
            {
                _itemMock.SetupDelete();
            }

            public void SetupFindingItemByActualId()
            {
                ItemRepositoryMock.SetupFindByAsync(new ItemId(OfflineTolerantItemId.ActualId.Value),
                    _itemMock.Object);
            }

            public void SetupFindingNoItemByActualId()
            {
                ItemRepositoryMock.SetupFindByAsync(new ItemId(OfflineTolerantItemId.ActualId.Value), null);
            }

            public void SetupFindingItemByTemporaryId()
            {
                ItemRepositoryMock.SetupFindByAsync(new TemporaryItemId(OfflineTolerantItemId.OfflineId.Value),
                    _itemMock.Object);
            }

            public void SetupFindingNoItemByTemporaryId()
            {
                ItemRepositoryMock.SetupFindByAsync(new TemporaryItemId(OfflineTolerantItemId.OfflineId.Value),
                    null);
            }

            public void SetupStoringItem()
            {
                ItemRepositoryMock.SetupStoreAsync(_itemMock.Object, _itemMock.Object);
            }

            public void SetupFindingShoppingList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, _shoppingListMock.Object);
            }

            public void SetupFindingNoShoppingList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, null);
            }

            public void SetupStoringShoppingList()
            {
                ShoppingListRepositoryMock.SetupStoreAsync(_shoppingListMock.Object);
            }

            #endregion Mock Setup

            #region Verify

            public void VerifyRemoveItemOnce()
            {
                _shoppingListMock.VerifyRemoveItem(_itemMock.Object.Id, ItemTypeId, Times.Once);
            }

            public void VerifyDeleteItemOnce()
            {
                _itemMock.VerifyDeleteOnce();
            }

            public void VerifyDeleteItemNever()
            {
                _itemMock.VerifyDeleteNever();
            }

            public void VerifyStoreItemOnce()
            {
                ItemRepositoryMock.VerifyStoreAsyncOnce(_itemMock.Object);
            }

            public void VerifyStoreItemNever()
            {
                ItemRepositoryMock.VerifyStoreAsyncNever();
            }

            public void VerifyStoreShoppingListOnce()
            {
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
        public async Task RemoveItemFromBasketAsync_WithOfflineTolerantItemIdNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var sut = _fixture.CreateSut();

            // Act
            Func<Task> function = async () => await sut.RemoveItemFromBasketAsync(_fixture.ShoppingListId,
                null, _fixture.ItemTypeId);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ArgumentNullException>().WithMessage("*offlineTolerantItemId*");
            }
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

            // Act
            await sut.RemoveItemFromBasketAsync(_fixture.ShoppingListId, _fixture.OfflineTolerantItemId,
                _fixture.ItemTypeId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyRemoveItemFromBasketWithStoreItemId();
            }
        }

        [Fact]
        public async Task RemoveItemFromBasketAsync_WithValidOfflineId_ShouldStoreShoppingList()
        {
            // Arrange
            _fixture.SetupWithValidOfflineId();
            var sut = _fixture.CreateSut();

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

        private sealed class RemoveItemFromBasketAsyncFixture : LocalFixture
        {
            private IStoreItem _storeItem;
            private ShoppingListMock _shoppingListMock;

            public ShoppingListId ShoppingListId { get; private set; }
            public ItemTypeId? ItemTypeId { get; private set; }
            public OfflineTolerantItemId OfflineTolerantItemId { get; private set; }

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
                ItemTypeId = Domain.StoreItems.Models.ItemTypeId.New;
            }

            public void SetupItemTypeIdNull()
            {
                ItemTypeId = null;
            }

            public void SetupItem()
            {
                _storeItem = StoreItemMother.Initial().Create();
            }

            public void SetupShoppingListMock()
            {
                _shoppingListMock = new ShoppingListMock(ShoppingListMother.Sections(3).Create());
            }

            #region Mock Setup

            public void SetupFindingShoppingList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, _shoppingListMock.Object);
            }

            public void SetupFindingNoShoppingList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, null);
            }

            public void SetupStoringShoppingList()
            {
                ShoppingListRepositoryMock.SetupStoreAsync(_shoppingListMock.Object);
            }

            public void SetupFindingItemByOfflineId()
            {
                var tempId = new TemporaryItemId(OfflineTolerantItemId.OfflineId.Value);
                ItemRepositoryMock.SetupFindByAsync(tempId, _storeItem);
            }

            public void SetupFindingNoItemByOfflineId()
            {
                var tempId = new TemporaryItemId(OfflineTolerantItemId.OfflineId.Value);
                ItemRepositoryMock.SetupFindByAsync(tempId, null);
            }

            #endregion Mock Setup

            #region Verify

            public void VerifyRemoveItemFromBasketWithStoreItemId()
            {
                _shoppingListMock.VerifyRemoveItemFromBasketOnce(_storeItem.Id, ItemTypeId);
            }

            public void VerifyRemoveItemFromBasketWithCommandActualId()
            {
                _shoppingListMock.VerifyRemoveItemFromBasketOnce(
                    new ItemId(OfflineTolerantItemId.ActualId.Value),
                    ItemTypeId);
            }

            public void VerifyStoringShoppingList()
            {
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
        public async Task PutItemInBasketAsync_WithOfflineTolerantItemIdNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var handler = _fixture.CreateSut();

            // Act
            Func<Task> function = async () => await handler.PutItemInBasketAsync(_fixture.ShoppingListId, null,
                _fixture.ItemTypeId);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ArgumentNullException>();
            }
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

            _fixture.SetupStoreItem();
            _fixture.SetupShoppingListMock();

            _fixture.SetupShoppingListRepositoryFindBy();
            _fixture.SetupItemRepositoryFindBy();
            _fixture.SetupStoringShoppingList();

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

        private sealed class PutItemInBasketAsyncFixture : LocalFixture
        {
            private ShoppingListMock _shoppingListMock;
            private IStoreItem _storeItem;

            public ShoppingListId ShoppingListId { get; private set; }
            public ItemTypeId? ItemTypeId { get; private set; }
            public OfflineTolerantItemId OfflineTolerantItemId { get; private set; }

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
                ItemTypeId = Domain.StoreItems.Models.ItemTypeId.New;
            }

            public void SetupItemTypeIdNull()
            {
                ItemTypeId = null;
            }

            public void SetupShoppingListMock()
            {
                _shoppingListMock = new ShoppingListMock(ShoppingListMother.Sections(2).Create());
            }

            public void SetupStoreItem()
            {
                _storeItem = StoreItemMother.Initial().Create();
            }

            #region Fixture Setup

            public void SetupShoppingListRepositoryFindBy()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, _shoppingListMock.Object);
            }

            public void SetupShoppingListRepositoryFindingNoList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, null);
            }

            public void SetupStoringShoppingList()
            {
                ShoppingListRepositoryMock.SetupStoreAsync(_shoppingListMock.Object);
            }

            public void SetupItemRepositoryFindBy()
            {
                var itemId = new TemporaryItemId(OfflineTolerantItemId.OfflineId.Value);
                ItemRepositoryMock.SetupFindByAsync(itemId, _storeItem);
            }

            public void SetupItemRepositoryFindingNoItem()
            {
                var itemId = new TemporaryItemId(OfflineTolerantItemId.OfflineId.Value);
                ItemRepositoryMock.SetupFindByAsync(itemId, null);
            }

            #endregion Fixture Setup

            #region Verify

            public void VerifyStoreAsync()
            {
                ShoppingListRepositoryMock.VerifyStoreAsyncOnce(_shoppingListMock.Object);
            }

            public void VerifyPutItemInBasketOnce()
            {
                _shoppingListMock.VerifyPutItemInBasket(_storeItem.Id, ItemTypeId, Times.Once);
            }

            public void VerifyPutItemInBasketWithOfflineIdOnce()
            {
                _shoppingListMock.VerifyPutItemInBasket(new ItemId(OfflineTolerantItemId.ActualId.Value),
                    ItemTypeId, Times.Once);
            }

            public void VerifyItemRepositoryFindByWithTemporaryItemId()
            {
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

        private sealed class FinishAsyncFixture : LocalFixture
        {
            private ShoppingListMock _shoppingListMock;
            private ShoppingListMock _createdShoppingListMock;

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
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, _shoppingListMock.Object);
            }

            public void SetupNotFindingShoppingList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListId, null);
            }

            public void SetupFinishingShoppingList()
            {
                _shoppingListMock.SetupFinish(CompletionDate, _createdShoppingListMock.Object);
            }

            public void SetupStoringShoppingListMock()
            {
                ShoppingListRepositoryMock.SetupStoreAsync(_shoppingListMock.Object);
            }

            public void SetupStoringCreatedShoppingListMock()
            {
                ShoppingListRepositoryMock.SetupStoreAsync(_createdShoppingListMock.Object);
            }

            public void VerifyFinishingShoppingList()
            {
                _shoppingListMock.VerifyFinish(CompletionDate, Times.Once);
            }

            public void VerifyStoringShoppingList()
            {
                ShoppingListRepositoryMock.VerifyStoreAsync(_shoppingListMock.Object, Times.Once);
            }

            public void VerifyStoringCreatedShoppingList()
            {
                ShoppingListRepositoryMock.VerifyStoreAsync(_createdShoppingListMock.Object, Times.Once);
            }
        }
    }

    private abstract class LocalFixture
    {
        protected readonly Fixture Fixture;
        protected readonly ShoppingListRepositoryMock ShoppingListRepositoryMock;
        protected readonly ItemRepositoryMock ItemRepositoryMock;

        protected LocalFixture()
        {
            Fixture = new CommonFixture().GetNewFixture();
            ShoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
            ItemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
        }

        public ShoppingListModificationService CreateSut()
        {
            return new ShoppingListModificationService(
                ShoppingListRepositoryMock.Object,
                ItemRepositoryMock.Object,
                default);
        }
    }
}