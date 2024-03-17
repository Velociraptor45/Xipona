using ProjectHermes.Xipona.Api.Core.Attributes;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Shared;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Conversion.ItemSearchReadModels;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Shared;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Manufacturers.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Shared;
using ProjectHermes.Xipona.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.Items.Services.Conversion.ItemSearchReadModels;

public class ItemSearchReadModelConversionServiceTests
{
    private readonly LocalFixture _fixture;

    public ItemSearchReadModelConversionServiceTests()
    {
        _fixture = new LocalFixture();
    }

    [Fact]
    public async Task ConvertAsync_WithNeitherItemCategoryNorManufacturer_ShouldConvertToReadModel()
    {
        // Arrange
        var service = _fixture.CreateSut();

        _fixture.SetupStore();
        _fixture.SetupItemsWithNeitherItemCategoryNorManufacturer();

        _fixture.SetupItemCategories();
        _fixture.SetupManufacturers();
        _fixture.SetupFindingItemCategories();
        _fixture.SetupFindingManufacturers();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Items);
        TestPropertyNotSetException.ThrowIfNull(_fixture.Store);

        // Act
        var result = await service.ConvertAsync(_fixture.Items, _fixture.Store);

        // Assert
        var expected = _fixture.CreateSimpleReadModels();

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(expected);
        }
    }

    [Fact]
    public async Task ConvertAsync_WithItemCategoryAndNoManufacturer_ShouldConvertToReadModel()
    {
        // Arrange
        var service = _fixture.CreateSut();

        _fixture.SetupStore();
        _fixture.SetupItemsWithoutManufacturer();

        _fixture.SetupItemCategories();
        _fixture.SetupManufacturers();
        _fixture.SetupFindingItemCategories();
        _fixture.SetupFindingManufacturers();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Items);
        TestPropertyNotSetException.ThrowIfNull(_fixture.Store);

        // Act
        var result = await service.ConvertAsync(_fixture.Items, _fixture.Store);

        // Assert
        var expected = _fixture.CreateSimpleReadModels();

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(expected);
        }
    }

    [Fact]
    public async Task ConvertAsync_WithManufacturerAndNoItemCategory_ShouldConvertToReadModel()
    {
        // Arrange
        var service = _fixture.CreateSut();

        _fixture.SetupStore();
        _fixture.SetupItemsWithoutItemCategory();

        _fixture.SetupItemCategories();
        _fixture.SetupManufacturers();
        _fixture.SetupFindingItemCategories();
        _fixture.SetupFindingManufacturers();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Items);
        TestPropertyNotSetException.ThrowIfNull(_fixture.Store);

        // Act
        var result = await service.ConvertAsync(_fixture.Items, _fixture.Store);

        // Assert
        var expected = _fixture.CreateSimpleReadModels();

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(expected);
        }
    }

    [Fact]
    public async Task ConvertAsync_WithItemCategoryAndManufacturer_ShouldConvertToReadModel()
    {
        // Arrange
        var service = _fixture.CreateSut();

        _fixture.SetupStore();
        _fixture.SetupItems();

        _fixture.SetupItemCategories();
        _fixture.SetupManufacturers();
        _fixture.SetupFindingItemCategories();
        _fixture.SetupFindingManufacturers();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Items);
        TestPropertyNotSetException.ThrowIfNull(_fixture.Store);

        // Act
        var result = await service.ConvertAsync(_fixture.Items, _fixture.Store);

        // Assert
        var expected = _fixture.CreateSimpleReadModels();

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(expected);
        }
    }

    private class LocalFixture
    {
        private readonly CommonFixture _commonFixture = new CommonFixture();
        private readonly ItemCategoryRepositoryMock _itemCategoryRepositoryMock = new(MockBehavior.Strict);
        private readonly ManufacturerRepositoryMock _manufacturerRepositoryMock = new(MockBehavior.Strict);
        private readonly Dictionary<ItemCategoryId, IItemCategory> _itemCategories = new();
        private readonly Dictionary<ManufacturerId, IManufacturer> _manufacturers = new();

        public List<IItem>? Items { get; private set; }
        public IStore? Store { get; private set; }

        public ItemSearchReadModelConversionService CreateSut()
        {
            return new ItemSearchReadModelConversionService(_itemCategoryRepositoryMock.Object,
                _manufacturerRepositoryMock.Object);
        }

        public void SetupStore()
        {
            Store = StoreMother.Initial().Create();
        }

        public void SetupItems()
        {
            var availability = CreateAvailability();

            Items = ((IEnumerable<IItem>)ItemMother.Initial()
                    .WithAvailability(availability)
                    .CreateMany(2))
                .ToList();
        }

        public void SetupItemsWithoutItemCategory()
        {
            var availability = CreateAvailability();

            Items = ((IEnumerable<IItem>)ItemMother.Initial()
                    .WithoutItemCategoryId()
                    .WithAvailability(availability)
                    .CreateMany(2))
                .ToList();
        }

        public void SetupItemsWithoutManufacturer()
        {
            var availability = CreateAvailability();

            Items = ((IEnumerable<IItem>)ItemMother.InitialWithoutManufacturer()
                    .WithAvailability(availability)
                    .CreateMany(2))
                .ToList();
        }

        public void SetupItemsWithNeitherItemCategoryNorManufacturer()
        {
            var availability = CreateAvailability();

            Items = ((IEnumerable<IItem>)ItemMother.InitialTemporary()
                    .WithAvailability(availability)
                    .CreateMany(2))
                .ToList();
        }

        private ItemAvailability CreateAvailability()
        {
            TestPropertyNotSetException.ThrowIfNull(Store);

            return ItemAvailabilityMother.Initial()
                .WithStoreId(Store.Id)
                .WithDefaultSectionId(_commonFixture.ChooseRandom(Store.Sections).Id)
                .Create();
        }

        public void SetupItemCategories()
        {
            TestPropertyNotSetException.ThrowIfNull(Items);

            var itemCategoryIds = Items
                .Where(i => i.ItemCategoryId != null)
                .Select(i => i.ItemCategoryId!.Value);

            foreach (var itemCategoryId in itemCategoryIds)
            {
                var itemCategory = ItemCategoryMother.NotDeleted().WithId(itemCategoryId).Create();
                _itemCategories.Add(itemCategoryId, itemCategory);
            }
        }

        public void SetupManufacturers()
        {
            TestPropertyNotSetException.ThrowIfNull(Items);

            var manufacturerIds = Items
                .Where(i => i.ManufacturerId != null)
                .Select(i => i.ManufacturerId!.Value);

            foreach (var manufacturerId in manufacturerIds)
            {
                var manufacturer = ManufacturerMother.NotDeleted().WithId(manufacturerId).Create();
                _manufacturers.Add(manufacturerId, manufacturer);
            }
        }

        public IEnumerable<SearchItemForShoppingResultReadModel> CreateSimpleReadModels()
        {
            TestPropertyNotSetException.ThrowIfNull(Items);
            TestPropertyNotSetException.ThrowIfNull(Store);

            foreach (IItem item in Items)
            {
                ManufacturerReadModel? manufacturerReadModel = null;
                ItemCategoryReadModel? itemCategoryReadModel = null;

                if (item.ManufacturerId != null)
                {
                    var manufacturer = _manufacturers[item.ManufacturerId.Value];

                    manufacturerReadModel = new ManufacturerReadModel(
                        manufacturer.Id,
                        manufacturer.Name,
                        manufacturer.IsDeleted);
                }

                if (item.ItemCategoryId != null)
                {
                    var itemCategory = _itemCategories[item.ItemCategoryId.Value];

                    itemCategoryReadModel = new ItemCategoryReadModel(
                        itemCategory.Id,
                        itemCategory.Name,
                        itemCategory.IsDeleted);
                }

                var availability = item.Availabilities.Single(av => av.StoreId == Store.Id);
                var section = Store.Sections.Single(s => s.Id == availability.DefaultSectionId);
                var sectionReadModel = new SectionReadModel(section.Id, section.Name,
                    section.SortingIndex, section.IsDefaultSection);

                yield return new SearchItemForShoppingResultReadModel(
                    item.Id,
                    null,
                    item.Name,
                    item.ItemQuantity.Type.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
                    availability.Price,
                    item.ItemQuantity.Type.GetAttribute<PriceLabelAttribute>().PriceLabel,
                    manufacturerReadModel,
                    itemCategoryReadModel,
                    sectionReadModel);
            }
        }

        #region Mock Setup

        public void SetupFindingItemCategories()
        {
            _itemCategoryRepositoryMock.SetupFindByAsync(_itemCategories.Keys, _itemCategories.Values);
        }

        public void SetupFindingManufacturers()
        {
            _manufacturerRepositoryMock.SetupFindByAsync(_manufacturers.Keys, _manufacturers.Values);
        }

        #endregion Mock Setup
    }
}