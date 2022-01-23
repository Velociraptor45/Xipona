using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.Attributes;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
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
using ShoppingList.Api.Core.TestKit.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Ports;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Models;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Ports;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Stores.Ports;
using System;
using System.Collections.Generic;
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

            local.SetupItem();
            local.SetupManufacturer();
            local.SetupStore();
            local.SetupFindingNoItemCategory();
            local.SetupFindingManufacturer();
            local.SetupFindingStore();

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
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public ItemCategoryRepositoryMock ItemCategoryRepositoryMock { get; }
            public ManufacturerRepositoryMock ManufacturerRepositoryMock { get; }
            public StoreRepositoryMock StoreRepositoryMock { get; }

            public IStoreItem StoreItem { get; private set; }
            public IStore Store { get; private set; }
            public ManufacturerId ManufacturerId => StoreItem.ManufacturerId;
            public ItemCategoryId ItemCategoryId => StoreItem.ItemCategoryId;
            public IItemCategory ItemCategory { get; private set; }
            public IManufacturer Manufacturer { get; private set; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                ItemCategoryRepositoryMock = new ItemCategoryRepositoryMock(MockBehavior.Strict);
                ManufacturerRepositoryMock = new ManufacturerRepositoryMock(MockBehavior.Strict);
                StoreRepositoryMock = new StoreRepositoryMock(MockBehavior.Strict);
            }

            public StoreItemReadModelConversionService CreateService()
            {
                return new StoreItemReadModelConversionService(
                    ItemCategoryRepositoryMock.Object,
                    ManufacturerRepositoryMock.Object,
                    StoreRepositoryMock.Object);
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
                var availability = StoreItem.Availabilities.First();
                var section = StoreSectionMother.Default()
                    .WithId(availability.DefaultSectionId)
                    .Create();
                Store = StoreMother.Initial()
                    .WithId(availability.StoreId)
                    .WithSection(section)
                    .Create();
            }

            public void SetupItemCategory()
            {
                ItemCategory = ItemCategoryMother.NotDeleted()
                    .WithId(ItemCategoryId)
                    .Create();
            }

            public void SetupManufacturer()
            {
                Manufacturer = ManufacturerMother.NotDeleted()
                    .WithId(ManufacturerId)
                    .Create();
            }

            public StoreItemReadModel CreateSimpleReadModel()
            {
                var manufacturerReadModel = this.Manufacturer == null
                ? null
                : new ManufacturerReadModel(
                    Manufacturer.Id,
                    Manufacturer.Name,
                    Manufacturer.IsDeleted);

                var itemCategoryReadModel = ItemCategory == null
                    ? null
                    : new ItemCategoryReadModel(
                        ItemCategory.Id,
                        ItemCategory.Name,
                        ItemCategory.IsDeleted);

                var availabilityReadModel = CreateAvailabilityReadModel(Store, StoreItem.Availabilities.First());

                var itemType = StoreItem.ItemTypes.FirstOrDefault();
                List<ItemTypeReadModel> itemTypeReadModels = new List<ItemTypeReadModel>();
                if (itemType != null)
                {
                    var itemTypeAvailability = itemType.Availabilities.First();
                    var itemTypeAvailabilityReadModel = CreateAvailabilityReadModel(Store, itemTypeAvailability);
                    itemTypeReadModels.Add(new ItemTypeReadModel(
                        itemType.Id,
                        itemType.Name,
                        itemTypeAvailabilityReadModel.ToMonoList()));
                }

                return new StoreItemReadModel(
                    StoreItem.Id,
                    StoreItem.Name,
                    StoreItem.IsDeleted,
                    StoreItem.Comment,
                    StoreItem.IsTemporary,
                    new QuantityTypeReadModel(
                        (int)StoreItem.QuantityType,
                        StoreItem.QuantityType.ToString(),
                        StoreItem.QuantityType.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
                        StoreItem.QuantityType.GetAttribute<PriceLabelAttribute>().PriceLabel,
                        StoreItem.QuantityType.GetAttribute<QuantityLabelAttribute>().QuantityLabel,
                        StoreItem.QuantityType.GetAttribute<QuantityNormalizerAttribute>().Value),
                    StoreItem.QuantityInPacket,
                    new QuantityTypeInPacketReadModel(
                        (int)StoreItem.QuantityTypeInPacket,
                        StoreItem.QuantityTypeInPacket.ToString(),
                        StoreItem.QuantityTypeInPacket.GetAttribute<QuantityLabelAttribute>().QuantityLabel),
                    itemCategoryReadModel,
                    manufacturerReadModel,
                    availabilityReadModel.ToMonoList(),
                    itemTypeReadModels);
            }

            private StoreItemAvailabilityReadModel CreateAvailabilityReadModel(IStore store,
                IStoreItemAvailability availability)
            {
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

                return new StoreItemAvailabilityReadModel(
                   storeReadModel,
                   availability.Price,
                   storeSectionReadModel);
            }

            #region Mock Setup

            public void SetupFindingItemCategory()
            {
                ItemCategoryRepositoryMock.SetupFindByAsync(ItemCategoryId, ItemCategory);
            }

            public void SetupFindingNoItemCategory()
            {
                ItemCategoryRepositoryMock.SetupFindByAsync(ItemCategoryId, null);
            }

            public void SetupFindingManufacturer()
            {
                ManufacturerRepositoryMock.SetupFindByAsync(ManufacturerId, Manufacturer);
            }

            public void SetupFindingNoManufacturer()
            {
                ManufacturerRepositoryMock.SetupFindByAsync(ManufacturerId, null);
            }

            public void SetupFindingStore()
            {
                StoreRepositoryMock.SetupFindByAsync(Store.Id.ToMonoList(), Store.ToMonoList());
            }

            #endregion Mock Setup
        }
    }
}