using FluentAssertions;
using Force.DeepCloner;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemDiscount;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemToShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.PutItemInBasket;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromBasket;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.Shared;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.Xipona.Api.Contracts.TestKit.Common;
using ProjectHermes.Xipona.Api.Contracts.TestKit.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Shared;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Repositories.ItemCategories.Contexts;
using ProjectHermes.Xipona.Api.Repositories.ItemCategories.Entities;
using ProjectHermes.Xipona.Api.Repositories.Items.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Manufacturers.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Manufacturers.Entities;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Contexts;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Contexts;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities;
using ProjectHermes.Xipona.Api.Repositories.Stores.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Stores.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.ItemCategories.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Items.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Manufacturers.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Stores.Entities;
using ProjectHermes.Xipona.Api.TestTools.AutoFixture;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System;
using Xunit;
using Item = ProjectHermes.Xipona.Api.Repositories.Items.Entities.Item;
using ItemType = ProjectHermes.Xipona.Api.Repositories.Items.Entities.ItemType;

namespace ProjectHermes.Xipona.Api.Endpoint.IntegrationTests.v1.Controllers;

public class ShoppingListControllerIntegrationTests
{
    public sealed class GetActiveShoppingListByStoreIdAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly GetActiveShoppingListByStoreIdAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task GetActiveShoppingListByStoreIdAsync_WithExistingShoppingList_ShouldReturnShoppingList()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupExistingShoppingListWithDiscount();
            _fixture.SetupExistingItems();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await sut.GetActiveShoppingListByStoreIdAsync(_fixture.ExpectedResult.Store.Id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var contract = okResult!.Value as ShoppingListContract;
            contract!.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class GetActiveShoppingListByStoreIdAsyncFixture(DockerFixture dockerFixture) :
            ShoppingListControllerFixture(dockerFixture)
        {
            private ShoppingList? _shoppingList;
            private List<Item>? _items;
            private ShoppingListItemContract? _item;
            private ShoppingListItemContract? _item2;
            private List<ItemCategory>? _itemCategories;
            private List<Manufacturer>? _manufacturers;
            private Store? _store;
            private ShoppingListSectionContract? _firstSection;
            private ShoppingListSectionContract? _secondSection;
            private string[]? _itemNameParts;
            private string[]? _item2NameParts;
            public ShoppingListContract? ExpectedResult { get; private set; }

            public void SetupExpectedResult()
            {
                var itemCategoryBuilder = new ContractTestBuilder<ItemCategoryContract>()
                    .FillConstructorWith("isDeleted", false);
                var manufacturerBuilder = new ContractTestBuilder<ManufacturerContract>()
                    .FillConstructorWith("isDeleted", false);

                _itemNameParts = [Guid.NewGuid().ToString(), Guid.NewGuid().ToString()];
                _item = ShoppingListItemContractMother.Valid()
                    .WithIsDeleted(false)
                    .WithIsTemporary(false)
                    .WithItemCategory(itemCategoryBuilder.Create())
                    .WithManufacturer(manufacturerBuilder.Create())
                    .WithName($"{_itemNameParts[0]} {_itemNameParts[1]}")
                    .Create();
                _item2NameParts = [Guid.NewGuid().ToString(), Guid.NewGuid().ToString()];
                _item2 = ShoppingListItemContractMother.Valid()
                    .WithIsDeleted(false)
                    .WithIsTemporary(false)
                    .WithItemCategory(itemCategoryBuilder.Create())
                    .WithManufacturer(manufacturerBuilder.Create())
                    .WithName($"{_item2NameParts[0]} {_item2NameParts[1]}")
                    .Create();

                IEnumerable<ShoppingListSectionContract> sections =
                [
                    new ContractTestBuilder<ShoppingListSectionContract>()
                        .FillPropertyWith(x => x.IsDefaultSection, true)
                        .FillPropertyWith(x => x.Items, [_item])
                        .Create(),
                    new ContractTestBuilder<ShoppingListSectionContract>()
                        .FillPropertyWith(x => x.IsDefaultSection, false)
                        .FillPropertyWith(x => x.Items, [_item2])
                        .Create()
                ];

                ExpectedResult = new ContractTestBuilder<ShoppingListContract>()
                    .FillConstructorWith("sections", sections)
                    .FillConstructorWith("completionDate", (DateTimeOffset?)null)
                    .Create();
            }

            public void SetupExistingShoppingListWithDiscount()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                TestPropertyNotSetException.ThrowIfNull(_item);
                TestPropertyNotSetException.ThrowIfNull(_item2);

                _firstSection = ExpectedResult.Sections.First();
                var item1 = ConvertItem(_firstSection.Id, _item);
                _secondSection = ExpectedResult.Sections.Last();
                var item2 = ConvertItem(_secondSection.Id, _item2);

                var firstItemDiscount = new DiscountEntityBuilder()
                    .WithShoppingListId(ExpectedResult.Id)
                    .WithItemId(item1.ItemId)
                    .WithItemTypeId(item1.ItemTypeId)
                    .WithDiscountPrice(_firstSection.Items.First().PricePerQuantity)
                    .Create();

                _shoppingList = new ShoppingList
                {
                    Id = ExpectedResult.Id,
                    CompletionDate = ExpectedResult.CompletionDate,
                    StoreId = ExpectedResult.Store.Id,
                    ItemsOnList = [item1, item2],
                    Discounts = [firstItemDiscount]
                };

                ItemsOnList ConvertItem(Guid sectionId, ShoppingListItemContract item)
                {
                    return new ItemsOnList
                    {
                        ShoppingListId = ExpectedResult.Id,
                        ItemId = item.Id,
                        ItemTypeId = item.TypeId,
                        InBasket = item.IsInBasket,
                        Quantity = item.Quantity,
                        SectionId = sectionId,
                    };
                }
            }

            public void SetupExistingItems()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                TestPropertyNotSetException.ThrowIfNull(_item);
                TestPropertyNotSetException.ThrowIfNull(_item2);
                TestPropertyNotSetException.ThrowIfNull(_firstSection);
                TestPropertyNotSetException.ThrowIfNull(_secondSection);
                TestPropertyNotSetException.ThrowIfNull(_itemNameParts);
                TestPropertyNotSetException.ThrowIfNull(_item2NameParts);

                var item1Type = ItemTypeEntityMother.InitialForStore(_shoppingList.StoreId, _firstSection.Id)
                    .WithName(_itemNameParts[1])
                    .WithId(_item.TypeId!.Value).Create();

                var item2Availability = ItemTypeAvailableAtEntityMother
                    .InitialForStore(_shoppingList.StoreId)
                    .WithDefaultSectionId(_secondSection.Id)
                    .WithPrice(ExpectedResult.Sections.Last().Items.First().PricePerQuantity)
                    .Create();
                var item2Type = ItemTypeEntityMother.Initial()
                    .WithId(_item2.TypeId!.Value)
                    .WithName(_item2NameParts[1])
                    .WithAvailableAt(item2Availability)
                    .Create();

                _itemCategories =
                [
                    ItemCategoryEntityMother.Active()
                        .WithName(_item.ItemCategory.Name)
                        .WithId(_item.ItemCategory.Id)
                        .Create(),
                    ItemCategoryEntityMother.Active()
                        .WithName(_item2.ItemCategory.Name)
                        .WithId(_item2.ItemCategory.Id)
                        .Create()
                ];

                _manufacturers =
                [
                    ManufacturerEntityMother.Active()
                        .WithName(_item.Manufacturer.Name)
                        .WithId(_item.Manufacturer.Id)
                        .Create(),
                    ManufacturerEntityMother.Active()
                        .WithName(_item2.Manufacturer.Name)
                        .WithId(_item2.Manufacturer.Id)
                        .Create()
                ];

                _items =
                [
                    ItemEntityMother.InitialWithTypes()
                        .WithId(_item.Id)
                        .WithName(_itemNameParts[0])
                        .WithItemType(item1Type)
                        .WithItemCategoryId(_item.ItemCategory.Id)
                        .WithManufacturerId(_item.Manufacturer.Id)
                        .WithQuantityType(_item.QuantityType.Id)
                        .WithQuantityInPacket(_item.QuantityInPacket)
                        .WithQuantityTypeInPacket(_item.QuantityTypeInPacket?.Id)
                        .WithComment(_item.Comment)
                        .Create(),
                    ItemEntityMother.InitialWithTypes()
                        .WithId(_item2.Id)
                        .WithName(_item2NameParts[0])
                        .WithItemType(item2Type)
                        .WithItemCategoryId(_item2.ItemCategory.Id)
                        .WithManufacturerId(_item2.Manufacturer.Id)
                        .WithQuantityType(_item2.QuantityType.Id)
                        .WithQuantityInPacket(_item2.QuantityInPacket)
                        .WithQuantityTypeInPacket(_item2.QuantityTypeInPacket?.Id)
                        .WithComment(_item2.Comment)
                        .Create()
                ];

                var section1 = new SectionEntityBuilder()
                    .WithId(_firstSection.Id)
                    .WithName(_firstSection.Name)
                    .WithIsDefaultSection(_firstSection.IsDefaultSection)
                    .WithIsDeleted(false)
                    .WithSortIndex(_firstSection.SortingIndex)
                    .Create();

                var section2 = new SectionEntityBuilder()
                    .WithId(_secondSection.Id)
                    .WithName(_secondSection.Name)
                    .WithIsDefaultSection(_secondSection.IsDefaultSection)
                    .WithIsDeleted(false)
                    .WithSortIndex(_secondSection.SortingIndex)
                    .Create();

                _store = StoreEntityMother
                    .Active()
                    .WithName(ExpectedResult.Store.Name)
                    .WithSections([section1, section2])
                    .WithId(_shoppingList.StoreId)
                    .Create();
            }

            public async Task SetupDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                TestPropertyNotSetException.ThrowIfNull(_items);
                TestPropertyNotSetException.ThrowIfNull(_itemCategories);
                TestPropertyNotSetException.ThrowIfNull(_manufacturers);
                TestPropertyNotSetException.ThrowIfNull(_store);

                await ApplyMigrationsAsync(ArrangeScope);

                var shoppingListContext = GetContextInstance<ShoppingListContext>(ArrangeScope);
                var itemContext = GetContextInstance<ItemContext>(ArrangeScope);
                var itemCategoryContext = GetContextInstance<ItemCategoryContext>(ArrangeScope);
                var manufacturerContext = GetContextInstance<ManufacturerContext>(ArrangeScope);
                var storeContext = GetContextInstance<StoreContext>(ArrangeScope);

                await shoppingListContext.AddAsync(_shoppingList);
                await itemContext.AddRangeAsync(_items);
                await itemCategoryContext.AddRangeAsync(_itemCategories);
                await manufacturerContext.AddRangeAsync(_manufacturers);
                await storeContext.AddAsync(_store);

                await shoppingListContext.SaveChangesAsync();
                await itemContext.SaveChangesAsync();
                await itemCategoryContext.SaveChangesAsync();
                await manufacturerContext.SaveChangesAsync();
                await storeContext.SaveChangesAsync();
            }
        }
    }

    public sealed class RemoveItemFromShoppingListAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly RemoveItemFromShoppingListAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task RemoveItemFromShoppingListAsync_WithItemWithoutTypes_ShouldRemoveItemFromShoppingList()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupItemWithoutTypes();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await sut.RemoveItemFromShoppingListAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion()
                    .ExcludeItemsOnListId());
        }

        [Fact]
        public async Task RemoveItemFromShoppingListAsync_WithItemWithTypes_ShouldRemoveItemFromShoppingList()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupItemWithTypes();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await sut.RemoveItemFromShoppingListAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion()
                    .ExcludeItemsOnListId());
        }

        [Fact]
        public async Task RemoveItemFromShoppingListAsync_WithItemOfflineId_ShouldRemoveItemFromShoppingList()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupItemWithoutTypesForOfflineUse();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await sut.RemoveItemFromShoppingListAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion()
                    .ExcludeItemsOnListId());
        }

        [Fact]
        public async Task RemoveItemFromShoppingListAsync_WithTemporaryItem_ShouldRemoveItemFromShoppingListAndDeleteItem()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupTemporaryItem();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItem);

            // Act
            var result = await sut.RemoveItemFromShoppingListAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion()
                    .ExcludeItemsOnListId());

            var items = (await _fixture.LoadAllItemsAsync(assertScope)).ToList();
            items.Should().HaveCount(1);
            items.Single().Should().BeEquivalentTo(_fixture.ExpectedItem,
                opt => opt.ExcludeItemCycleRef().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion()
                    .ExcludeItemsOnListId());
        }

        private sealed class RemoveItemFromShoppingListAsyncFixture(DockerFixture dockerFixture)
            : ShoppingListControllerFixture(dockerFixture)
        {
            private ShoppingList? _shoppingList;
            private Item? _item;
            private ItemIdContract? _itemId;
            private Guid? _itemTypeId;

            public Guid? ShoppingListId { get; private set; }
            public RemoveItemFromShoppingListContract? Contract { get; private set; }
            public ShoppingList? ExpectedResult { get; private set; }
            public Item? ExpectedItem { get; private set; }

            public void SetupExpectedResult()
            {
                _shoppingList = ShoppingListEntityMother.Active().Create();
                ExpectedResult = _shoppingList.DeepClone();
                ShoppingListId = _shoppingList.Id;
            }

            public void SetupItemWithoutTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                var itemOnList = new ItemsOnListEntityBuilder().WithoutItemTypeId().Create();
                _shoppingList.ItemsOnList.Add(itemOnList);
                _itemId = new(itemOnList.ItemId, null);
                _itemTypeId = null;

                _item = ItemEntityMother.InitialForStore(_shoppingList.StoreId).WithId(itemOnList.ItemId).Create();
            }

            public void SetupItemWithoutTypesForOfflineUse()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);

                var itemOnList = new ItemsOnListEntityBuilder().WithoutItemTypeId().Create();
                _item = ItemEntityMother.InitialForStore(_shoppingList.StoreId)
                    .WithId(itemOnList.ItemId)
                    .WithCreatedFrom(Guid.NewGuid())
                    .Create();

                _shoppingList.ItemsOnList.Add(itemOnList);
                _itemId = new(null, _item.CreatedFrom);
                _itemTypeId = null;
            }

            public void SetupItemWithTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);

                _item = ItemEntityMother.InitialWithTypesForStore(_shoppingList.StoreId).Create();

                var itemOnList = new ItemsOnListEntityBuilder()
                    .WithItemId(_item.Id)
                    .WithItemTypeId(CommonFixture.ChooseRandom(_item.ItemTypes).Id)
                    .Create();
                _itemId = new(itemOnList.ItemId, null);
                _itemTypeId = itemOnList.ItemTypeId;
                _shoppingList.ItemsOnList.Add(itemOnList);
            }

            public void SetupTemporaryItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                var itemOnList = new ItemsOnListEntityBuilder().WithoutItemTypeId().Create();
                _shoppingList.ItemsOnList.Add(itemOnList);
                _itemId = new(itemOnList.ItemId, null);
                _itemTypeId = null;

                _item = ItemEntityMother.Temporary(_shoppingList.StoreId).WithId(itemOnList.ItemId).Create();
                ExpectedItem = _item.DeepClone();
                ExpectedItem.Deleted = true;
            }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemId);
                Contract = new RemoveItemFromShoppingListContract(_itemId, _itemTypeId);
            }

            public async Task SetupDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                TestPropertyNotSetException.ThrowIfNull(_item);

                await ApplyMigrationsAsync(ArrangeScope);

                var shoppingListContext = ArrangeScope.ServiceProvider.GetRequiredService<ShoppingListContext>();
                var itemContext = ArrangeScope.ServiceProvider.GetRequiredService<ItemContext>();

                await shoppingListContext.AddAsync(_shoppingList);
                await itemContext.AddAsync(_item);

                await shoppingListContext.SaveChangesAsync();
                await itemContext.SaveChangesAsync();
            }
        }
    }

    public sealed class AddTemporaryItemToShoppingListAsync(DockerFixture dockerFixture)
        : IAssemblyFixture<DockerFixture>
    {
        private readonly AddTemporaryItemToShoppingListAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task AddTemporaryItemToShoppingListAsync_WithValidData_ShouldCreateAndAddTemporaryItemToShoppingList()
        {
            // Arrange
            _fixture.SetupExpectedItem();
            _fixture.SetupExpectedShoppingList();
            _fixture.SetupStore();
            _fixture.SetupExistingShoppingList();
            _fixture.SetupContract();
            _fixture.SetupExpectedResult();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedShoppingList);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItem);

            // Act
            var result = await sut.AddTemporaryItemToShoppingListAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var contract = okResult!.Value as TemporaryShoppingListItemContract;
            contract!.Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.Excluding(info => info.Path == "ItemId"));

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            var items = (await _fixture.LoadAllItemsAsync(assertScope)).ToList();

            items.Should().HaveCount(1);
            var item = items.Single();
            item.Should().BeEquivalentTo(_fixture.ExpectedItem, opt =>
                opt.ExcludeItemCycleRef().ExcludeRowVersion()
                    .Excluding(info => info.Path == "Id" || info.Path == "CreatedAt"));
            item.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(20));

            var itemId = item.Id;

            shoppingLists.Should().HaveCount(1);
            var shoppingList = shoppingLists.Single();
            shoppingList.Should().BeEquivalentTo(_fixture.ExpectedShoppingList, opt =>
                opt.ExcludeShoppingListCycleRef().ExcludeRowVersion()
                    .Excluding(info => info.Path == "ItemsOnList[0].ItemId" || info.Path == "ItemsOnList[0].Id")
                    .WithCreatedAtPrecision());
            shoppingList.ItemsOnList.First().ItemId.Should().Be(itemId);
            contract!.ItemId.Should().Be(itemId);
        }

        private sealed class AddTemporaryItemToShoppingListAsyncFixture(DockerFixture dockerFixture)
            : ShoppingListControllerFixture(dockerFixture)
        {
            private Store? _store;
            private ShoppingList? _existingShoppingList;

            public Guid? ShoppingListId => ExpectedShoppingList?.Id;
            public Item? ExpectedItem { get; private set; }
            public ShoppingList? ExpectedShoppingList { get; private set; }
            public AddTemporaryItemToShoppingListContract? Contract { get; private set; }
            public TemporaryShoppingListItemContract? ExpectedResult { get; private set; }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedItem);
                TestPropertyNotSetException.ThrowIfNull(ExpectedShoppingList);

                Contract = new AddTemporaryItemToShoppingListContract(
                    ExpectedItem.Name,
                    ExpectedItem.QuantityType,
                    ExpectedShoppingList.ItemsOnList.First().Quantity,
                    ExpectedItem.AvailableAt.First().Price,
                    ExpectedItem.AvailableAt.First().DefaultSectionId,
                    ExpectedItem.CreatedFrom!.Value);
            }

            public void SetupExpectedItem()
            {
                ExpectedItem = ItemEntityMother
                    .Initial()
                    .WithAvailableAt(new AvailableAtEntityBuilder().CreateMany(1).ToList())
                    .WithQuantityTypeInPacket(QuantityTypeInPacket.Unit.ToInt())
                    .WithQuantityType(QuantityType.Unit.ToInt())
                    .WithQuantityInPacket(1)
                    .WithCreatedFrom(Guid.NewGuid())
                    .WithComment(string.Empty)
                    .WithoutManufacturerId()
                    .WithoutItemCategoryId()
                    .WithIsTemporary(true)
                    .WithoutUpdatedOn()
                    .WithCreatedAt(DateTimeOffset.UtcNow)
                    .Create();
            }

            public void SetupExpectedShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedItem);

                var shoppingListId = Guid.NewGuid();
                var items = new ItemsOnListEntityBuilder()
                    .WithShoppingListId(shoppingListId)
                    .WithoutItemTypeId()
                    .WithInBasket(false)
                    .WithSectionId(ExpectedItem.AvailableAt.First().DefaultSectionId)
                    .CreateMany(1)
                    .ToList();

                ExpectedShoppingList = ShoppingListEntityMother
                    .Empty()
                    .WithStoreId(ExpectedItem.AvailableAt.First().StoreId)
                    .WithItemsOnList(items)
                    .Create();
            }

            public void SetupExpectedResult()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedItem);
                TestPropertyNotSetException.ThrowIfNull(Contract);
                ExpectedResult = new TemporaryShoppingListItemContract(ExpectedItem.Id, false, Contract.Quantity);
            }

            public void SetupExistingShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedShoppingList);

                _existingShoppingList = ShoppingListEntityMother
                    .Empty()
                    .WithId(ExpectedShoppingList.Id)
                    .WithStoreId(ExpectedShoppingList.StoreId)
                    .WithCreatedAt(ExpectedShoppingList.CreatedAt)
                    .Create();
            }

            public void SetupStore()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedShoppingList);

                _store = StoreEntityMother
                    .ValidSections(ExpectedShoppingList.ItemsOnList.First().SectionId.ToMonoList())
                    .WithId(ExpectedShoppingList.StoreId)
                    .Create();
            }

            public async Task SetupDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingShoppingList);
                TestPropertyNotSetException.ThrowIfNull(_store);

                await ApplyMigrationsAsync(ArrangeScope);

                await using var storeContext = GetContextInstance<StoreContext>(ArrangeScope);
                await using var shoppingListContext = GetContextInstance<ShoppingListContext>(ArrangeScope);

                await storeContext.AddAsync(_store);
                await shoppingListContext.AddAsync(_existingShoppingList);

                await storeContext.SaveChangesAsync();
                await shoppingListContext.SaveChangesAsync();
            }
        }
    }

    public sealed class AddItemToShoppingListAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly AddItemToShoppingListAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task AddItemToShoppingListAsync_WithNotAlreadyOnList_ShouldAddItemToShoppingList()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupShoppingList();
            _fixture.SetupItem();
            _fixture.SetupStore();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);

            // Act
            var result = await sut.AddItemToShoppingListAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().ExcludeItemsOnListId().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion());
        }

        [Fact]
        public async Task AddItemToShoppingListAsync_WithItemAlreadyOnList_ShouldReturnError()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupShoppingListWithItemAlreadyOnList();
            _fixture.SetupItem();
            _fixture.SetupStore();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);

            // Act
            var result = await sut.AddItemToShoppingListAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<UnprocessableEntityObjectResult>();
            var unprocessableEntityResult = (UnprocessableEntityObjectResult)result;
            unprocessableEntityResult.Value.Should().BeOfType<ErrorContract>();
            var errorContract = (ErrorContract)unprocessableEntityResult.Value!;
            errorContract.ErrorCode.Should().Be(ErrorReasonCode.ItemAlreadyOnShoppingList.ToInt());

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().ExcludeItemsOnListId().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion());
        }

        private sealed class AddItemToShoppingListAsyncFixture(DockerFixture dockerFixture)
            : ShoppingListControllerFixture(dockerFixture)
        {
            private ShoppingList? _shoppingList;
            private Guid? _itemId;
            private Item? _item;
            private ItemsOnList? _addedShoppingListItem;
            private Store? _store;

            public Guid? ShoppingListId { get; private set; }
            public AddItemToShoppingListContract? Contract { get; private set; }
            public ShoppingList? ExpectedResult { get; private set; }

            public void SetupExpectedResult()
            {
                ExpectedResult = ShoppingListEntityMother.ActiveWithItemsWithoutType().Create();
            }

            public void SetupShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                _shoppingList = ExpectedResult.DeepClone();
                _addedShoppingListItem = CommonFixture.ChooseRandom(_shoppingList.ItemsOnList, out var index);
                _shoppingList.ItemsOnList.Remove(_addedShoppingListItem);

                ExpectedResult.ItemsOnList.ElementAt(index).InBasket = false;

                ShoppingListId = _shoppingList.Id;
                _itemId = _addedShoppingListItem.ItemId;
            }

            public void SetupShoppingListWithItemAlreadyOnList()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                _shoppingList = ExpectedResult.DeepClone();
                _addedShoppingListItem = CommonFixture.ChooseRandom(_shoppingList.ItemsOnList);

                ShoppingListId = _shoppingList.Id;
                _itemId = _addedShoppingListItem.ItemId;
            }

            public void SetupStore()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                _store = StoreEntityMother.ValidSections(ExpectedResult.ItemsOnList.Select(i => i.SectionId))
                    .WithId(ExpectedResult.StoreId)
                    .Create();
            }

            public void SetupItem()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                TestPropertyNotSetException.ThrowIfNull(_itemId);

                _item = ItemEntityMother.InitialForStore(ExpectedResult.StoreId)
                    .WithId(_itemId.Value)
                    .Create();
            }

            public async Task SetupDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                TestPropertyNotSetException.ThrowIfNull(_item);
                TestPropertyNotSetException.ThrowIfNull(_store);

                await ApplyMigrationsAsync(ArrangeScope);

                var shoppingListContext = ArrangeScope.ServiceProvider.GetRequiredService<ShoppingListContext>();
                var itemContext = ArrangeScope.ServiceProvider.GetRequiredService<ItemContext>();
                var storeContext = ArrangeScope.ServiceProvider.GetRequiredService<StoreContext>();

                await shoppingListContext.AddAsync(_shoppingList);
                await itemContext.AddAsync(_item);
                await storeContext.AddAsync(_store);

                await shoppingListContext.SaveChangesAsync();
                await itemContext.SaveChangesAsync();
                await storeContext.SaveChangesAsync();
            }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemId);
                TestPropertyNotSetException.ThrowIfNull(_addedShoppingListItem);

                Contract = new AddItemToShoppingListContract(_itemId.Value, _addedShoppingListItem.SectionId,
                    _addedShoppingListItem.Quantity);
            }
        }
    }

    public sealed class AddItemWithTypeToShoppingListAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly AddItemWithTypeToShoppingListAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task AddItemToShoppingListAsync_WithNotAlreadyOnList_ShouldAddItemToShoppingList()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupShoppingList();
            _fixture.SetupItem();
            _fixture.SetupStore();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);

            // Act
            var result = await sut.AddItemWithTypeToShoppingListAsync(_fixture.ShoppingListId.Value,
                _fixture.ItemId.Value, _fixture.ItemTypeId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().ExcludeItemsOnListId().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion());
        }

        [Fact]
        public async Task AddItemToShoppingListAsync_WithOtherTypeAlreadyOnList_ShouldAddItemToShoppingList()
        {
            // Arrange
            _fixture.SetupExpectedResultWithTwoTypesOfSameItem();
            _fixture.SetupShoppingList();
            _fixture.SetupItemWithTwoTypes();
            _fixture.SetupStore();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);

            // Act
            var result = await sut.AddItemWithTypeToShoppingListAsync(_fixture.ShoppingListId.Value,
                _fixture.ItemId.Value, _fixture.ItemTypeId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().ExcludeItemsOnListId().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion());
        }

        [Fact]
        public async Task AddItemToShoppingListAsync_WithItemAlreadyOnList_ShouldReturnError()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupShoppingListWithItemAlreadyOnList();
            _fixture.SetupItem();
            _fixture.SetupStore();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);

            // Act
            var result = await sut.AddItemWithTypeToShoppingListAsync(_fixture.ShoppingListId.Value,
                _fixture.ItemId.Value, _fixture.ItemTypeId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<UnprocessableEntityObjectResult>();
            var unprocessableEntityResult = (UnprocessableEntityObjectResult)result;
            unprocessableEntityResult.Value.Should().BeOfType<ErrorContract>();
            var errorContract = (ErrorContract)unprocessableEntityResult.Value!;
            errorContract.ErrorCode.Should().Be(ErrorReasonCode.ItemAlreadyOnShoppingList.ToInt());

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().ExcludeItemsOnListId().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion());
        }

        private sealed class AddItemWithTypeToShoppingListAsyncFixture(DockerFixture dockerFixture)
            : ShoppingListControllerFixture(dockerFixture)
        {
            private ShoppingList? _shoppingList;
            private Item? _item;
            private ItemsOnList? _addedShoppingListItem;
            private Store? _store;

            public Guid? ShoppingListId { get; private set; }
            public Guid? ItemId { get; private set; }
            public Guid? ItemTypeId { get; private set; }
            public AddItemWithTypeToShoppingListContract? Contract { get; private set; }
            public ShoppingList? ExpectedResult { get; private set; }

            public void SetupExpectedResult()
            {
                ExpectedResult = ShoppingListEntityMother.Active().Create();
            }

            public void SetupExpectedResultWithTwoTypesOfSameItem()
            {
                var items = new ItemsOnListEntityBuilder().WithItemId(Guid.NewGuid()).CreateMany(2).ToList();

                ExpectedResult = ShoppingListEntityMother.Active().WithItemsOnList(items).Create();
            }

            public void SetupShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                _shoppingList = ExpectedResult.DeepClone();
                _addedShoppingListItem = CommonFixture.ChooseRandom(_shoppingList.ItemsOnList, out var index);
                _shoppingList.ItemsOnList.Remove(_addedShoppingListItem);

                ExpectedResult.ItemsOnList.ElementAt(index).InBasket = false;

                ShoppingListId = _shoppingList.Id;
                ItemId = _addedShoppingListItem.ItemId;
                ItemTypeId = _addedShoppingListItem.ItemTypeId;
            }

            public void SetupShoppingListWithItemAlreadyOnList()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                _shoppingList = ExpectedResult.DeepClone();
                _addedShoppingListItem = CommonFixture.ChooseRandom(_shoppingList.ItemsOnList);

                ShoppingListId = _shoppingList.Id;
                ItemId = _addedShoppingListItem.ItemId;
                ItemTypeId = _addedShoppingListItem.ItemTypeId;
            }

            public void SetupItem()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                TestPropertyNotSetException.ThrowIfNull(ItemId);
                TestPropertyNotSetException.ThrowIfNull(ItemTypeId);

                _item = ItemEntityMother.InitialWithTypesForStore(ExpectedResult.StoreId)
                    .WithId(ItemId.Value)
                    .Create();

                CommonFixture.ChooseRandom(_item.ItemTypes).Id = ItemTypeId.Value;
            }

            public void SetupItemWithTwoTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                TestPropertyNotSetException.ThrowIfNull(ItemId);

                ItemType[] types =
                [
                    ItemTypeEntityMother.InitialForStore(ExpectedResult.StoreId)
                        .WithId(ExpectedResult.ItemsOnList.First().ItemTypeId!.Value)
                        .Create(),
                    ItemTypeEntityMother.InitialForStore(ExpectedResult.StoreId)
                        .WithId(ExpectedResult.ItemsOnList.Last().ItemTypeId!.Value)
                        .Create(),
                ];

                _item = ItemEntityMother.InitialWithTypes()
                    .WithId(ItemId.Value)
                    .WithItemTypes(types)
                    .Create();
            }

            public void SetupStore()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                _store = StoreEntityMother.ValidSections(ExpectedResult.ItemsOnList.Select(i => i.SectionId))
                    .WithId(ExpectedResult.StoreId)
                    .Create();
            }

            public async Task SetupDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                TestPropertyNotSetException.ThrowIfNull(_item);
                TestPropertyNotSetException.ThrowIfNull(_store);

                await ApplyMigrationsAsync(ArrangeScope);

                var shoppingListContext = ArrangeScope.ServiceProvider.GetRequiredService<ShoppingListContext>();
                var itemContext = ArrangeScope.ServiceProvider.GetRequiredService<ItemContext>();
                var storeContext = ArrangeScope.ServiceProvider.GetRequiredService<StoreContext>();

                await shoppingListContext.AddAsync(_shoppingList);
                await itemContext.AddAsync(_item);
                await storeContext.AddAsync(_store);

                await shoppingListContext.SaveChangesAsync();
                await itemContext.SaveChangesAsync();
                await storeContext.SaveChangesAsync();
            }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemId);
                TestPropertyNotSetException.ThrowIfNull(_addedShoppingListItem);

                Contract = new AddItemWithTypeToShoppingListContract(_addedShoppingListItem.SectionId,
                    _addedShoppingListItem.Quantity);
            }
        }
    }

    public sealed class PutItemInBasketAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly PutItemInBasketAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task PutItemInBasketAsync_WithActualItemId_ShouldPutItemInBasket()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupShoppingList();
            _fixture.SetupItem();
            _fixture.SetupItemIdWithTypeId();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);

            // Act
            var result = await sut.PutItemInBasketAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion()
                    .ExcludeItemsOnListId());
        }

        [Fact]
        public async Task PutItemInBasketAsync_WithOfflineItemId_ShouldPutItemInBasket()
        {
            // Arrange
            _fixture.SetupExpectedResultForOfflineUse();
            _fixture.SetupShoppingList();
            _fixture.SetupItemForOfflineUse();
            _fixture.SetupItemIdForOfflineUse();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);

            // Act
            var result = await sut.PutItemInBasketAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion()
                    .ExcludeItemsOnListId());
        }

        [Fact]
        public async Task PutItemInBasketAsync_WithItemNotOnShoppingList_ShouldReturnError()
        {
            // Arrange
            _fixture.SetupExpectedResultForItemNotOnShoppingList();
            _fixture.SetupShoppingList();
            _fixture.SetupItemNotOnShoppingList();
            _fixture.SetupItemIdWithoutTypeId();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);

            // Act
            var result = await sut.PutItemInBasketAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<UnprocessableEntityObjectResult>();
            var unprocessableEntityResult = (UnprocessableEntityObjectResult)result;
            unprocessableEntityResult.Value.Should().BeOfType<ErrorContract>();
            var errorContract = (ErrorContract)unprocessableEntityResult.Value!;
            errorContract.ErrorCode.Should().Be(ErrorReasonCode.ItemNotOnShoppingList.ToInt());

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion()
                    .ExcludeItemsOnListId());
        }

        private sealed class PutItemInBasketAsyncFixture(DockerFixture dockerFixture)
            : ShoppingListControllerFixture(dockerFixture)
        {
            private ShoppingList? _shoppingList;
            private Item? _item;
            private ItemIdContract? _itemId;
            private Guid? _itemTypeId;
            public Guid? ShoppingListId { get; private set; }
            public PutItemInBasketContract? Contract { get; private set; }
            public ShoppingList? ExpectedResult { get; private set; }

            public void SetupExpectedResult()
            {
                ExpectedResult = ShoppingListEntityMother.InitialWithOneItem().Create();
                ExpectedResult.ItemsOnList.First().InBasket = true;
                ShoppingListId = ExpectedResult.Id;
            }

            public void SetupExpectedResultForItemNotOnShoppingList()
            {
                ExpectedResult = ShoppingListEntityMother.InitialWithOneItem().Create();
                ExpectedResult.ItemsOnList.First().InBasket = false;
                ShoppingListId = ExpectedResult.Id;
            }

            public void SetupExpectedResultForOfflineUse()
            {
                ExpectedResult = ShoppingListEntityMother.InitialWithOneItem().Create();
                ExpectedResult.ItemsOnList.First().ItemTypeId = null;
                ExpectedResult.ItemsOnList.First().InBasket = true;
                ShoppingListId = ExpectedResult.Id;
            }

            public void SetupShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                _shoppingList = ExpectedResult.DeepClone();
                _shoppingList.ItemsOnList.First().InBasket = false;
            }

            public void SetupItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);

                var shoppingListItem = _shoppingList.ItemsOnList.First();
                _item = ItemEntityMother.InitialWithTypesForStore(_shoppingList.StoreId)
                    .WithId(shoppingListItem.ItemId)
                    .Create();
                _item.ItemTypes.First().Id = shoppingListItem.ItemTypeId!.Value;
            }

            public void SetupItemForOfflineUse()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                _item = ItemEntityMother.Temporary(_shoppingList.StoreId)
                    .WithId(_shoppingList.ItemsOnList.First().ItemId)
                    .Create();
            }

            public void SetupItemNotOnShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                _item = ItemEntityMother.InitialForStore(_shoppingList.StoreId).Create();
            }

            public void SetupItemIdWithTypeId()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);
                _itemId = new ItemIdContract(_item.Id, null);
                _itemTypeId = _item.ItemTypes.First().Id;
            }

            public void SetupItemIdWithoutTypeId()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);
                _itemId = new ItemIdContract(_item.Id, null);
                _itemTypeId = null;
            }

            public void SetupItemIdForOfflineUse()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);
                _itemId = new ItemIdContract(null, _item.CreatedFrom);
            }

            public async Task SetupDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                TestPropertyNotSetException.ThrowIfNull(_item);

                await ApplyMigrationsAsync(ArrangeScope);

                var shoppingListContext = ArrangeScope.ServiceProvider.GetRequiredService<ShoppingListContext>();
                var itemContext = ArrangeScope.ServiceProvider.GetRequiredService<ItemContext>();

                await shoppingListContext.AddAsync(_shoppingList);
                await itemContext.AddAsync(_item);

                await shoppingListContext.SaveChangesAsync();
                await itemContext.SaveChangesAsync();
            }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);
                Contract = new PutItemInBasketContract(_itemId, _itemTypeId);
            }
        }
    }

    public sealed class RemoveItemFromBasketAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly RemoveItemFromBasketAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task RemoveItemFromBasketAsync_WithActualItemId_ShouldRemoveItemFromBasket()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupShoppingList();
            _fixture.SetupItem();
            _fixture.SetupItemIdWithTypeId();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);

            // Act
            var result = await sut.RemoveItemFromBasketAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion()
                    .ExcludeItemsOnListId());
        }

        [Fact]
        public async Task RemoveItemFromBasketAsync_WithOfflineItemId_ShouldRemoveItemFromBasket()
        {
            // Arrange
            _fixture.SetupExpectedResultForOfflineUse();
            _fixture.SetupShoppingList();
            _fixture.SetupItemForOfflineUse();
            _fixture.SetupItemIdForOfflineUse();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);

            // Act
            var result = await sut.RemoveItemFromBasketAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion()
                    .ExcludeItemsOnListId());
        }

        [Fact]
        public async Task RemoveItemFromBasketAsync_WithItemNotOnShoppingList_ShouldReturnError()
        {
            // Arrange
            _fixture.SetupExpectedResultForItemNotOnShoppingList();
            _fixture.SetupShoppingList();
            _fixture.SetupItemNotOnShoppingList();
            _fixture.SetupItemIdWithoutTypeId();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);

            // Act
            var result = await sut.RemoveItemFromBasketAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<UnprocessableEntityObjectResult>();
            var unprocessableEntityResult = (UnprocessableEntityObjectResult)result;
            unprocessableEntityResult.Value.Should().BeOfType<ErrorContract>();
            var errorContract = (ErrorContract)unprocessableEntityResult.Value!;
            errorContract.ErrorCode.Should().Be(ErrorReasonCode.ItemNotOnShoppingList.ToInt());

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion()
                    .ExcludeItemsOnListId());
        }

        private sealed class RemoveItemFromBasketAsyncFixture(DockerFixture dockerFixture)
            : ShoppingListControllerFixture(dockerFixture)
        {
            private ShoppingList? _shoppingList;
            private Item? _item;
            private ItemIdContract? _itemId;
            private Guid? _itemTypeId;
            public Guid? ShoppingListId { get; private set; }
            public RemoveItemFromBasketContract? Contract { get; private set; }
            public ShoppingList? ExpectedResult { get; private set; }

            public void SetupExpectedResult()
            {
                ExpectedResult = ShoppingListEntityMother.InitialWithOneItem().Create();
                ExpectedResult.ItemsOnList.First().InBasket = false;
                ShoppingListId = ExpectedResult.Id;
            }

            public void SetupExpectedResultForItemNotOnShoppingList()
            {
                ExpectedResult = ShoppingListEntityMother.InitialWithOneItem().Create();
                ExpectedResult.ItemsOnList.First().InBasket = true;
                ShoppingListId = ExpectedResult.Id;
            }

            public void SetupExpectedResultForOfflineUse()
            {
                ExpectedResult = ShoppingListEntityMother.InitialWithOneItem().Create();
                ExpectedResult.ItemsOnList.First().ItemTypeId = null;
                ExpectedResult.ItemsOnList.First().InBasket = false;
                ShoppingListId = ExpectedResult.Id;
            }

            public void SetupShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                _shoppingList = ExpectedResult.DeepClone();
                _shoppingList.ItemsOnList.First().InBasket = true;
            }

            public void SetupItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);

                var shoppingListItem = _shoppingList.ItemsOnList.First();
                _item = ItemEntityMother.InitialWithTypesForStore(_shoppingList.StoreId)
                    .WithId(shoppingListItem.ItemId)
                    .Create();
                _item.ItemTypes.First().Id = shoppingListItem.ItemTypeId!.Value;
            }

            public void SetupItemForOfflineUse()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                _item = ItemEntityMother.Temporary(_shoppingList.StoreId)
                    .WithId(_shoppingList.ItemsOnList.First().ItemId)
                    .Create();
            }

            public void SetupItemNotOnShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                _item = ItemEntityMother.InitialForStore(_shoppingList.StoreId).Create();
            }

            public void SetupItemIdWithTypeId()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);
                _itemId = new ItemIdContract(_item.Id, null);
                _itemTypeId = _item.ItemTypes.First().Id;
            }

            public void SetupItemIdWithoutTypeId()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);
                _itemId = new ItemIdContract(_item.Id, null);
                _itemTypeId = null;
            }

            public void SetupItemIdForOfflineUse()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);
                _itemId = new ItemIdContract(null, _item.CreatedFrom);
            }

            public async Task SetupDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                TestPropertyNotSetException.ThrowIfNull(_item);

                await ApplyMigrationsAsync(ArrangeScope);

                var shoppingListContext = ArrangeScope.ServiceProvider.GetRequiredService<ShoppingListContext>();
                var itemContext = ArrangeScope.ServiceProvider.GetRequiredService<ItemContext>();

                await shoppingListContext.AddAsync(_shoppingList);
                await itemContext.AddAsync(_item);

                await shoppingListContext.SaveChangesAsync();
                await itemContext.SaveChangesAsync();
            }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);
                Contract = new RemoveItemFromBasketContract(_itemId, _itemTypeId);
            }
        }
    }

    public sealed class ChangeItemQuantityOnShoppingListAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly ChangeItemQuantityOnShoppingListAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task ChangeItemQuantityOnShoppingListAsync_WithActualItemId_ShouldChangeItemQuantityOnShoppingList()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupShoppingList();
            _fixture.SetupItem();
            _fixture.SetupItemIdWithTypeId();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);

            // Act
            var result = await sut.ChangeItemQuantityOnShoppingListAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion()
                    .ExcludeItemsOnListId());
        }

        [Fact]
        public async Task ChangeItemQuantityOnShoppingListAsync_WithOfflineItemId_ShouldChangeItemQuantityOnShoppingList()
        {
            // Arrange
            _fixture.SetupExpectedResultForOfflineUse();
            _fixture.SetupShoppingList();
            _fixture.SetupItemForOfflineUse();
            _fixture.SetupItemIdForOfflineUse();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);

            // Act
            var result = await sut.ChangeItemQuantityOnShoppingListAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion()
                    .ExcludeItemsOnListId());
        }

        [Fact]
        public async Task ChangeItemQuantityOnShoppingListAsync_WithItemNotOnShoppingList_ShouldReturnError()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupShoppingListForItemNotOnShoppingList();
            _fixture.SetupItemNotOnShoppingList();
            _fixture.SetupItemIdWithoutTypeId();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);

            // Act
            var result = await sut.ChangeItemQuantityOnShoppingListAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<UnprocessableEntityObjectResult>();
            var unprocessableEntityResult = (UnprocessableEntityObjectResult)result;
            unprocessableEntityResult.Value.Should().BeOfType<ErrorContract>();
            var errorContract = (ErrorContract)unprocessableEntityResult.Value!;
            errorContract.ErrorCode.Should().Be(ErrorReasonCode.ItemNotOnShoppingList.ToInt());

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion()
                    .ExcludeItemsOnListId());
        }

        private sealed class ChangeItemQuantityOnShoppingListAsyncFixture(DockerFixture dockerFixture)
            : ShoppingListControllerFixture(dockerFixture)
        {
            private ShoppingList? _shoppingList;
            private Item? _item;
            private ItemIdContract? _itemId;
            private Guid? _itemTypeId;
            public Guid? ShoppingListId { get; private set; }
            public ChangeItemQuantityOnShoppingListContract? Contract { get; private set; }
            public ShoppingList? ExpectedResult { get; private set; }

            public void SetupExpectedResult()
            {
                ExpectedResult = ShoppingListEntityMother.InitialWithOneItem().Create();
                ShoppingListId = ExpectedResult.Id;
            }

            public void SetupExpectedResultForOfflineUse()
            {
                ExpectedResult = ShoppingListEntityMother.InitialWithOneItem().Create();
                ExpectedResult.ItemsOnList.First().ItemTypeId = null;
                ShoppingListId = ExpectedResult.Id;
            }

            public void SetupShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                _shoppingList = ExpectedResult.DeepClone();
                _shoppingList.ItemsOnList.First().Quantity = new DomainTestBuilder<float>().Create();
            }

            public void SetupShoppingListForItemNotOnShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                _shoppingList = ExpectedResult.DeepClone();
            }

            public void SetupItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);

                var shoppingListItem = _shoppingList.ItemsOnList.First();
                _item = ItemEntityMother.InitialWithTypesForStore(_shoppingList.StoreId)
                    .WithId(shoppingListItem.ItemId)
                    .Create();
                _item.ItemTypes.First().Id = shoppingListItem.ItemTypeId!.Value;
            }

            public void SetupItemForOfflineUse()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                _item = ItemEntityMother.Temporary(_shoppingList.StoreId)
                    .WithId(_shoppingList.ItemsOnList.First().ItemId)
                    .Create();
            }

            public void SetupItemNotOnShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                _item = ItemEntityMother.InitialForStore(_shoppingList.StoreId).Create();
            }

            public void SetupItemIdWithTypeId()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);
                _itemId = new ItemIdContract(_item.Id, null);
                _itemTypeId = _item.ItemTypes.First().Id;
            }

            public void SetupItemIdWithoutTypeId()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);
                _itemId = new ItemIdContract(_item.Id, null);
                _itemTypeId = null;
            }

            public void SetupItemIdForOfflineUse()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);
                _itemId = new ItemIdContract(null, _item.CreatedFrom);
            }

            public async Task SetupDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                TestPropertyNotSetException.ThrowIfNull(_item);

                await ApplyMigrationsAsync(ArrangeScope);

                var shoppingListContext = ArrangeScope.ServiceProvider.GetRequiredService<ShoppingListContext>();
                var itemContext = ArrangeScope.ServiceProvider.GetRequiredService<ItemContext>();

                await shoppingListContext.AddAsync(_shoppingList);
                await itemContext.AddAsync(_item);

                await shoppingListContext.SaveChangesAsync();
                await itemContext.SaveChangesAsync();
            }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                TestPropertyNotSetException.ThrowIfNull(_item);
                Contract = new ChangeItemQuantityOnShoppingListContract(_itemId, _itemTypeId,
                    ExpectedResult.ItemsOnList.First().Quantity);
            }
        }
    }

    public sealed class FinishListAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly FinishListAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task FinishListAsync_WithUndefinedFinishedAt_ShouldSetCorrectFinishedAtDate()
        {
            // Arrange
            _fixture.SetupShoppingListId();
            _fixture.SetupExistingShoppingList();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);

            // Act
            var result = await sut.FinishListAsync(_fixture.ShoppingListId.Value, null);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var allShoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            allShoppingLists.Should().HaveCount(2);
            allShoppingLists.Should().ContainSingle(sl => sl.Id == _fixture.ShoppingListId.Value);

            var finishedShoppingList = allShoppingLists.Single(sl => sl.Id == _fixture.ShoppingListId.Value);
            finishedShoppingList.CompletionDate.Should().NotBeNull();
            finishedShoppingList.CompletionDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(10));
        }

        [Fact]
        public async Task FinishListAsync_WithDefinedFinishedAt_ShouldSetCorrectFinishedAtDate()
        {
            // Arrange
            _fixture.SetupShoppingListId();
            _fixture.SetupFinishedAt();
            _fixture.SetupExistingShoppingList();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.FinishedAt);

            // Act
            var result = await sut.FinishListAsync(_fixture.ShoppingListId.Value, _fixture.FinishedAt);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var allShoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            allShoppingLists.Should().HaveCount(2);
            allShoppingLists.Should().ContainSingle(sl => sl.Id == _fixture.ShoppingListId.Value);

            var finishedShoppingList = allShoppingLists.Single(sl => sl.Id == _fixture.ShoppingListId.Value);
            finishedShoppingList.CompletionDate.Should().NotBeNull();
            finishedShoppingList.CompletionDate.Should().BeCloseTo(_fixture.FinishedAt.Value, TimeSpan.FromMilliseconds(10));
        }

        private sealed class FinishListAsyncFixture(DockerFixture dockerFixture)
            : ShoppingListControllerFixture(dockerFixture)
        {
            private ShoppingList? _existingShoppingList;

            public Guid? ShoppingListId { get; private set; }
            public DateTimeOffset? FinishedAt { get; private set; }

            public void SetupShoppingListId()
            {
                ShoppingListId = Guid.NewGuid();
            }

            public void SetupFinishedAt()
            {
                FinishedAt = new TestBuilder<DateTimeOffset>().Create();
            }

            public void SetupExistingShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ShoppingListId);

                var items = new ItemsOnListEntityBuilder()
                    .WithShoppingList(null!)
                    .WithShoppingListId(ShoppingListId.Value)
                    .CreateMany(3)
                    .ToList();

                _existingShoppingList = new ShoppingListEntityBuilder()
                    .WithoutCompletionDate()
                    .WithId(ShoppingListId.Value)
                    .WithItemsOnList(items)
                    .Create();
            }

            public async Task SetupDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingShoppingList);

                await ApplyMigrationsAsync(ArrangeScope);

                var itemContext = ArrangeScope.ServiceProvider.GetRequiredService<ShoppingListContext>();

                await itemContext.AddAsync(_existingShoppingList);

                await itemContext.SaveChangesAsync();
            }
        }
    }

    public sealed class AddItemDiscount(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly AddItemDiscountFixture _fixture = new AddItemDiscountFixture(dockerFixture);

        [Fact]
        public async Task AddItemDiscountAsync_WithValidData_ShouldAddItemDiscount()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupExistingShoppingList();
            _fixture.SetupContract();
            await _fixture.SetupDatabase();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);

            // Act
            var result = await sut.AddItemDiscountAsync(_fixture.ExpectedResult.Id, _fixture.Contract);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().ExcludeRowVersion().ExcludeItemsOnListId().ExcludeDiscountId()
                    .WithCreatedAtPrecision());
        }

        private sealed class AddItemDiscountFixture(DockerFixture dockerFixture) : ShoppingListControllerFixture(dockerFixture)
        {
            private ShoppingList? _shoppingList;
            public AddItemDiscountContract? Contract { get; private set; }
            public ShoppingList? ExpectedResult { get; private set; }

            public void SetupExpectedResult()
            {
                ExpectedResult = ShoppingListEntityMother.InitialWithOneItem().Create();
                var discount = new DiscountEntityBuilder()
                    .WithItemId(ExpectedResult.ItemsOnList.First().ItemId)
                    .WithItemTypeId(ExpectedResult.ItemsOnList.First().ItemTypeId)
                    .WithShoppingListId(ExpectedResult.Id)
                    .Create();
                ExpectedResult.Discounts = [discount];
            }

            public void SetupExistingShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                _shoppingList = ExpectedResult.DeepClone();
                _shoppingList.Discounts = [];
            }

            public async Task SetupDatabase()
            {
                await ApplyMigrationsAsync(ArrangeScope);

                await using var shoppingListContext = GetContextInstance<ShoppingListContext>(ArrangeScope);

                await shoppingListContext.AddAsync(_shoppingList!);

                await shoppingListContext.SaveChangesAsync();
            }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                var discount = ExpectedResult.Discounts.First();
                Contract = new AddItemDiscountContract(discount.DiscountPrice, discount.ItemId, discount.ItemTypeId);
            }
        }
    }

    private abstract class ShoppingListControllerFixture : DatabaseFixture
    {
        protected ShoppingListControllerFixture(DockerFixture dockerFixture) : base(dockerFixture)
        {
            ArrangeScope = CreateServiceScope();
        }

        protected readonly IServiceScope ArrangeScope;

        public ShoppingListController CreateSut()
        {
            var scope = CreateServiceScope();
            return scope.ServiceProvider.GetRequiredService<ShoppingListController>();
        }

        public override IEnumerable<DbContext> GetDbContexts(IServiceScope scope)
        {
            yield return scope.ServiceProvider.GetRequiredService<ShoppingListContext>();
            yield return scope.ServiceProvider.GetRequiredService<StoreContext>();
            yield return scope.ServiceProvider.GetRequiredService<ItemContext>();
            yield return scope.ServiceProvider.GetRequiredService<ItemCategoryContext>();
            yield return scope.ServiceProvider.GetRequiredService<ManufacturerContext>();
            yield return scope.ServiceProvider.GetRequiredService<RecipeContext>();
        }
    }
}