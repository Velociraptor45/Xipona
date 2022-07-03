﻿using ProjectHermes.ShoppingList.Api.Core.Attributes;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Conversion.ItemSearchReadModels;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Ports;
using ShoppingList.Api.Domain.TestKit.Items.Models;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Models;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Ports;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Items.Services.Conversion.ItemSearchReadModels;

public class ItemSearchReadModelConversionServiceTests
{
    private readonly LocalFixture _local;

    public ItemSearchReadModelConversionServiceTests()
    {
        _local = new LocalFixture();
    }

    [Fact]
    public async Task ConvertAsync_WithNeitherItemCategoryNorManufacturer_ShouldConvertToReadModel()
    {
        // Arrange
        var service = _local.CreateSut();

        _local.SetupStore();
        _local.SetupItemsWithNeitherItemCategoryNorManufacturer();

        _local.SetupItemCategories();
        _local.SetupManufacturers();
        _local.SetupFindingItemCategories();
        _local.SetupFindingManufacturers();

        // Act
        var result = await service.ConvertAsync(_local.Items, _local.Store, default);

        // Assert
        var expected = _local.CreateSimpleReadModels();

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(expected);
        }
    }

    [Fact]
    public async Task ConvertAsync_WithItemCategoryAndNoManufacturer_ShouldConvertToReadModel()
    {
        // Arrange
        var service = _local.CreateSut();

        _local.SetupStore();
        _local.SetupItemsWithoutManufacturer();

        _local.SetupItemCategories();
        _local.SetupManufacturers();
        _local.SetupFindingItemCategories();
        _local.SetupFindingManufacturers();

        // Act
        var result = await service.ConvertAsync(_local.Items, _local.Store, default);

        // Assert
        var expected = _local.CreateSimpleReadModels();

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(expected);
        }
    }

    [Fact]
    public async Task ConvertAsync_WithManufacturerAndNoItemCategory_ShouldConvertToReadModel()
    {
        // Arrange
        var service = _local.CreateSut();

        _local.SetupStore();
        _local.SetupItemsWithoutItemCategory();

        _local.SetupItemCategories();
        _local.SetupManufacturers();
        _local.SetupFindingItemCategories();
        _local.SetupFindingManufacturers();

        // Act
        var result = await service.ConvertAsync(_local.Items, _local.Store, default);

        // Assert
        var expected = _local.CreateSimpleReadModels();

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(expected);
        }
    }

    [Fact]
    public async Task ConvertAsync_WithItemCategoryAndManufacturer_ShouldConvertToReadModel()
    {
        // Arrange
        var service = _local.CreateSut();

        _local.SetupStore();
        _local.SetupItems();

        _local.SetupItemCategories();
        _local.SetupManufacturers();
        _local.SetupFindingItemCategories();
        _local.SetupFindingManufacturers();

        // Act
        var result = await service.ConvertAsync(_local.Items, _local.Store, default);

        // Assert
        var expected = _local.CreateSimpleReadModels();

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(expected);
        }
    }

    private class LocalFixture
    {
        public Fixture Fixture { get; }
        public CommonFixture CommonFixture { get; } = new CommonFixture();
        public ItemCategoryRepositoryMock ItemCategoryRepositoryMock { get; }

        public ManufacturerRepositoryMock ManufacturerRepositoryMock { get; }
        public List<IItem> Items { get; private set; }
        public IStore Store { get; private set; }
        public Dictionary<ItemCategoryId, IItemCategory> ItemCategories { get; } = new();
        public Dictionary<ManufacturerId, IManufacturer> Manufacturers { get; } = new();

        public LocalFixture()
        {
            Fixture = CommonFixture.GetNewFixture();

            ItemCategoryRepositoryMock = new ItemCategoryRepositoryMock(MockBehavior.Strict);
            ManufacturerRepositoryMock = new ManufacturerRepositoryMock(MockBehavior.Strict);
        }

        public ItemSearchReadModelConversionService CreateSut()
        {
            return new ItemSearchReadModelConversionService(ItemCategoryRepositoryMock.Object,
                ManufacturerRepositoryMock.Object);
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
            return ItemAvailabilityMother.Initial()
                .WithStoreId(Store.Id)
                .WithDefaultSectionId(CommonFixture.ChooseRandom(Store.Sections).Id)
                .Create();
        }

        public void SetupItemCategories()
        {
            var itemCategoryIds = Items
                .Where(i => i.ItemCategoryId != null)
                .Select(i => i.ItemCategoryId!.Value);

            foreach (var itemCategoryId in itemCategoryIds)
            {
                var itemCategory = ItemCategoryMother.NotDeleted().WithId(itemCategoryId).Create();
                ItemCategories.Add(itemCategoryId, itemCategory);
            }
        }

        public void SetupManufacturers()
        {
            var manufacturerIds = Items
                .Where(i => i.ManufacturerId != null)
                .Select(i => i.ManufacturerId!.Value);

            foreach (var manufacturerId in manufacturerIds)
            {
                var manufacturer = ManufacturerMother.NotDeleted().WithId(manufacturerId).Create();
                Manufacturers.Add(manufacturerId, manufacturer);
            }
        }

        public IEnumerable<SearchItemForShoppingResultReadModel> CreateSimpleReadModels()
        {
            foreach (IItem item in Items)
            {
                ManufacturerReadModel manufacturerReadModel = null;
                ItemCategoryReadModel itemCategoryReadModel = null;

                if (item.ManufacturerId != null)
                {
                    var manufacturer = Manufacturers[item.ManufacturerId.Value];

                    manufacturerReadModel = new ManufacturerReadModel(
                        manufacturer.Id,
                        manufacturer.Name,
                        manufacturer.IsDeleted);
                }

                if (item.ItemCategoryId != null)
                {
                    var itemCategory = ItemCategories[item.ItemCategoryId.Value];

                    itemCategoryReadModel = new ItemCategoryReadModel(
                        itemCategory.Id,
                        itemCategory.Name,
                        itemCategory.IsDeleted);
                }

                var availability = item.Availabilities.Single(av => av.StoreId == Store.Id);
                var section = Store.Sections.Single(s => s.Id == availability.DefaultSectionId);
                var sectionReadModel = new StoreSectionReadModel(section.Id, section.Name,
                    section.SortingIndex, section.IsDefaultSection);

                yield return new SearchItemForShoppingResultReadModel(
                    item.Id,
                    null,
                    item.Name.Value,
                    item.ItemQuantity.Type.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
                    availability.Price,
                    manufacturerReadModel,
                    itemCategoryReadModel,
                    sectionReadModel);
            }
        }

        #region Mock Setup

        public void SetupFindingItemCategories()
        {
            ItemCategoryRepositoryMock.SetupFindByAsync(ItemCategories.Keys, ItemCategories.Values);
        }

        public void SetupFindingManufacturers()
        {
            ManufacturerRepositoryMock.SetupFindByAsync(Manufacturers.Keys, Manufacturers.Values);
        }

        #endregion Mock Setup
    }
}