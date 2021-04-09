using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Mocks;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using ShoppingList.Api.Domain.TestKit.StoreItems.Mocks;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList
{
    public class ChangeItemQuantityOnShoppingListCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly ShoppingListFixture shoppingListFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemFixture storeItemFixture;

        public ChangeItemQuantityOnShoppingListCommandHandlerTests()
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
        public async Task HandleAsync_WithInvalidShoppingListId_ShouldThrowDomainException()
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
        public async Task HandleAsync_WithInvalidOfflineId_ShouldThrowDomainException()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();
            var command = local.CreateCommandWithOfflineId();

            ShoppingListMock listMock = local.ShoppingListMockFixture.Create();

            var temporaryItemId = new TemporaryItemId(command.OfflineTolerantItemId.OfflineId.Value);

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
        public async Task HandleAsync_WithActualId_ShouldChangeItemQuantityAndStoreIt()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();
            var command = local.CreateCommandWithActualId();

            ShoppingListMock listMock = local.ShoppingListMockFixture.Create();

            local.ShoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                listMock.VerifyChangeItemQuantityOnce(
                    new ItemId(command.OfflineTolerantItemId.ActualId.Value),
                    command.Quantity);
                local.ShoppingListRepositoryMock.VerifyStoreAsyncOnce(listMock.Object);
            }
        }

        [Fact]
        public async Task HandleAsync_WithOfflineId_ShouldChangeItemQuantityAndStoreIt()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();
            var command = local.CreateCommandWithOfflineId();

            ShoppingListMock listMock = local.ShoppingListMockFixture.Create();

            var temporaryItemId = new TemporaryItemId(command.OfflineTolerantItemId.OfflineId.Value);
            IStoreItem storeItem = local.StoreItemFixture.CreateValid();

            local.ShoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);
            local.ItemRepositoryMock.SetupFindByAsync(temporaryItemId, storeItem);

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                listMock.VerifyChangeItemQuantityOnce(storeItem.Id, command.Quantity);
                local.ShoppingListRepositoryMock.VerifyStoreAsyncOnce(listMock.Object);
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

            public ChangeItemQuantityOnShoppingListCommand CreateCommandWithActualId()
            {
                var offlineTolerantItemId = new OfflineTolerantItemId(CommonFixture.NextInt());
                Fixture.ConstructorArgumentFor<ChangeItemQuantityOnShoppingListCommand, OfflineTolerantItemId>("itemId",
                    offlineTolerantItemId);

                return Fixture.Create<ChangeItemQuantityOnShoppingListCommand>();
            }

            public ChangeItemQuantityOnShoppingListCommand CreateCommandWithOfflineId()
            {
                var offlineTolerantItemId = new OfflineTolerantItemId(Guid.NewGuid());
                Fixture.ConstructorArgumentFor<ChangeItemQuantityOnShoppingListCommand, OfflineTolerantItemId>("itemId",
                    offlineTolerantItemId);

                return Fixture.Create<ChangeItemQuantityOnShoppingListCommand>();
            }

            public ChangeItemQuantityOnShoppingListCommandHandler CreateCommandHandler()
            {
                return Fixture.Create<ChangeItemQuantityOnShoppingListCommandHandler>();
            }
        }
    }
}