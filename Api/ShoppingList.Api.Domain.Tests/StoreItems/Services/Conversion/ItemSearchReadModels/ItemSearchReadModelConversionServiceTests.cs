using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Core.Attributes;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.ItemSearchReadModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Fixtures;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Ports;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Fixtures;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Ports;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using ShoppingList.Api.Domain.TestKit.Stores.Fixtures;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Services.Conversion.ItemSearchReadModels
{
    public class ItemSearchReadModelConversionServiceTests
    {
        [Fact]
        public async Task ConvertAsync_WithNeitherItemCategoryNorManufacturer_ShouldConvertToReadModel()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            var store = local.CreateStore();
            var items = local.CreateItemsWithNeitherItemCategoryNorManufacturer(store).ToList();

            var itemCategoryIds = items
                .Where(i => i.ItemCategoryId != null)
                .Select(i => i.ItemCategoryId);

            var manufacturerIds = items
                .Where(i => i.ManufacturerId != null)
                .Select(i => i.ManufacturerId);

            var itemCategoryDict = local.CreateItemCategories(itemCategoryIds);
            var manufacturerDict = local.CreateManufacturers(manufacturerIds);

            local.ItemCategoryRepositoryMock.SetupFindByAsync(itemCategoryIds, itemCategoryDict.Values);
            local.ManufacturerRepositoryMock.SetupFindByAsync(manufacturerIds, manufacturerDict.Values);

            // Act
            var result = await service.ConvertAsync(items, store, default);

            // Assert
            var expected = local.ToSimpleReadModels(items, store, itemCategoryDict, manufacturerDict);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(expected);
            }
        }

        [Fact]
        public async Task ConvertAsync_WithItemCategoryAndNoManufacturer_ShouldConvertToReadModel()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            var store = local.CreateStore();
            var items = local.CreateItemsWithoutManufacturer(store).ToList();

            var itemCategoryIds = items
                .Where(i => i.ItemCategoryId != null)
                .Select(i => i.ItemCategoryId);

            var manufacturerIds = items
                .Where(i => i.ManufacturerId != null)
                .Select(i => i.ManufacturerId);

            var itemCategoryDict = local.CreateItemCategories(itemCategoryIds);
            var manufacturerDict = local.CreateManufacturers(manufacturerIds);

            local.ItemCategoryRepositoryMock.SetupFindByAsync(itemCategoryIds, itemCategoryDict.Values);
            local.ManufacturerRepositoryMock.SetupFindByAsync(manufacturerIds, manufacturerDict.Values);

            // Act
            var result = await service.ConvertAsync(items, store, default);

            // Assert
            var expected = local.ToSimpleReadModels(items, store, itemCategoryDict, manufacturerDict);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(expected);
            }
        }

        [Fact]
        public async Task ConvertAsync_WithManufacturerAndNoItemCategory_ShouldConvertToReadModel()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            var store = local.CreateStore();
            var items = local.CreateItemsWithoutItemCategory(store).ToList();

            var itemCategoryIds = items
                .Where(i => i.ItemCategoryId != null)
                .Select(i => i.ItemCategoryId);

            var manufacturerIds = items
                .Where(i => i.ManufacturerId != null)
                .Select(i => i.ManufacturerId);

            var itemCategoryDict = local.CreateItemCategories(itemCategoryIds);
            var manufacturerDict = local.CreateManufacturers(manufacturerIds);

            local.ItemCategoryRepositoryMock.SetupFindByAsync(itemCategoryIds, itemCategoryDict.Values);
            local.ManufacturerRepositoryMock.SetupFindByAsync(manufacturerIds, manufacturerDict.Values);

            // Act
            var result = await service.ConvertAsync(items, store, default);

            // Assert
            var expected = local.ToSimpleReadModels(items, store, itemCategoryDict, manufacturerDict);

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

            var store = local.CreateStore();
            var items = local.CreateItems(store).ToList();

            var itemCategoryIds = items
                .Where(i => i.ItemCategoryId != null)
                .Select(i => i.ItemCategoryId);

            var manufacturerIds = items
                .Where(i => i.ManufacturerId != null)
                .Select(i => i.ManufacturerId);

            var itemCategoryDict = local.CreateItemCategories(itemCategoryIds);
            var manufacturerDict = local.CreateManufacturers(manufacturerIds);

            local.ItemCategoryRepositoryMock.SetupFindByAsync(itemCategoryIds, itemCategoryDict.Values);
            local.ManufacturerRepositoryMock.SetupFindByAsync(manufacturerIds, manufacturerDict.Values);

            // Act
            var result = await service.ConvertAsync(items, store, default);

            // Assert
            var expected = local.ToSimpleReadModels(items, store, itemCategoryDict, manufacturerDict);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(expected);
            }
        }

        private class LocalFixture
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
            }

            public ItemSearchReadModelConversionService CreateService()
            {
                return Fixture.Create<ItemSearchReadModelConversionService>();
            }

            public IStore CreateStore()
            {
                return StoreFixture.CreateValid();
            }

            public IEnumerable<IStoreItem> CreateItems(IStore store)
            {
                List<StoreItemDefinition> definitions = new List<StoreItemDefinition>();
                for (int i = 0; i < 2; i++)
                {
                    var availabilityDef = new StoreItemAvailabilityDefinition
                    {
                        StoreId = store.Id,
                        DefaultSectionId = CommonFixture.ChooseRandom(store.Sections).Id
                    };
                    var availability = StoreItemAvailabilityFixture.Create(availabilityDef);

                    var def = new StoreItemDefinition
                    {
                        Availabilities = availability.ToMonoList()
                    };
                    definitions.Add(def);
                }

                return StoreItemFixture.CreateMany(definitions);
            }

            public IEnumerable<IStoreItem> CreateItemsWithoutItemCategory(IStore store)
            {
                List<StoreItemDefinition> definitions = new List<StoreItemDefinition>();
                for (int i = 0; i < 2; i++)
                {
                    var availabilityDef = new StoreItemAvailabilityDefinition
                    {
                        StoreId = store.Id,
                        DefaultSectionId = CommonFixture.ChooseRandom(store.Sections).Id
                    };
                    var availability = StoreItemAvailabilityFixture.Create(availabilityDef);

                    var def = new StoreItemDefinition
                    {
                        Availabilities = availability.ToMonoList(),
                        ItemCategoryId = null
                    };
                    definitions.Add(def);
                }

                return StoreItemFixture.CreateMany(definitions);
            }

            public IEnumerable<IStoreItem> CreateItemsWithoutManufacturer(IStore store)
            {
                List<StoreItemDefinition> definitions = new List<StoreItemDefinition>();
                for (int i = 0; i < 2; i++)
                {
                    var availabilityDef = new StoreItemAvailabilityDefinition
                    {
                        StoreId = store.Id,
                        DefaultSectionId = CommonFixture.ChooseRandom(store.Sections).Id
                    };
                    var availability = StoreItemAvailabilityFixture.Create(availabilityDef);

                    var def = new StoreItemDefinition
                    {
                        Availabilities = availability.ToMonoList(),
                        ManufacturerId = null
                    };
                    definitions.Add(def);
                }

                return StoreItemFixture.CreateMany(definitions);
            }

            public IEnumerable<IStoreItem> CreateItemsWithNeitherItemCategoryNorManufacturer(IStore store)
            {
                List<StoreItemDefinition> definitions = new List<StoreItemDefinition>();
                for (int i = 0; i < 2; i++)
                {
                    var availabilityDef = new StoreItemAvailabilityDefinition
                    {
                        StoreId = store.Id,
                        DefaultSectionId = CommonFixture.ChooseRandom(store.Sections).Id
                    };
                    var availability = StoreItemAvailabilityFixture.Create(availabilityDef);

                    var def = new StoreItemDefinition
                    {
                        Availabilities = availability.ToMonoList(),
                        ItemCategoryId = null,
                        ManufacturerId = null
                    };
                    definitions.Add(def);
                }

                return StoreItemFixture.CreateMany(definitions);
            }

            public Dictionary<ItemCategoryId, IItemCategory> CreateItemCategories(
                IEnumerable<ItemCategoryId> itemCategoryIds)
            {
                Dictionary<ItemCategoryId, IItemCategory> dict = new Dictionary<ItemCategoryId, IItemCategory>();
                foreach (var itemCategoryId in itemCategoryIds)
                {
                    var itemCategory = ItemCategoryFixture.GetItemCategory(itemCategoryId);
                    dict.Add(itemCategoryId, itemCategory);
                }
                return dict;
            }

            public Dictionary<ManufacturerId, IManufacturer> CreateManufacturers(
                IEnumerable<ManufacturerId> manufacturerIds)
            {
                Dictionary<ManufacturerId, IManufacturer> dict = new Dictionary<ManufacturerId, IManufacturer>();
                foreach (var manufacturerId in manufacturerIds)
                {
                    var manufacturer = ManufacturerFixture.Create(ManufacturerDefinition.FromId(manufacturerId));
                    dict.Add(manufacturerId, manufacturer);
                }
                return dict;
            }

            public IEnumerable<ItemSearchReadModel> ToSimpleReadModels(IEnumerable<IStoreItem> items,
                IStore store, Dictionary<ItemCategoryId, IItemCategory> itemCategories,
                Dictionary<ManufacturerId, IManufacturer> manufacturers)
            {
                foreach (IStoreItem item in items)
                {
                    ManufacturerReadModel manufacturerReadModel = null;
                    ItemCategoryReadModel itemCategoryReadModel = null;

                    if (item.ManufacturerId != null)
                    {
                        var manufacturer = manufacturers[item.ManufacturerId];

                        manufacturerReadModel = new ManufacturerReadModel(
                            manufacturer.Id,
                            manufacturer.Name,
                            manufacturer.IsDeleted);
                    }

                    if (item.ItemCategoryId != null)
                    {
                        var itemCategory = itemCategories[item.ItemCategoryId];

                        itemCategoryReadModel = new ItemCategoryReadModel(
                                itemCategory.Id,
                                itemCategory.Name,
                                itemCategory.IsDeleted);
                    }

                    var availability = item.Availabilities.Single(av => av.StoreId == store.Id);
                    var section = store.Sections.Single(s => s.Id == availability.DefaultSectionId);
                    var sectionReadModel = new StoreSectionReadModel(section.Id, section.Name,
                        section.SortingIndex, section.IsDefaultSection);

                    yield return new ItemSearchReadModel(
                        item.Id,
                        item.Name,
                        item.QuantityType.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
                        availability.Price,
                        manufacturerReadModel,
                        itemCategoryReadModel,
                        sectionReadModel);
                }
            }
        }
    }
}