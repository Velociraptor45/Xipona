using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Services;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.AddItemToShoppingList;

public class AddItemToShoppingListCommandHandlerTests
{
    private readonly LocalFixture _fixture;

    public AddItemToShoppingListCommandHandlerTests()
    {
        _fixture = new LocalFixture();
    }

    [Fact]
    public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var handler = _fixture.CreateSut();

        // Act
        Func<Task<bool>> action = async () => await handler.HandleAsync(null, default);

        // Assert
        using (new AssertionScope())
        {
            await action.Should().ThrowAsync<ArgumentNullException>().WithMessage("*command*");
        }
    }

    [Fact]
    public async Task HandleAsync_WithInvalidShoppingListId_ShouldThrowDomainException()
    {
        // Arrange
        var handler = _fixture.CreateSut();
        _fixture.SetupCommandWithOfflineId();
        _fixture.SetupNotFindingShoppingList();

        // Act
        Func<Task> function = async () => await handler.HandleAsync(_fixture.Command, default);

        // Assert
        using (new AssertionScope())
        {
            await function.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ShoppingListNotFound);
        }
    }

    [Fact]
    public async Task HandleAsync_WithOfflineId_ShouldAddItemToList()
    {
        // Arrange
        var handler = _fixture.CreateSut();
        _fixture.SetupCommandWithOfflineId();
        _fixture.SetupShoppingList();
        _fixture.SetupFindingShoppingList();
        _fixture.SetupAddingItemToShoppingListWithOfflineId();
        _fixture.SetupStoringShoppingList();

        // Act
        bool result = await handler.HandleAsync(_fixture.Command, default);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeTrue();
            _fixture.VerifyStoringShoppingListOnce();
            _fixture.VerifyAddingItemToShoppingListWithOfflineId();
        }
    }

    [Fact]
    public async Task HandleAsync_WithActualId_ShouldAddItemToList()
    {
        // Arrange
        var handler = _fixture.CreateSut();
        _fixture.SetupCommandWithActualId();
        _fixture.SetupShoppingList();
        _fixture.SetupFindingShoppingList();
        _fixture.SetupAddingItemToShoppingListWithActualId();
        _fixture.SetupStoringShoppingList();

        // Act
        bool result = await handler.HandleAsync(_fixture.Command, default);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeTrue();
            _fixture.VerifyStoringShoppingListOnce();
            _fixture.VerifyAddingItemToShoppingListWithActualId();
        }
    }

    private sealed class LocalFixture
    {
        private ShoppingListMock _shoppingListMock;
        private readonly Fixture _fixture;
        private readonly CommonFixture _commonFixture = new CommonFixture();
        private readonly ShoppingListMockFixture _shoppingListMockFixture;
        private readonly ShoppingListRepositoryMock _shoppingListRepositoryMock;
        private readonly AddItemToShoppingListServiceMock _addItemToShoppingListServiceMock;

        public LocalFixture()
        {
            _fixture = _commonFixture.GetNewFixture();

            _shoppingListMockFixture = new ShoppingListMockFixture();

            _shoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
            _addItemToShoppingListServiceMock = new AddItemToShoppingListServiceMock(MockBehavior.Strict);
        }

        public AddItemToShoppingListCommand Command { get; private set; }

        public void SetupCommandWithActualId()
        {
            _fixture.ConstructorArgumentFor<AddItemToShoppingListCommand, OfflineTolerantItemId>("itemId",
                OfflineTolerantItemId.FromActualId(Guid.NewGuid()));

            Command = _fixture.Create<AddItemToShoppingListCommand>();
        }

        public void SetupCommandWithOfflineId()
        {
            _fixture.ConstructorArgumentFor<AddItemToShoppingListCommand, OfflineTolerantItemId>("itemId",
                OfflineTolerantItemId.FromOfflineId(Guid.NewGuid()));

            Command = _fixture.Create<AddItemToShoppingListCommand>();
        }

        public AddItemToShoppingListCommandHandler CreateSut()
        {
            return new AddItemToShoppingListCommandHandler(
                _shoppingListRepositoryMock.Object,
                _addItemToShoppingListServiceMock.Object);
        }

        public void SetupShoppingList()
        {
            _shoppingListMock = _shoppingListMockFixture.Create();
        }

        public void SetupFindingShoppingList()
        {
            _shoppingListRepositoryMock.SetupFindByAsync(Command.ShoppingListId, _shoppingListMock.Object);
        }

        public void SetupNotFindingShoppingList()
        {
            _shoppingListRepositoryMock.SetupFindByAsync(Command.ShoppingListId, null);
        }

        public void SetupAddingItemToShoppingListWithActualId()
        {
            _addItemToShoppingListServiceMock.SetupAddItemToShoppingList(
                _shoppingListMock.Object,
                new ItemId(Command.ItemId.ActualId.Value),
                Command.SectionId,
                Command.Quantity);
        }

        public void SetupAddingItemToShoppingListWithOfflineId()
        {
            _addItemToShoppingListServiceMock.SetupAddItemToShoppingList(
                _shoppingListMock.Object,
                new TemporaryItemId(Command.ItemId.OfflineId.Value),
                Command.SectionId,
                Command.Quantity);
        }

        public void SetupStoringShoppingList()
        {
            _shoppingListRepositoryMock.SetupStoreAsync(_shoppingListMock.Object);
        }

        public void VerifyAddingItemToShoppingListWithActualId()
        {
            _addItemToShoppingListServiceMock.VerifyAddItemToShoppingListOnce(
                _shoppingListMock.Object,
                new ItemId(Command.ItemId.ActualId.Value),
                Command.SectionId,
                Command.Quantity);
        }

        public void VerifyAddingItemToShoppingListWithOfflineId()
        {
            _addItemToShoppingListServiceMock.VerifyAddItemToShoppingListOnce(
                _shoppingListMock.Object,
                new TemporaryItemId(Command.ItemId.OfflineId.Value),
                Command.SectionId,
                Command.Quantity);
        }

        public void VerifyStoringShoppingListOnce()
        {
            _shoppingListRepositoryMock.VerifyStoreAsyncOnce(_shoppingListMock.Object);
        }
    }
}