using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures;
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
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemSectionFixture storeItemSectionFixture;
        private readonly StoreItemFixture storeItemFixture;
        private readonly CreateItemCommandFixture createItemCommandFixture;

        public CreateItemCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
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

            Mock<IItemCategoryRepository> itemCategoryRepositoryMock = fixture.Freeze<Mock<IItemCategoryRepository>>();
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

            Mock<IManufacturerRepository> manufacturerRepositoryMock = fixture.Freeze<Mock<IManufacturerRepository>>();
            ManufacturerId manufacturerId = new ManufacturerId(commonFixture.NextInt());

            var handler = fixture.Create<CreateItemCommandHandler>();
            var command = createItemCommandFixture.GetCreateItemCommand(manufacturerId);

            manufacturerRepositoryMock.SetupFindByAsync(manufacturerId, null);

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
        public async Task HandleAsync_WithValidDataAndFilledManufacturerId_ShouldCreateItem()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            Mock<IStoreItemFactory> storeItemFactoryMock = fixture.Freeze<Mock<IStoreItemFactory>>();
            Mock<IItemRepository> itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            Mock<IItemCategoryRepository> itemCategoryRepositoryMock = fixture.Freeze<Mock<IItemCategoryRepository>>();
            Mock<IManufacturerRepository> manufacturerRepositoryMock = fixture.Freeze<Mock<IManufacturerRepository>>();
            Mock<IStoreItemAvailabilityFactory> availabilityFactoryMock = fixture.Freeze<Mock<IStoreItemAvailabilityFactory>>();
            Mock<IStoreItemSectionReadRepository> sectionReadRepositoryMock = fixture.Freeze<Mock<IStoreItemSectionReadRepository>>();

            IManufacturer manufacturer = fixture.Create<Manufacturer>();
            IItemCategory itemCategory = fixture.Create<ItemCategory>();
            IStoreItem storeItem = storeItemFixture.GetStoreItem();

            var handler = fixture.Create<CreateItemCommandHandler>();
            var command = createItemCommandFixture.GetCreateItemCommand(itemCategory.Id, manufacturer.Id);

            // setup sections
            IEnumerable<StoreItemSectionId> sectionIds = command.ItemCreation.Availabilities
                .Select(av => new StoreItemSectionId(av.StoreItemSectionId.Value));
            IEnumerable<IStoreItemSection> sections = storeItemSectionFixture.CreateMany(sectionIds);
            sectionReadRepositoryMock.SetupFindByAsync(sectionIds, sections);

            // setup availabilities
            IEnumerable<IStoreItemAvailability> availabilities = storeItemAvailabilityFixture.GetAvailabilities(sections);
            availabilityFactoryMock.SetupMultipleCreate(availabilities);

            storeItemFactoryMock.SetupCreate(command.ItemCreation, itemCategory, manufacturer, availabilities, storeItem);
            itemCategoryRepositoryMock.SetupFindByAsync(itemCategory.Id, itemCategory);
            manufacturerRepositoryMock.SetupFindByAsync(manufacturer.Id, manufacturer);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                itemRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<IStoreItem>(item => item == storeItem),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidDataAndManufacturerIdNull_ShouldCreateItem()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            Mock<IStoreItemFactory> storeItemFactoryMock = fixture.Freeze<Mock<IStoreItemFactory>>();
            Mock<IItemRepository> itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            Mock<IItemCategoryRepository> itemCategoryRepositoryMock = fixture.Freeze<Mock<IItemCategoryRepository>>();
            Mock<IManufacturerRepository> manufacturerRepositoryMock = fixture.Freeze<Mock<IManufacturerRepository>>();
            Mock<IStoreItemAvailabilityFactory> availabilityFactoryMock = fixture.Freeze<Mock<IStoreItemAvailabilityFactory>>();
            Mock<IStoreItemSectionReadRepository> sectionReadRepositoryMock = fixture.Freeze<Mock<IStoreItemSectionReadRepository>>();

            IItemCategory itemCategory = fixture.Create<ItemCategory>();
            IStoreItem storeItem = storeItemFixture.GetStoreItem();

            var handler = fixture.Create<CreateItemCommandHandler>();
            var command = createItemCommandFixture.GetCreateItemCommand(itemCategory.Id, null);

            // setup sections
            IEnumerable<StoreItemSectionId> sectionIds = command.ItemCreation.Availabilities
                .Select(av => new StoreItemSectionId(av.StoreItemSectionId.Value));
            IEnumerable<IStoreItemSection> sections = storeItemSectionFixture.CreateMany(sectionIds);
            sectionReadRepositoryMock.SetupFindByAsync(sectionIds, sections);

            // setup availabilities
            IEnumerable<IStoreItemAvailability> availabilities = storeItemAvailabilityFixture.GetAvailabilities(sections);
            availabilityFactoryMock.SetupMultipleCreate(availabilities);

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
                itemRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<IStoreItem>(item => item == storeItem),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        //todo: further tests
    }
}