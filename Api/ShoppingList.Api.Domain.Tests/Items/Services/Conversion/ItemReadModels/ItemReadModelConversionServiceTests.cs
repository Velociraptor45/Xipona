using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Conversion.ItemReadModels;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries.Quantities;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Ports;
using ShoppingList.Api.Domain.TestKit.Items.Models;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Models;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Ports;
using ShoppingList.Api.Domain.TestKit.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Stores.Models.Factories;
using ShoppingList.Api.Domain.TestKit.Stores.Ports;
using ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Items.Services.Conversion.ItemReadModels;

public class ItemReadModelConversionServiceTests
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

        TestPropertyNotSetException.ThrowIfNull(local.Item);

        // Act
        Func<Task<ItemReadModel>> function = async () => await service.ConvertAsync(local.Item, default);

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

        TestPropertyNotSetException.ThrowIfNull(local.Item);

        // Act
        Func<Task<ItemReadModel>> function = async () => await service.ConvertAsync(local.Item, default);

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

        TestPropertyNotSetException.ThrowIfNull(local.Item);

        // Act
        var result = await service.ConvertAsync(local.Item, default);

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

        TestPropertyNotSetException.ThrowIfNull(local.Item);

        // Act
        var result = await service.ConvertAsync(local.Item, default);

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

        TestPropertyNotSetException.ThrowIfNull(local.Item);

        // Act
        var result = await service.ConvertAsync(local.Item, default);

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

        TestPropertyNotSetException.ThrowIfNull(local.Item);

        // Act
        var result = await service.ConvertAsync(local.Item, default);

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
        private ManufacturerId ManufacturerId => Item!.ManufacturerId!.Value;
        private ItemCategoryId ItemCategoryId => Item!.ItemCategoryId!.Value;

        public LocalFixture()
        {
            _itemCategoryRepositoryMock = new ItemCategoryRepositoryMock(MockBehavior.Strict);
            _manufacturerRepositoryMock = new ManufacturerRepositoryMock(MockBehavior.Strict);
            _storeRepositoryMock = new StoreRepositoryMock(MockBehavior.Strict);
            _sectionFactoryMock = new StoreSectionFactoryMock(MockBehavior.Strict);
        }

        public IItem? Item { get; private set; }

        public ItemReadModelConversionService CreateService()
        {
            return new ItemReadModelConversionService(
                _itemCategoryRepositoryMock.Object,
                _manufacturerRepositoryMock.Object,
                _storeRepositoryMock.Object);
        }

        public void SetupItem()
        {
            Item = ItemMother.Initial()
                .WithAvailability(ItemAvailabilityMother.Initial().Create())
                .Create();
        }

        public void SetupItemWithoutItemCategory()
        {
            Item = ItemMother.Initial()
                .WithAvailability(ItemAvailabilityMother.Initial().Create())
                .WithoutItemCategoryId().Create();
        }

        public void SetupItemWithoutManufacturer()
        {
            Item = ItemMother.InitialWithoutManufacturer()
                .WithAvailability(ItemAvailabilityMother.Initial().Create())
                .Create();
        }

        public void SetupItemWithNeitherItemCategoryNorManufacturer()
        {
            Item = ItemMother.InitialTemporary()
                .WithAvailability(ItemAvailabilityMother.Initial().Create())
                .Create();
        }

        public void SetupStore()
        {
            TestPropertyNotSetException.ThrowIfNull(Item);
            var availability = Item.Availabilities.First();
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

        public ItemReadModel CreateSimpleReadModel()
        {
            TestPropertyNotSetException.ThrowIfNull(_store);
            TestPropertyNotSetException.ThrowIfNull(Item);

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

            var availabilityReadModel = CreateAvailabilityReadModel(_store, Item.Availabilities.First());

            var itemType = Item.ItemTypes.FirstOrDefault();
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

            var itemQuantityInPacket = Item.ItemQuantity.InPacket;
            var quantityTypeInPacketReadModel = itemQuantityInPacket is null
                ? null
                : new QuantityTypeInPacketReadModel(itemQuantityInPacket.Type);

            return new ItemReadModel(
                Item.Id,
                Item.Name,
                Item.IsDeleted,
                Item.Comment,
                Item.IsTemporary,
                new QuantityTypeReadModel(Item.ItemQuantity.Type),
                itemQuantityInPacket?.Quantity,
                quantityTypeInPacketReadModel,
                itemCategoryReadModel,
                manufacturerReadModel,
                availabilityReadModel.ToMonoList(),
                itemTypeReadModels);
        }

        private static ItemAvailabilityReadModel CreateAvailabilityReadModel(IStore store,
            IItemAvailability availability)
        {
            var section = store.Sections.First();
            var storeSectionReadModel = new ItemSectionReadModel(
                section.Id,
                section.Name,
                section.SortingIndex);

            var storeReadModel = new ItemStoreReadModel(
                store.Id,
                store.Name,
                storeSectionReadModel.ToMonoList());

            return new ItemAvailabilityReadModel(
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