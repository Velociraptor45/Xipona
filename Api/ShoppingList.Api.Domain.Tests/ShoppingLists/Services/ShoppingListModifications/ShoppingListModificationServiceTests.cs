using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.ShoppingListModifications;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Services.ShoppingListModifications
{
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
                public float Quantity { get; private set; }

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
                    Quantity = Fixture.Create<float>();
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

        public class RemoveItemAsync
        {
            private readonly RemoveItemAsyncFixture _fixture;

            public RemoveItemAsync()
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

        private abstract class LocalFixture
        {
            protected Fixture Fixture;
            protected ShoppingListRepositoryMock ShoppingListRepositoryMock;
            protected ItemRepositoryMock ItemRepositoryMock;

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
}