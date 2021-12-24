using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
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
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList
{
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
            var handler = _local.CreateCommandHandler();

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
            var handler = _local.CreateCommandHandler();
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
            var handler = _local.CreateCommandHandler();
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
            var handler = _local.CreateCommandHandler();
            _local.SetupCommand();
            _local.SetupShoppingListMock();
            _local.SetupShoppingListRepositoryFindBy();

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
            var handler = _local.CreateCommandHandler();
            _local.SetupTemporaryItemId();
            _local.SetupCommand();
            _local.SetupShoppingListMock();
            _local.SetupStoreItem();

            _local.SetupShoppingListRepositoryFindBy();
            _local.SetupItemRepositoryFindBy();

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
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public ShoppingListRepositoryMock ShoppingListRepositoryMock { get; }
            public ItemRepositoryMock ItemRepositoryMock { get; }

            public ShoppingListMock ShoppingListMock { get; set; }
            public ChangeItemQuantityOnShoppingListCommand Command { get; private set; }
            public TemporaryItemId TemporaryItemId { get; private set; }
            public IStoreItem StoreItem { get; private set; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                ShoppingListRepositoryMock = new ShoppingListRepositoryMock(Fixture);
                ItemRepositoryMock = new ItemRepositoryMock(Fixture);
            }

            public void SetupCommand()
            {
                OfflineTolerantItemId offlineTolerantItemId;
                if (TemporaryItemId == null)
                    offlineTolerantItemId = new OfflineTolerantItemId(CommonFixture.NextInt());
                else
                    offlineTolerantItemId = new OfflineTolerantItemId(TemporaryItemId.Value);

                Fixture.ConstructorArgumentFor<ChangeItemQuantityOnShoppingListCommand, OfflineTolerantItemId>("itemId",
                    offlineTolerantItemId);

                Command = Fixture.Create<ChangeItemQuantityOnShoppingListCommand>();
            }

            public ChangeItemQuantityOnShoppingListCommandHandler CreateCommandHandler()
            {
                return Fixture.Create<ChangeItemQuantityOnShoppingListCommandHandler>();
            }

            public void SetupTemporaryItemId()
            {
                TemporaryItemId = new TemporaryItemId(Guid.NewGuid());
            }

            public void SetupShoppingListMock()
            {
                ShoppingListMock = new ShoppingListMock(ShoppingListMother.Sections(2).Create());
            }

            public void SetupStoreItem()
            {
                StoreItem = StoreItemMother.Initial().Create();
            }

            #region Fixture Setup

            public void SetupShoppingListRepositoryFindBy()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(Command.ShoppingListId, ShoppingListMock.Object);
            }

            public void SetupShoppingListRepositoryFindingNoList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(Command.ShoppingListId, null);
            }

            public void SetupItemRepositoryFindBy()
            {
                ItemRepositoryMock.SetupFindByAsync(TemporaryItemId, StoreItem);
            }

            public void SetupItemRepositoryFindingNoItem()
            {
                ItemRepositoryMock.SetupFindByAsync(TemporaryItemId, null);
            }

            #endregion Fixture Setup

            #region Verify

            public void VerifyStoreAsync()
            {
                ShoppingListRepositoryMock.VerifyStoreAsyncOnce(ShoppingListMock.Object);
            }

            public void VerifyChangeItemQuantityOnce()
            {
                ShoppingListMock.VerifyChangeItemQuantityOnce(StoreItem.Id, Command.ItemTypeId, Command.Quantity);
            }

            public void VerifyChangeItemQuantityWithOfflineIdOnce()
            {
                ShoppingListMock.VerifyChangeItemQuantityOnce(
                    new ItemId(Command.OfflineTolerantItemId.ActualId.Value),
                    Command.ItemTypeId,
                    Command.Quantity);
            }

            #endregion Verify
        }
    }
}