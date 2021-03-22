using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Mocks;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using ShoppingList.Api.Domain.TestKit.StoreItems.Mocks;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.PutItemInBasket
{
    public class PutItemInBasketCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly ShoppingListFixture shoppingListFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemFixture storeItemFixture;

        public PutItemInBasketCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            shoppingListFixture = new ShoppingListFixture(commonFixture);
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

            ShoppingListRepositoryMock shoppingListRepositoryMock = new ShoppingListRepositoryMock(fixture);

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
            ShoppingListRepositoryMock shoppingListRepositoryMock = new ShoppingListRepositoryMock(fixture);

            fixture.ConstructorArgumentFor<PutItemInBasketCommand, Domain.ShoppingLists.Models.ItemId>(
                "itemId", new Domain.ShoppingLists.Models.ItemId(commonFixture.NextInt()));
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
                        It.Is<Domain.ShoppingLists.Models.ItemId>(id => id == command.ItemId && id.IsActualId)),
                    Times.Once);
                shoppingListRepositoryMock.VerifyStoreAsyncOnce(listMock.Object);
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidOfflineIdCommand_ShouldPutItemInBasket()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            ShoppingListRepositoryMock shoppingListRepositoryMock = new ShoppingListRepositoryMock(fixture);
            ItemRepositoryMock itemRepositoryMock = new ItemRepositoryMock(fixture);

            Mock<IShoppingList> listMock = new Mock<IShoppingList>();

            Domain.StoreItems.Models.ItemId actualItemId = new Domain.StoreItems.Models.ItemId(commonFixture.NextInt());
            var storeItemBaseDefinition = StoreItemDefinition.FromId(actualItemId);
            IStoreItem storeItem = storeItemFixture.CreateValid(storeItemBaseDefinition);

            var listItemIdOffline = new ShoppingListItemId(Guid.NewGuid());
            fixture.ConstructorArgumentFor<PutItemInBasketCommand, Domain.ShoppingLists.Models.ItemId>("itemId", (Domain.ShoppingLists.Models.ItemId)listItemIdOffline);
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
                itemRepositoryMock.VerifyFindByAsync(new StoreItemId(listItemIdOffline.Offline.Value));
                listMock.Verify(
                    i => i.PutItemInBasket(
                        It.Is<Domain.ShoppingLists.Models.ItemId>(id => id.Actual.Value == actualItemId.Actual.Value && id.IsActualId)),
                    Times.Once);
                shoppingListRepositoryMock.VerifyStoreAsyncOnce(listMock.Object);
            }
        }
    }
}