using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.FinishShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.FinishShoppingList
{
    public class FinishShoppingListCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;

        public FinishShoppingListCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
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

            var shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();

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

            Mock<ITransactionGenerator> transactionGeneratorMock = fixture.Freeze<Mock<ITransactionGenerator>>();
            Mock<ITransaction> transactionMock = new Mock<ITransaction>();

            Mock<IShoppingList> listMock = new Mock<IShoppingList>();
            Mock<IShoppingList> remainingListMock = new Mock<IShoppingList>();
            var shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();

            var command = fixture.Create<FinishShoppingListCommand>();
            var handler = fixture.Create<FinishShoppingListCommandHandler>();

            shoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);
            transactionGeneratorMock.SetupGenerateAsync(transactionMock.Object);
            //listMock
            //    .Setup(i => i.Finish(
            //        It.Is<DateTime>(date => date == command.CompletionDate)))
            //    .Returns(remainingListMock.Object);
            //todo: rewrite this

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                //listMock.Verify(
                //    i => i.Finish(
                //        It.Is<DateTime>(date => date == command.CompletionDate)),
                //    Times.Once);
                transactionGeneratorMock.Verify(
                    i => i.GenerateAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once);
                shoppingListRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<IShoppingList>(list => list == listMock.Object),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
                shoppingListRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<IShoppingList>(list => list == remainingListMock.Object),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
                transactionMock.Verify(
                    i => i.CommitAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }
    }
}