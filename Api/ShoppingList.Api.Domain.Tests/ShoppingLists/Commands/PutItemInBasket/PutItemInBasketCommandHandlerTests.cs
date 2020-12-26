using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.PutItemInBasket
{
    public class PutItemInBasketCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly ShoppingListItemFixture shoppingListItemFixture;
        private readonly ShoppingListFixture shoppingListFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemFixture storeItemFixture;

        public PutItemInBasketCommandHandlerTests()
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
            var handler = fixture.Create<PutItemInBasketCommandHandler>();

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

            var command = fixture.Create<PutItemInBasketCommand>();
            var handler = fixture.Create<PutItemInBasketCommandHandler>();

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
        public async Task HandleAsync_WithValidActualIdCommand_ShouldPutItemInBasket()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            Mock<IShoppingList> listMock = new Mock<IShoppingList>();
            var shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();

            fixture.ConstructorArgumentFor<PutItemInBasketCommand, ShoppingListItemId>(
                "itemId", new ShoppingListItemId(commonFixture.NextInt()));
            var command = fixture.Create<PutItemInBasketCommand>();
            var handler = fixture.Create<PutItemInBasketCommandHandler>();

            shoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                listMock.Verify(
                    i => i.PutItemInBasket(
                        It.Is<ShoppingListItemId>(id => id == command.ItemId && id.IsActualId)),
                    Times.Once);
                shoppingListRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<IShoppingList>(list => list == listMock.Object),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidOfflineIdCommand_ShouldPutItemInBasket()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            var itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();

            Mock<IShoppingList> listMock = new Mock<IShoppingList>();

            StoreItemId actualItemId = new StoreItemId(commonFixture.NextInt());
            IStoreItem storeItem = storeItemFixture.GetStoreItem(actualItemId);

            var listItemIdOffline = new ShoppingListItemId(Guid.NewGuid());
            fixture.ConstructorArgumentFor<PutItemInBasketCommand, ShoppingListItemId>("itemId", listItemIdOffline);
            var command = fixture.Create<PutItemInBasketCommand>();
            var handler = fixture.Create<PutItemInBasketCommandHandler>();

            shoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);
            itemRepositoryMock.SetupFindByAsync(new StoreItemId(listItemIdOffline.Offline.Value), storeItem);

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                itemRepositoryMock.Verify(
                    i => i.FindByAsync(
                        It.Is<StoreItemId>(id => id.Offline.Value == listItemIdOffline.Offline.Value),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
                listMock.Verify(
                    i => i.PutItemInBasket(
                        It.Is<ShoppingListItemId>(id => id.Actual.Value == actualItemId.Actual.Value && id.IsActualId)),
                    Times.Once);
                shoppingListRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<IShoppingList>(list => list == listMock.Object),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }
    }
}