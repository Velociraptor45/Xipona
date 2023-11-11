using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Conversion;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Conversion.ItemSearchReadModels;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Ports;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Items.Services.Search;

public class ItemSearchServiceTests
{
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
        public async Task SearchAsync_WithDeletedItemWithTypes_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemCategoryId();
            _fixture.SetupValidatingItemCategoryId();
            _fixture.SetupItemWithDeletedTypes();
            _fixture.SetupFindingItems();
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

    private abstract class ItemSearchServiceFixture
    {
        protected readonly ItemRepositoryMock ItemRepositoryMock = new(MockBehavior.Strict);
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