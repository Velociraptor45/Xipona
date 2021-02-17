using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Mocks;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
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
        private readonly ShoppingListSectionFixture shoppingListSectionFixture;
        private readonly ShoppingListFixture shoppingListFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemFixture storeItemFixture;
        private readonly ShoppingListMockFixture shoppingListMockFixture;

        public AddItemToShoppingListCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            shoppingListItemFixture = new ShoppingListItemFixture(commonFixture);
            shoppingListSectionFixture = new ShoppingListSectionFixture(commonFixture, shoppingListItemFixture);
            shoppingListFixture = new ShoppingListFixture(shoppingListSectionFixture, commonFixture);
            storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            storeItemFixture = new StoreItemFixture(storeItemAvailabilityFixture, commonFixture);
            shoppingListMockFixture = new ShoppingListMockFixture(commonFixture, shoppingListFixture);
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
        public async Task HandleAsync_WithInvalidStoreItemId_ShouldThrowDomainException()
        {
            // todo implement
        }

        [Fact]
        public async Task HandleAsync_WithItemAtStoreNotAvailable_ShouldThrowDomainException()
        {
            // todo implement
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

            ShoppingListMock listMock = shoppingListMockFixture.Create();
            ShoppingListStoreId storeId = listMock.Object.Store.Id;
            IStoreItemAvailability availability = storeItemAvailabilityFixture.GetAvailability(storeId);
            IStoreItem storeItem = storeItemFixture.GetStoreItem(new StoreItemId(command.ShoppingListItemId.Offline.Value),
                additionalAvailabilities: availability.ToMonoList());
            IShoppingListItem listItem = shoppingListItemFixture.Create();

            shoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);
            shoppingListItemFactoryMock.SetupCreate(storeItem, availability.Price, false, command.Quantity, listItem);
            itemRepositoryMock.SetupFindByAsync(new StoreItemId(command.ShoppingListItemId.Offline.Value), storeItem);

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                listMock.VerifyAddItemOnce(listItem, command.SectionId);
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

            ShoppingListMock listMock = shoppingListMockFixture.Create();
            ShoppingListStoreId storeId = listMock.Object.Store.Id;
            IStoreItemAvailability availability = storeItemAvailabilityFixture.GetAvailability(storeId);
            IStoreItem storeItem = storeItemFixture.GetStoreItem(new StoreItemId(command.ShoppingListItemId.Actual.Value),
                additionalAvailabilities: availability.ToMonoList());
            IShoppingListItem listItem = shoppingListItemFixture.Create();

            shoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);
            shoppingListItemFactoryMock.SetupCreate(storeItem, availability.Price, false, command.Quantity, listItem);
            itemRepositoryMock.SetupFindByAsync(storeItem.Id, storeItem);

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                listMock.VerifyAddItemOnce(listItem, command.SectionId);
                shoppingListRepositoryMock.Verify(
                    i => i.StoreAsync(It.IsAny<IShoppingList>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }
    }
}