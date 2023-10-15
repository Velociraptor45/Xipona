using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Items.Models;

public class ItemTypesTests
{
    public sealed class Update
    {
        private readonly UpdateFixture _fixture = new();

        public static IEnumerable<object?[]> GetUpdatePriceItemTypeIdCombinations()
        {
            yield return new object?[] { null, ItemTypeId.New };

            var itemTypeId = ItemTypeId.New;
            yield return new object?[] { itemTypeId, itemTypeId };
        }

        [Theory]
        [MemberData(nameof(GetUpdatePriceItemTypeIdCombinations))]
        public void Update_WithItemTypeNotAvailableAtStore_ShouldCallCorrectMethod(ItemTypeId? itemTypeIdArg,
            ItemTypeId _)
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupPrice();
            _fixture.SetupItemTypeId(itemTypeIdArg);
            _fixture.SetupItemTypeMockNotAvailableAtStore();
            _fixture.SetupCallingUpdate();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value);

            // Assert
            _fixture.VerifyCallingUpdate();
        }

        [Theory]
        [MemberData(nameof(GetUpdatePriceItemTypeIdCombinations))]
        public void Update_WithItemTypeNotAvailableAtStore_ShouldReturnExpectedResult(ItemTypeId? itemTypeIdArg,
            ItemTypeId _)
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupPrice();
            _fixture.SetupItemTypeId(itemTypeIdArg);
            _fixture.SetupItemTypeMockNotAvailableAtStore();
            _fixture.SetupCallingUpdate();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            var result = sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedItemTypes);
        }

        [Fact]
        public void Update_WithItemTypeAvailableAtStoreButTypeIdNotMatching_ShouldCallCorrectMethod()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupPrice();
            _fixture.SetupItemTypeId(ItemTypeId.New);
            _fixture.SetupItemTypeMockAvailableAtStore(ItemTypeId.New);
            _fixture.SetupCallingUpdate();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value);

            // Assert
            _fixture.VerifyCallingUpdate();
        }

        [Fact]
        public void Update_WithItemTypeAvailableAtStoreButTypeIdNotMatching_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupPrice();
            _fixture.SetupItemTypeId(ItemTypeId.New);
            _fixture.SetupItemTypeMockAvailableAtStore(ItemTypeId.New);
            _fixture.SetupCallingUpdate();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            var result = sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedItemTypes);
        }

        [Theory]
        [MemberData(nameof(GetUpdatePriceItemTypeIdCombinations))]
        public void Update_WithItemTypeAvailableAtStore_ShouldCallCorrectMethod(ItemTypeId? itemTypeIdArg,
            ItemTypeId itemTypeId)
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupPrice();
            _fixture.SetupItemTypeId(itemTypeIdArg);
            _fixture.SetupItemTypeMockAvailableAtStore(itemTypeId);
            _fixture.SetupCallingUpdateWithPrice();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value);

            // Assert
            _fixture.VerifyCallingUpdateWithPrice();
        }

        [Theory]
        [MemberData(nameof(GetUpdatePriceItemTypeIdCombinations))]
        public void Update_WithItemTypeAvailableAtStore_ShouldReturnExpectedResult(ItemTypeId? itemTypeIdArg,
            ItemTypeId itemTypeId)
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupPrice();
            _fixture.SetupItemTypeId(itemTypeIdArg);
            _fixture.SetupItemTypeMockAvailableAtStore(itemTypeId);
            _fixture.SetupCallingUpdateWithPrice();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            var result = sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedItemTypes);
        }

        private sealed class UpdateFixture
        {
            private readonly ItemTypeFactoryMock _itemTypeFactoryMock = new(MockBehavior.Strict);
            private List<ItemTypeMock>? _itemTypeMocks;
            public Price? Price { get; private set; }
            public StoreId? StoreId { get; private set; }
            public ItemTypeId? ItemTypeId { get; private set; }
            public IReadOnlyCollection<ItemType>? ExpectedItemTypes { get; private set; }

            public ItemTypes CreateSut()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemTypeMocks);

                return new ItemTypes(_itemTypeMocks.Select(m => m.Object), _itemTypeFactoryMock.Object);
            }

            public void SetupStoreId()
            {
                StoreId = Domain.Stores.Models.StoreId.New;
            }

            public void SetupItemTypeId(ItemTypeId? itemTypeId)
            {
                ItemTypeId = itemTypeId;
            }

            public void SetupPrice()
            {
                Price = new DomainTestBuilder<Price>().Create();
            }

            public void SetupItemTypeMockAvailableAtStore(ItemTypeId itemTypeId)
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                var itemType = new ItemTypeBuilder().WithId(itemTypeId).Create();
                _itemTypeMocks = new() { new ItemTypeMock(itemType, MockBehavior.Strict) };
                _itemTypeMocks.First().SetupIsAvailableAtStore(StoreId.Value, true);
            }

            public void SetupItemTypeMockNotAvailableAtStore()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                _itemTypeMocks = new() { new ItemTypeMock(new ItemTypeBuilder().Create(), MockBehavior.Strict) };
                _itemTypeMocks.First().SetupIsAvailableAtStore(StoreId.Value, false);
            }

            public void SetupCallingUpdateWithPrice()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);
                TestPropertyNotSetException.ThrowIfNull(Price);
                TestPropertyNotSetException.ThrowIfNull(_itemTypeMocks);

                ExpectedItemTypes = new ItemTypeBuilder().CreateMany(1).ToList();
                _itemTypeMocks.First().SetupUpdate(StoreId.Value, Price.Value, ExpectedItemTypes.First());
            }

            public void SetupCallingUpdate()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemTypeMocks);

                ExpectedItemTypes = new ItemTypeBuilder().CreateMany(1).ToList();
                _itemTypeMocks.First().SetupUpdate(ExpectedItemTypes.First());
            }

            public void VerifyCallingUpdateWithPrice()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);
                TestPropertyNotSetException.ThrowIfNull(Price);
                TestPropertyNotSetException.ThrowIfNull(_itemTypeMocks);

                _itemTypeMocks.First().VerifyUpdate(StoreId.Value, Price.Value, Times.Once);
            }

            public void VerifyCallingUpdate()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemTypeMocks);

                _itemTypeMocks.First().VerifyUpdate(Times.Once);
            }
        }
    }
}