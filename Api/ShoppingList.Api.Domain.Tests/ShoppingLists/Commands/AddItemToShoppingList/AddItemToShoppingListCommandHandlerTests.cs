using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

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

            var shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            var shoppingListItemFactoryMock = fixture.Freeze<Mock<IShoppingListItemFactory>>();
            var itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            var handler = fixture.Create<AddItemToShoppingListCommandHandler>();

            fixture.ConstructorArgumentFor<AddItemToShoppingListCommand, ShoppingListItemId>("shoppingListItemId",
                new ShoppingListItemId(Guid.NewGuid()));
            var command = fixture.Create<AddItemToShoppingListCommand>();

            Mock<IShoppingList> listMock = new Mock<IShoppingList>();
            StoreId storeId = new StoreId(commonFixture.NextInt());
            IStoreItemAvailability availability = storeItemAvailabilityFixture.GetAvailability(storeId);
            IStoreItem storeItem = storeItemFixture.GetStoreItem(new StoreItemId(command.ShoppingListItemId.Offline.Value),
                additionalAvailabilities: availability.ToMonoList());
            IShoppingListItem listItem = shoppingListItemFixture.GetShoppingListItem();

            listMock
                .Setup(instace => instace.Store.Id)
                .Returns(storeId);

            shoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);
            shoppingListItemFactoryMock.SetupCreate(storeItem, availability.Price, false, command.Quantity, listItem);
            itemRepositoryMock.SetupFindByAsync(new StoreItemId(command.ShoppingListItemId.Offline.Value), storeItem);

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                listMock.Verify(
                    i => i.AddItem(It.Is<IShoppingListItem>(item => item == listItem)),
                    Times.Once);
                shoppingListRepositoryMock.Verify(
                    i => i.StoreAsync(It.IsAny<IShoppingList>(), It.IsAny<CancellationToken>()),
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
            var shoppingListItemFactoryMock = fixture.Freeze<Mock<IShoppingListItemFactory>>();
            var itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            var handler = fixture.Create<AddItemToShoppingListCommandHandler>();
            var command = fixture.Create<AddItemToShoppingListCommand>();

            Mock<IShoppingList> listMock = new Mock<IShoppingList>();
            StoreId storeId = new StoreId(commonFixture.NextInt());
            IStoreItemAvailability availability = storeItemAvailabilityFixture.GetAvailability(storeId);
            IStoreItem storeItem = storeItemFixture.GetStoreItem(new StoreItemId(command.ShoppingListItemId.Actual.Value),
                additionalAvailabilities: availability.ToMonoList());
            IShoppingListItem listItem = shoppingListItemFixture.GetShoppingListItem();

            listMock
                .Setup(instace => instace.Store.Id)
                .Returns(storeId);

            shoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);
            shoppingListItemFactoryMock.SetupCreate(storeItem, availability.Price, false, command.Quantity, listItem);
            itemRepositoryMock.SetupFindByAsync(storeItem.Id, storeItem);

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                listMock.Verify(
                    i => i.AddItem(It.Is<IShoppingListItem>(item => item == listItem)),
                    Times.Once);
                shoppingListRepositoryMock.Verify(
                    i => i.StoreAsync(It.IsAny<IShoppingList>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }
    }
}