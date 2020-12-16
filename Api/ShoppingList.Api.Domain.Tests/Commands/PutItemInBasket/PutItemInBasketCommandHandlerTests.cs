using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Models.Fixtures;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Commands.PutItemInBasket
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
        public async Task HandleAsync_WithValidActualIdCommand_ShouldPutItemInBasket()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var itemNotInBasket = shoppingListItemFixture.GetShoppingListItem(isInBasket: false);
            var shoppingList = shoppingListFixture.GetShoppingList(itemCount: 0, itemNotInBasket.ToMonoList());
            var shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();

            fixture.ConstructorArgumentFor<PutItemInBasketCommand, ShoppingListItemId>("itemId", itemNotInBasket.Id);
            var command = fixture.Create<PutItemInBasketCommand>();
            var handler = fixture.Create<PutItemInBasketCommandHandler>();

            shoppingListRepositoryMock
                .Setup(instance => instance.FindByAsync(
                    It.Is<ShoppingListId>(id => id == command.ShoppingListId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(shoppingList));

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                shoppingList.Items.Should().ContainSingle();
                shoppingList.Items.First().IsInBasket.Should().BeTrue();
                shoppingListRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<ShoppingLists.Models.ShoppingList>(list => list.Id == shoppingList.Id),
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

            var itemNotInBasket = shoppingListItemFixture.GetShoppingListItem(isInBasket: false);
            var shoppingList = shoppingListFixture.GetShoppingList(itemCount: 0, itemNotInBasket.ToMonoList());
            var listItemIdOffline = new ShoppingListItemId(Guid.NewGuid());
            var storeItemIdActual = new StoreItemId(itemNotInBasket.Id.Actual.Value);
            var storeItemIdOffline = new StoreItemId(listItemIdOffline.Offline.Value);
            IStoreItem storeItem = storeItemFixture.GetStoreItem(storeItemIdActual);

            fixture.ConstructorArgumentFor<PutItemInBasketCommand, ShoppingListItemId>("itemId", listItemIdOffline);
            var command = fixture.Create<PutItemInBasketCommand>();
            var handler = fixture.Create<PutItemInBasketCommandHandler>();

            shoppingListRepositoryMock
                .Setup(instance => instance.FindByAsync(
                    It.Is<ShoppingListId>(id => id == command.ShoppingListId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(shoppingList));

            itemRepositoryMock
                .Setup(instance => instance.FindByAsync(
                    It.Is<StoreItemId>(id => id.Offline.Value == listItemIdOffline.Offline.Value),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(storeItem));

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                shoppingList.Items.Should().ContainSingle();
                shoppingList.Items.First().IsInBasket.Should().BeTrue();
                shoppingListRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<ShoppingLists.Models.ShoppingList>(list => list.Id == shoppingList.Id),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
                itemRepositoryMock.Verify(
                    i => i.FindByAsync(
                        It.Is<StoreItemId>(id => id == storeItemIdOffline),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }
    }
}