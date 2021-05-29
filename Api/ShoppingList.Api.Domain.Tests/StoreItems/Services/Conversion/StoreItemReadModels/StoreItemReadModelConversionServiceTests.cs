using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Core.Attributes;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypesInPacket;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.StoreItemReadModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Fixtures;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Ports;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Fixtures;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Ports;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using ShoppingList.Api.Domain.TestKit.Stores.Fixtures;
using ShoppingList.Api.Domain.TestKit.Stores.Ports;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Services.Conversion.StoreItemReadModels
{
    public class StoreItemReadModelConversionServiceTests
    {
        [Fact]
        public async Task ConvertAsync_WithItemIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            // Act
            Func<Task<StoreItemReadModel>> function = async () => await service.ConvertAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task ConvertAsync_WithInvalidItemCategoryId_ShouldConvertToReadModel()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            var item = local.CreateItem();
            var availability = item.Availabilities.First();
            var manufacturer = local.CreateManufacturer(item.ManufacturerId);
            var store = local.CreateStore(availability.StoreId, availability.DefaultSectionId);

            local.ItemCategoryRepositoryMock.SetupFindByAsync(item.ItemCategoryId, null);
            local.ManufacturerRepositoryMock.SetupFindByAsync(item.ManufacturerId, manufacturer);
            local.StoreRepositoryMock.SetupFindByAsync(availability.StoreId.ToMonoList(), store.ToMonoList());

            // Act
            Func<Task<StoreItemReadModel>> function = async () => await service.ConvertAsync(item, default);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ItemCategoryNotFound);
            }
        }

        [Fact]
        public async Task ConvertAsync_WithInvalidManufacturer_ShouldConvertToReadModel()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            var item = local.CreateItem();
            var availability = item.Availabilities.First();
            var itemCategory = local.CreateItemCategory(item.ItemCategoryId);
            var store = local.CreateStore(availability.StoreId, availability.DefaultSectionId);

            local.ItemCategoryRepositoryMock.SetupFindByAsync(item.ItemCategoryId, itemCategory);
            local.ManufacturerRepositoryMock.SetupFindByAsync(item.ManufacturerId, null);
            local.StoreRepositoryMock.SetupFindByAsync(availability.StoreId.ToMonoList(), store.ToMonoList());

            // Act
            Func<Task<StoreItemReadModel>> function = async () => await service.ConvertAsync(item, default);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ManufacturerNotFound);
            }
        }

        [Fact]
        public async Task ConvertAsync_WithItemCategoryIsNull_ShouldConvertToReadModel()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            var item = local.CreateItemWithoutItemCategory();
            var availability = item.Availabilities.First();
            var manufacturer = local.CreateManufacturer(item.ManufacturerId);
            var store = local.CreateStore(availability.StoreId, availability.DefaultSectionId);

            local.ManufacturerRepositoryMock.SetupFindByAsync(item.ManufacturerId, manufacturer);
            local.StoreRepositoryMock.SetupFindByAsync(availability.StoreId.ToMonoList(), store.ToMonoList());

            // Act
            var result = await service.ConvertAsync(item, default);

            // Assert
            var expected = local.ToSimpleReadModel(item, null, manufacturer, store);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(expected);
            }
        }

        [Fact]
        public async Task ConvertAsync_WithManufacturerIsNull_ShouldConvertToReadModel()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            var item = local.CreateItemWithoutManufacturer();
            var availability = item.Availabilities.First();
            var itemCategory = local.CreateItemCategory(item.ItemCategoryId);
            var store = local.CreateStore(availability.StoreId, availability.DefaultSectionId);

            local.ItemCategoryRepositoryMock.SetupFindByAsync(item.ItemCategoryId, itemCategory);
            local.StoreRepositoryMock.SetupFindByAsync(availability.StoreId.ToMonoList(), store.ToMonoList());

            // Act
            var result = await service.ConvertAsync(item, default);

            // Assert
            var expected = local.ToSimpleReadModel(item, itemCategory, null, store);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(expected);
            }
        }

        [Fact]
        public async Task ConvertAsync_WithItemCategoryAndManufacturerAreNull_ShouldConvertToReadModel()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            var item = local.CreateItemWithNeitherItemCategoryNorManufacturer();
            var availability = item.Availabilities.First();
            var store = local.CreateStore(availability.StoreId, availability.DefaultSectionId);

            local.StoreRepositoryMock.SetupFindByAsync(availability.StoreId.ToMonoList(), store.ToMonoList());

            // Act
            var result = await service.ConvertAsync(item, default);

            // Assert
            var expected = local.ToSimpleReadModel(item, null, null, store);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(expected);
            }
        }

        [Fact]
        public async Task ConvertAsync_WithItemCategoryAndManufacturer_ShouldConvertToReadModel()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            var item = local.CreateItem();
            var availability = item.Availabilities.First();
            var itemCategory = local.CreateItemCategory(item.ItemCategoryId);
            var manufacturer = local.CreateManufacturer(item.ManufacturerId);
            var store = local.CreateStore(availability.StoreId, availability.DefaultSectionId);

            local.ItemCategoryRepositoryMock.SetupFindByAsync(item.ItemCategoryId, itemCategory);
            local.ManufacturerRepositoryMock.SetupFindByAsync(item.ManufacturerId, manufacturer);
            local.StoreRepositoryMock.SetupFindByAsync(availability.StoreId.ToMonoList(), store.ToMonoList());

            // Act
            var result = await service.ConvertAsync(item, default);

            // Assert
            var expected = local.ToSimpleReadModel(item, itemCategory, manufacturer, store);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(expected);
            }
        }

        public class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public StoreItemAvailabilityFixture StoreItemAvailabilityFixture { get; }
            public StoreItemFixture StoreItemFixture { get; }
            public StoreSectionFixture StoreSectionFixture { get; }
            public StoreFixture StoreFixture { get; }
            public ItemCategoryFixture ItemCategoryFixture { get; }
            public ManufacturerFixture ManufacturerFixture { get; }
            public ItemCategoryRepositoryMock ItemCategoryRepositoryMock { get; }
            public ManufacturerRepositoryMock ManufacturerRepositoryMock { get; }
            public StoreRepositoryMock StoreRepositoryMock { get; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                StoreItemAvailabilityFixture = new StoreItemAvailabilityFixture(CommonFixture);
                StoreItemFixture = new StoreItemFixture(StoreItemAvailabilityFixture, CommonFixture);
                StoreSectionFixture = new StoreSectionFixture(CommonFixture);
                StoreFixture = new StoreFixture(CommonFixture);
                ItemCategoryFixture = new ItemCategoryFixture(CommonFixture);
                ManufacturerFixture = new ManufacturerFixture(CommonFixture);

                ItemCategoryRepositoryMock = new ItemCategoryRepositoryMock(Fixture);
                ManufacturerRepositoryMock = new ManufacturerRepositoryMock(Fixture);
                StoreRepositoryMock = new StoreRepositoryMock(Fixture);
            }

            public StoreItemReadModelConversionService CreateService()
            {
                return Fixture.Create<StoreItemReadModelConversionService>();
            }

            public IStoreItem CreateItem()
            {
                var itemCategoryId = new ItemCategoryId(CommonFixture.NextInt());
                var manufacturerId = new ManufacturerId(CommonFixture.NextInt());
                return CreateItem(itemCategoryId, manufacturerId);
            }

            public IStoreItem CreateItemWithoutItemCategory()
            {
                var manufacturerId = new ManufacturerId(CommonFixture.NextInt());
                return CreateItem(null, manufacturerId);
            }

            public IStoreItem CreateItemWithoutManufacturer()
            {
                var itemCategoryId = new ItemCategoryId(CommonFixture.NextInt());
                return CreateItem(itemCategoryId, null);
            }

            public IStoreItem CreateItemWithNeitherItemCategoryNorManufacturer()
            {
                return CreateItem(null, null);
            }

            private IStoreItem CreateItem(ItemCategoryId itemCategoryId, ManufacturerId manufacturerId)
            {
                var def = new StoreItemDefinition
                {
                    ItemCategoryId = itemCategoryId,
                    ManufacturerId = manufacturerId,
                    Availabilities = StoreItemAvailabilityFixture.CreateManyValid(1)
                };

                return StoreItemFixture.CreateValid(def);
            }

            public IStore CreateStore(StoreId storeId, SectionId sectionId)
            {
                var sectionDef = StoreSectionDefinition.FromId(sectionId);

                var storeDef = new StoreDefinition
                {
                    Id = storeId,
                    Sections = StoreSectionFixture.CreateMany(sectionDef, 1)
                };
                return StoreFixture.CreateValid(storeDef);
            }

            public IItemCategory CreateItemCategory(ItemCategoryId itemCategoryId)
            {
                return ItemCategoryFixture.GetItemCategory(itemCategoryId);
            }

            public IManufacturer CreateManufacturer(ManufacturerId manufacturerId)
            {
                var def = ManufacturerDefinition.FromId(manufacturerId);
                return ManufacturerFixture.Create(def);
            }

            public StoreItemReadModel ToSimpleReadModel(IStoreItem item, IItemCategory itemCategory,
                IManufacturer manufacturer, IStore store)
            {
                var manufacturerReadModel = manufacturer == null
                ? null
                : new ManufacturerReadModel(
                    manufacturer.Id,
                    manufacturer.Name,
                    manufacturer.IsDeleted);

                var itemCategoryReadModel = itemCategory == null
                    ? null
                    : new ItemCategoryReadModel(
                        itemCategory.Id,
                        itemCategory.Name,
                        itemCategory.IsDeleted);

                var section = store.Sections.First();
                var storeSectionReadModel = new StoreSectionReadModel(
                    section.Id,
                    section.Name,
                    section.SortingIndex,
                    section.IsDefaultSection);

                var storeReadModel = new StoreItemStoreReadModel(
                    store.Id,
                    store.Name,
                    storeSectionReadModel.ToMonoList());

                var availability = item.Availabilities.First();
                var availabilityReadModel = new StoreItemAvailabilityReadModel(
                    storeReadModel,
                    availability.Price,
                    storeSectionReadModel);

                return new StoreItemReadModel(
                    item.Id,
                    item.Name,
                    item.IsDeleted,
                    item.Comment,
                    item.IsTemporary,
                    new QuantityTypeReadModel(
                        (int)item.QuantityType,
                        item.QuantityType.ToString(),
                        item.QuantityType.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
                        item.QuantityType.GetAttribute<PriceLabelAttribute>().PriceLabel,
                        item.QuantityType.GetAttribute<QuantityLabelAttribute>().QuantityLabel,
                        item.QuantityType.GetAttribute<QuantityNormalizerAttribute>().Value),
                    item.QuantityInPacket,
                    new QuantityTypeInPacketReadModel(
                        (int)item.QuantityTypeInPacket,
                        item.QuantityTypeInPacket.ToString(),
                        item.QuantityTypeInPacket.GetAttribute<QuantityLabelAttribute>().QuantityLabel),
                    itemCategoryReadModel,
                    manufacturerReadModel,
                    availabilityReadModel.ToMonoList());
            }
        }
    }
}