using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.Tests;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.MakeTemporaryItemPermanent
{
    public class MakeTemporaryItemPermanentCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly StoreFixture storeFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemFixture storeItemFixture;
        private readonly ShoppingListItemFixture shoppingListItemFixture;
        private readonly ShoppingListFixture shoppingListFixture;
        private readonly MakeTemporaryItemPermanentCommandFixture makeTemporaryItemPermanentCommandFixture;

        public MakeTemporaryItemPermanentCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            storeFixture = new StoreFixture(commonFixture);
            storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            storeItemFixture = new StoreItemFixture(storeItemAvailabilityFixture, commonFixture);
            shoppingListItemFixture = new ShoppingListItemFixture(commonFixture);
            shoppingListFixture = new ShoppingListFixture(shoppingListItemFixture, commonFixture);
            makeTemporaryItemPermanentCommandFixture = new MakeTemporaryItemPermanentCommandFixture(commonFixture);
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            var handler = fixture.Create<MakeTemporaryItemPermanentCommandHandler>();

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await action.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidItemId_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            Mock<IItemRepository> itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();

            var command = fixture.Create<MakeTemporaryItemPermanentCommand>();
            var handler = fixture.Create<MakeTemporaryItemPermanentCommandHandler>();

            itemRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<StoreItemId>(id => id == command.PermanentItem.Id),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IStoreItem>(null));

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ItemNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidItemCategoryId_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            Mock<IItemRepository> itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            Mock<IItemCategoryRepository> itemCategoryRepositoryMock = fixture.Freeze<Mock<IItemCategoryRepository>>();
            IStoreItem storeItem = storeItemFixture.GetStoreItem(isTemporary: true);

            var command = fixture.Create<MakeTemporaryItemPermanentCommand>();
            var handler = fixture.Create<MakeTemporaryItemPermanentCommandHandler>();

            itemRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<StoreItemId>(id => id == command.PermanentItem.Id),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(storeItem));

            itemCategoryRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<ItemCategoryId>(id => id == command.PermanentItem.ItemCategoryId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IItemCategory>(null));

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ItemCategoryNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithNonTemporaryItem_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            Mock<IItemRepository> itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            IStoreItem storeItem = storeItemFixture.GetStoreItem(isTemporary: false);

            var command = fixture.Create<MakeTemporaryItemPermanentCommand>();
            var handler = fixture.Create<MakeTemporaryItemPermanentCommandHandler>();

            itemRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<StoreItemId>(id => id == command.PermanentItem.Id),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(storeItem));

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ItemNotTemporary);
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidManufacturerId_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            Mock<IItemRepository> itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            Mock<IItemCategoryRepository> itemCategoryRepositoryMock = fixture.Freeze<Mock<IItemCategoryRepository>>();
            Mock<IManufacturerRepository> manufacturerRepositoryMock = fixture.Freeze<Mock<IManufacturerRepository>>();

            IStoreItem storeItem = storeItemFixture.GetStoreItem(isTemporary: true);
            IItemCategory itemCategory = fixture.Create<IItemCategory>();
            var manufacturerId = new ManufacturerId(commonFixture.NextInt());

            var command = makeTemporaryItemPermanentCommandFixture.GetCommand(manufacturerId);
            var handler = fixture.Create<MakeTemporaryItemPermanentCommandHandler>();

            itemRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<StoreItemId>(id => id == command.PermanentItem.Id),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(storeItem));

            itemCategoryRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<ItemCategoryId>(id => id == command.PermanentItem.ItemCategoryId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(itemCategory));

            manufacturerRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<ManufacturerId>(id => id == manufacturerId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IManufacturer>(null));

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ManufacturerNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidStoreId_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            Mock<IItemRepository> itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            Mock<IItemCategoryRepository> itemCategoryRepositoryMock = fixture.Freeze<Mock<IItemCategoryRepository>>();
            Mock<IManufacturerRepository> manufacturerRepositoryMock = fixture.Freeze<Mock<IManufacturerRepository>>();
            Mock<IStoreRepository> storeRepositoryMock = fixture.Freeze<Mock<IStoreRepository>>();

            var manufacturerId = new ManufacturerId(commonFixture.NextInt());
            var allStores = storeFixture.GetStores(isDeleted: false).ToList();
            var command = CreateCommandWithAvailabilities(manufacturerId, allStores);
            allStores.Shuffle();

            var handler = fixture.Create<MakeTemporaryItemPermanentCommandHandler>();

            IStoreItem storeItem = storeItemFixture.GetStoreItem(isTemporary: true);
            IItemCategory itemCategory = fixture.Create<IItemCategory>();
            IManufacturer manufacturer = fixture.Create<IManufacturer>();

            var activeStores = allStores.Skip(1).ToList();

            itemRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<StoreItemId>(id => id == command.PermanentItem.Id),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(storeItem));

            itemCategoryRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<ItemCategoryId>(id => id == command.PermanentItem.ItemCategoryId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(itemCategory));

            manufacturerRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<ManufacturerId>(id => id == manufacturerId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(manufacturer));

            storeRepositoryMock
                .Setup(i => i.GetAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(activeStores.AsEnumerable()));

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.StoreNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidData_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            Mock<IItemRepository> itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            Mock<IItemCategoryRepository> itemCategoryRepositoryMock = fixture.Freeze<Mock<IItemCategoryRepository>>();
            Mock<IManufacturerRepository> manufacturerRepositoryMock = fixture.Freeze<Mock<IManufacturerRepository>>();
            Mock<IStoreRepository> storeRepositoryMock = fixture.Freeze<Mock<IStoreRepository>>();

            var manufacturerId = new ManufacturerId(commonFixture.NextInt());
            var allStores = storeFixture.GetStores(isDeleted: false).ToList();
            var command = CreateCommandWithAvailabilities(manufacturerId, allStores);
            allStores.Shuffle();

            var handler = fixture.Create<MakeTemporaryItemPermanentCommandHandler>();

            Mock<IStoreItem> storeItemMock = new Mock<IStoreItem>();
            IItemCategory itemCategory = fixture.Create<IItemCategory>();
            IManufacturer manufacturer = fixture.Create<IManufacturer>();

            storeItemMock
                .Setup(i => i.IsTemporary)
                .Returns(true);

            itemRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<StoreItemId>(id => id == command.PermanentItem.Id),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(storeItemMock.Object));

            itemCategoryRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<ItemCategoryId>(id => id == command.PermanentItem.ItemCategoryId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(itemCategory));

            manufacturerRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<ManufacturerId>(id => id == manufacturerId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(manufacturer));

            storeRepositoryMock
                .Setup(i => i.GetAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(allStores.AsEnumerable()));

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                storeItemMock.Verify(
                    i => i.MakePermanent(
                        It.Is<PermanentItem>(pi => pi == command.PermanentItem),
                        It.Is<IItemCategory>(cat => cat == itemCategory),
                        It.Is<IManufacturer>(man => man == manufacturer)),
                    Times.Once);
                itemRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<IStoreItem>(item => item == storeItemMock.Object),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        private MakeTemporaryItemPermanentCommand CreateCommandWithAvailabilities(ManufacturerId manufacturerId,
            IEnumerable<IStore> stores)
        {
            List<int> storeIdsForAvailabilities = stores.Select(s => s.Id.Value).ToList();
            var availabilities = new List<IStoreItemAvailability>();
            foreach (var id in storeIdsForAvailabilities)
            {
                availabilities.Add(storeItemAvailabilityFixture.GetAvailability(id));
            }
            return makeTemporaryItemPermanentCommandFixture.GetCommand(manufacturerId, availabilities);
        }
    }
}