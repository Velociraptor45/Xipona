using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

using DomainModels = ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList
{
    public class ChangeItemQuantityOnShoppingListCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly ShoppingListItemFixture shoppingListItemFixture;
        private readonly ShoppingListFixture shoppingListFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemFixture storeItemFixture;

        public ChangeItemQuantityOnShoppingListCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            shoppingListItemFixture = new ShoppingListItemFixture(commonFixture);
            shoppingListFixture = new ShoppingListFixture(shoppingListItemFixture, commonFixture);
            storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            storeItemFixture = new StoreItemFixture(storeItemAvailabilityFixture, commonFixture);
        }

        /*
         * all valid
         *
         */

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var handler = fixture.Create<ChangeItemQuantityOnShoppingListCommandHandler>();

            // Act
            Func<Task> function = async () => await handler.HandleAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidShoppingListId_ShouldThrowShoppingListNotFoundException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var shoppingListRepoMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            var handler = fixture.Create<ChangeItemQuantityOnShoppingListCommandHandler>();
            var command = fixture.Create<ChangeItemQuantityOnShoppingListCommand>();

            shoppingListRepoMock
                .Setup(instance => instance.FindByAsync(
                    It.Is<ShoppingListId>(id => id == command.ShoppingListId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<DomainModels.ShoppingList>(null));

            // Act
            Func<Task> function = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ShoppingListNotFoundException>();
            }
        }
    }
}