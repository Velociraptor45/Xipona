using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.StoreItemReadModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries.Quantities;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Ports;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Models;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Stores.Models.Factories;
using ShoppingList.Api.Domain.TestKit.Stores.Ports;
using ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Services.Conversion.StoreItemReadModels;

public class StoreItemReadModelConversionServiceTests
{
    [Fact]
    public async Task ConvertAsync_WithInvalidItemCategoryId_ShouldConvertToReadModel()
    {
        // Arrange
        var local = new LocalFixture();
        var service = local.CreateService();

        local.SetupItem();
        local.SetupManufacturer();
        local.SetupStore();
        local.SetupFindingNoItemCategory();
        local.SetupFindingManufacturer();
        local.SetupFindingStore();

        TestPropertyNotSetException.ThrowIfNull(local.StoreItem);

        // Act
        Func<Task<StoreItemReadModel>> function = async () => await service.ConvertAsync(local.StoreItem, default);

        // Assert
        using (new AssertionScope())
        {
            await function.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemCategoryNotFound);
        }
    }

    [Fact]
    public async Task ConvertAsync_WithInvalidManufacturer_ShouldConvertToReadModel()
    {
        // Arrange
        var local = new LocalFixture();
        var service = local.CreateService();

        local.SetupItem();
        local.SetupItemCategory();
        local.SetupStore();
        local.SetupFindingItemCategory();
        local.SetupFindingNoManufacturer();
        local.SetupFindingStore();

        TestPropertyNotSetException.ThrowIfNull(local.StoreItem);

        // Act
        Func<Task<StoreItemReadModel>> function = async () => await service.ConvertAsync(local.StoreItem, default);

        // Assert
        using (new AssertionScope())
        {
            await function.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ManufacturerNotFound);
        }
    }

    [Fact]
    public async Task ConvertAsync_WithItemCategoryIsNull_ShouldConvertToReadModel()
    {
        // Arrange
        var local = new LocalFixture();
        var service = local.CreateService();

        local.SetupItemWithoutItemCategory();
        local.SetupManufacturer();
        local.SetupStore();
        local.SetupFindingManufacturer();
        local.SetupFindingStore();

        TestPropertyNotSetException.ThrowIfNull(local.StoreItem);

        // Act
        var result = await service.ConvertAsync(local.StoreItem, default);

        // Assert
        var expected = local.CreateSimpleReadModel();

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

        local.SetupItemWithoutManufacturer();
        local.SetupItemCategory();
        local.SetupStore();
        local.SetupFindingItemCategory();
        local.SetupFindingStore();

        TestPropertyNotSetException.ThrowIfNull(local.StoreItem);

        // Act
        var result = await service.ConvertAsync(local.StoreItem, default);

        // Assert
        var expected = local.CreateSimpleReadModel();

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

        local.SetupItemWithNeitherItemCategoryNorManufacturer();
        local.SetupStore();
        local.SetupFindingStore();

        TestPropertyNotSetException.ThrowIfNull(local.StoreItem);

        // Act
        var result = await service.ConvertAsync(local.StoreItem, default);

        // Assert
        var expected = local.CreateSimpleReadModel();

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

        local.SetupItem();
        local.SetupItemCategory();
        local.SetupManufacturer();
        local.SetupStore();

        local.SetupFindingItemCategory();
        local.SetupFindingManufacturer();
        local.SetupFindingStore();

        TestPropertyNotSetException.ThrowIfNull(local.StoreItem);

        // Act
        var result = await service.ConvertAsync(local.StoreItem, default);

        // Assert
        var expected = local.CreateSimpleReadModel();

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(expected);
        }
    }

    private class LocalFixture
    {
        private readonly StoreSectionFactoryMock _sectionFactoryMock;
        private readonly ItemCategoryRepositoryMock _itemCategoryRepositoryMock;
        private readonly ManufacturerRepositoryMock _manufacturerRepositoryMock;
        private readonly StoreRepositoryMock _storeRepositoryMock;
        private IStore? _store;
        private IItemCategory? _itemCategory;
        private IManufacturer? _manufacturer;
        private ManufacturerId ManufacturerId => StoreItem!.ManufacturerId!.Value;
        private ItemCategoryId ItemCategoryId => StoreItem!.ItemCategoryId!.Value;

        public LocalFixture()
        {
            _itemCategoryRepositoryMock = new ItemCategoryRepositoryMock(MockBehavior.Strict);
            _manufacturerRepositoryMock = new ManufacturerRepositoryMock(MockBehavior.Strict);
            _storeRepositoryMock = new StoreRepositoryMock(MockBehavior.Strict);
            _sectionFactoryMock = new StoreSectionFactoryMock(MockBehavior.Strict);
        }

        public IItem? StoreItem { get; private set; }

        public StoreItemReadModelConversionService CreateService()
        {
            return new StoreItemReadModelConversionService(
                _itemCategoryRepositoryMock.Object,
                _manufacturerRepositoryMock.Object,
                _storeRepositoryMock.Object);
        }

        public void SetupItem()
        {
            StoreItem = StoreItemMother.Initial()
                .WithAvailability(StoreItemAvailabilityMother.Initial().Create())
                .Create();
        }

        public void SetupItemWithoutItemCategory()
        {
            StoreItem = StoreItemMother.Initial()
                .WithAvailability(StoreItemAvailabilityMother.Initial().Create())
                .WithoutItemCategoryId().Create();
        }

        public void SetupItemWithoutManufacturer()
        {
            StoreItem = StoreItemMother.InitialWithoutManufacturer()
                .WithAvailability(StoreItemAvailabilityMother.Initial().Create())
                .Create();
        }

        public void SetupItemWithNeitherItemCategoryNorManufacturer()
        {
            StoreItem = StoreItemMother.InitialTemporary()
                .WithAvailability(StoreItemAvailabilityMother.Initial().Create())
                .Create();
        }

        public void SetupStore()
        {
            TestPropertyNotSetException.ThrowIfNull(StoreItem);
            var availability = StoreItem.Availabilities.First();
            var section = StoreSectionMother.Default()
                .WithId(availability.DefaultSectionId)
                .Create();
            var sections = new StoreSections(section.ToMonoList(), _sectionFactoryMock.Object);

            _store = StoreMother.Initial()
                .WithId(availability.StoreId)
                .WithSections(sections)
                .Create();
        }

        public void SetupItemCategory()
        {
            _itemCategory = ItemCategoryMother.NotDeleted()
                .WithId(ItemCategoryId)
                .Create();
        }

        public void SetupManufacturer()
        {
            _manufacturer = ManufacturerMother.NotDeleted()
                .WithId(ManufacturerId)
                .Create();
        }

        public StoreItemReadModel CreateSimpleReadModel()
        {
            TestPropertyNotSetException.ThrowIfNull(_store);
            TestPropertyNotSetException.ThrowIfNull(StoreItem);

            var manufacturerReadModel = _manufacturer == null
                ? null
                : new ManufacturerReadModel(
                    _manufacturer.Id,
                    _manufacturer.Name,
                    _manufacturer.IsDeleted);

            var itemCategoryReadModel = _itemCategory == null
                ? null
                : new ItemCategoryReadModel(
                    _itemCategory.Id,
                    _itemCategory.Name,
                    _itemCategory.IsDeleted);

            var availabilityReadModel = CreateAvailabilityReadModel(_store, StoreItem.Availabilities.First());

            var itemType = StoreItem.ItemTypes.FirstOrDefault();
            List<ItemTypeReadModel> itemTypeReadModels = new();
            if (itemType != null)
            {
                var itemTypeAvailability = itemType.Availabilities.First();
                var itemTypeAvailabilityReadModel = CreateAvailabilityReadModel(_store, itemTypeAvailability);
                itemTypeReadModels.Add(new ItemTypeReadModel(
                    itemType.Id,
                    itemType.Name,
                    itemTypeAvailabilityReadModel.ToMonoList()));
            }

            var itemQuantityInPacket = StoreItem.ItemQuantity.InPacket;
            var quantityTypeInPacketReadModel = itemQuantityInPacket is null
                ? null
                : new QuantityTypeInPacketReadModel(itemQuantityInPacket.Type);

            return new StoreItemReadModel(
                StoreItem.Id,
                StoreItem.Name,
                StoreItem.IsDeleted,
                StoreItem.Comment,
                StoreItem.IsTemporary,
                new QuantityTypeReadModel(StoreItem.ItemQuantity.Type),
                itemQuantityInPacket?.Quantity,
                quantityTypeInPacketReadModel,
                itemCategoryReadModel,
                manufacturerReadModel,
                availabilityReadModel.ToMonoList(),
                itemTypeReadModels);
        }

        private static StoreItemAvailabilityReadModel CreateAvailabilityReadModel(IStore store,
            IItemAvailability availability)
        {
            var section = store.Sections.First();
            var storeSectionReadModel = new StoreItemSectionReadModel(
                section.Id,
                section.Name,
                section.SortingIndex);

            var storeReadModel = new StoreItemStoreReadModel(
                store.Id,
                store.Name,
                storeSectionReadModel.ToMonoList());

            return new StoreItemAvailabilityReadModel(
                storeReadModel,
                availability.Price,
                storeSectionReadModel);
        }

        #region Mock Setup

        public void SetupFindingItemCategory()
        {
            _itemCategoryRepositoryMock.SetupFindByAsync(ItemCategoryId, _itemCategory);
        }

        public void SetupFindingNoItemCategory()
        {
            _itemCategoryRepositoryMock.SetupFindByAsync(ItemCategoryId, null);
        }

        public void SetupFindingManufacturer()
        {
            _manufacturerRepositoryMock.SetupFindByAsync(ManufacturerId, _manufacturer);
        }

        public void SetupFindingNoManufacturer()
        {
            _manufacturerRepositoryMock.SetupFindByAsync(ManufacturerId, null);
        }

        public void SetupFindingStore()
        {
            TestPropertyNotSetException.ThrowIfNull(_store);
            _storeRepositoryMock.SetupFindByAsync(_store.Id.ToMonoList(), _store.ToMonoList());
        }

        #endregion Mock Setup
    }
}