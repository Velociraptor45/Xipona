using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Conversion;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Conversion.ItemSearchReadModels;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Shared;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Ports;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Items.Services.Search;

public class ItemSearchServiceTests
{
    public class OtherTests // todo cleanup
    {
        private readonly LocalFixture _fixture;

        public OtherTests()
        {
            _fixture = new LocalFixture();
        }

        [Fact]
        public async Task SearchAsync_WithNameIsEmpty_ShouldReturnEmptyResult()
        {
            // Arrange
            _fixture.SetupStoreId();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.SearchForShoppingListAsync(string.Empty, _fixture.StoreId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchAsync_WithNameIsWhiteSpace_ShouldReturnEmptyResult()
        {
            // Arrange
            _fixture.SetupStoreId();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.SearchForShoppingListAsync("  ", _fixture.StoreId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchAsync_WithStoreNotFound_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupParameters();
            _fixture.SetupNotFindingStore();
            var sut = _fixture.CreateSut();

            // Act
            Func<Task> func = async () => await sut.SearchForShoppingListAsync(_fixture.Name, _fixture.StoreId);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.StoreNotFound);
        }

        [Fact]
        public async Task SearchAsync_WithShoppingListNotFound_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupParameters();
            _fixture.SetupFindingNoItems();
            _fixture.SetupFindingStore();
            _fixture.SetupNotFindingShoppingList();
            var sut = _fixture.CreateSut();

            // Act
            Func<Task> func = async () => await sut.SearchForShoppingListAsync(_fixture.Name, _fixture.StoreId);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ShoppingListNotFound);
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
            var result = await sut.SearchForShoppingListAsync(_fixture.Name, _fixture.StoreId);

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
            var result = await sut.SearchForShoppingListAsync(_fixture.Name, _fixture.StoreId);

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
            _fixture.SetupFindingShoppingListWithItemWithTypes(true, false);
            _fixture.SetupFindingStore();
            _fixture.SetupFindingNoItemTypeMapping();
            _fixture.SetupItemTypesPartiallyNotOnShoppingList();
            _fixture.SetupConversionServiceReceivingEmptyItemList();
            _fixture.SetupConversionServiceReceivingEmptyItemWithTypesList();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.SearchForShoppingListAsync(_fixture.Name, _fixture.StoreId);

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
            _fixture.SetupFindingShoppingListWithItemWithTypes(false, false);
            _fixture.SetupFindingStore();
            _fixture.SetupFindingNoItemTypeMapping();
            _fixture.SetupAllItemTypesNotOnShoppingList();
            _fixture.SetupConversionServiceReceivingEmptyItemList();
            _fixture.SetupConversionServiceReceivingItemWithTypeList();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.SearchForShoppingListAsync(_fixture.Name, _fixture.StoreId);

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
            _fixture.SetupFindingShoppingListWithItemWithTypes(false, false);
            _fixture.SetupFindingStore();
            _fixture.SetupFindingNoItemTypeMapping();
            _fixture.SetupAllItemTypesOnShoppingListOrNotAvailable();
            _fixture.SetupConversionServiceReceivingEmptyItemList();
            _fixture.SetupConversionServiceReceivingEmptyItemWithTypesList();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.SearchForShoppingListAsync(_fixture.Name, _fixture.StoreId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task
            SearchAsync_WithItemWithTypesPartiallyOnShoppingListAndAvailableAtStore_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupParameters();
            _fixture.SetupItemsWithTypes(true, 2);
            _fixture.SetupFindingItems();
            _fixture.SetupFindingShoppingListWithItemWithTypes(true, false);
            _fixture.SetupFindingStore();
            _fixture.SetupFindingNoItemTypeMapping();
            _fixture.SetupItemTypesPartiallyNotOnShoppingList();
            _fixture.SetupConversionServiceReceivingEmptyItemList();
            _fixture.SetupConversionServiceReceivingItemWithTypeList();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.SearchForShoppingListAsync(_fixture.Name, _fixture.StoreId);

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
            var result = await sut.SearchForShoppingListAsync(_fixture.Name, _fixture.StoreId);

            // Assert
            result.Should().BeEquivalentTo(_fixture.Result);
        }

        [Fact]
        public async Task SearchAsync_WithItemTypeMappingNotOnShoppingList_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupParameters();
            _fixture.SetupItemsFromTypeMapping(true);
            _fixture.SetupFindingNoItems();
            _fixture.SetupFindingShoppingListWithItemWithTypes(false, false);
            _fixture.SetupFindingStore();

            _fixture.SetupItemTypeMapping();
            _fixture.SetupFindingItemTypeMapping();
            _fixture.SetupFindingItemsFromTypeMapping();

            _fixture.SetupAllItemTypeMappingsNotOnShoppingList();

            _fixture.SetupConversionServiceReceivingEmptyItemList();
            _fixture.SetupConversionServiceReceivingItemWithTypeList();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.SearchForShoppingListAsync(_fixture.Name, _fixture.StoreId);

            // Assert
            result.Should().BeEquivalentTo(_fixture.Result);
        }

        [Fact]
        public async Task SearchAsync_WithItemFromTypeMappingNotFound_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupParameters();
            _fixture.SetupItemsFromTypeMapping(true);
            _fixture.SetupFindingNoItems();
            _fixture.SetupFindingShoppingListWithItemWithTypes(false, false);
            _fixture.SetupFindingStore();

            _fixture.SetupItemTypeMapping();
            _fixture.SetupFindingItemTypeMapping();
            _fixture.SetupFindingNoItemsFromTypeMapping();

            _fixture.SetupAllItemTypeMappingsNotOnShoppingList();

            _fixture.SetupConversionServiceReceivingEmptyItemList();
            _fixture.SetupConversionServiceReceivingItemWithTypeList();
            var sut = _fixture.CreateSut();

            // Act
            Func<Task> func = async () => await sut.SearchForShoppingListAsync(_fixture.Name, _fixture.StoreId);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemNotFound);
        }

        [Fact]
        public async Task SearchAsync_WithItemTypeMappingAlreadyOnShoppingList_ShouldReturnEmptyResult()
        {
            // Arrange
            _fixture.SetupParameters();
            _fixture.SetupItemsFromTypeMapping(true);
            _fixture.SetupFindingNoItems();
            _fixture.SetupFindingShoppingListWithItemWithTypes(false, true);
            _fixture.SetupFindingStore();

            _fixture.SetupItemTypeMapping();
            _fixture.SetupFindingItemTypeMapping();
            _fixture.SetupFindingNoItemsFromTypeMappingWithEmptyInput();

            _fixture.SetupAllItemTypeMappingsOnShoppingListOrNotAvailable();

            _fixture.SetupConversionServiceReceivingEmptyItemList();
            _fixture.SetupConversionServiceReceivingEmptyItemWithTypesList();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.SearchForShoppingListAsync(_fixture.Name, _fixture.StoreId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchAsync_WithItemTypeDuplicatedByItemAndNotOnShoppingList_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupParameters();
            _fixture.SetupItemsFromTypeMapping(true);
            _fixture.SetupItemsWithTypesFromTypeMapping(true);
            _fixture.SetupFindingItems();
            _fixture.SetupFindingShoppingListWithItemWithTypes(false, false);
            _fixture.SetupFindingStore();

            _fixture.SetupItemTypeMapping();
            _fixture.SetupFindingItemTypeMapping();
            _fixture.SetupFindingNoItemsFromTypeMappingWithEmptyInput();

            _fixture.SetupAllItemTypesNotOnShoppingList();

            _fixture.SetupConversionServiceReceivingEmptyItemList();
            _fixture.SetupConversionServiceReceivingItemWithTypeList();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.SearchForShoppingListAsync(_fixture.Name, _fixture.StoreId);

            // Assert
            result.Should().BeEquivalentTo(_fixture.Result);
        }

        [Fact]
        public async Task SearchAsync_WithItemTypeDuplicatedByItemAndOnShoppingList_ShouldReturnEmptyResult()
        {
            // Arrange
            _fixture.SetupParameters();
            _fixture.SetupItemsFromTypeMapping(true);
            _fixture.SetupItemsWithTypesFromTypeMapping(true);
            _fixture.SetupFindingItems();
            _fixture.SetupFindingShoppingListWithItemWithTypes(true, false);
            _fixture.SetupFindingStore();

            _fixture.SetupItemTypeMapping();
            _fixture.SetupFindingItemTypeMapping();
            _fixture.SetupFindingNoItemsFromTypeMappingWithEmptyInput();

            _fixture.SetupAllItemTypesOnShoppingListOrNotAvailable();

            _fixture.SetupConversionServiceReceivingEmptyItemList();
            _fixture.SetupConversionServiceReceivingEmptyItemWithTypesList();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.SearchForShoppingListAsync(_fixture.Name, _fixture.StoreId);

            // Assert
            result.Should().BeEmpty();
        }

        private sealed class LocalFixture : ItemSearchServiceFixture
        {
            private List<IItem>? _items;
            private readonly List<IItem> _itemsFromTypeMapping = new();
            private Store? _store;
            private readonly List<ItemWithMatchingItemTypeIds> _itemToTypeIdMappings = new();
            private IShoppingList? _shoppingList;
            private List<(ItemId, ItemTypeId)>? _itemTypeMapping;

            public string Name { get; private set; } = string.Empty;
            public StoreId StoreId { get; private set; }

            public List<SearchItemForShoppingResultReadModel> Result { get; } = new();

            public void SetupName()
            {
                Name = Fixture.Create<string>();
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

                _items = ItemMother.Initial().CreateMany(1).ToList<IItem>();
                _items.Add(ItemMother.InitialWithTypes().WithTypes(types).Create());
            }

            public void SetupItemsWithTypes(bool availableAtStore, int typeCount = 1)
            {
                var types = GetItemTypes(availableAtStore, typeCount);

                _items = ItemMother.InitialWithTypes().WithTypes(types).CreateMany(1).ToList<IItem>();
            }

            public void SetupItemsWithTypesFromTypeMapping(bool availableAtStore)
            {
                var item = _itemsFromTypeMapping.Single();

                var typesBuilder = new ItemTypeBuilder()
                    .WithId(CommonFixture.ChooseRandom(item.ItemTypes).Id);

                if (availableAtStore)
                {
                    var availability = ItemAvailabilityMother.ForStore(StoreId).CreateMany(1);
                    typesBuilder.WithAvailabilities(availability);
                }

                var types = new ItemTypes(typesBuilder.CreateMany(1), ItemTypeFactoryMock.Object);

                _items = ItemMother.InitialWithTypes().WithId(item.Id).WithTypes(types).CreateMany(1)
                    .ToList<IItem>();
            }

            public void SetupItemsFromTypeMapping(bool availableAtStore, int typeCount = 1)
            {
                var types = GetItemTypes(availableAtStore, typeCount);

                _itemsFromTypeMapping.Add(ItemMother.InitialWithTypes().WithTypes(types).Create());
            }

            private ItemTypes GetItemTypes(bool availableAtStore, int count)
            {
                var typesBuilder = new ItemTypeBuilder();

                if (availableAtStore)
                {
                    var availability = ItemAvailabilityMother.ForStore(StoreId).CreateMany(1);
                    typesBuilder.WithAvailabilities(availability);
                }

                return new ItemTypes(typesBuilder.CreateMany(count), ItemTypeFactoryMock.Object);
            }

            public void SetupItemsWithoutTypes()
            {
                _items = ItemMother.Initial().CreateMany(1).ToList<IItem>();
            }

            public void SetupFindingItems()
            {
                TestPropertyNotSetException.ThrowIfNull(_items);
                ItemRepositoryMock.SetupFindActiveByAsync(Name, StoreId, _items);
            }

            public void SetupFindingNoItems()
            {
                ItemRepositoryMock.SetupFindActiveByAsync(Name, StoreId, Enumerable.Empty<IItem>());
            }

            public void SetupFindingItemsFromTypeMapping()
            {
                ItemRepositoryMock.SetupFindActiveByAsync(_itemsFromTypeMapping.Select(i => i.Id), _itemsFromTypeMapping);
            }

            public void SetupFindingNoItemsFromTypeMapping()
            {
                ItemRepositoryMock.SetupFindActiveByAsync(_itemsFromTypeMapping.Select(i => i.Id), Enumerable.Empty<IItem>());
            }

            public void SetupFindingNoItemsFromTypeMappingWithEmptyInput()
            {
                ItemRepositoryMock.SetupFindActiveByAsync(Enumerable.Empty<ItemId>(), Enumerable.Empty<IItem>());
            }

            public void SetupFindingShoppingListWithItemWithoutTypes(bool containsItem)
            {
                TestPropertyNotSetException.ThrowIfNull(_items);
                var builder = new ShoppingListItemBuilder().WithoutTypeId();

                if (containsItem)
                {
                    builder.WithId(_items.Single().Id);
                    builder.WithoutTypeId();
                }

                var items = builder.CreateMany(1);

                _shoppingList = ShoppingListMother.OneSection(items).Create();
                ShoppingListRepositoryMock.SetupFindActiveByAsync(StoreId, _shoppingList);
            }

            public void SetupFindingShoppingListWithItemWithAndWithoutTypes(bool containsItemWithTypes,
                bool containsItemWithoutTypes)
            {
                TestPropertyNotSetException.ThrowIfNull(_items);
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
                ShoppingListRepositoryMock.SetupFindActiveByAsync(StoreId, _shoppingList);
            }

            public void SetupFindingShoppingListWithItemWithTypes(bool containsItem,
                bool containsAdditionalItemTypeMappingItem)
            {
                var builder = new ShoppingListItemBuilder();

                if (containsItem)
                {
                    TestPropertyNotSetException.ThrowIfNull(_items);
                    var item = _items.Single(i => i.HasItemTypes);
                    builder.WithId(item.Id);
                    builder.WithTypeId(CommonFixture.ChooseRandom(item.ItemTypes).Id);
                }

                var items = builder.CreateMany(1).ToList();

                if (containsAdditionalItemTypeMappingItem)
                {
                    var item = _itemsFromTypeMapping.Single();
                    var additionalListItem = new ShoppingListItemBuilder()
                        .WithId(item.Id)
                        .WithTypeId(CommonFixture.ChooseRandom(item.ItemTypes).Id)
                        .Create();

                    items.Add(additionalListItem);
                }

                _shoppingList = ShoppingListMother.OneSection(items).Create();
                ShoppingListRepositoryMock.SetupFindActiveByAsync(StoreId, _shoppingList);
            }

            public void SetupNotFindingShoppingList()
            {
                ShoppingListRepositoryMock.SetupFindActiveByAsync(StoreId, null);
            }

            public void SetupFindingStore()
            {
                _store = StoreMother.Sections(1).WithId(StoreId).Create();
                StoreRepositoryMock.SetupFindByAsync(StoreId, _store);
            }

            public void SetupNotFindingStore()
            {
                StoreRepositoryMock.SetupFindByAsync(StoreId, null);
            }

            public void SetupFindingNoItemTypeMapping()
            {
                ItemTypeReadRepositoryMock.SetupFindActiveByAsync(Name, StoreId,
                    Enumerable.Empty<(ItemId, ItemTypeId)>());
            }

            public void SetupFindingItemTypeMapping()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemTypeMapping);
                ItemTypeReadRepositoryMock.SetupFindActiveByAsync(Name, StoreId, _itemTypeMapping);
            }

            public void SetupItemTypeMapping()
            {
                _itemTypeMapping = _itemsFromTypeMapping.Where(i => i.HasItemTypes)
                    .SelectMany(i => i.ItemTypes.Select(t => (i.Id, t.Id)))
                    .ToList();
            }

            public void SetupAllItemTypesOnShoppingListOrNotAvailable()
            {
                TestPropertyNotSetException.ThrowIfNull(_items);
                var item = _items.Single();
                _itemToTypeIdMappings.Add(new ItemWithMatchingItemTypeIds(item, Enumerable.Empty<ItemTypeId>()));
            }

            public void SetupAllItemTypesNotOnShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_items);
                var item = _items.Single();
                _itemToTypeIdMappings.Add(new ItemWithMatchingItemTypeIds(item, item.ItemTypes.Select(t => t.Id)));
            }

            public void SetupItemTypesPartiallyNotOnShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_items);
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                var item = _items.Single(i => i.HasItemTypes);
                var itemTypeIds = item.ItemTypes.Select(t => t.Id);
                var typesNotOnList =
                    itemTypeIds.Except(_shoppingList.Items.Where(i => i.TypeId != null).Select(t => t.TypeId!.Value));
                _itemToTypeIdMappings.Add(new ItemWithMatchingItemTypeIds(item, typesNotOnList));
            }

            public void SetupAllItemTypeMappingsOnShoppingListOrNotAvailable()
            {
                var item = _itemsFromTypeMapping.Single();
                _itemToTypeIdMappings.Add(new ItemWithMatchingItemTypeIds(item, Enumerable.Empty<ItemTypeId>()));
            }

            public void SetupAllItemTypeMappingsNotOnShoppingList()
            {
                var item = _itemsFromTypeMapping.Single();
                _itemToTypeIdMappings.Add(new ItemWithMatchingItemTypeIds(item, item.ItemTypes.Select(t => t.Id)));
            }

            public void SetupConversionServiceReceivingEmptyItemList()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);
                ConversionServiceMock.SetupConvertAsync(Enumerable.Empty<IItem>(), _store,
                    Enumerable.Empty<SearchItemForShoppingResultReadModel>());
            }

            public void SetupConversionServiceReceivingItemList()
            {
                TestPropertyNotSetException.ThrowIfNull(_items);
                TestPropertyNotSetException.ThrowIfNull(_store);
                var itemReadModels = Fixture.CreateMany<SearchItemForShoppingResultReadModel>().ToList();
                Result.AddRange(itemReadModels);
                var items = _items.Where(i => !i.HasItemTypes);
                ConversionServiceMock.SetupConvertAsync(items, _store, itemReadModels);
            }

            public void SetupConversionServiceReceivingItemWithTypeList()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);
                var itemReadModels = Fixture
                    .CreateMany<SearchItemForShoppingResultReadModel>(_itemToTypeIdMappings
                        .Select(m => m.MatchingItemTypeIds).Count())
                    .ToList();
                Result.AddRange(itemReadModels);
                ConversionServiceMock.SetupConvertAsync(_itemToTypeIdMappings, _store, itemReadModels);
            }

            public void SetupConversionServiceReceivingEmptyItemWithTypesList()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);
                ConversionServiceMock.SetupConvertAsync(Enumerable.Empty<ItemWithMatchingItemTypeIds>(), _store,
                    Enumerable.Empty<SearchItemForShoppingResultReadModel>());
            }
        }
    }

    public sealed class SearchAsync_ItemCategoryId
    {
        private readonly SearchAsyncFixture _fixture;

        public SearchAsync_ItemCategoryId()
        {
            _fixture = new SearchAsyncFixture();
        }

        [Fact]
        public async Task SearchAsync_WithItem_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemCategoryId();
            _fixture.SetupValidatingItemCategoryId();
            _fixture.SetupItemWithoutTypes();
            _fixture.SetupFindingItems();
            _fixture.SetupConvertedAvailabilitiesWithoutTypes();
            _fixture.SetupConvertingAvailabilities();
            _fixture.SetupExpectedResultWithoutTypes();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCategoryId);

            // Act
            var results = await sut.SearchAsync(_fixture.ItemCategoryId.Value);

            // Assert
            results.Should().BeEquivalentTo(_fixture.ExpectedResults);
        }

        [Fact]
        public async Task SearchAsync_WithItem_ShouldValidateItemCategoryId()
        {
            // Arrange
            _fixture.SetupItemCategoryId();
            _fixture.SetupValidatingItemCategoryId();
            _fixture.SetupItemWithoutTypes();
            _fixture.SetupFindingItems();
            _fixture.SetupConvertedAvailabilitiesWithoutTypes();
            _fixture.SetupConvertingAvailabilities();
            _fixture.SetupExpectedResultWithoutTypes();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCategoryId);

            // Act
            await sut.SearchAsync(_fixture.ItemCategoryId.Value);

            // Assert
            _fixture.VerifyValidatingItemCategoryId();
        }

        [Fact]
        public async Task SearchAsync_WithItemWithTypes_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemCategoryId();
            _fixture.SetupValidatingItemCategoryId();
            _fixture.SetupItemWithTypes();
            _fixture.SetupFindingItems();
            _fixture.SetupConvertedAvailabilitiesWithTypes();
            _fixture.SetupConvertingAvailabilities();
            _fixture.SetupExpectedResultWithTypes();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCategoryId);

            // Act
            var results = await sut.SearchAsync(_fixture.ItemCategoryId.Value);

            // Assert
            results.Should().BeEquivalentTo(_fixture.ExpectedResults);
        }

        [Fact]
        public async Task SearchAsync_WithItemWithTypes_ShouldValidateItemCategoryId()
        {
            // Arrange
            _fixture.SetupItemCategoryId();
            _fixture.SetupValidatingItemCategoryId();
            _fixture.SetupItemWithTypes();
            _fixture.SetupFindingItems();
            _fixture.SetupConvertedAvailabilitiesWithTypes();
            _fixture.SetupConvertingAvailabilities();
            _fixture.SetupExpectedResultWithTypes();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCategoryId);

            // Act
            await sut.SearchAsync(_fixture.ItemCategoryId.Value);

            // Assert
            _fixture.VerifyValidatingItemCategoryId();
        }

        [Fact]
        public async Task SearchAsync_WithItemAndItemWithTypes_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemCategoryId();
            _fixture.SetupValidatingItemCategoryId();
            _fixture.SetupItemWithTypes();
            _fixture.SetupItemWithoutTypes();
            _fixture.SetupFindingItems();
            _fixture.SetupConvertedAvailabilitiesWithTypes();
            _fixture.SetupConvertedAvailabilitiesWithoutTypes();
            _fixture.SetupConvertingAvailabilities();
            _fixture.SetupExpectedResultWithTypes();
            _fixture.SetupExpectedResultWithoutTypes();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCategoryId);

            // Act
            var results = await sut.SearchAsync(_fixture.ItemCategoryId.Value);

            // Assert
            results.Should().BeEquivalentTo(_fixture.ExpectedResults);
        }

        [Fact]
        public async Task SearchAsync_WithItemAndItemWithTypes_ShouldValidateItemCategoryId()
        {
            // Arrange
            _fixture.SetupItemCategoryId();
            _fixture.SetupValidatingItemCategoryId();
            _fixture.SetupItemWithTypes();
            _fixture.SetupItemWithoutTypes();
            _fixture.SetupFindingItems();
            _fixture.SetupConvertedAvailabilitiesWithTypes();
            _fixture.SetupConvertedAvailabilitiesWithoutTypes();
            _fixture.SetupConvertingAvailabilities();
            _fixture.SetupExpectedResultWithTypes();
            _fixture.SetupExpectedResultWithoutTypes();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCategoryId);

            // Act
            await sut.SearchAsync(_fixture.ItemCategoryId.Value);

            // Assert
            _fixture.VerifyValidatingItemCategoryId();
        }

        private sealed class SearchAsyncFixture : ItemSearchServiceFixture
        {
            private IItem? _foundItem;
            private IItem? _foundItemWithTypes;

            private readonly Dictionary<(ItemId, ItemTypeId?), IEnumerable<ItemAvailabilityReadModel>>
                _convertedAvailabilities = new();

            public List<SearchItemByItemCategoryResult> ExpectedResults { get; } = new();
            public ItemCategoryId? ItemCategoryId { get; private set; }

            private IEnumerable<IItem> Items
            {
                get
                {
                    if (_foundItem is not null)
                        yield return _foundItem;
                    if (_foundItemWithTypes is not null)
                        yield return _foundItemWithTypes;
                }
            }

            public void SetupValidatingItemCategoryId()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemCategoryId);

                ValidatorMock.SetupValidateAsync(ItemCategoryId.Value);
            }

            public void SetupItemCategoryId()
            {
                ItemCategoryId = Domain.ItemCategories.Models.ItemCategoryId.New;
            }

            public void SetupItemWithTypes()
            {
                _foundItemWithTypes = ItemMother.InitialWithTypes().Create();
            }

            public void SetupItemWithoutTypes()
            {
                _foundItem = ItemMother.Initial().Create();
            }

            public void SetupFindingItems()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemCategoryId);

                ItemRepositoryMock.SetupFindActiveByAsync(ItemCategoryId.Value, Items);
            }

            public void SetupExpectedResultWithoutTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(_foundItem);

                ExpectedResults.Add(new SearchItemByItemCategoryResult(
                    _foundItem.Id,
                    null,
                    _foundItem.Name,
                    _convertedAvailabilities[(_foundItem.Id, null)]));
            }

            public void SetupExpectedResultWithTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(_foundItemWithTypes);

                foreach (var type in _foundItemWithTypes.ItemTypes)
                {
                    ExpectedResults.Add(new SearchItemByItemCategoryResult(
                        _foundItemWithTypes.Id,
                        type.Id,
                        $"{_foundItemWithTypes.Name} {type.Name}",
                        _convertedAvailabilities[(_foundItemWithTypes.Id, type.Id)]));
                }
            }

            public void SetupConvertedAvailabilitiesWithTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(_foundItemWithTypes);

                foreach (var type in _foundItemWithTypes.ItemTypes)
                {
                    var avs = new DomainTestBuilder<ItemAvailabilityReadModel>().CreateMany(2);
                    _convertedAvailabilities.Add((_foundItemWithTypes.Id, type.Id), avs);
                }
            }

            public void SetupConvertedAvailabilitiesWithoutTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(_foundItem);

                var avs = new DomainTestBuilder<ItemAvailabilityReadModel>().CreateMany(2);
                _convertedAvailabilities.Add((_foundItem.Id, null), avs);
            }

            public void SetupConvertingAvailabilities()
            {
                AvailabilityConversionServiceMock.SetupConvertAsync(Items, _convertedAvailabilities);
            }

            public void VerifyValidatingItemCategoryId()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemCategoryId);

                ValidatorMock.VerifyValidateAsync(ItemCategoryId.Value, Times.Once);
            }
        }
    }

    private abstract class ItemSearchServiceFixture
    {
        protected readonly Fixture Fixture;
        protected readonly CommonFixture CommonFixture = new();

        protected readonly ItemRepositoryMock ItemRepositoryMock = new(MockBehavior.Strict);
        protected readonly ShoppingListRepositoryMock ShoppingListRepositoryMock = new(MockBehavior.Strict);
        protected readonly StoreRepositoryMock StoreRepositoryMock = new(MockBehavior.Strict);
        protected readonly ItemTypeReadRepositoryMock ItemTypeReadRepositoryMock = new(MockBehavior.Strict);
        protected readonly ItemSearchReadModelConversionServiceMock ConversionServiceMock = new(MockBehavior.Strict);
        protected readonly ItemTypeFactoryMock ItemTypeFactoryMock = new(MockBehavior.Strict);
        protected readonly ItemAvailabilityReadModelConversionServiceMock AvailabilityConversionServiceMock = new(MockBehavior.Strict);
        protected readonly ValidatorMock ValidatorMock = new(MockBehavior.Strict);

        protected ItemSearchServiceFixture()
        {
            Fixture = CommonFixture.GetNewFixture();
        }

        public ItemSearchService CreateSut()
        {
            return new ItemSearchService(
                ItemRepositoryMock.Object,
                ShoppingListRepositoryMock.Object,
                StoreRepositoryMock.Object,
                ItemTypeReadRepositoryMock.Object,
                ConversionServiceMock.Object,
                _ => ValidatorMock.Object,
                _ => AvailabilityConversionServiceMock.Object,
                default);
        }
    }
}