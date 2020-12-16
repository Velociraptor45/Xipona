using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Models.Fixtures;
using ShoppingList.Api.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Commands.RemoveItemFromShoppingList
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
        public async Task HandleAsync_WithValidActualIdOfPermanentItemCommand_ShouldRemoveItemFromBasket()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            var itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();

            var listItem = shoppingListItemFixture.GetShoppingListItem();
            var shoppingList = shoppingListFixture.GetShoppingList(itemCount: 2, listItem.ToMonoList());
            var storeItemIdActual = new StoreItemId(listItem.Id.Actual.Value);
            IStoreItem storeItem = storeItemFixture.GetStoreItem(storeItemIdActual, isTemporary: false, isDeleted: false);

            fixture.ConstructorArgumentFor<RemoveItemFromShoppingListCommand, ShoppingListItemId>(
                "shoppingListItemId", listItem.Id);
            var command = fixture.Create<RemoveItemFromShoppingListCommand>();
            var handler = fixture.Create<RemoveItemFromShoppingListCommandHandler>();

            shoppingListRepositoryMock
                .Setup(instance => instance.FindByAsync(
                    It.Is<ShoppingListId>(id => id == command.ShoppingListId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(shoppingList));

            itemRepositoryMock
                .Setup(instance => instance.FindByAsync(
                    It.Is<StoreItemId>(id => id.Actual.Value == listItem.Id.Actual.Value),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(storeItem));

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                shoppingList.Items.Should().HaveCount(2);
                shoppingList.Items.Should().NotContain(item => item.Id == listItem.Id);
                storeItem.IsDeleted.Should().BeFalse();
                shoppingListRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<ShoppingLists.Models.ShoppingList>(list => list.Id == shoppingList.Id),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
                itemRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.IsAny<StoreItem>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never);
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidActualIdOfTemporaryItemCommand_ShouldRemoveItemFromBasketAndDeleteIt()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            var itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();

            var listItem = shoppingListItemFixture.GetShoppingListItem();
            var shoppingList = shoppingListFixture.GetShoppingList(itemCount: 2, listItem.ToMonoList());
            var storeItemIdActual = new StoreItemId(listItem.Id.Actual.Value);
            IStoreItem storeItem = storeItemFixture.GetStoreItem(storeItemIdActual, isTemporary: true, isDeleted: false);

            fixture.ConstructorArgumentFor<RemoveItemFromShoppingListCommand, ShoppingListItemId>(
                "shoppingListItemId", listItem.Id);
            var command = fixture.Create<RemoveItemFromShoppingListCommand>();
            var handler = fixture.Create<RemoveItemFromShoppingListCommandHandler>();

            shoppingListRepositoryMock
                .Setup(instance => instance.FindByAsync(
                    It.Is<ShoppingListId>(id => id == command.ShoppingListId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(shoppingList));

            itemRepositoryMock
                .Setup(instance => instance.FindByAsync(
                    It.Is<StoreItemId>(id => id.Actual.Value == listItem.Id.Actual.Value),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(storeItem));

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                shoppingList.Items.Should().HaveCount(2);
                shoppingList.Items.Should().NotContain(item => item.Id == listItem.Id);
                storeItem.IsDeleted.Should().BeTrue();
                shoppingListRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<ShoppingLists.Models.ShoppingList>(list => list.Id == shoppingList.Id),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
                itemRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<StoreItem>(i => i.Id == storeItemIdActual),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }
    }
}