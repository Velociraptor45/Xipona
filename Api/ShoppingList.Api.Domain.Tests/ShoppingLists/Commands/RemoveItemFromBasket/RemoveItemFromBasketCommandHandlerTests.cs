using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.RemoveItemFromBasket
{
    public class RemoveItemFromBasketCommandHandlerTests
    {
        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();

            // Act
            Func<Task> function = async () => await handler.HandleAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidOfflineId_ShouldThrowDomainException()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();
            var command = local.CreateCommandWithOfflineId();

            var temporaryItemId = new TemporaryItemId(command.OfflineTolerantItemId.OfflineId.Value);
            ShoppingListMock listMock = local.ShoppingListMockFixture.Create();

            local.ShoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);
            local.ItemRepositoryMock.SetupFindByAsync(temporaryItemId, null);

            // Act
            Func<Task> function = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidListId_ShouldThrowDomainException()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();
            var command = local.CreateCommandWithOfflineId();

            local.ShoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, null);

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
        public async Task HandleAsync_WithActualId_ShouldRemoveItemFromBasket()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();
            var command = local.CreateCommandWithActualId();

            ShoppingListMock shoppingListMock = local.ShoppingListMockFixture.Create();

            local.ShoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, shoppingListMock.Object);

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                shoppingListMock.VerifyRemoveItemFromBasketOnce(new ItemId(command.OfflineTolerantItemId.ActualId.Value));
                local.ShoppingListRepositoryMock.VerifyStoreAsyncOnce(shoppingListMock.Object);
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidOfflineId_ShouldRemoveItemFromBasket()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();
            var command = local.CreateCommandWithOfflineId();

            IStoreItem storeItem = local.StoreItemFixture.CreateValid();

            var temporaryItemId = new TemporaryItemId(command.OfflineTolerantItemId.OfflineId.Value);
            ShoppingListMock shoppingListMock = local.ShoppingListMockFixture.Create();

            local.ShoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, shoppingListMock.Object);
            local.ItemRepositoryMock.SetupFindByAsync(temporaryItemId, storeItem);

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                shoppingListMock.VerifyRemoveItemFromBasketOnce(storeItem.Id);
                local.ShoppingListRepositoryMock.VerifyStoreAsyncOnce(shoppingListMock.Object);
            }
        }

        private sealed class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public StoreItemFixture StoreItemFixture { get; }
            public ShoppingListMockFixture ShoppingListMockFixture { get; }
            public ShoppingListRepositoryMock ShoppingListRepositoryMock { get; }
            public ItemRepositoryMock ItemRepositoryMock { get; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                StoreItemFixture = new StoreItemFixture(new StoreItemAvailabilityFixture(CommonFixture), CommonFixture);
                ShoppingListMockFixture = new ShoppingListMockFixture(CommonFixture, new ShoppingListFixture(CommonFixture));

                ShoppingListRepositoryMock = new ShoppingListRepositoryMock(Fixture);
                ItemRepositoryMock = new ItemRepositoryMock(Fixture);
            }

            public RemoveItemFromBasketCommand CreateCommandWithActualId()
            {
                var offlineTolerantItemId = new OfflineTolerantItemId(CommonFixture.NextInt());
                Fixture.ConstructorArgumentFor<RemoveItemFromBasketCommand, OfflineTolerantItemId>("itemId",
                    offlineTolerantItemId);

                return Fixture.Create<RemoveItemFromBasketCommand>();
            }

            public RemoveItemFromBasketCommand CreateCommandWithOfflineId()
            {
                var offlineTolerantItemId = new OfflineTolerantItemId(Guid.NewGuid());
                Fixture.ConstructorArgumentFor<RemoveItemFromBasketCommand, OfflineTolerantItemId>("itemId",
                    offlineTolerantItemId);

                return Fixture.Create<RemoveItemFromBasketCommand>();
            }

            public RemoveItemFromBasketCommandHandler CreateCommandHandler()
            {
                return Fixture.Create<RemoveItemFromBasketCommandHandler>();
            }
        }
    }
}