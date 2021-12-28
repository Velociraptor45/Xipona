using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Core.TestKit.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.AddItemToShoppingList
{
    public class AddItemToShoppingListCommandHandlerTests
    {
        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateSut();

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
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateSut();
            var command = local.CreateCommandWithOfflineId();

            local.ShoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, null);

            // Act
            Func<Task> function = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ShoppingListNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithOfflineId_ShouldAddItemToList()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateSut();
            var command = local.CreateCommandWithOfflineId();

            ShoppingListMock listMock = local.ShoppingListMockFixture.Create();
            local.ShoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                local.ShoppingListRepositoryMock.VerifyStoreAsyncOnce(listMock.Object);

                local.AddItemToShoppingListServiceMock.VerifyAddItemToShoppingListOnce(
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
            var local = new LocalFixture();
            var handler = local.CreateSut();
            var command = local.CreateCommandWithActualId();

            ShoppingListMock listMock = local.ShoppingListMockFixture.Create();
            local.ShoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                local.ShoppingListRepositoryMock.VerifyStoreAsyncOnce(listMock.Object);

                local.AddItemToShoppingListServiceMock.VerifyAddItemToShoppingListOnce(
                    listMock.Object,
                    new ItemId(command.ItemId.ActualId.Value),
                    command.SectionId,
                    command.Quantity);
            }
        }

        private sealed class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public ShoppingListMockFixture ShoppingListMockFixture { get; }

            public ShoppingListRepositoryMock ShoppingListRepositoryMock { get; }
            public AddItemToShoppingListServiceMock AddItemToShoppingListServiceMock { get; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                ShoppingListMockFixture = new ShoppingListMockFixture();

                ShoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
                AddItemToShoppingListServiceMock = new AddItemToShoppingListServiceMock(MockBehavior.Strict);
            }

            public AddItemToShoppingListCommand CreateCommandWithActualId()
            {
                Fixture.ConstructorArgumentFor<AddItemToShoppingListCommand, OfflineTolerantItemId>("itemId",
                    new OfflineTolerantItemId(CommonFixture.NextInt()));

                return Fixture.Create<AddItemToShoppingListCommand>();
            }

            public AddItemToShoppingListCommand CreateCommandWithOfflineId()
            {
                Fixture.ConstructorArgumentFor<AddItemToShoppingListCommand, OfflineTolerantItemId>("itemId",
                    new OfflineTolerantItemId(Guid.NewGuid()));

                return Fixture.Create<AddItemToShoppingListCommand>();
            }

            public AddItemToShoppingListCommandHandler CreateSut()
            {
                return new AddItemToShoppingListCommandHandler(
                    ShoppingListRepositoryMock.Object,
                    AddItemToShoppingListServiceMock.Object);
            }
        }
    }
}