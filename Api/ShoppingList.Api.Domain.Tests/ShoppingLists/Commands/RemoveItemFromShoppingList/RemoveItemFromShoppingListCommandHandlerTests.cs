using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
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

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.RemoveItemFromShoppingList
{
    public class RemoveItemFromShoppingListCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly ShoppingListFixture shoppingListFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemFixture storeItemFixture;

        public RemoveItemFromShoppingListCommandHandlerTests()
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

            ShoppingListRepositoryMock shoppingListRepositoryMock = new ShoppingListRepositoryMock(fixture);

            var command = fixture.Create<RemoveItemFromShoppingListCommand>();
            var handler = fixture.Create<RemoveItemFromShoppingListCommandHandler>();

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
        public async Task HandleAsync_WithInvalidShoppingListItemId_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            ShoppingListRepositoryMock shoppingListRepositoryMock = new ShoppingListRepositoryMock(fixture);
            ItemRepositoryMock itemRepositoryMock = new ItemRepositoryMock(fixture);
            Mock<IShoppingList> listMock = new Mock<IShoppingList>();

            var command = fixture.Create<RemoveItemFromShoppingListCommand>();
            var handler = fixture.Create<RemoveItemFromShoppingListCommandHandler>();

            shoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);
            itemRepositoryMock.SetupFindByAsync(new ItemId(command.OfflineTolerantItemId.ActualId.Value), null);

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
        public async Task HandleAsync_WithValidActualId_ShouldRemoveItemFromBasket(bool isActualItemId, bool isTemporaryItem)
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            ShoppingListRepositoryMock shoppingListRepositoryMock = new ShoppingListRepositoryMock(fixture);
            ItemRepositoryMock itemRepositoryMock = new ItemRepositoryMock(fixture);

            Mock<IShoppingList> listMock = new Mock<IShoppingList>();
            Mock<IStoreItem> itemMock = new Mock<IStoreItem>();

            //// test actual and offline id
            OfflineTolerantItemId offlineItemId = isActualItemId ?
                new OfflineTolerantItemId(commonFixture.NextInt()) :
                new OfflineTolerantItemId(Guid.NewGuid());

            fixture.ConstructorArgumentFor<RemoveItemFromShoppingListCommand, OfflineTolerantItemId>(
                    "shoppingListItemId", offlineItemId);
            var command = fixture.Create<RemoveItemFromShoppingListCommand>();
            var handler = fixture.Create<RemoveItemFromShoppingListCommandHandler>();

            shoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);

            if (isActualItemId)
                itemRepositoryMock.SetupFindByAsync(new ItemId(command.OfflineTolerantItemId.ActualId.Value), itemMock.Object);
            else
                itemRepositoryMock.SetupFindByAsync(new TemporaryItemId(command.OfflineTolerantItemId.OfflineId.Value), itemMock.Object);

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
                        It.Is<ItemId>(id => id == itemMock.Object.Id)),
                    Times.Once);

                if (isTemporaryItem)
                {
                    itemMock.Verify(i => i.Delete(), Times.Once);
                    itemRepositoryMock.VerifyStoreAsyncOnce(itemMock.Object);
                }
                else
                {
                    itemMock.Verify(i => i.Delete(), Times.Never);
                    itemRepositoryMock.VerifyStoreAsyncNever();
                }

                shoppingListRepositoryMock.VerifyStoreAsyncOnce(listMock.Object);
            }
        }
    }
}