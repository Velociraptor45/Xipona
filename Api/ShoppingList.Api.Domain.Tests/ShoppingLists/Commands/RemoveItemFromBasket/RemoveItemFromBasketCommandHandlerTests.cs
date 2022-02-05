using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.RemoveItemFromBasket;

public class RemoveItemFromBasketCommandHandlerTests
{
    private readonly LocalFixture _local;

    public RemoveItemFromBasketCommandHandlerTests()
    {
        _local = new LocalFixture();
    }

    [Fact]
    public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var handler = _local.CreateSut();

        // Act
        Func<Task> function = async () => await handler.HandleAsync(null, default);

        // Assert
        using (new AssertionScope())
        {
            await function.Should().ThrowAsync<ArgumentNullException>();
        }
    }

    [Fact]
    public async Task HandleAsync_WithInvalidOfflineId_ShouldThrowDomainException()
    {
        // Arrange
        _local.SetupCommandWithOfflineId();
        _local.SetupShoppingListMock();
        _local.SetupFindingShoppingList();
        _local.SetupFindingNoItemByOfflineId();
        var handler = _local.CreateSut();

        // Act
        Func<Task> function = async () => await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            await function.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemNotFound);
        }
    }

    [Fact]
    public async Task HandleAsync_WithInvalidListId_ShouldThrowDomainException()
    {
        // Arrange
        _local.SetupCommandWithOfflineId();
        _local.SetupFindingNoShoppingList();
        var handler = _local.CreateSut();

        // Act
        Func<Task> function = async () => await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            await function.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ShoppingListNotFound);
        }
    }

    #region WithActualId

    [Fact]
    public async Task HandleAsync_WithActualId_ShouldReturnTrue()
    {
        // Arrange
        _local.SetupWithActualId();
        var handler = _local.CreateSut();

        // Act
        bool result = await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleAsync_WithActualId_ShouldRemoveItemFromBasket()
    {
        // Arrange
        _local.SetupWithActualId();
        var handler = _local.CreateSut();

        // Act
        await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            _local.VerifyRemoveItemFromBasketWithCommandActualId();
        }
    }

    [Fact]
    public async Task HandleAsync_WithActualId_ShouldStoreShoppingList()
    {
        // Arrange
        _local.SetupWithActualId();
        var handler = _local.CreateSut();

        // Act
        await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            _local.VerifyStoringShoppingList();
        }
    }

    #endregion WithActualId

    #region WithValidOfflineId

    [Fact]
    public async Task HandleAsync_WithValidOfflineId_ShouldReturnTrue()
    {
        // Arrange
        _local.SetupWithValidOfflineId();
        var handler = _local.CreateSut();

        // Act
        bool result = await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleAsync_WithValidOfflineId_ShouldRemoveItemFromBasket()
    {
        // Arrange
        _local.SetupWithValidOfflineId();
        var handler = _local.CreateSut();

        // Act
        await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            _local.VerifyRemoveItemFromBasketWithStoreItemId();
        }
    }

    [Fact]
    public async Task HandleAsync_WithValidOfflineId_ShouldStoreShoppingList()
    {
        // Arrange
        _local.SetupWithValidOfflineId();
        var handler = _local.CreateSut();

        // Act
        await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            _local.VerifyStoringShoppingList();
        }
    }

    #endregion WithValidOfflineId

    private sealed class LocalFixture
    {
        private readonly Fixture _fixture;
        private readonly CommonFixture _commonFixture = new CommonFixture();
        private readonly ShoppingListRepositoryMock _shoppingListRepositoryMock;
        private readonly ItemRepositoryMock _itemRepositoryMock;
        private IStoreItem _storeItem;
        private ShoppingListMock _shoppingListMock;

        public LocalFixture()
        {
            _fixture = _commonFixture.GetNewFixture();

            _shoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
            _itemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
        }

        public RemoveItemFromBasketCommand Command { get; private set; }

        public void SetupCommandWithActualId()
        {
            SetupCommand(new OfflineTolerantItemId(_commonFixture.NextInt()));
        }

        public void SetupCommandWithOfflineId()
        {
            SetupCommand(new OfflineTolerantItemId(Guid.NewGuid()));
        }

        private void SetupCommand(OfflineTolerantItemId id)
        {
            _fixture.ConstructorArgumentFor<RemoveItemFromBasketCommand, OfflineTolerantItemId>("itemId", id);
            if (!id.IsActualId)
                _fixture.ConstructorArgumentFor<RemoveItemFromBasketCommand, ItemTypeId?>("itemTypeId", null);

            Command = _fixture.Create<RemoveItemFromBasketCommand>();
        }

        public RemoveItemFromBasketCommandHandler CreateSut()
        {
            return new RemoveItemFromBasketCommandHandler(
                _shoppingListRepositoryMock.Object,
                _itemRepositoryMock.Object);
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
            _shoppingListRepositoryMock.SetupFindByAsync(Command.ShoppingListId, _shoppingListMock.Object);
        }

        public void SetupFindingNoShoppingList()
        {
            _shoppingListRepositoryMock.SetupFindByAsync(Command.ShoppingListId, null);
        }

        public void SetupStoringShoppingList()
        {
            _shoppingListRepositoryMock.SetupStoreAsync(_shoppingListMock.Object);
        }

        public void SetupFindingItemByOfflineId()
        {
            var tempId = new TemporaryItemId(Command.OfflineTolerantItemId.OfflineId.Value);
            _itemRepositoryMock.SetupFindByAsync(tempId, _storeItem);
        }

        public void SetupFindingNoItemByOfflineId()
        {
            var tempId = new TemporaryItemId(Command.OfflineTolerantItemId.OfflineId.Value);
            _itemRepositoryMock.SetupFindByAsync(tempId, null);
        }

        #endregion Mock Setup

        #region Verify

        public void VerifyRemoveItemFromBasketWithStoreItemId()
        {
            _shoppingListMock.VerifyRemoveItemFromBasketOnce(_storeItem.Id, Command.ItemTypeId);
        }

        public void VerifyRemoveItemFromBasketWithCommandActualId()
        {
            _shoppingListMock.VerifyRemoveItemFromBasketOnce(
                new ItemId(Command.OfflineTolerantItemId.ActualId.Value),
                Command.ItemTypeId);
        }

        public void VerifyStoringShoppingList()
        {
            _shoppingListRepositoryMock.VerifyStoreAsyncOnce(_shoppingListMock.Object);
        }

        #endregion Verify

        #region Aggregates

        public void SetupWithValidOfflineId()
        {
            SetupCommandWithOfflineId();
            SetupItem();
            SetupShoppingListMock();
            SetupFindingShoppingList();
            SetupFindingItemByOfflineId();
            SetupStoringShoppingList();
        }

        public void SetupWithActualId()
        {
            SetupCommandWithActualId();
            SetupShoppingListMock();
            SetupFindingShoppingList();
            SetupStoringShoppingList();
        }

        #endregion Aggregates
    }
}