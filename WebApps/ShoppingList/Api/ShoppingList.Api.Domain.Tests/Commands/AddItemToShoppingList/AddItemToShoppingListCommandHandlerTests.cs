using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ShoppingList.Api.Domain.Commands.AddItemToShoppingList;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

using DomainModels = ShoppingList.Api.Domain.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Commands.AddItemToShoppingList
{
    public class AddItemToShoppingListCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;

        public AddItemToShoppingListCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var handler = fixture.Create<AddItemToShoppingListCommandHandler>();

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await action.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task HandleAsync_WithOfflineId_ShouldStoreItem()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            var storeId = commonFixture.NextInt();
            fixture.Inject(new ShoppingListItemId(Guid.NewGuid()));
            fixture.Inject(new StoreId(storeId));
            var repositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            var handler = fixture.Create<AddItemToShoppingListCommandHandler>();
            var command = fixture.Create<AddItemToShoppingListCommand>();

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                repositoryMock.Verify(
                    i => i.StoreAsync(It.IsAny<DomainModels.ShoppingList>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        [Fact]
        public async Task HandleAsync_WithActualId_ShouldStoreItem()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var storeId = commonFixture.NextInt();
            var listItemId = commonFixture.NextInt();

            fixture.Inject(new ShoppingListItemId(listItemId));
            fixture.Inject(new StoreId(storeId));
            var repositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            var handler = fixture.Create<AddItemToShoppingListCommandHandler>();
            var command = fixture.Create<AddItemToShoppingListCommand>();

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                repositoryMock.Verify(
                    i => i.StoreAsync(It.IsAny<DomainModels.ShoppingList>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }
    }
}