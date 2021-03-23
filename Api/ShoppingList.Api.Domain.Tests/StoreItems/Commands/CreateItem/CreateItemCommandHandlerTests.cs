using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Mocks;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Mocks;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using ShoppingList.Api.Domain.TestKit.StoreItems.Mocks;
using ShoppingList.Api.Domain.TestKit.Stores.Fixtures;
using ShoppingList.Api.Domain.TestKit.Stores.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.CreateItem
{
    public class CreateItemCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly StoreFixture storeFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemSectionFixture storeItemSectionFixture;
        private readonly StoreItemFixture storeItemFixture;
        private readonly CreateItemCommandFixture createItemCommandFixture;

        public CreateItemCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            storeFixture = new StoreFixture(commonFixture);
            storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            storeItemSectionFixture = new StoreItemSectionFixture(commonFixture);
            storeItemFixture = new StoreItemFixture(new StoreItemAvailabilityFixture(commonFixture), commonFixture);
            createItemCommandFixture = new CreateItemCommandFixture(commonFixture);
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            var handler = fixture.Create<CreateItemCommandHandler>();

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await action.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidItemCategoryId_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            ItemCategoryRepositoryMock itemCategoryRepositoryMock = new ItemCategoryRepositoryMock(fixture);
            var handler = fixture.Create<CreateItemCommandHandler>();
            var command = fixture.Create<CreateItemCommand>();

            itemCategoryRepositoryMock.SetupFindByAsync(command.ItemCreation.ItemCategoryId, null);

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ItemCategoryNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidManufacturerId_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            ManufacturerRepositoryMock manufacturerRepositoryMock = new ManufacturerRepositoryMock(fixture);
            ItemCategoryRepositoryMock itemCategoryRepositoryMock = new ItemCategoryRepositoryMock(fixture);

            ManufacturerId manufacturerId = new ManufacturerId(commonFixture.NextInt());
            IItemCategory itemCategory = fixture.Create<ItemCategory>();

            var handler = fixture.Create<CreateItemCommandHandler>();
            var command = createItemCommandFixture.GetCreateItemCommand(itemCategory.Id, manufacturerId);

            manufacturerRepositoryMock.SetupFindByAsync(manufacturerId, null);
            itemCategoryRepositoryMock.SetupFindByAsync(itemCategory.Id, itemCategory);

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ManufacturerNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidStoreStoreIdFromAvailability_ShouldThrowDomainException()
        {
            // todo implement
        }

        [Fact]
        public async Task HandleAsync_WithInvalidStoreItemSection_ShouldThrowDomainException()
        {
            // todo implement
        }

        [Fact]
        public async Task HandleAsync_WithValidDataAndFilledManufacturerId_ShouldCreateItem()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            StoreItemFactoryMock storeItemFactoryMock = new StoreItemFactoryMock(fixture);
            ItemRepositoryMock itemRepositoryMock = new ItemRepositoryMock(fixture);
            ItemCategoryRepositoryMock itemCategoryRepositoryMock = new ItemCategoryRepositoryMock(fixture);
            ManufacturerRepositoryMock manufacturerRepositoryMock = new ManufacturerRepositoryMock(fixture);
            StoreItemAvailabilityFactoryMock availabilityFactoryMock = new StoreItemAvailabilityFactoryMock(fixture);
            StoreItemSectionReadRepositoryMock sectionReadRepositoryMock = new StoreItemSectionReadRepositoryMock(fixture);
            StoreRepositoryMock storeRepositoryMock = new StoreRepositoryMock(fixture);

            IManufacturer manufacturer = fixture.Create<Manufacturer>();
            IItemCategory itemCategory = fixture.Create<ItemCategory>();
            IStoreItem storeItem = storeItemFixture.CreateValid();
            List<IStoreItemAvailability> availabilities = storeItem.Availabilities.ToList();

            // setup availability creation
            List<IStore> stores = storeFixture.GetStores(availabilities.Count).ToList();
            for (int i = 0; i < availabilities.Count; i++)
            {
                var av = availabilities[i];
                var store = stores[i];
                storeRepositoryMock.SetupFindActiveByAsync(av.StoreId.Id.AsStoreId(), store);
                availabilityFactoryMock.SetupCreate(store, av.Price, av.DefaultSectionId.Id, av);
            }

            var handler = fixture.Create<CreateItemCommandHandler>();
            var command = createItemCommandFixture.GetCreateItemCommand(itemCategory.Id, manufacturer.Id, availabilities);

            // setup retrieving availability sections
            var availabilitySections = availabilities.Select(av => av.DefaultSectionId);
            sectionReadRepositoryMock.SetupFindByAsync(availabilitySections);

            storeItemFactoryMock.SetupCreate(command.ItemCreation, itemCategory, manufacturer, availabilities, storeItem);
            itemCategoryRepositoryMock.SetupFindByAsync(itemCategory.Id, itemCategory);
            manufacturerRepositoryMock.SetupFindByAsync(manufacturer.Id, manufacturer);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                itemRepositoryMock.VerifyStoreAsyncOnce(storeItem);
                for (int i = 0; i < availabilities.Count; i++)
                {
                    var shortAv = command.ItemCreation.Availabilities.ElementAt(i);
                    var av = availabilities[i];
                    var store = stores[i];
                    storeRepositoryMock.VerifyFindActiveByAsyncOnce(shortAv.StoreId.AsStoreId());
                    availabilityFactoryMock.VerifyCreateOnce(store, shortAv.Price, av.DefaultSectionId.Id);
                }
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidDataAndManufacturerIdNull_ShouldCreateItem()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            StoreItemFactoryMock storeItemFactoryMock = new StoreItemFactoryMock(fixture);
            ItemRepositoryMock itemRepositoryMock = new ItemRepositoryMock(fixture);
            ItemCategoryRepositoryMock itemCategoryRepositoryMock = new ItemCategoryRepositoryMock(fixture);
            Mock<IManufacturerRepository> manufacturerRepositoryMock = fixture.Freeze<Mock<IManufacturerRepository>>();
            StoreItemAvailabilityFactoryMock availabilityFactoryMock = new StoreItemAvailabilityFactoryMock(fixture);
            StoreItemSectionReadRepositoryMock sectionReadRepositoryMock = new StoreItemSectionReadRepositoryMock(fixture);
            StoreRepositoryMock storeRepositoryMock = new StoreRepositoryMock(fixture);

            IItemCategory itemCategory = fixture.Create<ItemCategory>();
            IStoreItem storeItem = storeItemFixture.CreateValid();
            List<IStoreItemAvailability> availabilities = storeItem.Availabilities.ToList();

            // setup availability creation
            List<IStore> stores = storeFixture.GetStores(availabilities.Count).ToList();
            for (int i = 0; i < availabilities.Count; i++)
            {
                var av = availabilities[i];
                var store = stores[i];
                storeRepositoryMock.SetupFindActiveByAsync(av.StoreId.Id.AsStoreId(), store);
                availabilityFactoryMock.SetupCreate(store, av.Price, av.DefaultSectionId.Id, av);
            }

            var handler = fixture.Create<CreateItemCommandHandler>();
            var command = createItemCommandFixture.GetCreateItemCommand(itemCategory.Id, null, availabilities);

            // setup retrieving availability sections
            var availabilitySections = availabilities.Select(av => av.DefaultSectionId);
            sectionReadRepositoryMock.SetupFindByAsync(availabilitySections);

            storeItemFactoryMock.SetupCreate(command.ItemCreation, itemCategory, null, availabilities, storeItem);
            itemCategoryRepositoryMock.SetupFindByAsync(itemCategory.Id, itemCategory);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                manufacturerRepositoryMock.Verify(
                    i => i.FindByAsync(
                        It.IsAny<ManufacturerId>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never);
                itemRepositoryMock.VerifyStoreAsyncOnce(storeItem);
            }
        }
    }
}