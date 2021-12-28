using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Core.TestKit.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.RemoveItemFromBasket
{
    public class RemoveItemFromBasketCommandHandlerTests
    {
        private readonly LocalFixture _local;

        public RemoveItemFromBasketCommandHandlerTests()
        {
            _local = new LocalFixture();
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var handler = _local.CreateSut();

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
            _local.SetupCommandWithOfflineId();
            _local.SetupShoppingListMock();
            _local.SetupFindingShoppingList();
            _local.SetupFindingNoItemByOfflineId();
            var handler = _local.CreateSut();

            // Act
            Func<Task> function = async () => await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidListId_ShouldThrowDomainException()
        {
            // Arrange
            _local.SetupCommandWithOfflineId();
            _local.SetupFindingNoShoppingList();
            var handler = _local.CreateSut();

            // Act
            Func<Task> function = async () => await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ShoppingListNotFound);
            }
        }

        #region WithActualId

        [Fact]
        public async Task HandleAsync_WithActualId_ShouldReturnTrue()
        {
            // Arrange
            _local.SetupWithActualId();
            var handler = _local.CreateSut();

            // Act
            bool result = await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task HandleAsync_WithActualId_ShouldRemoveItemFromBasket()
        {
            // Arrange
            _local.SetupWithActualId();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyRemoveItemFromBasketWithCommandActualId();
            }
        }

        [Fact]
        public async Task HandleAsync_WithActualId_ShouldStoreShoppingList()
        {
            // Arrange
            _local.SetupWithActualId();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyStoringShoppingList();
            }
        }

        #endregion WithActualId

        #region WithValidOfflineId

        [Fact]
        public async Task HandleAsync_WithValidOfflineId_ShouldReturnTrue()
        {
            // Arrange
            _local.SetupWithValidOfflineId();
            var handler = _local.CreateSut();

            // Act
            bool result = await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidOfflineId_ShouldRemoveItemFromBasket()
        {
            // Arrange
            _local.SetupWithValidOfflineId();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyRemoveItemFromBasketWithStoreItemId();
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidOfflineId_ShouldStoreShoppingList()
        {
            // Arrange
            _local.SetupWithValidOfflineId();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyStoringShoppingList();
            }
        }

        #endregion WithValidOfflineId

        private sealed class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public ShoppingListRepositoryMock ShoppingListRepositoryMock { get; }
            public ItemRepositoryMock ItemRepositoryMock { get; }

            public RemoveItemFromBasketCommand Command { get; private set; }
            public IStoreItem StoreItem { get; private set; }
            public ShoppingListMock ShoppingListMock { get; private set; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                ShoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
                ItemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
            }

            public void SetupCommandWithActualId()
            {
                SetupCommand(new OfflineTolerantItemId(CommonFixture.NextInt()));
            }

            public void SetupCommandWithOfflineId()
            {
                SetupCommand(new OfflineTolerantItemId(Guid.NewGuid()));
            }

            private void SetupCommand(OfflineTolerantItemId id)
            {
                Fixture.ConstructorArgumentFor<RemoveItemFromBasketCommand, OfflineTolerantItemId>("itemId", id);

                Command = Fixture.Create<RemoveItemFromBasketCommand>();
            }

            public RemoveItemFromBasketCommandHandler CreateSut()
            {
                return new RemoveItemFromBasketCommandHandler(
                    ShoppingListRepositoryMock.Object,
                    ItemRepositoryMock.Object);
            }

            public void SetupItem()
            {
                StoreItem = StoreItemMother.Initial().Create();
            }

            public void SetupShoppingListMock()
            {
                ShoppingListMock = new ShoppingListMock(ShoppingListMother.Sections(3).Create());
            }

            #region Mock Setup

            public void SetupFindingShoppingList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(Command.ShoppingListId, ShoppingListMock.Object);
            }

            public void SetupFindingNoShoppingList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(Command.ShoppingListId, null);
            }

            public void SetupFindingItemByOfflineId()
            {
                var tempId = new TemporaryItemId(Command.OfflineTolerantItemId.OfflineId.Value);
                ItemRepositoryMock.SetupFindByAsync(tempId, StoreItem);
            }

            public void SetupFindingNoItemByOfflineId()
            {
                var tempId = new TemporaryItemId(Command.OfflineTolerantItemId.OfflineId.Value);
                ItemRepositoryMock.SetupFindByAsync(tempId, null);
            }

            #endregion Mock Setup

            #region Verify

            public void VerifyRemoveItemFromBasketWithStoreItemId()
            {
                ShoppingListMock.VerifyRemoveItemFromBasketOnce(StoreItem.Id, Command.ItemTypeId);
            }

            public void VerifyRemoveItemFromBasketWithCommandActualId()
            {
                ShoppingListMock.VerifyRemoveItemFromBasketOnce(
                    new ItemId(Command.OfflineTolerantItemId.ActualId.Value),
                    Command.ItemTypeId);
            }

            public void VerifyStoringShoppingList()
            {
                ShoppingListRepositoryMock.VerifyStoreAsyncOnce(ShoppingListMock.Object);
            }

            #endregion Verify

            #region Aggregates

            public void SetupWithValidOfflineId()
            {
                SetupCommandWithOfflineId();
                SetupItem();
                SetupShoppingListMock();
                SetupFindingShoppingList();
                SetupFindingItemByOfflineId();
            }

            public void SetupWithActualId()
            {
                SetupCommandWithActualId();
                SetupShoppingListMock();
                SetupFindingShoppingList();
            }

            #endregion Aggregates
        }
    }
}