using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Shared;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Conversion.ItemReadModels;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries.Quantities;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Shared;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.TestKit.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Manufacturers.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Stores.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.TestKit.Stores.Ports;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.Items.Services.Conversion.ItemReadModels;

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
        Func<Task<ItemReadModel>> function = async () => await service.ConvertAsync(local.Item);

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
        Func<Task<ItemReadModel>> function = async () => await service.ConvertAsync(local.Item);

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
        var result = await service.ConvertAsync(local.Item);

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
        var result = await service.ConvertAsync(local.Item);

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
        var result = await service.ConvertAsync(local.Item);

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
        var result = await service.ConvertAsync(local.Item);

        // Assert
        var expected = local.CreateSimpleReadModel();

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(expected);
        }
    }

    [Fact]
    public async Task ConvertAsync_WithDeletedItemType_ShouldNotConvertItemTypeToReadModel()
    {
        // Arrange
        var local = new LocalFixture();
        var service = local.CreateService();

        local.SetupItemWithDeletedType();
        local.SetupItemCategory();
        local.SetupManufacturer();
        local.SetupStore();

        local.SetupFindingItemCategory();
        local.SetupFindingManufacturer();
        local.SetupFindingStore();

        TestPropertyNotSetException.ThrowIfNull(local.Item);

        // Act
        var result = await service.ConvertAsync(local.Item);

        // Assert
        var expected = local.CreateSimpleReadModelWithDeletedType();

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(expected);
        }
    }

    private class LocalFixture
    {
        private readonly SectionFactoryMock _sectionFactoryMock = new(MockBehavior.Strict);
        private readonly ItemCategoryRepositoryMock _itemCategoryRepositoryMock = new(MockBehavior.Strict);
        private readonly ManufacturerRepositoryMock _manufacturerRepositoryMock = new(MockBehavior.Strict);
        private readonly StoreRepositoryMock _storeRepositoryMock = new(MockBehavior.Strict);
        private IStore? _store;
        private IItemCategory? _itemCategory;
        private IManufacturer? _manufacturer;
        private ManufacturerId ManufacturerId => Item!.ManufacturerId!.Value;
        private ItemCategoryId ItemCategoryId => Item!.ItemCategoryId!.Value;

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

        public void SetupItemWithDeletedType()
        {
            var storeId = StoreId.New;
            var types = new List<IItemType>()
            {
                new ItemTypeBuilder()
                    .WithIsDeleted(false)
                    .WithAvailability(ItemAvailabilityMother.Initial().WithStoreId(storeId).Create())
                    .Create(),
                new ItemTypeBuilder()
                    .WithIsDeleted(true)
                    .WithAvailability(ItemAvailabilityMother.Initial().WithStoreId(storeId).Create())
                    .Create()
            };
            Item = ItemMother.InitialWithTypes()
                .WithTypes(new ItemTypes(types, new ItemTypeFactoryMock(MockBehavior.Strict).Object))
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
            var availability = Item.Availabilities.FirstOrDefault() ?? Item.ItemTypes.First().Availabilities.First();
            var section = SectionMother.Default()
                .WithId(availability.DefaultSectionId)
                .Create();
            var sections = new Sections([section], _sectionFactoryMock.Object);

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

        public ItemReadModel CreateSimpleReadModelWithDeletedType()
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

            var itemType = Item.ItemTypes.First();
            var itemTypeAvailability = itemType.Availabilities.First();
            var itemTypeAvailabilityReadModel = CreateAvailabilityReadModel(_store, itemTypeAvailability);
            List<ItemTypeReadModel> itemTypeReadModels = new()
            {
                new ItemTypeReadModel(
                    itemType.Id,
                    itemType.Name,
                    [itemTypeAvailabilityReadModel])
            };

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
                new List<ItemAvailabilityReadModel>(),
                itemTypeReadModels);
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
                    [itemTypeAvailabilityReadModel]));
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
                [availabilityReadModel],
                itemTypeReadModels);
        }

        private static ItemAvailabilityReadModel CreateAvailabilityReadModel(IStore store,
            ItemAvailability availability)
        {
            var section = store.Sections.First();
            var sectionReadModel = new ItemSectionReadModel(
                section.Id,
                section.Name,
                section.SortingIndex);

            var storeReadModel = new ItemStoreReadModel(
                store.Id,
                store.Name,
                [sectionReadModel]);

            return new ItemAvailabilityReadModel(
                storeReadModel,
                availability.Price,
                sectionReadModel);
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
            _storeRepositoryMock.SetupFindActiveByAsync([_store.Id], [_store]);
        }

        #endregion Mock Setup
    }
}