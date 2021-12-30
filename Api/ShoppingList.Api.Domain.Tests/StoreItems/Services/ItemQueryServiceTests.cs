using AutoFixture;
using FluentAssertions;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Services.Conversion.ItemSearchReadModels;
using ShoppingList.Api.Domain.TestKit.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Stores.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Services
{
    public class ItemQueryServiceTests
    {
        private readonly LocalFixture _fixture;

        public ItemQueryServiceTests()
        {
            _fixture = new LocalFixture();
        }

        [Fact]
        public async Task SearchAsync_WithItemWithoutTypesAlreadyOnShoppingList_ShouldReturnEmptyResult()
        {
            // Arrange
            _fixture.SetupParameters();
            _fixture.SetupItemsWithoutTypes();
            _fixture.SetupFindingItems();
            _fixture.SetupFindingShoppingListWithItemWithoutTypes(true);
            _fixture.SetupFindingStore();
            _fixture.SetupFindingNoItemTypeMapping();
            _fixture.SetupConversionServiceReceivingEmptyItemList();
            _fixture.SetupConversionServiceReceivingEmptyItemWithTypesList();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.SearchAsync(_fixture.Name, _fixture.StoreId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchAsync_WithItemWithoutTypesNotOnShoppingList_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupParameters();
            _fixture.SetupItemsWithoutTypes();
            _fixture.SetupFindingItems();
            _fixture.SetupFindingShoppingListWithItemWithoutTypes(false);
            _fixture.SetupFindingStore();
            _fixture.SetupFindingNoItemTypeMapping();
            _fixture.SetupConversionServiceReceivingItemList();
            _fixture.SetupConversionServiceReceivingEmptyItemWithTypesList();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.SearchAsync(_fixture.Name, _fixture.StoreId);

            // Assert
            result.Should().BeEquivalentTo(_fixture.Result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task SearchAsync_WithItemWithTypesAlreadyOnShoppingList_ShouldReturnEmptyResult(
            bool availableAtStore)
        {
            // Arrange
            _fixture.SetupParameters();
            _fixture.SetupItemsWithTypes(availableAtStore);
            _fixture.SetupFindingItems();
            _fixture.SetupFindingShoppingListWithItemWithTypes(true);
            _fixture.SetupFindingStore();
            _fixture.SetupFindingNoItemTypeMapping();
            _fixture.SetupItemTypesPartiallyNotOnShoppingList();
            _fixture.SetupConversionServiceReceivingEmptyItemList();
            _fixture.SetupConversionServiceReceivingEmptyItemWithTypesList();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.SearchAsync(_fixture.Name, _fixture.StoreId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchAsync_WithItemWithTypesNotOnShoppingListButAvailableAtStore_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupParameters();
            _fixture.SetupItemsWithTypes(true);
            _fixture.SetupFindingItems();
            _fixture.SetupFindingShoppingListWithItemWithTypes(false);
            _fixture.SetupFindingStore();
            _fixture.SetupFindingNoItemTypeMapping();
            _fixture.SetupAllItemTypesNotOnShoppingList();
            _fixture.SetupConversionServiceReceivingEmptyItemList();
            _fixture.SetupConversionServiceReceivingItemWithTypeList();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.SearchAsync(_fixture.Name, _fixture.StoreId);

            // Assert
            result.Should().BeEquivalentTo(_fixture.Result);
        }

        [Fact]
        public async Task SearchAsync_WithItemWithTypesNotOnShoppingListAndNotAvailableAtStore_ShouldReturnEmptyResult()
        {
            // Arrange
            _fixture.SetupParameters();
            _fixture.SetupItemsWithTypes(false);
            _fixture.SetupFindingItems();
            _fixture.SetupFindingShoppingListWithItemWithTypes(false);
            _fixture.SetupFindingStore();
            _fixture.SetupFindingNoItemTypeMapping();
            _fixture.SetupAllItemTypesOnShoppingListOrNotAvailable();
            _fixture.SetupConversionServiceReceivingEmptyItemList();
            _fixture.SetupConversionServiceReceivingEmptyItemWithTypesList();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.SearchAsync(_fixture.Name, _fixture.StoreId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchAsync_WithItemWithTypesPartiallyOnShoppingListAndAvailableAtStore_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupParameters();
            _fixture.SetupItemsWithTypes(true, 2);
            _fixture.SetupFindingItems();
            _fixture.SetupFindingShoppingListWithItemWithTypes(true);
            _fixture.SetupFindingStore();
            _fixture.SetupFindingNoItemTypeMapping();
            _fixture.SetupItemTypesPartiallyNotOnShoppingList();
            _fixture.SetupConversionServiceReceivingEmptyItemList();
            _fixture.SetupConversionServiceReceivingItemWithTypeList();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.SearchAsync(_fixture.Name, _fixture.StoreId);

            // Assert
            result.Should().BeEquivalentTo(_fixture.Result);
        }

        [Fact]
        public async Task SearchAsync_WithItemWithAndWithoutTypesNotOnShoppingList_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupParameters();
            _fixture.SetupItemsWithAndWithoutTypes();
            _fixture.SetupFindingItems();
            _fixture.SetupFindingShoppingListWithItemWithAndWithoutTypes(false, false);
            _fixture.SetupFindingStore();
            _fixture.SetupFindingNoItemTypeMapping();
            _fixture.SetupItemTypesPartiallyNotOnShoppingList();
            _fixture.SetupConversionServiceReceivingItemList();
            _fixture.SetupConversionServiceReceivingItemWithTypeList();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.SearchAsync(_fixture.Name, _fixture.StoreId);

            // Assert
            result.Should().BeEquivalentTo(_fixture.Result);
        }

        [Fact]
        public async Task SearchAsync_WithItemTypeMappingNotOnShoppingList_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupParameters();
            _fixture.SetupItemsWithTypes(true);
            _fixture.SetupFindingNoItems();
            _fixture.SetupFindingShoppingListWithItemWithTypes(false);
            _fixture.SetupFindingStore();
            _fixture.SetupItemTypeMapping();
            _fixture.SetupFindingItemTypeMapping();
            _fixture.SetupFindingItemsById();
            _fixture.SetupAllItemTypesNotOnShoppingList();
            _fixture.SetupConversionServiceReceivingEmptyItemList();
            _fixture.SetupConversionServiceReceivingItemWithTypeList();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.SearchAsync(_fixture.Name, _fixture.StoreId);

            // Assert
            result.Should().BeEquivalentTo(_fixture.Result);
        }

        [Fact]
        public async Task SearchAsync_WithItemTypeDuplicatedByItem_ShouldReturnExpectedResult() //todo
        {
            // Arrange
            _fixture.SetupParameters();
            _fixture.SetupItemsWithTypes(true);
            _fixture.SetupFindingNoItems();
            _fixture.SetupFindingShoppingListWithItemWithTypes(false);
            _fixture.SetupFindingStore();
            _fixture.SetupItemTypeMapping();
            _fixture.SetupFindingItemTypeMapping();
            _fixture.SetupFindingItemsById();
            _fixture.SetupAllItemTypesNotOnShoppingList();
            _fixture.SetupConversionServiceReceivingEmptyItemList();
            _fixture.SetupConversionServiceReceivingItemWithTypeList();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.SearchAsync(_fixture.Name, _fixture.StoreId);

            // Assert
            result.Should().BeEquivalentTo(_fixture.Result);
        }

        private sealed class LocalFixture
        {
            private readonly Fixture _fixture;
            private readonly CommonFixture _commonFixture = new CommonFixture();

            private readonly ItemRepositoryMock _itemRepositoryMock;
            private readonly ShoppingListRepositoryMock _shoppingListRepositoryMock;
            private readonly StoreRepositoryMock _storeRepositoryMock;
            private readonly ItemTypeReadRepositoryMock _itemTypeReadRepositoryMock;
            private readonly ItemSearchReadModelConversionServiceMock _conversionServiceMock;

            private List<IStoreItem> _items;
            private Store _store;
            private readonly List<ItemWithMatchingItemTypeIds> _itemToTypeIdMappings = new List<ItemWithMatchingItemTypeIds>();
            private IShoppingList _shoppingList;
            private List<(ItemId, ItemTypeId)> _itemTypeMapping;

            public LocalFixture()
            {
                _fixture = _commonFixture.GetNewFixture();

                _itemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
                _shoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
                _storeRepositoryMock = new StoreRepositoryMock(MockBehavior.Strict);
                _itemTypeReadRepositoryMock = new ItemTypeReadRepositoryMock(MockBehavior.Strict);
                _conversionServiceMock = new ItemSearchReadModelConversionServiceMock(MockBehavior.Strict);
            }

            public string Name { get; private set; }
            public StoreId StoreId { get; private set; }

            public List<ItemSearchReadModel> Result { get; } = new List<ItemSearchReadModel>();

            public ItemQueryService CreateSut()
            {
                return new ItemQueryService(
                    _itemRepositoryMock.Object,
                    _shoppingListRepositoryMock.Object,
                    _storeRepositoryMock.Object,
                    _itemTypeReadRepositoryMock.Object,
                    _conversionServiceMock.Object,
                    default);
            }

            public void SetupName()
            {
                Name = _fixture.Create<string>();
            }

            public void SetupStoreId()
            {
                StoreId = new StoreIdBuilder().Create();
            }

            public void SetupParameters()
            {
                SetupName();
                SetupStoreId();
            }

            public void SetupItemsWithAndWithoutTypes()
            {
                var types = GetItemTypes(true, 1);

                _items = StoreItemMother.Initial().CreateMany(1).ToList<IStoreItem>();
                _items.Add(StoreItemMother.InitialWithTypes().WithTypes(types).Create());
            }

            public void SetupItemsWithTypes(bool availableAtStore, int typeCount = 1)
            {
                var types = GetItemTypes(availableAtStore, typeCount);

                _items = StoreItemMother.InitialWithTypes().WithTypes(types).CreateMany(1).ToList<IStoreItem>();
            }

            private IEnumerable<IItemType> GetItemTypes(bool availableAtStore, int count)
            {
                var typesBuilder = new ItemTypeBuilder();

                if (availableAtStore)
                {
                    var availability = StoreItemAvailabilityMother.ForStore(StoreId).CreateMany(1);
                    typesBuilder.WithAvailabilities(availability);
                }

                return typesBuilder.CreateMany(count);
            }

            public void SetupItemsWithoutTypes()
            {
                _items = StoreItemMother.Initial().CreateMany(1).ToList<IStoreItem>();
            }

            public void SetupFindingItems()
            {
                _itemRepositoryMock.SetupFindActiveByAsync(Name, StoreId, _items);
            }

            public void SetupFindingNoItems()
            {
                _itemRepositoryMock.SetupFindActiveByAsync(Name, StoreId, Enumerable.Empty<IStoreItem>());
            }

            public void SetupFindingItemsById()
            {
                _itemRepositoryMock.SetupFindByAsync(_items.Select(i => i.Id), _items);
            }

            public void SetupFindingShoppingListWithItemWithoutTypes(bool containsItem)
            {
                var builder = new ShoppingListItemBuilder().WithoutTypeId();

                if (containsItem)
                {
                    builder.WithId(_items.Single().Id);
                    builder.WithoutTypeId();
                }

                var items = builder.CreateMany(1);

                _shoppingList = ShoppingListMother.OneSection(items).Create();
                _shoppingListRepositoryMock.SetupFindActiveByAsync(StoreId, _shoppingList);
            }

            public void SetupFindingShoppingListWithItemWithAndWithoutTypes(bool containsItemWithTypes,
                bool containsItemWithoutTypes)
            {
                var builderWithTypes = new ShoppingListItemBuilder();
                var builderWithoutTypes = new ShoppingListItemBuilder();
                if (containsItemWithTypes)
                {
                    var item = _items.Single(i => i.HasItemTypes);
                    builderWithTypes.WithId(item.Id);
                    builderWithTypes.WithTypeId(item.ItemTypes.First().Id);
                }
                if (containsItemWithoutTypes)
                {
                    builderWithoutTypes.WithId(_items.Single(i => !i.HasItemTypes).Id);
                    builderWithoutTypes.WithoutTypeId();
                }

                var items = builderWithTypes.CreateMany(1).ToList();
                items.Add(builderWithoutTypes.Create());

                _shoppingList = ShoppingListMother.OneSection(items).Create();
                _shoppingListRepositoryMock.SetupFindActiveByAsync(StoreId, _shoppingList);
            }

            public void SetupFindingShoppingListWithItemWithTypes(bool containsItem)
            {
                var builder = new ShoppingListItemBuilder();

                if (containsItem)
                {
                    var item = _items.Single(i => i.HasItemTypes);
                    builder.WithId(item.Id);
                    builder.WithTypeId(_commonFixture.ChooseRandom(item.ItemTypes).Id);
                }

                var items = builder.CreateMany(1);

                _shoppingList = ShoppingListMother.OneSection(items).Create();
                _shoppingListRepositoryMock.SetupFindActiveByAsync(StoreId, _shoppingList);
            }

            public void SetupNotFindingShoppingList()
            {
                _shoppingListRepositoryMock.SetupFindActiveByAsync(StoreId, null);
            }

            public void SetupFindingStore()
            {
                _store = StoreMother.Sections(1).WithId(StoreId).Create();
                _storeRepositoryMock.SetupFindByAsync(StoreId, _store);
            }

            public void SetupNotFindingStore()
            {
                _storeRepositoryMock.SetupFindByAsync(StoreId, null);
            }

            public void SetupFindingNoItemTypeMapping()
            {
                _itemTypeReadRepositoryMock.SetupFindActiveByAsync(Name, StoreId,
                    Enumerable.Empty<(ItemId, ItemTypeId)>());
            }

            public void SetupFindingItemTypeMapping()
            {
                _itemTypeReadRepositoryMock.SetupFindActiveByAsync(Name, StoreId, _itemTypeMapping);
            }

            public void SetupItemTypeMapping()
            {
                _itemTypeMapping = _items.Where(i => i.HasItemTypes)
                    .SelectMany(i => i.ItemTypes.Select(t => (i.Id, t.Id)))
                    .ToList();
            }

            public void SetupAllItemTypesOnShoppingListOrNotAvailable()
            {
                var item = _items.Single();
                _itemToTypeIdMappings.Add(new ItemWithMatchingItemTypeIds(item, Enumerable.Empty<ItemTypeId>()));
            }

            public void SetupAllItemTypesNotOnShoppingList()
            {
                var item = _items.Single();
                _itemToTypeIdMappings.Add(new ItemWithMatchingItemTypeIds(item, item.ItemTypes.Select(t => t.Id)));
            }

            public void SetupItemTypesPartiallyNotOnShoppingList()
            {
                var item = _items.Single(i => i.HasItemTypes);
                var itemTypeIds = item.ItemTypes.Select(t => t.Id);
                var typesNotOnList =
                    itemTypeIds.Except(_shoppingList.Items.Where(i => i.TypeId != null).Select(t => t.TypeId));
                _itemToTypeIdMappings.Add(new ItemWithMatchingItemTypeIds(item, typesNotOnList));
            }

            public void SetupConversionServiceReceivingEmptyItemList()
            {
                _conversionServiceMock.SetupConvertAsync(Enumerable.Empty<IStoreItem>(), _store,
                    Enumerable.Empty<ItemSearchReadModel>());
            }

            public void SetupConversionServiceReceivingItemList()
            {
                var itemReadModels = _fixture.CreateMany<ItemSearchReadModel>().ToList();
                Result.AddRange(itemReadModels);
                var items = _items.Where(i => !i.HasItemTypes);
                _conversionServiceMock.SetupConvertAsync(items, _store, itemReadModels);
            }

            public void SetupConversionServiceReceivingItemWithTypeList()
            {
                var itemReadModels = _fixture
                    .CreateMany<ItemSearchReadModel>(_itemToTypeIdMappings.Select(m => m.MatchingItemTypeIds).Count())
                    .ToList();
                Result.AddRange(itemReadModels);
                _conversionServiceMock.SetupConvertAsync(_itemToTypeIdMappings, _store, itemReadModels);
            }

            public void SetupConversionServiceReceivingEmptyItemWithTypesList()
            {
                _conversionServiceMock.SetupConvertAsync(Enumerable.Empty<ItemWithMatchingItemTypeIds>(), _store,
                    Enumerable.Empty<ItemSearchReadModel>());
            }
        }
    }
}