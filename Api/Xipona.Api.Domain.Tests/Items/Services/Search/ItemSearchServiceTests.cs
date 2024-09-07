using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.Conversion;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.Conversion.ItemSearchReadModels;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.Xipona.Api.Domain.TestKit.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Manufacturers.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Stores.Ports;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.Items.Services.Search;

public class ItemSearchServiceTests
{
    public class SearchAsync
    {
        private readonly SearchAsyncFixture _fixture = new();

        [Fact]
        public async Task SearchAsync_WithSearchInputEmpty_ShouldReturnEmptyResult()
        {
            // Arrange
            _fixture.SetupSearchInputEmpty();
            _fixture.SetupPage();
            _fixture.SetupPageSize();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.SearchInput);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Page);
            TestPropertyNotSetException.ThrowIfNull(_fixture.PageSize);

            // Act
            var result = await sut.SearchAsync(_fixture.SearchInput, _fixture.Page.Value, _fixture.PageSize.Value);

            // Assert
            result.Should().BeEmpty();
        }

        private sealed class SearchAsyncFixture : ItemSearchServiceFixture
        {
            public string? SearchInput { get; private set; }
            public int? Page { get; private set; }
            public int? PageSize { get; private set; }

            public void SetupSearchInputEmpty()
            {
                SearchInput = string.Empty;
            }

            public void SetupPage()
            {
                Page = new TestBuilder<int>().Create();
            }

            public void SetupPageSize()
            {
                PageSize = new TestBuilder<int>().Create();
            }
        }
    }

    public class SearchForShoppingListAsync
    {
        private readonly SearchForShoppingListAsyncFixture _fixture = new();

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

        private sealed class SearchForShoppingListAsyncFixture : ItemSearchServiceFixture
        {
            public string Name { get; private set; } = string.Empty;
            public StoreId StoreId { get; private set; }

            public void SetupName()
            {
                Name = new TestBuilder<string>().Create();
            }

            public void SetupStoreId()
            {
                StoreId = StoreId.New;
            }

            public void SetupParameters()
            {
                SetupName();
                SetupStoreId();
            }

            public void SetupNotFindingStore()
            {
                StoreRepositoryMock.SetupFindActiveByAsync(StoreId, null);
            }
        }
    }

    public sealed class SearchAsync_ItemCategoryId
    {
        private readonly SearchAsyncFixture _fixture = new();

        [Fact]
        public async Task SearchAsync_WithItem_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemCategoryId();
            _fixture.SetupValidatingItemCategoryId();
            _fixture.SetupItemWithoutTypes();
            _fixture.SetupFindingItems();
            _fixture.SetupManufacturerForItem();
            _fixture.SetupFindingManufacturers();
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
            _fixture.SetupManufacturerForItem();
            _fixture.SetupFindingManufacturers();
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
            _fixture.SetupManufacturerForItemWithTypes();
            _fixture.SetupFindingManufacturers();
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
            _fixture.SetupManufacturerForItemWithTypes();
            _fixture.SetupFindingManufacturers();
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
        public async Task SearchAsync_WithDeletedItemWithTypes_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemCategoryId();
            _fixture.SetupValidatingItemCategoryId();
            _fixture.SetupItemWithDeletedTypes();
            _fixture.SetupFindingItems();
            _fixture.SetupManufacturerForItemWithTypes();
            _fixture.SetupFindingManufacturers();
            _fixture.SetupConvertedAvailabilitiesWithTypes();
            _fixture.SetupConvertingAvailabilities();
            _fixture.SetupExpectedResultWithDeletedTypes();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCategoryId);

            // Act
            var results = await sut.SearchAsync(_fixture.ItemCategoryId.Value);

            // Assert
            results.Should().BeEquivalentTo(_fixture.ExpectedResults);
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
            _fixture.SetupManufacturerForItem();
            _fixture.SetupManufacturerForItemWithTypes();
            _fixture.SetupFindingManufacturers();
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
            _fixture.SetupManufacturerForItem();
            _fixture.SetupManufacturerForItemWithTypes();
            _fixture.SetupFindingManufacturers();
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

            private readonly List<Manufacturer> _manufacturers = new();

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

            public void SetupItemWithDeletedTypes()
            {
                _foundItemWithTypes = ItemMother
                    .InitialWithTypes()
                    .WithTypes(new ItemTypes(
                        new ItemTypeBuilder().WithIsDeleted(true).CreateMany(2),
                        ItemTypeFactoryMock.Object))
                    .Create();
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

            public void SetupManufacturerForItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_foundItem);
                _manufacturers.Add(new ManufacturerBuilder().WithId(_foundItem.ManufacturerId!.Value).Create());
            }

            public void SetupManufacturerForItemWithTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(_foundItemWithTypes);
                _manufacturers.Add(new ManufacturerBuilder().WithId(_foundItemWithTypes.ManufacturerId!.Value).Create());
            }

            public void SetupFindingManufacturers()
            {
                ManufacturerRepositoryMock.SetupFindByAsync(_manufacturers.Select(m => m.Id), _manufacturers);
            }

            public void SetupExpectedResultWithoutTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(_foundItem);

                ExpectedResults.Add(new SearchItemByItemCategoryResult(
                    _foundItem.Id,
                    null,
                    _foundItem.Name,
                    _manufacturers.Single(m => m.Id == _foundItem.ManufacturerId).Name,
                    _convertedAvailabilities[(_foundItem.Id, null)].ToList()));
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
                        _manufacturers.Single(m => m.Id == _foundItemWithTypes.ManufacturerId).Name,
                        _convertedAvailabilities[(_foundItemWithTypes.Id, type.Id)].ToList()));
                }
            }

            public void SetupExpectedResultWithDeletedTypes()
            {
                ExpectedResults.Clear();
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

    public sealed class GetTotalSearchResultCountAsync
    {
        private readonly GetTotalSearchResultCountAsyncFixture _fixture = new();

        [Fact]
        public async Task GetTotalSearchResultCountAsync_WithSearchInputEmpty_ShouldReturnZero()
        {
            // Arrange
            _fixture.SetupExpectedResultZero();
            _fixture.SetupSearchInputEmpty();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.SearchInput);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await sut.GetTotalSearchResultCountAsync(_fixture.SearchInput);

            // Assert
            result.Should().Be(_fixture.ExpectedResult);
        }

        [Fact]
        public async Task GetTotalSearchResultCountAsync_WithValidSearchInput_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupSearchInput();
            _fixture.SetupFindingItems();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.SearchInput);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await sut.GetTotalSearchResultCountAsync(_fixture.SearchInput);

            // Assert
            result.Should().Be(_fixture.ExpectedResult);
        }

        private sealed class GetTotalSearchResultCountAsyncFixture : ItemSearchServiceFixture
        {
            public string? SearchInput { get; private set; }
            public int? ExpectedResult { get; private set; }

            public void SetupSearchInputEmpty()
            {
                SearchInput = string.Empty;
            }

            public void SetupSearchInput()
            {
                SearchInput = new TestBuilder<string>().Create();
            }

            public void SetupFindingItems()
            {
                TestPropertyNotSetException.ThrowIfNull(SearchInput);
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                ItemRepositoryMock.SetupGetTotalCountByAsync(SearchInput, ExpectedResult.Value);
            }

            public void SetupExpectedResult()
            {
                ExpectedResult = new TestBuilder<int>().Create();
            }

            public void SetupExpectedResultZero()
            {
                ExpectedResult = 0;
            }
        }
    }

    private abstract class ItemSearchServiceFixture
    {
        protected readonly ItemRepositoryMock ItemRepositoryMock = new(MockBehavior.Strict);
        protected readonly ManufacturerRepositoryMock ManufacturerRepositoryMock = new(MockBehavior.Strict);
        protected readonly ShoppingListRepositoryMock ShoppingListRepositoryMock = new(MockBehavior.Strict);
        protected readonly StoreRepositoryMock StoreRepositoryMock = new(MockBehavior.Strict);
        protected readonly ItemTypeReadRepositoryMock ItemTypeReadRepositoryMock = new(MockBehavior.Strict);
        protected readonly ItemCategoryRepositoryMock ItemCategoryRepositoryMock = new(MockBehavior.Strict);
        protected readonly ItemSearchReadModelConversionServiceMock ConversionServiceMock = new(MockBehavior.Strict);
        protected readonly ItemTypeFactoryMock ItemTypeFactoryMock = new(MockBehavior.Strict);
        protected readonly ItemAvailabilityReadModelConversionServiceMock AvailabilityConversionServiceMock = new(MockBehavior.Strict);
        protected readonly ValidatorMock ValidatorMock = new(MockBehavior.Strict);

        public ItemSearchService CreateSut()
        {
            return new ItemSearchService(
                ItemRepositoryMock.Object,
                ManufacturerRepositoryMock.Object,
                ShoppingListRepositoryMock.Object,
                StoreRepositoryMock.Object,
                ItemTypeReadRepositoryMock.Object,
                ItemCategoryRepositoryMock.Object,
                ConversionServiceMock.Object,
                ValidatorMock.Object,
                AvailabilityConversionServiceMock.Object);
        }
    }
}