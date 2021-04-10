using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.FinishShoppingList;
using ShoppingList.Api.Domain.TestKit.Common.Mocks;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.FinishShoppingList
{
    public class FinishShoppingListCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly ShoppingListMockFixture shoppingListMockFixture;
        private readonly ShoppingListSectionFixture shoppingListSectionFixture;

        public FinishShoppingListCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            shoppingListSectionFixture = new ShoppingListSectionFixture(commonFixture);
            var shoppingListFixture = new ShoppingListFixture(commonFixture);
            shoppingListMockFixture = new ShoppingListMockFixture(commonFixture, shoppingListFixture);
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var handler = fixture.Create<FinishShoppingListCommandHandler>();

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
            var fixture = commonFixture.GetNewFixture();

            ShoppingListRepositoryMock shoppingListRepositoryMock = new ShoppingListRepositoryMock(fixture);

            var command = fixture.Create<FinishShoppingListCommand>();
            var handler = fixture.Create<FinishShoppingListCommandHandler>();

            shoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, null);

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
        public async Task HandleAsync_WithValidData_ShouldFinishShoppingList()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            TransactionGeneratorMock transactionGeneratorMock = new TransactionGeneratorMock(fixture);
            ShoppingListRepositoryMock shoppingListRepositoryMock = new ShoppingListRepositoryMock(fixture);
            Mock<ITransaction> transactionMock = new Mock<ITransaction>();

            ShoppingListMock listMock = shoppingListMockFixture.Create();
            ShoppingListMock remainingListMock = shoppingListMockFixture.Create();

            var command = fixture.Create<FinishShoppingListCommand>();
            var handler = fixture.Create<FinishShoppingListCommandHandler>();

            listMock.SetupFinish(command.CompletionDate, remainingListMock.Object);

            shoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);
            transactionGeneratorMock.SetupGenerateAsync(transactionMock.Object);

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                listMock.VerifyFinishOnce(command.CompletionDate);
                transactionGeneratorMock.VerifyGenerateAsyncOnce();
                shoppingListRepositoryMock.VerifyStoreAsyncOnce(listMock.Object);
                shoppingListRepositoryMock.VerifyStoreAsyncOnce(remainingListMock.Object);
                transactionMock.Verify(
                    i => i.CommitAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }
    }
}