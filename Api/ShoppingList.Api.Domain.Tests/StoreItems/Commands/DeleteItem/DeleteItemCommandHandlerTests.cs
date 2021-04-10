using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.DeleteItem;
using ShoppingList.Api.Domain.TestKit.Common.Mocks;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Mocks;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using ShoppingList.Api.Domain.TestKit.StoreItems.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.DeleteItem
{
    public class DeleteItemCommandHandlerTests
    {
        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await action.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidItemId_ShouldThrowDomainException()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();
            var command = local.CreateCommand();

            local.ItemRepositoryMock.SetupFindByAsync(command.ItemId, null);

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ItemNotFound);
            }
        }

        [Theory]
        [ClassData(typeof(HandleAsyncWithShoppingListsTestData))]
        public async Task HandleAsync_WithItemOnShoppingLists_ShouldDeleteItemAndRemoveItFromActiveShoppingLists(
            List<ShoppingListMock> shoppingListMocks)
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();
            var command = local.CreateCommand();

            TransactionMock transactionMock = new TransactionMock();
            StoreItemMock storeItemMock = local.StoreItemMockFixture.Create();

            local.ItemRepositoryMock.SetupFindByAsync(command.ItemId, storeItemMock.Object);
            local.ShoppingListRepositoryMock.SetupFindActiveByAsync(storeItemMock.Object.Id,
                shoppingListMocks.Select(m => m.Object));
            local.TransactionGeneratorMock.SetupGenerateAsync(transactionMock.Object);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                storeItemMock.VerifyDeleteOnce();
                local.ItemRepositoryMock.VerifyStoreAsyncOnce(storeItemMock.Object);

                if (!shoppingListMocks.Any())
                {
                    local.ShoppingListRepositoryMock.VerifyStoreAsyncNever();
                }
                else
                {
                    foreach (var shoppingListMock in shoppingListMocks)
                    {
                        shoppingListMock.VerifyRemoveItemOnce(storeItemMock.Object.Id);
                        local.ShoppingListRepositoryMock.VerifyStoreAsyncOnce(shoppingListMock.Object);
                    }
                }

                transactionMock.VerifyCommitAsyncOnce();
            }
        }

        private sealed class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public StoreItemFixture StoreItemFixture { get; }
            public StoreItemMockFixture StoreItemMockFixture { get; }
            public ShoppingListRepositoryMock ShoppingListRepositoryMock { get; }
            public ItemRepositoryMock ItemRepositoryMock { get; }
            public TransactionGeneratorMock TransactionGeneratorMock { get; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                StoreItemFixture = new StoreItemFixture(new StoreItemAvailabilityFixture(CommonFixture), CommonFixture);
                StoreItemMockFixture = new StoreItemMockFixture(CommonFixture, StoreItemFixture);

                ShoppingListRepositoryMock = new ShoppingListRepositoryMock(Fixture);
                ItemRepositoryMock = new ItemRepositoryMock(Fixture);
                TransactionGeneratorMock = new TransactionGeneratorMock(Fixture);
            }

            public DeleteItemCommand CreateCommand()
            {
                return Fixture.Create<DeleteItemCommand>();
            }

            public DeleteItemCommandHandler CreateCommandHandler()
            {
                return Fixture.Create<DeleteItemCommandHandler>();
            }
        }
    }
}