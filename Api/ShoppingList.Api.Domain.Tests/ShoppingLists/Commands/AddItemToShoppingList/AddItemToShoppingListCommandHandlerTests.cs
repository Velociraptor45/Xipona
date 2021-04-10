using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Common.Fixtures;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Services;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.AddItemToShoppingList
{
    public class AddItemToShoppingListCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly IModelFixture<IShoppingListItem, ShoppingListItemDefinition> shoppingListItemFixture;
        private readonly ShoppingListFixture shoppingListFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemFixture storeItemFixture;
        private readonly ShoppingListMockFixture shoppingListMockFixture;

        public AddItemToShoppingListCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            shoppingListItemFixture = new ShoppingListItemFixture(commonFixture).AsModelFixture();
            shoppingListFixture = new ShoppingListFixture(commonFixture);
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
        public async Task HandleAsync_WithInvalidShoppingListId_ShouldThrowDomainException()
        {
            // todo implement
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

            ShoppingListRepositoryMock shoppingListRepositoryMock = new ShoppingListRepositoryMock(fixture);
            AddItemToShoppingListServiceMock addItemToShoppingListServiceMock = new AddItemToShoppingListServiceMock(fixture);
            var handler = fixture.Create<AddItemToShoppingListCommandHandler>();
            var offlineTolerantItemId = new OfflineTolerantItemId(Guid.NewGuid());

            fixture.ConstructorArgumentFor<AddItemToShoppingListCommand, OfflineTolerantItemId>("itemId",
                offlineTolerantItemId);
            var command = fixture.Create<AddItemToShoppingListCommand>();

            // setup ShoppingList
            ShoppingListMock listMock = shoppingListMockFixture.Create();
            shoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                shoppingListRepositoryMock.VerifyStoreAsyncOnce(listMock.Object);

                addItemToShoppingListServiceMock.VerifyAddItemToShoppingListOnce(
                    listMock.Object,
                    new TemporaryItemId(command.ItemId.OfflineId.Value),
                    command.SectionId,
                    command.Quantity);
            }
        }

        [Fact]
        public async Task HandleAsync_WithActualId_ShouldAddItemToList()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var offlineTolerantItemId = new OfflineTolerantItemId(commonFixture.NextInt());

            fixture.ConstructorArgumentFor<AddItemToShoppingListCommand, OfflineTolerantItemId>("itemId",
                offlineTolerantItemId);
            ShoppingListRepositoryMock shoppingListRepositoryMock = new ShoppingListRepositoryMock(fixture);
            AddItemToShoppingListServiceMock addItemToShoppingListServiceMock = new AddItemToShoppingListServiceMock(fixture);
            var handler = fixture.Create<AddItemToShoppingListCommandHandler>();
            var command = fixture.Create<AddItemToShoppingListCommand>();

            // setup ShoppingList
            ShoppingListMock listMock = shoppingListMockFixture.Create();
            shoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                shoppingListRepositoryMock.VerifyStoreAsyncOnce(listMock.Object);

                addItemToShoppingListServiceMock.VerifyAddItemToShoppingListOnce(
                    listMock.Object,
                    new ItemId(command.ItemId.ActualId.Value),
                    command.SectionId,
                    command.Quantity);
            }
        }
    }
}