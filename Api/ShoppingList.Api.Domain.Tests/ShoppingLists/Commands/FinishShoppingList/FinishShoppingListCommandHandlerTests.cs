using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.FinishShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Mocks;
using System;
using System.Linq;
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
            var shoppingListItemFixture = new ShoppingListItemFixture(commonFixture);
            shoppingListSectionFixture = new ShoppingListSectionFixture(commonFixture, shoppingListItemFixture);
            var shoppingListFixture = new ShoppingListFixture(shoppingListSectionFixture, commonFixture);
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
            Mock<IShoppingListRepository> shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            Mock<IShoppingListFactory> shoppingListFactoryMock = fixture.Freeze<Mock<IShoppingListFactory>>();
            Mock<ITransaction> transactionMock = new Mock<ITransaction>();

            ShoppingListMock listMock = shoppingListMockFixture.Create();
            ShoppingListMock remainingListMock = shoppingListMockFixture.Create();

            var remainingSections = shoppingListSectionFixture.CreateMany(5).ToList();
            listMock.SetupGetSectionsWithItemsNotInBasket(remainingSections);

            var command = fixture.Create<FinishShoppingListCommand>();
            var handler = fixture.Create<FinishShoppingListCommandHandler>();

            shoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);
            shoppingListFactoryMock.SetupCreate(listMock.Object.Store, remainingSections, null, remainingListMock.Object);
            transactionGeneratorMock.SetupGenerateAsync(transactionMock.Object);

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                listMock.VerifySetCompletionDateOnce(command.CompletionDate);
                listMock.VerifyGetSectionsWithItemsNotInBasketOnce();
                listMock.VerifyRemoveAllItemsNotInBasketOnce();
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