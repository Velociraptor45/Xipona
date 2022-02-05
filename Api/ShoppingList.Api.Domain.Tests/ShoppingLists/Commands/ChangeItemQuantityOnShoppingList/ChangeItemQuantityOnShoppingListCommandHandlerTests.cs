using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;

public class ChangeItemQuantityOnShoppingListCommandHandlerTests
{
    private readonly LocalFixture _local;

    public ChangeItemQuantityOnShoppingListCommandHandlerTests()
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
    public async Task HandleAsync_WithInvalidShoppingListId_ShouldThrowDomainException()
    {
        // Arrange
        var handler = _local.CreateSut();
        _local.SetupTemporaryItemId();
        _local.SetupCommand();

        _local.SetupShoppingListRepositoryFindingNoList();

        // Act
        Func<Task> function = async () => await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            (await function.Should().ThrowAsync<DomainException>())
                .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ShoppingListNotFound);
        }
    }

    [Fact]
    public async Task HandleAsync_WithInvalidOfflineId_ShouldThrowDomainException()
    {
        // Arrange
        var handler = _local.CreateSut();
        _local.SetupTemporaryItemId();
        _local.SetupCommand();

        _local.SetupShoppingListMock();

        _local.SetupShoppingListRepositoryFindBy();
        _local.SetupItemRepositoryFindingNoItem();

        // Act
        Func<Task> function = async () => await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            (await function.Should().ThrowAsync<DomainException>())
                .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound);
        }
    }

    [Fact]
    public async Task HandleAsync_WithActualId_ShouldChangeItemQuantityAndStoreIt()
    {
        // Arrange
        var handler = _local.CreateSut();
        _local.SetupCommand();
        _local.SetupShoppingListMock();
        _local.SetupShoppingListRepositoryFindBy();
        _local.SetupStoringShoppingList();

        // Act
        bool result = await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeTrue();
            _local.VerifyChangeItemQuantityWithOfflineIdOnce();
            _local.VerifyStoreAsync();
        }
    }

    [Fact]
    public async Task HandleAsync_WithOfflineId_ShouldChangeItemQuantityAndStoreIt()
    {
        // Arrange
        var handler = _local.CreateSut();
        _local.SetupTemporaryItemId();
        _local.SetupCommand();
        _local.SetupShoppingListMock();
        _local.SetupStoreItem();

        _local.SetupShoppingListRepositoryFindBy();
        _local.SetupItemRepositoryFindBy();
        _local.SetupStoringShoppingList();

        // Act
        bool result = await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeTrue();
            _local.VerifyChangeItemQuantityOnce();
            _local.VerifyStoreAsync();
        }
    }

    private sealed class LocalFixture
    {
        private readonly Fixture _fixture;
        private readonly CommonFixture _commonFixture = new CommonFixture();
        private readonly ShoppingListRepositoryMock _shoppingListRepositoryMock;
        private readonly ItemRepositoryMock _itemRepositoryMock;
        private ShoppingListMock _shoppingListMock;
        private TemporaryItemId? _temporaryItemId;
        private IStoreItem _storeItem;

        public LocalFixture()
        {
            _fixture = _commonFixture.GetNewFixture();

            _shoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
            _itemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
        }

        public ChangeItemQuantityOnShoppingListCommand Command { get; private set; }

        public void SetupCommand()
        {
            OfflineTolerantItemId offlineTolerantItemId;
            if (_temporaryItemId == null)
            {
                offlineTolerantItemId = new OfflineTolerantItemId(_commonFixture.NextInt());
            }
            else
            {
                _fixture.ConstructorArgumentFor<ChangeItemQuantityOnShoppingListCommand, ItemTypeId?>("itemTypeId",
                    null);
                offlineTolerantItemId = new OfflineTolerantItemId(_temporaryItemId.Value.Value);
            }

            _fixture.ConstructorArgumentFor<ChangeItemQuantityOnShoppingListCommand, OfflineTolerantItemId>("itemId",
                offlineTolerantItemId);

            Command = _fixture.Create<ChangeItemQuantityOnShoppingListCommand>();
        }

        public ChangeItemQuantityOnShoppingListCommandHandler CreateSut()
        {
            return new ChangeItemQuantityOnShoppingListCommandHandler(
                _shoppingListRepositoryMock.Object,
                _itemRepositoryMock.Object);
        }

        public void SetupTemporaryItemId()
        {
            _temporaryItemId = TemporaryItemId.New;
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
            _shoppingListRepositoryMock.SetupFindByAsync(Command.ShoppingListId, _shoppingListMock.Object);
        }

        public void SetupShoppingListRepositoryFindingNoList()
        {
            _shoppingListRepositoryMock.SetupFindByAsync(Command.ShoppingListId, null);
        }

        public void SetupStoringShoppingList()
        {
            _shoppingListRepositoryMock.SetupStoreAsync(_shoppingListMock.Object);
        }

        public void SetupItemRepositoryFindBy()
        {
            _itemRepositoryMock.SetupFindByAsync(_temporaryItemId.Value, _storeItem);
        }

        public void SetupItemRepositoryFindingNoItem()
        {
            _itemRepositoryMock.SetupFindByAsync(_temporaryItemId.Value, null);
        }

        #endregion Fixture Setup

        #region Verify

        public void VerifyStoreAsync()
        {
            _shoppingListRepositoryMock.VerifyStoreAsyncOnce(_shoppingListMock.Object);
        }

        public void VerifyChangeItemQuantityOnce()
        {
            _shoppingListMock.VerifyChangeItemQuantityOnce(_storeItem.Id, Command.ItemTypeId, Command.Quantity);
        }

        public void VerifyChangeItemQuantityWithOfflineIdOnce()
        {
            _shoppingListMock.VerifyChangeItemQuantityOnce(
                new ItemId(Command.OfflineTolerantItemId.ActualId.Value),
                Command.ItemTypeId,
                Command.Quantity);
        }

        #endregion Verify
    }
}