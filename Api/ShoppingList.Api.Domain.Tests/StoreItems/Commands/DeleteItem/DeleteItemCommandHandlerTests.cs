using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.DeleteItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Mocks;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using ShoppingList.Api.Domain.TestKit.StoreItems.Mocks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.DeleteItem
{
    public class DeleteItemCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly StoreItemMockFixture storeItemMockFixture;

        public DeleteItemCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            var storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            var storeItemFixture = new StoreItemFixture(storeItemAvailabilityFixture, commonFixture);
            storeItemMockFixture = new StoreItemMockFixture(commonFixture, storeItemFixture);
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            var handler = fixture.Create<DeleteItemCommandHandler>();

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
            var fixture = commonFixture.GetNewFixture();

            Mock<IItemRepository> itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();

            var command = fixture.Create<DeleteItemCommand>();
            var handler = fixture.Create<DeleteItemCommandHandler>();

            itemRepositoryMock.SetupFindByAsync(command.ItemId, null);

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
            var fixture = commonFixture.GetNewFixture();

            Mock<IItemRepository> itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            Mock<IShoppingListRepository> shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            Mock<ITransactionGenerator> transactionGeneratorMock = fixture.Freeze<Mock<ITransactionGenerator>>();

            Mock<ITransaction> transactionMock = new Mock<ITransaction>();
            StoreItemMock storeItemMock = storeItemMockFixture.Create();

            var command = fixture.Create<DeleteItemCommand>();
            var handler = fixture.Create<DeleteItemCommandHandler>();

            itemRepositoryMock.SetupFindByAsync(command.ItemId, storeItemMock.Object);
            shoppingListRepositoryMock.SetupFindActiveByAsync(storeItemMock.Object.Id, shoppingListMocks.Select(m => m.Object));
            transactionGeneratorMock.SetupGenerateAsync(transactionMock.Object);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                storeItemMock.VerifyDeleteOnce();
                itemRepositoryMock.VerifyStoreAsyncOnce(storeItemMock.Object);

                if (!shoppingListMocks.Any())
                {
                    shoppingListRepositoryMock.VerifyStoreAsyncNever();
                }
                else
                {
                    foreach (var shoppingListMock in shoppingListMocks)
                    {
                        shoppingListMock.VerifyRemoveItemOnce(storeItemMock.Object.Id.ToShoppingListItemId());
                        shoppingListRepositoryMock.VerifyStoreAsyncOnce(shoppingListMock.Object);
                    }
                }

                transactionMock.VerifyCommitAsyncOnce();
            }
        }

        private class HandleAsyncWithShoppingListsTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var commonFixture = new CommonFixture();
                var shoppingListItemFixture = new ShoppingListItemFixture(commonFixture);
                var shoppingListSectionFixture = new ShoppingListSectionFixture(commonFixture, shoppingListItemFixture);
                var shoppingListFixture = new ShoppingListFixture(shoppingListSectionFixture, commonFixture);
                var shoppingListMockFixture = new ShoppingListMockFixture(commonFixture, shoppingListFixture);

                yield return new object[]
                {
                    shoppingListMockFixture.Create().ToMonoList()
                };
                yield return new object[]
                {
                    shoppingListMockFixture.CreateMany(3).ToList()
                };
                yield return new object[]
                {
                    new List<ShoppingListMock>()
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}