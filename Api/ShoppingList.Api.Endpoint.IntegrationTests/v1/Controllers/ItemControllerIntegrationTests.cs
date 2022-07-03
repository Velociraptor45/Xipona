using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.UpdateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
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
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Contexts;
using ShoppingList.Api.Core.TestKit;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ShoppingList.Api.Domain.TestKit.Items.Models;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Models;
using ShoppingList.Api.Domain.TestKit.Stores.Models;
using ShoppingList.Api.TestTools.Exceptions;
using System;
using Xunit;
using Item = ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities.Item;

namespace ShoppingList.Api.Endpoint.IntegrationTests.v1.Controllers;

public class ItemControllerIntegrationTests
{
    [Collection("IntegrationTests")]
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
                    t.AvailableAt.Select(av => new ItemAvailabilityContract()
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
                var builder = StoreItemMother.InitialWithTypes();

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

                FirstLevelPredecessor = StoreItemMother.InitialWithTypes()
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
                SecondLevelPredecessor = StoreItemMother.InitialWithTypes()
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
                var factory = ArrangeScope.ServiceProvider.GetRequiredService<IStoreSectionFactory>();

                foreach (var av in Contract.ItemTypes.SelectMany(t => t.Availabilities))
                {
                    var section = new StoreSectionBuilder()
                        .WithId(new SectionId(av.DefaultSectionId))
                        .CreateMany(1);

                    var store = new StoreBuilder()
                        .WithId(new StoreId(av.StoreId))
                        .WithSections(new StoreSections(section, factory))
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

    private class ItemControllerFixture : DatabaseFixture, IDisposable
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

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ArrangeScope.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}