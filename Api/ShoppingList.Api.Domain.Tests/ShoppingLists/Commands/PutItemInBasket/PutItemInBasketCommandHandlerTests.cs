using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.PutItemInBasket
{
    public class PutItemInBasketCommandHandlerTests
    {
        private readonly LocalFixture _local;

        public PutItemInBasketCommandHandlerTests()
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
        public async Task HandleAsync_WithInvalidShoppingListId_ShouldThrowDomainException()
        {
            // Arrange
            var handler = _local.CreateSut();
            _local.SetupTemporaryItemId();
            _local.SetupCommand();

            _local.SetupShoppingListRepositoryFindingNoList();

            // Act
            Func<Task> function = async () => await handler.HandleAsync(_local.Command, default);

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
            var handler = _local.CreateSut();
            _local.SetupTemporaryItemId();
            _local.SetupCommand();

            _local.SetupShoppingListMock();

            _local.SetupShoppingListRepositoryFindBy();
            _local.SetupItemRepositoryFindingNoItem();

            // Act
            Func<Task> function = async () => await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithActualId_ShouldPutItemInBasket()
        {
            // Arrange
            var handler = _local.CreateSut();
            _local.SetupCommand();

            _local.SetupShoppingListMock();
            _local.SetupShoppingListRepositoryFindBy();
            _local.SetupStoringShoppingList();

            // Act
            bool result = await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                _local.VerifyPutItemInBasketWithOfflineIdOnce();
                _local.VerifyStoreAsync();
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidOfflineId_ShouldPutItemInBasket()
        {
            // Arrange
            var handler = _local.CreateSut();
            _local.SetupTemporaryItemId();
            _local.SetupCommand();

            _local.SetupStoreItem();
            _local.SetupShoppingListMock();

            _local.SetupShoppingListRepositoryFindBy();
            _local.SetupItemRepositoryFindBy();
            _local.SetupStoringShoppingList();

            // Act
            bool result = await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                _local.VerifyItemRepositoryFindByWithTemporaryItemId();
                _local.VerifyPutItemInBasketOnce();
                _local.VerifyStoreAsync();
            }
        }

        private sealed class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public ShoppingListRepositoryMock ShoppingListRepositoryMock { get; }
            public ItemRepositoryMock ItemRepositoryMock { get; }

            public PutItemInBasketCommand Command { get; private set; }
            public TemporaryItemId? TemporaryItemId { get; private set; }
            public ShoppingListMock ShoppingListMock { get; private set; }
            public IStoreItem StoreItem { get; private set; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                ShoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
                ItemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
            }

            public void SetupCommand()
            {
                OfflineTolerantItemId offlineTolerantItemId;
                if (TemporaryItemId == null)
                {
                    offlineTolerantItemId = new OfflineTolerantItemId(CommonFixture.NextInt());
                }
                else
                {
                    Fixture.ConstructorArgumentFor<PutItemInBasketCommand, ItemTypeId?>("itemTypeId", null);
                    offlineTolerantItemId = new OfflineTolerantItemId(TemporaryItemId.Value.Value);
                }

                Fixture.ConstructorArgumentFor<PutItemInBasketCommand, OfflineTolerantItemId>("itemId",
                    offlineTolerantItemId);

                Command = Fixture.Create<PutItemInBasketCommand>();
            }

            public void SetupTemporaryItemId()
            {
                TemporaryItemId = new TemporaryItemId(Guid.NewGuid());
            }

            public void SetupShoppingListMock()
            {
                ShoppingListMock = new ShoppingListMock(ShoppingListMother.Sections(2).Create());
            }

            public void SetupStoreItem()
            {
                StoreItem = StoreItemMother.Initial().Create();
            }

            public PutItemInBasketCommandHandler CreateSut()
            {
                return new PutItemInBasketCommandHandler(
                    ShoppingListRepositoryMock.Object,
                    ItemRepositoryMock.Object);
            }

            #region Fixture Setup

            public void SetupShoppingListRepositoryFindBy()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(Command.ShoppingListId, ShoppingListMock.Object);
            }

            public void SetupShoppingListRepositoryFindingNoList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(Command.ShoppingListId, null);
            }

            public void SetupStoringShoppingList()
            {
                ShoppingListRepositoryMock.SetupStoreAsync(ShoppingListMock.Object);
            }

            public void SetupItemRepositoryFindBy()
            {
                ItemRepositoryMock.SetupFindByAsync(TemporaryItemId.Value, StoreItem);
            }

            public void SetupItemRepositoryFindingNoItem()
            {
                ItemRepositoryMock.SetupFindByAsync(TemporaryItemId.Value, null);
            }

            #endregion Fixture Setup

            #region Verify

            public void VerifyStoreAsync()
            {
                ShoppingListRepositoryMock.VerifyStoreAsyncOnce(ShoppingListMock.Object);
            }

            public void VerifyPutItemInBasketOnce()
            {
                ShoppingListMock.VerifyPutItemInBasket(StoreItem.Id, Command.ItemTypeId, Times.Once);
            }

            public void VerifyPutItemInBasketWithOfflineIdOnce()
            {
                ShoppingListMock.VerifyPutItemInBasket(new ItemId(Command.OfflineTolerantItemId.ActualId.Value),
                    Command.ItemTypeId, Times.Once);
            }

            public void VerifyItemRepositoryFindByWithTemporaryItemId()
            {
                ItemRepositoryMock.VerifyFindByAsync(TemporaryItemId.Value);
            }

            #endregion Verify
        }
    }
}