using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures;
using ShoppingList.Api.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.CreateTemporaryItem
{
    public class CreateTemporaryItemCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly StoreFixture storeFixture;

        public CreateTemporaryItemCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            storeFixture = new StoreFixture(commonFixture);
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            var handler = fixture.Create<CreateTemporaryItemCommandHandler>();

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await action.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidCommand_ShouldStoreItemAndReturnTrue()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            IStore store = storeFixture.GetStore(isDeleted: false);

            var itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            var storeRepositoryMock = fixture.Freeze<Mock<IStoreRepository>>();
            var handler = fixture.Create<CreateTemporaryItemCommandHandler>();
            var command = fixture.Create<CreateTemporaryItemCommand>();

            storeRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<StoreId>(id => id == command.TemporaryItemCreation.Availability.StoreId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(store));

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                itemRepositoryMock.Verify(
                    i => i.StoreAsync(It.IsAny<StoreItem>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }
    }
}