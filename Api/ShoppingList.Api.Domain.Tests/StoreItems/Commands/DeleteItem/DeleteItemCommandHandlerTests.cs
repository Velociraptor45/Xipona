using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.DeleteItem;
using ShoppingList.Api.Core.TestKit.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.Common.Mocks;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
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
            var handler = local.CreateSut();

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
            local.SetupCommand();
            local.SetupFindingNoItem();

            var handler = local.CreateSut();

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                await action.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemNotFound);
            }
        }

        [Theory]
        [ClassData(typeof(HandleAsyncWithShoppingListsTestData))]
        public async Task HandleAsync_WithItemOnShoppingLists_ShouldDeleteItemAndRemoveItFromActiveShoppingLists(
            List<ShoppingListMock> shoppingListMocks)
        {
            // Arrange
            var local = new LocalFixture();
            local.SetupCommand();
            local.SetupTransactionMock();
            local.SetupStoreItemMock();
            local.SetupFindingItem();
            local.SetupFindingShoppingList(shoppingListMocks);
            local.SetupGeneratingTransaction();

            var handler = local.CreateSut();

            // Act
            var result = await handler.HandleAsync(local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                local.VerifyDeleteStoreItemOnce();
                local.VerifyStoringItemOnce();
                if (!shoppingListMocks.Any())
                {
                    local.VerifyStoreNoShoppingList();
                }
                else
                {
                    local.VerifyRemoveItemFromShoppingList(shoppingListMocks);
                    local.VerifyStoreAllShoppingLists(shoppingListMocks);
                }
                local.VerifyCommittingTransactionOnce();
            }
        }

        private sealed class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public ShoppingListRepositoryMock ShoppingListRepositoryMock { get; }
            public ItemRepositoryMock ItemRepositoryMock { get; }
            public TransactionGeneratorMock TransactionGeneratorMock { get; }

            public DeleteItemCommand Command { get; private set; }
            public StoreItemMock StoreItemMock { get; private set; }
            public TransactionMock TransactionMock { get; private set; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                ShoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
                ItemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
                TransactionGeneratorMock = new TransactionGeneratorMock(MockBehavior.Strict);
            }

            public void SetupCommand()
            {
                Command = Fixture.Create<DeleteItemCommand>();
            }

            public DeleteItemCommandHandler CreateSut()
            {
                return new DeleteItemCommandHandler(
                    ItemRepositoryMock.Object,
                    ShoppingListRepositoryMock.Object,
                    TransactionGeneratorMock.Object);
            }

            public void SetupStoreItemMock()
            {
                StoreItemMock = new StoreItemMock(StoreItemMother.Initial().Create());
            }

            public void SetupTransactionMock()
            {
                TransactionMock = new TransactionMock();
            }

            #region Mock Setup

            public void SetupGeneratingTransaction()
            {
                TransactionGeneratorMock.SetupGenerateAsync(TransactionMock.Object);
            }

            public void SetupFindingItem()
            {
                ItemRepositoryMock.SetupFindByAsync(Command.ItemId, StoreItemMock.Object);
            }

            public void SetupFindingNoItem()
            {
                ItemRepositoryMock.SetupFindByAsync(Command.ItemId, null);
            }

            public void SetupFindingShoppingList(List<ShoppingListMock> shoppingListMocks)
            {
                ShoppingListRepositoryMock.SetupFindActiveByAsync(StoreItemMock.Object.Id,
                    shoppingListMocks.Select(m => m.Object));
            }

            #endregion Mock Setup

            #region Verify

            public void VerifyDeleteStoreItemOnce()
            {
                StoreItemMock.VerifyDeleteOnce();
            }

            public void VerifyStoringItemOnce()
            {
                ItemRepositoryMock.VerifyStoreAsyncOnce(StoreItemMock.Object);
            }

            public void VerifyStoreNoShoppingList()
            {
                ShoppingListRepositoryMock.VerifyStoreAsyncNever();
            }

            public void VerifyStoreAllShoppingLists(List<ShoppingListMock> shoppingListMocks)
            {
                foreach (var shoppingListMock in shoppingListMocks)
                {
                    ShoppingListRepositoryMock.VerifyStoreAsyncOnce(shoppingListMock.Object);
                }
            }

            public void VerifyRemoveItemFromShoppingList(List<ShoppingListMock> shoppingListMocks)
            {
                foreach (var shoppingListMock in shoppingListMocks)
                {
                    shoppingListMock.VerifyRemoveItemOnce(StoreItemMock.Object.Id);
                }
            }

            public void VerifyCommittingTransactionOnce()
            {
                TransactionMock.VerifyCommitAsyncOnce();
            }

            #endregion Verify
        }
    }
}