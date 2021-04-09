using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Common.Mocks;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Mocks;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using ShoppingList.Api.Domain.TestKit.StoreItems.Mocks;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.RemoveItemFromShoppingList
{
    public class RemoveItemFromShoppingListCommandHandlerTests
    {
        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();

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
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();
            var command = local.CreateCommandWithOfflineId();

            local.ShoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, null);

            // Act
            Func<Task> function = async () => await handler.HandleAsync(command, default);

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
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();
            var command = local.CreateCommandWithActualId();

            ShoppingListMock listMock = local.ShoppingListMockFixture.Create();

            local.ShoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);
            local.ItemRepositoryMock.SetupFindByAsync(new ItemId(command.OfflineTolerantItemId.ActualId.Value), null);

            // Act
            Func<Task> function = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidOfflineId_ShouldThrowDomainException()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();
            var command = local.CreateCommandWithOfflineId();

            var temporaryItemId = new TemporaryItemId(command.OfflineTolerantItemId.OfflineId.Value);
            ShoppingListMock listMock = local.ShoppingListMockFixture.Create();

            local.ShoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);
            local.ItemRepositoryMock.SetupFindByAsync(temporaryItemId, null);

            // Act
            Func<Task> function = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound);
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
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();
            var command = isActualItemId ? local.CreateCommandWithActualId() : local.CreateCommandWithOfflineId();

            ShoppingListMock listMock = local.ShoppingListMockFixture.Create();

            var storeItem = local.StoreItemFixture.Create(StoreItemDefinition.FromTemporary(isTemporaryItem));
            StoreItemMock itemMock = new StoreItemMock(storeItem);

            local.ShoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);

            if (isActualItemId)
                local.ItemRepositoryMock.SetupFindByAsync(new ItemId(command.OfflineTolerantItemId.ActualId.Value), itemMock.Object);
            else
                local.ItemRepositoryMock.SetupFindByAsync(new TemporaryItemId(command.OfflineTolerantItemId.OfflineId.Value), itemMock.Object);

            TransactionMock transactionMock = new TransactionMock();
            local.TransactionGeneratorMock.SetupGenerateAsync(transactionMock.Object);

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();

                listMock.VerifyRemoveItemOnce(itemMock.Object.Id);

                local.TransactionGeneratorMock.VerifyGenerateAsyncOnce();
                if (isTemporaryItem)
                {
                    itemMock.VerifyDeleteOnce();
                    local.ItemRepositoryMock.VerifyStoreAsyncOnce(itemMock.Object);
                }
                else
                {
                    itemMock.VerifyDeleteNever();
                    local.ItemRepositoryMock.VerifyStoreAsyncNever();
                }

                local.ShoppingListRepositoryMock.VerifyStoreAsyncOnce(listMock.Object);
                transactionMock.VerifyCommitAsyncOnce();
            }
        }

        private sealed class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public StoreItemFixture StoreItemFixture { get; }
            public ShoppingListMockFixture ShoppingListMockFixture { get; }
            public ShoppingListRepositoryMock ShoppingListRepositoryMock { get; }
            public ItemRepositoryMock ItemRepositoryMock { get; }
            public TransactionGeneratorMock TransactionGeneratorMock { get; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                StoreItemFixture = new StoreItemFixture(new StoreItemAvailabilityFixture(CommonFixture), CommonFixture);
                ShoppingListMockFixture = new ShoppingListMockFixture(CommonFixture, new ShoppingListFixture(CommonFixture));

                ShoppingListRepositoryMock = new ShoppingListRepositoryMock(Fixture);
                ItemRepositoryMock = new ItemRepositoryMock(Fixture);
                TransactionGeneratorMock = new TransactionGeneratorMock(Fixture);
            }

            public RemoveItemFromShoppingListCommand CreateCommandWithActualId()
            {
                var offlineTolerantItemId = new OfflineTolerantItemId(CommonFixture.NextInt());
                Fixture.ConstructorArgumentFor<RemoveItemFromShoppingListCommand, OfflineTolerantItemId>("itemId",
                    offlineTolerantItemId);

                return Fixture.Create<RemoveItemFromShoppingListCommand>();
            }

            public RemoveItemFromShoppingListCommand CreateCommandWithOfflineId()
            {
                var offlineTolerantItemId = new OfflineTolerantItemId(Guid.NewGuid());
                Fixture.ConstructorArgumentFor<RemoveItemFromShoppingListCommand, OfflineTolerantItemId>("itemId",
                    offlineTolerantItemId);

                return Fixture.Create<RemoveItemFromShoppingListCommand>();
            }

            public RemoveItemFromShoppingListCommandHandler CreateCommandHandler()
            {
                return Fixture.Create<RemoveItemFromShoppingListCommandHandler>();
            }
        }
    }
}