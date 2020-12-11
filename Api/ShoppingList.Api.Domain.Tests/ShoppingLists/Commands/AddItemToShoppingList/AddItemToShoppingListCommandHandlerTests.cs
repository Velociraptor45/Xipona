using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures;
using ShoppingList.Api.Domain.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

using DomainModels = ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.AddItemToShoppingList
{
    public class AddItemToShoppingListCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly ShoppingListItemFixture shoppingListItemFixture;
        private readonly ShoppingListFixture shoppingListFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemFixture storeItemFixture;

        public AddItemToShoppingListCommandHandlerTests()
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
        public async Task HandleAsync_WithOfflineId_ShouldAddItemToList()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            fixture.ConstructorArgumentFor<AddItemToShoppingListCommand, ShoppingListItemId>("shoppingListItemId",
                new ShoppingListItemId(Guid.NewGuid()));
            var shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            var itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            var handler = fixture.Create<AddItemToShoppingListCommandHandler>();
            var command = fixture.Create<AddItemToShoppingListCommand>();

            var (storeItem, list) = GetValidStoreItemAndShoppingList();

            shoppingListRepositoryMock
                .Setup(instance => instance.FindByAsync(It.IsAny<ShoppingListId>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(list));

            itemRepositoryMock
                .Setup(instance => instance.FindByAsync(It.IsAny<StoreItemId>(), It.IsAny<StoreId>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(storeItem));

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                shoppingListRepositoryMock.Verify(
                    i => i.StoreAsync(It.IsAny<DomainModels.ShoppingList>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        [Fact]
        public async Task HandleAsync_WithActualId_ShouldAddItemToList()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            fixture.ConstructorArgumentFor<AddItemToShoppingListCommand, ShoppingListItemId>("shoppingListItemId",
                new ShoppingListItemId(commonFixture.NextInt()));
            var shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            var itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            var handler = fixture.Create<AddItemToShoppingListCommandHandler>();
            var command = fixture.Create<AddItemToShoppingListCommand>();

            var (storeItem, list) = GetValidStoreItemAndShoppingList();

            shoppingListRepositoryMock
                .Setup(instance => instance.FindByAsync(It.IsAny<ShoppingListId>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(list));

            itemRepositoryMock
                .Setup(instance => instance.FindByAsync(It.IsAny<StoreItemId>(), It.IsAny<StoreId>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(storeItem));

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                shoppingListRepositoryMock.Verify(
                    i => i.StoreAsync(It.IsAny<DomainModels.ShoppingList>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        private (StoreItem, DomainModels.ShoppingList) GetValidStoreItemAndShoppingList()
        {
            var availabilities = storeItemAvailabilityFixture.GetAvailabilities(count: 3).ToList();
            var storeIdForShoppingList = availabilities[commonFixture.NextInt(0, availabilities.Count - 1)].StoreId.Value;
            var list = shoppingListFixture.GetShoppingList(new StoreId(storeIdForShoppingList));

            // this prevents that the item is already on the list
            var usedItemIds = list.Items.Select(i => i.Id.Actual.Value);
            int storeItemId = commonFixture.NextInt(usedItemIds);
            var storeItem = storeItemFixture.GetStoreItem(new StoreItemId(storeItemId), 0, availabilities);

            return (storeItem, list);
        }
    }
}