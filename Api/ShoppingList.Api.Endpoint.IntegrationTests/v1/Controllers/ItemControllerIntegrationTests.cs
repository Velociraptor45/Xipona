﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.UpdateItemPrice;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.UpdateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.SearchItemsByItemCategory;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Shared;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities;
using ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.Items.Entities;
using ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.Stores.Entities;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System;
using System.Text.RegularExpressions;
using Xunit;
using Item = ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities.Item;
using ItemAvailabilityContract = ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared.ItemAvailabilityContract;
using ItemCategoryEntities = ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Entities;
using ItemType = ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities.ItemType;
using Store = ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities.Store;

namespace ProjectHermes.ShoppingList.Api.Endpoint.IntegrationTests.v1.Controllers;

public class ItemControllerIntegrationTests
{
    [Collection(DockerCollection.Name)]
    public sealed class UpdateItemWithTypesAsync
    {
        private readonly UpdateItemWithTypesAsyncFixture _fixture;

        public UpdateItemWithTypesAsync(DockerFixture dockerFixture)
        {
            _fixture = new UpdateItemWithTypesAsyncFixture(dockerFixture);
        }

        [Fact]
        public async Task UpdateItemWithTypesAsync_WithItemUpdatedTwiceAlready_ShouldReturnOk()
        {
            // Arrange
            await _fixture.PrepareDatabaseAsync();
            await _fixture.SetupSecondLevelPredecessorAsync();
            await _fixture.SetupFirstLevelPredecessorAsync();
            await _fixture.SetupCurrentItemAsync();
            await _fixture.SetupContractAsync();

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.CurrentItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.FirstLevelPredecessor);
            TestPropertyNotSetException.ThrowIfNull(_fixture.SecondLevelPredecessor);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);

            // Act
            var response = await sut.UpdateItemWithTypesAsync(_fixture.CurrentItem.Id.Value, _fixture.Contract);

            // Assert
            response.Should().BeOfType<OkResult>();
            var allEntities = (await _fixture.LoadAllItemEntities()).ToList();

            allEntities.Should().Contain(e => e.Id == _fixture.SecondLevelPredecessor.Id.Value);
            allEntities.Should().Contain(e => e.Id == _fixture.FirstLevelPredecessor.Id.Value);
            allEntities.Should().Contain(e => e.Id == _fixture.CurrentItem.Id.Value);
            allEntities.Should().Contain(e =>
                e.Id != _fixture.SecondLevelPredecessor.Id.Value
                && e.Id != _fixture.FirstLevelPredecessor.Id.Value
                && e.Id != _fixture.CurrentItem.Id.Value);

            var secondLevelEntity = allEntities.Single(e => e.Id == _fixture.SecondLevelPredecessor.Id.Value);
            var firstLevelEntity = allEntities.Single(e => e.Id == _fixture.FirstLevelPredecessor.Id.Value);
            var currentEntity = allEntities.Single(e => e.Id == _fixture.CurrentItem.Id.Value);
            var newEntity = allEntities.Single(e =>
                e.Id != _fixture.SecondLevelPredecessor.Id.Value
                && e.Id != _fixture.FirstLevelPredecessor.Id.Value
                && e.Id != _fixture.CurrentItem.Id.Value);

            // second level item should not be altered
            secondLevelEntity.Deleted.Should().BeTrue();
            secondLevelEntity.Predecessor.Should().BeNull();
            secondLevelEntity.AvailableAt.Should().BeEmpty();
            secondLevelEntity.ItemTypes.Should().HaveCount(_fixture.SecondLevelPredecessor.ItemTypes.Count);
            foreach (var type in _fixture.SecondLevelPredecessor.ItemTypes)
            {
                secondLevelEntity.ItemTypes.Should().Contain(e => e.Id == type.Id.Value);
                var entityType = secondLevelEntity.ItemTypes.Single(e => e.Id == type.Id.Value);
                foreach (var av in type.Availabilities)
                {
                    entityType.AvailableAt.Should().HaveCount(type.Availabilities.Count);
                    entityType.AvailableAt.Should().Contain(eav =>
                        eav.ItemTypeId == type.Id.Value
                        && eav.StoreId == av.StoreId.Value
                        && Math.Abs(eav.Price - av.Price.Value) < 0.01f
                        && eav.DefaultSectionId == av.DefaultSectionId.Value);
                }
            }

            // first level item should not be altered
            firstLevelEntity.Deleted.Should().BeTrue();
            firstLevelEntity.PredecessorId.Should().Be(secondLevelEntity.Id);
            firstLevelEntity.AvailableAt.Should().BeEmpty();
            firstLevelEntity.ItemTypes.Should().HaveCount(_fixture.FirstLevelPredecessor.ItemTypes.Count);
            foreach (var type in _fixture.FirstLevelPredecessor.ItemTypes)
            {
                firstLevelEntity.ItemTypes.Should().Contain(e => e.Id == type.Id.Value);
                var entityType = firstLevelEntity.ItemTypes.Single(e => e.Id == type.Id.Value);
                foreach (var av in type.Availabilities)
                {
                    entityType.AvailableAt.Should().HaveCount(type.Availabilities.Count);
                    entityType.AvailableAt.Should().Contain(eav =>
                        eav.ItemTypeId == type.Id.Value
                        && eav.StoreId == av.StoreId.Value
                        && Math.Abs(eav.Price - av.Price.Value) < 0.01f
                        && eav.DefaultSectionId == av.DefaultSectionId.Value);
                }
            }

            // second level item should not be deleted but not altered otherwise
            currentEntity.Deleted.Should().BeTrue();
            currentEntity.PredecessorId.Should().Be(firstLevelEntity.Id);
            currentEntity.AvailableAt.Should().BeEmpty();
            currentEntity.ItemTypes.Should().HaveCount(_fixture.CurrentItem.ItemTypes.Count);
            foreach (var type in _fixture.CurrentItem.ItemTypes)
            {
                currentEntity.ItemTypes.Should().Contain(e => e.Id == type.Id.Value);
                var entityType = currentEntity.ItemTypes.Single(e => e.Id == type.Id.Value);
                foreach (var av in type.Availabilities)
                {
                    entityType.AvailableAt.Should().HaveCount(type.Availabilities.Count);
                    entityType.AvailableAt.Should().Contain(eav =>
                        eav.ItemTypeId == type.Id.Value
                        && eav.StoreId == av.StoreId.Value
                        && Math.Abs(eav.Price - av.Price.Value) < 0.01f
                        && eav.DefaultSectionId == av.DefaultSectionId.Value);
                }
            }

            // there should be a new entity with the CurrentItem as predecessor
            // and the ItemTypes of the CurrentItem as predecessors of its ItemTypes
            // and the correct values should be set in the ItemTypes
            newEntity.Deleted.Should().BeFalse();
            newEntity.PredecessorId.Should().Be(currentEntity.Id);
            newEntity.AvailableAt.Should().BeEmpty();
            newEntity.ItemTypes.Should().HaveCount(_fixture.Contract.ItemTypes.Count());
            newEntity.ItemTypes.Select(t => t.PredecessorId).Should().NotContainNulls();
            var contractItemTypes = _fixture.Contract.ItemTypes.ToList();
            var entitiesAsContractTypes = newEntity.ItemTypes
                .Select(t => new UpdateItemTypeContract(t.PredecessorId!.Value, t.Name,
                    t.AvailableAt.Select(av => new ItemAvailabilityContract
                    {
                        DefaultSectionId = av.DefaultSectionId,
                        Price = av.Price,
                        StoreId = av.StoreId
                    })))
                .ToList();

            entitiesAsContractTypes.Should().BeEquivalentTo(contractItemTypes);
        }

        private sealed class UpdateItemWithTypesAsyncFixture : ItemControllerFixture
        {
            private List<IStore> _newStores = new();
            private IManufacturer? _currentManufacturer;
            private IItemCategory? _currentItemCategory;
            private IManufacturer? _firstLevelManufacturer;
            private IItemCategory? _firstLevelItemCategory;
            private IManufacturer? _secondLevelManufacturer;
            private IItemCategory? _secondLevelItemCategory;
            private IManufacturer? _newManufacturer;
            private IItemCategory? _newItemCategory;

            public UpdateItemWithTypesAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public IItem? CurrentItem { get; private set; }
            public IItem? SecondLevelPredecessor { get; private set; }
            public IItem? FirstLevelPredecessor { get; private set; }
            public UpdateItemWithTypesContract? Contract { get; private set; }

            public async Task SetupCurrentItemAsync()
            {
                var builder = ItemMother.InitialWithTypes();

                if (FirstLevelPredecessor is not null)
                {
                    var factory = ArrangeScope.ServiceProvider.GetRequiredService<IItemTypeFactory>();
                    var types = FirstLevelPredecessor.ItemTypes
                        .Select(t =>
                        {
                            var type = new ItemTypeBuilder().Create();
                            type.SetPredecessor(t);
                            return type;
                        });
                    builder.WithTypes(new ItemTypes(types, factory));
                }

                CurrentItem = builder.Create();
                _currentItemCategory = ItemCategoryMother.NotDeleted()
                    .WithId(CurrentItem.ItemCategoryId ?? ItemCategoryId.New)
                    .Create();
                _currentManufacturer = ManufacturerMother.NotDeleted()
                    .WithId(CurrentItem.ManufacturerId ?? ManufacturerId.New)
                    .Create();

                if (FirstLevelPredecessor is not null)
                    CurrentItem.SetPredecessor(FirstLevelPredecessor);

                await StoreAsync(CurrentItem, _currentManufacturer, _currentItemCategory);
            }

            public async Task SetupFirstLevelPredecessorAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(SecondLevelPredecessor);

                var factory = ArrangeScope.ServiceProvider.GetRequiredService<IItemTypeFactory>();
                var types = SecondLevelPredecessor.ItemTypes
                    .Select(t =>
                    {
                        var type = new ItemTypeBuilder().Create();
                        type.SetPredecessor(t);
                        return type;
                    });

                FirstLevelPredecessor = ItemMother.InitialWithTypes()
                    .WithIsDeleted(true)
                    .WithTypes(new ItemTypes(types, factory))
                    .Create();
                FirstLevelPredecessor.SetPredecessor(SecondLevelPredecessor);
                _firstLevelItemCategory = ItemCategoryMother.NotDeleted()
                    .WithId(FirstLevelPredecessor.ItemCategoryId ?? ItemCategoryId.New)
                    .Create();
                _firstLevelManufacturer = ManufacturerMother.NotDeleted()
                    .WithId(FirstLevelPredecessor.ManufacturerId ?? ManufacturerId.New)
                    .Create();

                await StoreAsync(FirstLevelPredecessor, _firstLevelManufacturer, _firstLevelItemCategory);
            }

            public async Task SetupSecondLevelPredecessorAsync()
            {
                SecondLevelPredecessor = ItemMother.InitialWithTypes()
                    .WithIsDeleted(true)
                    .Create();
                _secondLevelItemCategory = ItemCategoryMother.NotDeleted()
                    .WithId(SecondLevelPredecessor.ItemCategoryId ?? ItemCategoryId.New)
                    .Create();
                _secondLevelManufacturer = ManufacturerMother.NotDeleted()
                    .WithId(SecondLevelPredecessor.ManufacturerId ?? ManufacturerId.New)
                    .Create();

                await StoreAsync(SecondLevelPredecessor, _secondLevelManufacturer, _secondLevelItemCategory);
            }

            public async Task SetupContractAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(CurrentItem);

                var itemTypes = CurrentItem.ItemTypes
                    .Select(t => new TestBuilder<UpdateItemTypeContract>()
                        .AfterCreation(c => c.OldId = t.Id.Value)
                        .Create())
                    .ToList();

                Contract = new TestBuilder<UpdateItemWithTypesContract>()
                    .AfterCreation(c => c.QuantityType = CurrentItem.ItemQuantity.Type.ToInt())
                    .AfterCreation(c => c.QuantityTypeInPacket = CurrentItem.ItemQuantity.InPacket?.Type.ToInt())
                    .AfterCreation(c => c.ItemTypes = itemTypes)
                    .Create();

                _newItemCategory = ItemCategoryMother.NotDeleted()
                    .WithId(new ItemCategoryId(Contract.ItemCategoryId))
                    .Create();
                _newManufacturer = ManufacturerMother.NotDeleted()
                    .WithId(Contract.ManufacturerId.HasValue
                        ? new ManufacturerId(Contract.ManufacturerId.Value)
                        : ManufacturerId.New)
                    .Create();

                _newStores = new List<IStore>();
                var factory = ArrangeScope.ServiceProvider.GetRequiredService<ISectionFactory>();

                foreach (var av in Contract.ItemTypes.SelectMany(t => t.Availabilities))
                {
                    var section = new SectionBuilder()
                        .WithId(new SectionId(av.DefaultSectionId))
                        .CreateMany(1);

                    var store = new StoreBuilder()
                        .WithIsDeleted(false)
                        .WithId(new StoreId(av.StoreId))
                        .WithSections(new Sections(section, factory))
                        .Create();

                    _newStores.Add(store);
                }

                await StoreAsync(itemCategory: _newItemCategory, manufacturer: _newManufacturer, stores: _newStores);
            }

            public async Task PrepareDatabaseAsync()
            {
                await ApplyMigrationsAsync(ArrangeScope);
            }
        }
    }

    [Collection(DockerCollection.Name)]
    public sealed class UpdateItemPriceAsync
    {
        private readonly UpdateItemPriceAsyncFixture _fixture;

        public UpdateItemPriceAsync(DockerFixture dockerFixture)
        {
            _fixture = new UpdateItemPriceAsyncFixture(dockerFixture);
        }

        [Fact]
        public async Task UpdateItemPriceAsync_WithoutTypes_ShouldUpdateItem()
        {
            // Arrange
            _fixture.SetupItemId();
            _fixture.SetupContractWithoutItemType();
            _fixture.SetupExistingItemWithoutTypes();
            _fixture.SetupExpectedResultWithoutTypes();
            await _fixture.PrepareDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedOldItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedNewItem);

            // Act
            await sut.UpdateItemPriceAsync(_fixture.ItemId.Value.Value, _fixture.Contract);

            // Assert
            var allStoredItems = (await _fixture.LoadAllItemEntities()).ToList();
            allStoredItems.Should().HaveCount(2);
            allStoredItems.Should().Contain(i => i.Id == _fixture.ExpectedOldItem.Id);
            allStoredItems.Should().Contain(i => i.Id != _fixture.ExpectedOldItem.Id);

            var oldItem = allStoredItems.First(i => i.Id == _fixture.ExpectedOldItem.Id);
            oldItem.Should().BeEquivalentTo(_fixture.ExpectedOldItem,
                opt => opt.Excluding(info => Regex.IsMatch(info.SelectedMemberPath, @"AvailableAt\[\d+\].Item")));

            var newItem = allStoredItems.First(i => i.Id != _fixture.ExpectedOldItem.Id);
            newItem.Should().BeEquivalentTo(_fixture.ExpectedNewItem,
                opt => opt.Excluding(info => info.SelectedMemberPath == "Id"
                                             || info.SelectedMemberPath == "Predecessor"
                                             || Regex.IsMatch(info.SelectedMemberPath, @"AvailableAt\[\d+\].Item")));
        }

        [Fact]
        public async Task UpdateItemPriceAsync_WithItemTypes_ShouldUpdateItem()
        {
            // Arrange
            _fixture.SetupItemId();
            _fixture.SetupContractWithoutItemType();
            _fixture.SetupExistingItemWithTypes();
            _fixture.SetupExpectedResultWithTypes();
            await _fixture.PrepareDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedOldItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedNewItem);

            // Act
            await sut.UpdateItemPriceAsync(_fixture.ItemId.Value.Value, _fixture.Contract);

            // Assert
            var allStoredItems = (await _fixture.LoadAllItemEntities()).ToList();
            allStoredItems.Should().HaveCount(2);
            allStoredItems.Should().Contain(i => i.Id == _fixture.ExpectedOldItem.Id);
            allStoredItems.Should().Contain(i => i.Id != _fixture.ExpectedOldItem.Id);

            var oldItem = allStoredItems.First(i => i.Id == _fixture.ExpectedOldItem.Id);
            oldItem.Should().BeEquivalentTo(_fixture.ExpectedOldItem,
                opt => opt.Excluding(info =>
                    Regex.IsMatch(info.SelectedMemberPath, @"ItemTypes\[\d+\].Item")
                    || Regex.IsMatch(info.SelectedMemberPath, @"AvailableAt\[\d+\].ItemType")
                    ));

            var newItem = allStoredItems.First(i => i.Id != _fixture.ExpectedOldItem.Id);
            newItem.Should().BeEquivalentTo(_fixture.ExpectedNewItem,
                opt => opt.Excluding(info => info.SelectedMemberPath == "Id"
                                             || info.SelectedMemberPath == "Predecessor"
                                             || Regex.IsMatch(info.SelectedMemberPath, @"ItemTypes\[\d+\].Id")
                                             || Regex.IsMatch(info.SelectedMemberPath, @"ItemTypes\[\d+\].Item")
                                             || Regex.IsMatch(info.SelectedMemberPath, @"ItemTypes\[\d+\].Predecessor\b")
                                             || Regex.IsMatch(info.SelectedMemberPath, @"AvailableAt\[\d+\].ItemType")
                                             ));
        }

        private sealed class UpdateItemPriceAsyncFixture : ItemControllerFixture
        {
            private Item? _existingItem;

            public UpdateItemPriceAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public ItemId? ItemId { get; private set; }
            public UpdateItemPriceContract? Contract { get; private set; }
            public Item? ExpectedNewItem { get; private set; }
            public Item? ExpectedOldItem { get; private set; }

            public void SetupItemId()
            {
                ItemId = Domain.Items.Models.ItemId.New;
            }

            public void SetupContractWithoutItemType()
            {
                var price = new DomainTestBuilder<Price>().Create();
                Contract = new TestBuilder<UpdateItemPriceContract>()
                    .FillPropertyWith(c => c.ItemTypeId, null)
                    .FillPropertyWith(c => c.Price, price.Value)
                    .Create();
            }

            public void SetupExistingItemWithoutTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(Contract);
                TestPropertyNotSetException.ThrowIfNull(ItemId);

                _existingItem = ItemEntityMother.InitialForStore(Contract.StoreId)
                    .WithId(ItemId.Value)
                    .Create();
            }

            public void SetupExistingItemWithTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(Contract);
                TestPropertyNotSetException.ThrowIfNull(ItemId);

                var itemTypes = ItemTypeEntityMother.InitialForStore(Contract.StoreId)
                    .CreateMany(1)
                    .ToList();
                _existingItem = ItemEntityMother.InitialWithTypes()
                    .WithId(ItemId.Value)
                    .WithItemTypes(itemTypes)
                    .Create();
            }

            public async Task PrepareDatabaseAsync()
            {
                await ApplyMigrationsAsync(ArrangeScope);

                TestPropertyNotSetException.ThrowIfNull(_existingItem);

                using var scope = CreateServiceScope();
                await using var context = GetContextInstance<ItemContext>(ArrangeScope);

                context.Add(_existingItem);

                await context.SaveChangesAsync();
            }

            public void SetupExpectedResultWithoutTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingItem);
                TestPropertyNotSetException.ThrowIfNull(Contract);

                ExpectedOldItem = new Item
                {
                    Id = _existingItem.Id,
                    AvailableAt = _existingItem.AvailableAt,
                    Name = _existingItem.Name,
                    Comment = _existingItem.Comment,
                    CreatedFrom = _existingItem.CreatedFrom,
                    Deleted = true,
                    IsTemporary = _existingItem.IsTemporary,
                    ItemCategoryId = _existingItem.ItemCategoryId,
                    ManufacturerId = _existingItem.ManufacturerId,
                    QuantityInPacket = _existingItem.QuantityInPacket,
                    QuantityType = _existingItem.QuantityType,
                    QuantityTypeInPacket = _existingItem.QuantityTypeInPacket,
                    PredecessorId = null,
                    ItemTypes = new List<ItemType>()
                };

                ExpectedNewItem = new Item
                {
                    AvailableAt = _existingItem.AvailableAt.Select(av => new AvailableAt
                    {
                        StoreId = av.StoreId,
                        Price = Contract.Price,
                        DefaultSectionId = av.DefaultSectionId
                    }).ToList(),
                    Name = _existingItem.Name,
                    Comment = _existingItem.Comment,
                    CreatedFrom = _existingItem.CreatedFrom,
                    Deleted = false,
                    IsTemporary = false,
                    ItemCategoryId = _existingItem.ItemCategoryId,
                    ManufacturerId = _existingItem.ManufacturerId,
                    QuantityInPacket = _existingItem.QuantityInPacket,
                    QuantityType = _existingItem.QuantityType,
                    QuantityTypeInPacket = _existingItem.QuantityTypeInPacket,
                    PredecessorId = _existingItem.Id,
                    ItemTypes = new List<ItemType>()
                };
            }

            public void SetupExpectedResultWithTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingItem);
                TestPropertyNotSetException.ThrowIfNull(Contract);

                ExpectedOldItem = new Item
                {
                    Id = _existingItem.Id,
                    AvailableAt = new List<AvailableAt>(),
                    Name = _existingItem.Name,
                    Comment = _existingItem.Comment,
                    CreatedFrom = null,
                    Deleted = true,
                    IsTemporary = _existingItem.IsTemporary,
                    ItemCategoryId = _existingItem.ItemCategoryId,
                    ManufacturerId = _existingItem.ManufacturerId,
                    QuantityInPacket = _existingItem.QuantityInPacket,
                    QuantityType = _existingItem.QuantityType,
                    QuantityTypeInPacket = _existingItem.QuantityTypeInPacket,
                    PredecessorId = null,
                    ItemTypes = _existingItem.ItemTypes
                };

                ExpectedNewItem = new Item
                {
                    AvailableAt = new List<AvailableAt>(),
                    Name = _existingItem.Name,
                    Comment = _existingItem.Comment,
                    CreatedFrom = null,
                    Deleted = false,
                    IsTemporary = false,
                    ItemCategoryId = _existingItem.ItemCategoryId,
                    ManufacturerId = _existingItem.ManufacturerId,
                    QuantityInPacket = _existingItem.QuantityInPacket,
                    QuantityType = _existingItem.QuantityType,
                    QuantityTypeInPacket = _existingItem.QuantityTypeInPacket,
                    PredecessorId = _existingItem.Id,
                    ItemTypes = _existingItem.ItemTypes.Select(t => new ItemType
                    {
                        Name = t.Name,
                        PredecessorId = t.Id,
                        AvailableAt = t.AvailableAt.Select(av =>
                            new ItemTypeAvailableAtEntityBuilder(av)
                                .WithPrice(Contract.Price)
                                .Create())
                            .ToList()
                    }).ToList()
                };
            }
        }
    }

    [Collection(DockerCollection.Name)]
    public sealed class SearchItemsByItemCategoryAsync
    {
        private readonly SearchItemsByItemCategoryAsyncFixture _fixture;

        public SearchItemsByItemCategoryAsync(DockerFixture dockerFixture)
        {
            _fixture = new SearchItemsByItemCategoryAsyncFixture(dockerFixture);
        }

        [Fact]
        public async Task SearchItemsByItemCategoryAsync_WithValidItemCategoryId_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemCategory();
            _fixture.SetupItemsWithAndWithoutItemCategory();
            await _fixture.PrepareDatabaseAsync();
            _fixture.SetupExpectedResult();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCategoryId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await sut.SearchItemsByItemCategoryAsync(_fixture.ItemCategoryId.Value);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
            var resultValue = (result as OkObjectResult)!.Value;
            resultValue.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class SearchItemsByItemCategoryAsyncFixture : ItemControllerFixture
        {
            private readonly CommonFixture _commonFixture = new();
            private Item? _itemWithItemCategory;
            private Item? _itemWithoutItemCategory;
            private ItemCategoryEntities.ItemCategory? _itemCategory;
            private Store? _store;

            public SearchItemsByItemCategoryAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public Guid? ItemCategoryId => _itemCategory?.Id;
            public IReadOnlyCollection<SearchItemByItemCategoryResultContract>? ExpectedResult { get; private set; }

            public void SetupItemCategory()
            {
                _itemCategory = new TestBuilder<ItemCategoryEntities.ItemCategory>()
                    .FillPropertyWith(c => c.Deleted, false)
                    .Create();
            }

            public void SetupItemsWithAndWithoutItemCategory()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                _store = StoreEntityMother.Initial().Create();

                var defaultSectionId = _commonFixture.ChooseRandom(_store.Sections).Id;

                _itemWithItemCategory =
                    ItemEntityMother.Initial().WithItemCategoryId(_itemCategory.Id)
                        .WithAvailabilities(AvailableAtEntityMother
                            .InitialForStore(_store.Id)
                            .WithDefaultSectionId(defaultSectionId)
                            .CreateMany(1))
                        .Create();
                _itemWithoutItemCategory = ItemEntityMother.Initial().Create();
            }

            public async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemWithItemCategory);
                TestPropertyNotSetException.ThrowIfNull(_itemWithoutItemCategory);
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                TestPropertyNotSetException.ThrowIfNull(_store);

                await ApplyMigrationsAsync(ArrangeScope);

                var itemContext = ArrangeScope.ServiceProvider.GetRequiredService<ItemContext>();
                var itemCategoryContext = ArrangeScope.ServiceProvider.GetRequiredService<ItemCategoryContext>();
                var storeContext = ArrangeScope.ServiceProvider.GetRequiredService<StoreContext>();

                itemCategoryContext.Add(_itemCategory);
                storeContext.Add(_store);
                itemContext.Add(_itemWithItemCategory);
                itemContext.Add(_itemWithoutItemCategory);

                await itemCategoryContext.SaveChangesAsync();
                await itemContext.SaveChangesAsync();
                await storeContext.SaveChangesAsync();
            }

            public void SetupExpectedResult()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);
                TestPropertyNotSetException.ThrowIfNull(_itemWithItemCategory);

                ExpectedResult = new SearchItemByItemCategoryResultContract(
                    _itemWithItemCategory.Id,
                    null,
                    _itemWithItemCategory.Name,
                    _itemWithItemCategory.AvailableAt.Select(av =>
                        new SearchItemByItemCategoryAvailabilityContract(
                            _store.Id,
                            _store.Name,
                            av.Price)))
                    .ToMonoList();
            }
        }
    }

    private class ItemControllerFixture : DatabaseFixture
    {
        protected ItemControllerFixture(DockerFixture dockerFixture) : base(dockerFixture)
        {
            ArrangeScope = CreateServiceScope();
        }

        protected readonly IServiceScope ArrangeScope;

        public override IEnumerable<DbContext> GetDbContexts(IServiceScope scope)
        {
            yield return scope.ServiceProvider.GetRequiredService<ItemContext>();
            yield return scope.ServiceProvider.GetRequiredService<ShoppingListContext>();
            yield return scope.ServiceProvider.GetRequiredService<ItemCategoryContext>();
            yield return scope.ServiceProvider.GetRequiredService<ManufacturerContext>();
            yield return scope.ServiceProvider.GetRequiredService<StoreContext>();
        }

        public ItemController CreateSut()
        {
            var scope = CreateServiceScope();
            return scope.ServiceProvider.GetRequiredService<ItemController>();
        }

        private IItemRepository CreateItemRepository(IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<IItemRepository>();
        }

        private IManufacturerRepository CreateManufacturerRepository(IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<IManufacturerRepository>();
        }

        private IItemCategoryRepository CreateItemCategoryRepository(IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<IItemCategoryRepository>();
        }

        private IStoreRepository CreateStoreRepository(IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<IStoreRepository>();
        }

        protected async Task StoreAsync(IItem? item = null, IManufacturer? manufacturer = null,
            IItemCategory? itemCategory = null, IEnumerable<IStore>? stores = null)
        {
            using var scope = CreateServiceScope();
            using var transaction = await CreateTransactionAsync(scope);
            var itemRepository = CreateItemRepository(scope);
            var manufacturerRepository = CreateManufacturerRepository(scope);
            var itemCategoryRepository = CreateItemCategoryRepository(scope);
            var storeRepository = CreateStoreRepository(scope);

            if (itemCategory is not null)
                await itemCategoryRepository.StoreAsync(itemCategory, default);
            if (manufacturer is not null)
                await manufacturerRepository.StoreAsync(manufacturer, default);
            if (item is not null)
                await itemRepository.StoreAsync(item, default);
            if (stores is not null)
            {
                foreach (var store in stores)
                {
                    await storeRepository.StoreAsync(store, default);
                }
            }

            await transaction.CommitAsync(default);
        }

        public async Task<IEnumerable<Item>> LoadAllItemEntities()
        {
            using var assertScope = CreateServiceScope();
            var dbContext = assertScope.ServiceProvider.GetRequiredService<ItemContext>();

            return await dbContext.Items.AsNoTracking()
                .Include(item => item.AvailableAt)
                .Include(item => item.Predecessor)
                .Include(item => item.ItemTypes)
                .ThenInclude(itemType => itemType.AvailableAt)
                .Include(item => item.ItemTypes)
                .ThenInclude(itemType => itemType.Predecessor)
                .ToListAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ArrangeScope.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}