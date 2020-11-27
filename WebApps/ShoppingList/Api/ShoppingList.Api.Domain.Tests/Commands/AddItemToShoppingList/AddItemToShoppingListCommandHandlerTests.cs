using AutoFixture;
using AutoFixture.AutoMoq;
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

using Models = ShoppingList.Api.Domain.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Commands.AddItemToShoppingList
{
    public class AddItemToShoppingListCommandHandlerTests
    {
        public Fixture GetNewFixture()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            return fixture;
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixture = GetNewFixture();
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
            var storeId = new Random().Next(1, int.MaxValue);
            var fixture = GetNewFixture();
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
                    i => i.StoreAsync(It.IsAny<Models.ShoppingList>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        [Fact]
        public async Task HandleAsync_WithActualId_ShouldStoreItem()
        {
            // Arrange
            var random = new Random();
            var storeId = random.Next(1, int.MaxValue);
            var listItemId = random.Next(1, int.MaxValue);

            var fixture = GetNewFixture();
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
                    i => i.StoreAsync(It.IsAny<Models.ShoppingList>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }
    }
}