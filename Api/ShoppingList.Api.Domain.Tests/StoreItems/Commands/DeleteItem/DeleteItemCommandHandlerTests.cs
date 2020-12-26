using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.DeleteItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.DeleteItem
{
    public class DeleteItemCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly StoreFixture storeFixture;
        private readonly StoreItemFixture storeItemFixture;
        private readonly ShoppingListItemFixture shoppingListItemFixture;
        private readonly ShoppingListFixture shoppingListFixture;

        public DeleteItemCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            storeFixture = new StoreFixture(commonFixture);
            storeItemFixture = new StoreItemFixture(new StoreItemAvailabilityFixture(commonFixture), commonFixture);
            shoppingListItemFixture = new ShoppingListItemFixture(commonFixture);
            shoppingListFixture = new ShoppingListFixture(shoppingListItemFixture, commonFixture);
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

            itemRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<StoreItemId>(id => id == command.ItemId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IStoreItem>(null));

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
        public async Task HandleAsync_WithItemOnShoppingLists_ShouldDeleteItemAndDispatchRemoveCommands(
            List<IShoppingList> shoppingLists)
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            Mock<IItemRepository> itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            Mock<IShoppingListRepository> shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            Mock<ICommandDispatcher> commandDispatcherMock = fixture.Freeze<Mock<ICommandDispatcher>>();
            Mock<ITransactionGenerator> transactionGeneratorMock = fixture.Freeze<Mock<ITransactionGenerator>>();

            Mock<ITransaction> transactionMock = new Mock<ITransaction>();
            Mock<IStoreItem> storeItemMock = new Mock<IStoreItem>();
            var storeItemId = new StoreItemId(commonFixture.NextInt());

            var command = fixture.Create<DeleteItemCommand>();
            var handler = fixture.Create<DeleteItemCommandHandler>();

            storeItemMock
                .Setup(i => i.Id)
                .Returns(storeItemId);

            itemRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<StoreItemId>(id => id == command.ItemId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(storeItemMock.Object));

            shoppingListRepositoryMock
                .Setup(i => i.FindActiveByAsync(
                    It.Is<StoreItemId>(id => id == storeItemId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(shoppingLists.AsEnumerable()));

            transactionGeneratorMock
                .Setup(i => i.GenerateAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(transactionMock.Object));

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                storeItemMock.Verify(
                    i => i.Delete(),
                    Times.Once);

                itemRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<IStoreItem>(item => item == storeItemMock.Object),
                        It.IsAny<CancellationToken>()),
                    Times.Once);

                if (!shoppingLists.Any())
                {
                    commandDispatcherMock.Verify(
                        i => i.DispatchAsync(
                            It.IsAny<RemoveItemFromShoppingListCommand>(),
                            It.IsAny<CancellationToken>()),
                        Times.Never);
                }
                else
                {
                    for (int i = 0; i < shoppingLists.Count; i++)
                    {
                        commandDispatcherMock.Verify(
                            instance => instance.DispatchAsync(
                                It.Is<RemoveItemFromShoppingListCommand>(cmd =>
                                    cmd.ShoppingListId == shoppingLists[i].Id
                                    && cmd.ShoppingListItemId == new ShoppingListItemId(storeItemId.Actual.Value)),
                                It.IsAny<CancellationToken>()),
                            Times.Once);
                    }
                }

                transactionMock.Verify(
                    i => i.CommitAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        private class HandleAsyncWithShoppingListsTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var commonFixture = new CommonFixture();
                var shoppingListItemFixture = new ShoppingListItemFixture(commonFixture);
                var shoppingListFixture = new ShoppingListFixture(shoppingListItemFixture, commonFixture);

                yield return new object[]
                {
                    new List<IShoppingList>()
                };
                yield return new object[]
                {
                    new List<IShoppingList>
                    {
                        shoppingListFixture.GetShoppingList(),
                        shoppingListFixture.GetShoppingList(),
                        shoppingListFixture.GetShoppingList()
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}