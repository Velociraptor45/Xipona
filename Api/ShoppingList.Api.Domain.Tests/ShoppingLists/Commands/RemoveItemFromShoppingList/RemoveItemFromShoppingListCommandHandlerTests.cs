using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Core.TestKit.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.Common.Mocks;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.RemoveItemFromShoppingList
{
    public class RemoveItemFromShoppingListCommandHandlerTests
    {
        private readonly LocalFixture _local;

        public RemoveItemFromShoppingListCommandHandlerTests()
        {
            _local = new LocalFixture();
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var handler = _local.CreateSut();
            _local.SetupCommandNull();

            // Act
            Func<Task> function = async () => await handler.HandleAsync(_local.Command, default);

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
            _local.SetupCommandWithOfflineId();
            _local.SetupFindingNoShoppingList();

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
        public async Task HandleAsync_WithInvalidActualItemId_ShouldThrowDomainException()
        {
            // Arrange
            var handler = _local.CreateSut();
            _local.SetupCommandWithActualId();

            _local.SetupShoppingListMock();
            _local.SetupFindingShoppingList();
            _local.SetupFindingNoItemByActualId();

            // Act
            Func<Task> function = async () => await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidOfflineId_ShouldThrowDomainException()
        {
            // Arrange
            var handler = _local.CreateSut();
            _local.SetupCommandWithOfflineId();

            _local.SetupShoppingListMock();
            _local.SetupFindingShoppingList();
            _local.SetupFindingNoItemByTemporaryId();

            // Act
            Func<Task> function = async () => await handler.HandleAsync(_local.Command, default);

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
        public async Task HandleAsync_WithValidActualId_ShouldRemoveItemFromBasket(bool isActualItemId, bool isTemporaryItem)
        {
            // Arrange
            var handler = _local.CreateSut();

            if (isTemporaryItem)
                _local.SetupTemporaryItemMock();
            else
                _local.SetupItemMock();

            if (isActualItemId)
            {
                _local.SetupCommandWithActualId();
                _local.SetupFindingItemByActualId();
            }
            else
            {
                _local.SetupCommandWithOfflineId();
                _local.SetupFindingItemByTemporaryId();
            }

            _local.SetupShoppingListMock();
            _local.SetupFindingShoppingList();
            _local.SetupTransactionMock();
            _local.SetupGeneratingTransaction();

            // Act
            bool result = await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                _local.VerifyRemoveItemOnce();
                _local.VerifyGenerateTransactionOnce();
                _local.VerifyStoreShoppingListOnce();
                _local.VerifyCommitTransactionOnce();

                if (isTemporaryItem)
                {
                    _local.VerifyDeleteItemOnce();
                    _local.VerifyStoreItemOnce();
                }
                else
                {
                    _local.VerifyDeleteItemNever();
                    _local.VerifyStoreItemNever();
                }
            }
        }

        private sealed class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();

            public ShoppingListRepositoryMock ShoppingListRepositoryMock { get; }
            public ItemRepositoryMock ItemRepositoryMock { get; }
            public TransactionGeneratorMock TransactionGeneratorMock { get; }

            public RemoveItemFromShoppingListCommand Command { get; private set; }
            public StoreItemMock ItemMock { get; private set; }
            public ShoppingListMock ShoppingListMock { get; private set; }
            public TransactionMock TransactionMock { get; private set; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                ShoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
                ItemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
                TransactionGeneratorMock = new TransactionGeneratorMock(MockBehavior.Strict);
            }

            public void SetupCommandWithActualId()
            {
                SetupCommand(new OfflineTolerantItemId(CommonFixture.NextInt()));
            }

            public void SetupCommandWithOfflineId()
            {
                SetupCommand(new OfflineTolerantItemId(Guid.NewGuid()));
            }

            private void SetupCommand(OfflineTolerantItemId id)
            {
                Fixture.ConstructorArgumentFor<RemoveItemFromShoppingListCommand, OfflineTolerantItemId>("itemId", id);

                Command = Fixture.Create<RemoveItemFromShoppingListCommand>();
            }

            public void SetupCommandNull()
            {
                Command = null;
            }

            public RemoveItemFromShoppingListCommandHandler CreateSut()
            {
                return new RemoveItemFromShoppingListCommandHandler(
                    ShoppingListRepositoryMock.Object,
                    ItemRepositoryMock.Object,
                    TransactionGeneratorMock.Object);
            }

            public void SetupTemporaryItemMock()
            {
                var item = StoreItemMother.InitialTemporary().Create();
                ItemMock = new StoreItemMock(item);
            }

            public void SetupItemMock()
            {
                var item = StoreItemMother.Initial().Create();
                ItemMock = new StoreItemMock(item);
            }

            public void SetupShoppingListMock()
            {
                ShoppingListMock = new ShoppingListMock(ShoppingListMother.Sections(3).Create());
            }

            public void SetupTransactionMock()
            {
                TransactionMock = new TransactionMock();
            }

            #region Mock Setup

            public void SetupFindingItemByActualId()
            {
                ItemRepositoryMock.SetupFindByAsync(new ItemId(Command.OfflineTolerantItemId.ActualId.Value),
                    ItemMock.Object);
            }

            public void SetupFindingNoItemByActualId()
            {
                ItemRepositoryMock.SetupFindByAsync(new ItemId(Command.OfflineTolerantItemId.ActualId.Value), null);
            }

            public void SetupFindingItemByTemporaryId()
            {
                ItemRepositoryMock.SetupFindByAsync(new TemporaryItemId(Command.OfflineTolerantItemId.OfflineId.Value),
                    ItemMock.Object);
            }

            public void SetupFindingNoItemByTemporaryId()
            {
                ItemRepositoryMock.SetupFindByAsync(new TemporaryItemId(Command.OfflineTolerantItemId.OfflineId.Value),
                    null);
            }

            public void SetupFindingShoppingList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(Command.ShoppingListId, ShoppingListMock.Object);
            }

            public void SetupFindingNoShoppingList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(Command.ShoppingListId, null);
            }

            public void SetupGeneratingTransaction()
            {
                TransactionGeneratorMock.SetupGenerateAsync(TransactionMock.Object);
            }

            #endregion Mock Setup

            #region Verify

            public void VerifyRemoveItemOnce()
            {
                ShoppingListMock.VerifyRemoveItemOnce(ItemMock.Object.Id);
            }

            public void VerifyGenerateTransactionOnce()
            {
                TransactionGeneratorMock.VerifyGenerateAsyncOnce();
            }

            public void VerifyDeleteItemOnce()
            {
                ItemMock.VerifyDeleteOnce();
            }

            public void VerifyDeleteItemNever()
            {
                ItemMock.VerifyDeleteNever();
            }

            public void VerifyStoreItemOnce()
            {
                ItemRepositoryMock.VerifyStoreAsyncOnce(ItemMock.Object);
            }

            public void VerifyStoreItemNever()
            {
                ItemRepositoryMock.VerifyStoreAsyncNever();
            }

            public void VerifyStoreShoppingListOnce()
            {
                ShoppingListRepositoryMock.VerifyStoreAsyncOnce(ShoppingListMock.Object);
            }

            public void VerifyCommitTransactionOnce()
            {
                TransactionMock.VerifyCommitAsyncOnce();
            }

            #endregion Verify
        }
    }
}