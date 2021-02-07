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
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Mocks;
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
        private readonly StoreItemSectionFixture storeItemSectionFixture;
        private readonly StoreItemFixture storeItemFixture;
        private readonly ShoppingListItemFixture shoppingListItemFixture;
        private readonly ShoppingListSectionFixture shoppingListSectionFixture;
        private readonly ShoppingListFixture shoppingListFixture;
        private readonly StoreItemMockFixture storeItemMockFixture;
        private readonly MakeTemporaryItemPermanentCommandFixture makeTemporaryItemPermanentCommandFixture;

        public MakeTemporaryItemPermanentCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            storeFixture = new StoreFixture(commonFixture);
            storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            storeItemSectionFixture = new StoreItemSectionFixture(commonFixture);
            storeItemFixture = new StoreItemFixture(storeItemAvailabilityFixture, commonFixture);
            shoppingListItemFixture = new ShoppingListItemFixture(commonFixture);
            shoppingListSectionFixture = new ShoppingListSectionFixture(commonFixture, shoppingListItemFixture);
            shoppingListFixture = new ShoppingListFixture(shoppingListSectionFixture, commonFixture);
            storeItemMockFixture = new StoreItemMockFixture(commonFixture, storeItemFixture);
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

            itemRepositoryMock.SetupFindByAsync(command.PermanentItem.Id, null);

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

            itemRepositoryMock.SetupFindByAsync(command.PermanentItem.Id, storeItem);
            itemCategoryRepositoryMock.SetupFindByAsync(command.PermanentItem.ItemCategoryId, null);

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

            itemRepositoryMock.SetupFindByAsync(command.PermanentItem.Id, storeItem);

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

            itemRepositoryMock.SetupFindByAsync(command.PermanentItem.Id, storeItem);
            itemCategoryRepositoryMock.SetupFindByAsync(command.PermanentItem.ItemCategoryId, itemCategory);
            manufacturerRepositoryMock.SetupFindByAsync(manufacturerId, null);

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

            itemRepositoryMock.SetupFindByAsync(command.PermanentItem.Id, storeItem);
            itemCategoryRepositoryMock.SetupFindByAsync(command.PermanentItem.ItemCategoryId, itemCategory);
            manufacturerRepositoryMock.SetupFindByAsync(manufacturerId, manufacturer);
            storeRepositoryMock.SetupGetAsync(activeStores.AsEnumerable());

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
        public async Task HandleAsync_WithInvalidStoreItemSection_ShouldThrowDomainException()
        {
            // todo implement
        }

        [Fact]
        public async Task HandleAsync_WithValidData_ShouldMakeTemporaryItemPermanent()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            Mock<IItemRepository> itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            Mock<IItemCategoryRepository> itemCategoryRepositoryMock = fixture.Freeze<Mock<IItemCategoryRepository>>();
            Mock<IManufacturerRepository> manufacturerRepositoryMock = fixture.Freeze<Mock<IManufacturerRepository>>();
            Mock<IStoreRepository> storeRepositoryMock = fixture.Freeze<Mock<IStoreRepository>>();
            Mock<IStoreItemAvailabilityFactory> availabilityFactoryMock = fixture.Freeze<Mock<IStoreItemAvailabilityFactory>>();
            Mock<IStoreItemSectionReadRepository> sectionReadRepositoryMock = fixture.Freeze<Mock<IStoreItemSectionReadRepository>>();

            var manufacturerId = new ManufacturerId(commonFixture.NextInt());
            var allStores = storeFixture.GetStores(isDeleted: false).ToList();
            var command = CreateCommandWithAvailabilities(manufacturerId, allStores);
            allStores.Shuffle();

            var handler = fixture.Create<MakeTemporaryItemPermanentCommandHandler>();

            StoreItemMock storeItemMock = storeItemMockFixture.Create();
            IItemCategory itemCategory = fixture.Create<IItemCategory>();
            IManufacturer manufacturer = fixture.Create<IManufacturer>();

            // setup sections
            IEnumerable<StoreItemSectionId> sectionIds = command.PermanentItem.Availabilities
                .Select(av => new StoreItemSectionId(av.StoreItemSectionId.Value));
            IEnumerable<IStoreItemSection> sections = storeItemSectionFixture.CreateMany(sectionIds);
            sectionReadRepositoryMock.SetupFindByAsync(sectionIds, sections);

            // setup availabilities
            IEnumerable<IStoreItemAvailability> availabilities = storeItemAvailabilityFixture.GetAvailabilities(sections);
            availabilityFactoryMock.SetupMultipleCreate(availabilities);

            itemRepositoryMock.SetupFindByAsync(command.PermanentItem.Id, storeItemMock.Object);
            itemCategoryRepositoryMock.SetupFindByAsync(command.PermanentItem.ItemCategoryId, itemCategory);
            manufacturerRepositoryMock.SetupFindByAsync(manufacturerId, manufacturer);
            storeRepositoryMock.SetupGetAsync(allStores.AsEnumerable());

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                storeItemMock.VerifyMakePermanentOnce(command.PermanentItem, itemCategory, manufacturer, availabilities);
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