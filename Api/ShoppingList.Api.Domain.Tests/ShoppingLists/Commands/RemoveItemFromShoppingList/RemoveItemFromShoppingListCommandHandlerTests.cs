using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.RemoveItemFromShoppingList
{
    public class RemoveItemFromShoppingListCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly ShoppingListItemFixture shoppingListItemFixture;
        private readonly ShoppingListFixture shoppingListFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemFixture storeItemFixture;

        public RemoveItemFromShoppingListCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            shoppingListItemFixture = new ShoppingListItemFixture(commonFixture);
            shoppingListFixture = new ShoppingListFixture(shoppingListItemFixture, commonFixture);
            storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            storeItemFixture = new StoreItemFixture(storeItemAvailabilityFixture, commonFixture);
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var handler = fixture.Create<RemoveItemFromShoppingListCommandHandler>();

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

            var command = fixture.Create<RemoveItemFromShoppingListCommand>();
            var handler = fixture.Create<RemoveItemFromShoppingListCommandHandler>();

            shoppingListRepositoryMock
                .Setup(instance => instance.FindByAsync(
                    It.Is<ShoppingListId>(id => id == command.ShoppingListId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IShoppingList>(null));

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
        public async Task HandleAsync_WithInvalidShoppingListItemId_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            var shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            var itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            Mock<IShoppingList> listMock = new Mock<IShoppingList>();

            var command = fixture.Create<RemoveItemFromShoppingListCommand>();
            var handler = fixture.Create<RemoveItemFromShoppingListCommandHandler>();

            shoppingListRepositoryMock
                .Setup(instance => instance.FindByAsync(
                    It.Is<ShoppingListId>(id => id == command.ShoppingListId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(listMock.Object));

            itemRepositoryMock
                .Setup(instance => instance.FindByAsync(
                    It.Is<StoreItemId>(id => id.IsActualId ?
                        id.Actual.Value == command.ShoppingListItemId.Actual.Value :
                        id.Offline.Value == command.ShoppingListItemId.Offline.Value),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IStoreItem>(null));

            // Act
            Func<Task> function = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound);
            }
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        public async Task HandleAsync_WithValidId_ShouldRemoveItemFromBasket(bool isActualItemId, bool isTemporaryItem)
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            var itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();

            Mock<IShoppingList> listMock = new Mock<IShoppingList>();
            Mock<IStoreItem> itemMock = new Mock<IStoreItem>();

            //// test actual and offline id
            ShoppingListItemId listItemId = isActualItemId ?
                new ShoppingListItemId(commonFixture.NextInt()) :
                new ShoppingListItemId(Guid.NewGuid());

            fixture.ConstructorArgumentFor<RemoveItemFromShoppingListCommand, ShoppingListItemId>(
                    "shoppingListItemId", listItemId);
            var command = fixture.Create<RemoveItemFromShoppingListCommand>();
            var handler = fixture.Create<RemoveItemFromShoppingListCommandHandler>();

            shoppingListRepositoryMock
                .Setup(instance => instance.FindByAsync(
                    It.Is<ShoppingListId>(id => id == command.ShoppingListId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(listMock.Object));

            itemRepositoryMock
                .Setup(instance => instance.FindByAsync(
                    It.Is<StoreItemId>(id => id.IsActualId ?
                        id.Actual.Value == command.ShoppingListItemId.Actual.Value :
                        id.Offline.Value == command.ShoppingListItemId.Offline.Value),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(itemMock.Object));

            itemMock
                .Setup(instance => instance.IsTemporary)
                .Returns(isTemporaryItem);

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();

                listMock.Verify(
                    i => i.RemoveItem(
                        It.Is<ShoppingListItemId>(id => id == command.ShoppingListItemId)),
                    Times.Once);

                if (isTemporaryItem)
                {
                    itemMock.Verify(i => i.Delete(), Times.Once);
                    itemRepositoryMock.Verify(
                        i => i.StoreAsync(
                            It.Is<IStoreItem>(i => i == itemMock.Object),
                            It.IsAny<CancellationToken>()),
                        Times.Once);
                }
                else
                {
                    itemMock.Verify(i => i.Delete(), Times.Never);
                    itemRepositoryMock.Verify(
                        i => i.StoreAsync(
                            It.IsAny<IStoreItem>(),
                            It.IsAny<CancellationToken>()),
                        Times.Never);
                }

                shoppingListRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<IShoppingList>(list => list == listMock.Object),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }
    }
}