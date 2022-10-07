using ProjectHermes.ShoppingList.Api.Core.TestKit.Services;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.TemporaryItems;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Shared;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System.Text.RegularExpressions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Items.Models;

public class ItemTests
{
    private readonly CommonFixture _commonFixture;

    public ItemTests()
    {
        _commonFixture = new CommonFixture();
    }

    #region Delete

    [Fact]
    public void Delete_WithNotDeletedItem_ShouldMarkItemAsDeleted()
    {
        // Arrange
        var item = ItemMother.Initial().Create();

        // Act
        item.Delete();

        // Assert
        using (new AssertionScope())
        {
            item.IsDeleted.Should().BeTrue();
        }
    }

    #endregion Delete

    #region IsAvailableInStore

    [Fact]
    public void IsAvailableInStore_WithNotAvailableInStore_ShouldReturnFalse()
    {
        // Arrange
        IItem testObject = ItemMother.Initial().Create();

        // Act
        StoreId storeId = new StoreIdBuilder().Create();
        var result = testObject.IsAvailableInStore(storeId);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeFalse();
        }
    }

    [Fact]
    public void IsAvailableInStore_WithAvailableInStore_ShouldReturnTrue()
    {
        // Arrange
        IItem testObject = ItemMother.Initial().Create();
        var availabilityStoreIds = testObject.Availabilities.Select(av => av.StoreId).ToList();

        // Act
        StoreId chosenStoreId = _commonFixture.ChooseRandom(availabilityStoreIds);
        var result = testObject.IsAvailableInStore(chosenStoreId);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeTrue();
        }
    }

    #endregion IsAvailableInStore

    #region MakePermanent

    [Fact]
    public void MakePermanent_WithValidData_ShouldMakeItemPermanent()
    {
        // Arrange
        Fixture fixture = _commonFixture.GetNewFixture();

        IItem testObject = ItemMother.Initial().Create();
        PermanentItem permanentItem = fixture.Create<PermanentItem>();
        IEnumerable<IItemAvailability> availabilities =
            ItemAvailabilityMother.Initial().CreateMany(3).ToList();

        // Act
        testObject.MakePermanent(permanentItem, availabilities);

        // Assert
        using (new AssertionScope())
        {
            testObject.Name.Should().Be(permanentItem.Name);
            testObject.Comment.Should().Be(permanentItem.Comment);
            testObject.ItemQuantity.Should().Be(permanentItem.ItemQuantity);
            testObject.Availabilities.Should().BeEquivalentTo(availabilities);
            testObject.ItemCategoryId.Should().Be(permanentItem.ItemCategoryId);
            testObject.ManufacturerId.Should().Be(permanentItem.ManufacturerId);
            testObject.IsTemporary.Should().BeFalse();
        }
    }

    #endregion MakePermanent

    #region Modify

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Modify_WithValidData_ShouldModifyItem(bool isTemporary)
    {
        // Arrange
        Fixture fixture = _commonFixture.GetNewFixture();

        IItem testObject = ItemMother.Initial().WithIsTemporary(isTemporary).Create();
        ItemModification itemModify = fixture.Create<ItemModification>();
        IEnumerable<IItemAvailability> availabilities =
            ItemAvailabilityMother.Initial().CreateMany(3).ToList();

        // Act
        testObject.Modify(itemModify, availabilities);

        // Assert
        using (new AssertionScope())
        {
            testObject.Name.Should().Be(itemModify.Name);
            testObject.Comment.Should().Be(itemModify.Comment);
            testObject.ItemQuantity.Should().Be(itemModify.ItemQuantity);
            testObject.Availabilities.Should().BeEquivalentTo(availabilities);
            testObject.ItemCategoryId.Should().Be(itemModify.ItemCategoryId);
            testObject.ManufacturerId.Should().Be(itemModify.ManufacturerId);
            testObject.IsTemporary.Should().Be(isTemporary);
        }
    }

    #endregion Modify

    #region GetDefaultSectionIdForStore

    [Fact]
    public void GetDefaultSectionIdForStore_WithInvalidStoreId_ShouldThrowDomainException()
    {
        // Arrange
        IItem testObject = ItemMother.Initial().Create();
        var requestStoreId = new StoreIdBuilder().Create();

        // Act
        Action action = () => testObject.GetDefaultSectionIdForStore(requestStoreId);

        // Assert
        using (new AssertionScope())
        {
            action.Should().Throw<DomainException>()
                .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ItemAtStoreNotAvailable);
        }
    }

    [Fact]
    public void GetDefaultSectionIdForStore_WithValidStoreId_ShouldReturnSectionId()
    {
        // Arrange
        IItem testObject = ItemMother.Initial().Create();
        var chosenAvailability = _commonFixture.ChooseRandom(testObject.Availabilities);

        // Act
        var result = testObject.GetDefaultSectionIdForStore(chosenAvailability.StoreId);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(chosenAvailability.DefaultSectionId);
        }
    }

    #endregion GetDefaultSectionIdForStore

    #region SetPredecessor

    [Fact]
    public void SetPredecessor_WithValidPredecessor_ShouldSetPredecessor()
    {
        // Arrange
        IItem testObject = ItemMother.Initial().Create();
        IItem predecessor = ItemMother.Initial().Create();

        // Act
        testObject.SetPredecessor(predecessor);

        // Assert
        using (new AssertionScope())
        {
            testObject.Predecessor.Should().BeEquivalentTo(predecessor);
        }
    }

    #endregion SetPredecessor

    public class UpdateAsync_WithoutTypes
    {
        private readonly UpdateAsyncFixture _fixture;

        public UpdateAsync_WithoutTypes()
        {
            _fixture = new UpdateAsyncFixture();
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupNotDeleted();
            _fixture.SetupNotTemporary();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedItem(sut);
            _fixture.SetupItemUpdate();
            _fixture.SetupValidatingItemCategorySuccess();
            _fixture.SetupValidatingManufacturerSuccess();
            _fixture.SetupValidatingAvailabilitiesSuccess();
            _fixture.SetupDateTimeServiceReturningExpectedUpdatedOn();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemUpdate);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItem);

            // Act
            var result = await sut.UpdateAsync(_fixture.ItemUpdate, _fixture.ValidatorMock.Object,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedItem, opt => opt.Excluding(info => info.Path == "Id"));
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldDeleteOldItem()
        {
            // Arrange
            _fixture.SetupNotDeleted();
            _fixture.SetupNotTemporary();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedItem(sut);
            _fixture.SetupItemUpdate();
            _fixture.SetupValidatingItemCategorySuccess();
            _fixture.SetupValidatingManufacturerSuccess();
            _fixture.SetupValidatingAvailabilitiesSuccess();
            _fixture.SetupDateTimeServiceReturningExpectedUpdatedOn();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemUpdate);

            // Act
            await sut.UpdateAsync(_fixture.ItemUpdate, _fixture.ValidatorMock.Object,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            sut.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldSetUpdateOnAtOldItem()
        {
            // Arrange
            _fixture.SetupNotDeleted();
            _fixture.SetupNotTemporary();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedItem(sut);
            _fixture.SetupItemUpdate();
            _fixture.SetupValidatingItemCategorySuccess();
            _fixture.SetupValidatingManufacturerSuccess();
            _fixture.SetupValidatingAvailabilitiesSuccess();
            _fixture.SetupDateTimeServiceReturningExpectedUpdatedOn();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemUpdate);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedUpdatedOn);

            // Act
            await sut.UpdateAsync(_fixture.ItemUpdate, _fixture.ValidatorMock.Object,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            sut.UpdatedOn.Should().Be(_fixture.ExpectedUpdatedOn);
        }

        private sealed class UpdateAsyncFixture : UpdateFixture
        {
            public ItemUpdate? ItemUpdate { get; private set; }
            public IItem? ExpectedItem { get; private set; }

            public void SetupNotDeleted()
            {
                Builder.WithIsDeleted(false);
            }

            public void SetupNotTemporary()
            {
                Builder.WithIsTemporary(false);
            }

            public void SetupExpectedItem(IItem sut)
            {
                ExpectedItem = ItemMother.Initial().Create();
                ExpectedItem.SetPredecessor(sut);
            }

            public void SetupItemUpdate()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedItem);
                TestPropertyNotSetException.ThrowIfNull(ExpectedItem.ItemCategoryId);
                TestPropertyNotSetException.ThrowIfNull(ExpectedItem.ManufacturerId);

                ItemUpdate = new ItemUpdate(
                    ItemId.New,
                    ExpectedItem.Name,
                    ExpectedItem.Comment,
                    ExpectedItem.ItemQuantity,
                    ExpectedItem.ItemCategoryId.Value,
                    ExpectedItem.ManufacturerId.Value,
                    ExpectedItem.Availabilities);
            }

            public void SetupValidatingItemCategorySuccess()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemUpdate);

                SetupValidatingItemCategorySuccess(ItemUpdate.ItemCategoryId);
            }

            public void SetupValidatingManufacturerSuccess()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemUpdate);
                TestPropertyNotSetException.ThrowIfNull(ItemUpdate.ManufacturerId);

                SetupValidatingManufacturerSuccess(ItemUpdate.ManufacturerId.Value);
            }

            public void SetupValidatingAvailabilitiesSuccess()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemUpdate);

                SetupValidatingAvailabilitiesSuccess(ItemUpdate.Availabilities);
            }
        }
    }

    public class Update_Price
    {
        private readonly UpdateFixture_Price _fixture;

        public Update_Price()
        {
            _fixture = new UpdateFixture_Price();
        }

        [Fact]
        public void Update_WithItemWithoutTypes_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupAsItemWithoutTypes();
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupPrice();
            _fixture.SetupItemAvailableAtStore();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);
            _fixture.SetupDateTimeServiceReturningExpectedUpdatedOn();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            var result = sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.Excluding(info => info.Path == "Id"));
            result.Id.Should().NotBe(sut.Id);
        }

        [Fact]
        public void Update_WithItemWithoutTypes_ShouldDeleteItem()
        {
            // Arrange
            _fixture.SetupAsItemWithoutTypes();
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupPrice();
            _fixture.SetupItemAvailableAtStore();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);
            _fixture.SetupDateTimeServiceReturningExpectedUpdatedOn();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            sut.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public void Update_WithItemWithoutTypes_ShouldSetUpdatedOn()
        {
            // Arrange
            _fixture.SetupAsItemWithoutTypes();
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupPrice();
            _fixture.SetupItemAvailableAtStore();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);
            _fixture.SetupDateTimeServiceReturningExpectedUpdatedOn();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedUpdatedOn);

            // Act
            sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            sut.UpdatedOn.Should().NotBeNull();
            sut.UpdatedOn.Should().Be(_fixture.ExpectedUpdatedOn.Value);
        }

        [Fact]
        public void Update_WithItemWithoutTypesAndInStoreNotAvailable_ShouldThrow()
        {
            // Arrange
            _fixture.SetupAsItemWithoutTypes();
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupPrice();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            var func = () => sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.ItemAtStoreNotAvailable);
        }

        [Fact]
        public void Update_WithItemWithTypesAndItemTypeIdNull_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupAsItemWithTypes();
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupPrice();
            _fixture.SetupItemTypeAvailableAtStore();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultWithAllTypesUpdated(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            var result = sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.Excluding(info =>
                    info.Path == "Id"
                    || Regex.IsMatch(info.Path, @"ItemTypes\[\d+\].Id")));

            result.Id.Should().NotBe(sut.Id);
            var newItemTypeIds = result.ItemTypes.Select(t => t.Id).ToList();
            foreach (var existingItemTypeId in sut.ItemTypes.Select(t => t.Id))
            {
                newItemTypeIds.Should().NotContain(existingItemTypeId);
            }
        }

        [Fact]
        public void Update_WithItemWithTypesAndItemTypeIdNotNull_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupAsItemWithTypes();
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeId();
            _fixture.SetupPrice();
            _fixture.SetupItemTypeAvailableAtStore();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultWithOneTypeUpdated(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            var result = sut.Update(_fixture.StoreId.Value, _fixture.ItemTypeId, _fixture.Price.Value,
                _fixture.DateTimeServiceMock.Object);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.Excluding(info =>
                    info.Path == "Id"
                    || Regex.IsMatch(info.Path, @"ItemTypes\[\d+\].Id")));

            result.Id.Should().NotBe(sut.Id);
            var newItemTypeIds = result.ItemTypes.Select(t => t.Id).ToList();
            foreach (var existingItemTypeId in sut.ItemTypes.Select(t => t.Id))
            {
                newItemTypeIds.Should().NotContain(existingItemTypeId);
            }
        }

        private sealed class UpdateFixture_Price : UpdateFixture
        {
            private readonly ItemTypeFactoryMock _itemTypeFactoryMock = new(MockBehavior.Strict);

            public Price? Price { get; private set; }
            public StoreId? StoreId { get; private set; }
            public ItemTypeId? ItemTypeId { get; private set; }

            public void SetupAsItemWithTypes()
            {
                ItemMother.InitialWithTypes(Builder);
            }

            public void SetupAsItemWithoutTypes()
            {
                ItemMother.Initial(Builder);
            }

            public void SetupItemAvailableAtStore()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);
                Builder.WithAvailability(ItemAvailabilityMother.ForStore(StoreId.Value).Create());
            }

            public void SetupItemTypeAvailableAtStore()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);
                var type1 = new ItemTypeBuilder()
                    .WithAvailability(ItemAvailabilityMother.ForStore(StoreId.Value).Create())
                    .WithId(ItemTypeId ?? Domain.Items.Models.ItemTypeId.New)
                    .Create();
                var type2 = new ItemTypeBuilder()
                    .Create();

                Builder.WithTypes(new ItemTypes(new List<IItemType> { type1, type2 }, _itemTypeFactoryMock.Object));
            }

            public void SetupStoreId()
            {
                StoreId = Domain.Stores.Models.StoreId.New;
            }

            public void SetupItemTypeId()
            {
                ItemTypeId = Domain.Items.Models.ItemTypeId.New;
            }

            public void SetupItemTypeIdNull()
            {
                ItemTypeId = null;
            }

            public void SetupPrice()
            {
                Price = new DomainTestBuilder<Price>().Create();
            }

            public void SetupExpectedResult(Item item)
            {
                TestPropertyNotSetException.ThrowIfNull(Price);

                ExpectedResult = new Item(
                    ItemId.New,
                    item.Name,
                    false,
                    item.Comment,
                    item.IsTemporary,
                    item.ItemQuantity,
                    item.ItemCategoryId,
                    item.ManufacturerId,
                    item.Availabilities.Select(av => new ItemAvailability(av.StoreId, Price.Value, av.DefaultSectionId)),
                    item.TemporaryId,
                    null);
                ExpectedResult.SetPredecessor(item);
            }

            public void SetupExpectedResultWithAllTypesUpdated(Item item)
            {
                TestPropertyNotSetException.ThrowIfNull(Price);
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                ExpectedResult = new Item(
                    ItemId.New,
                    item.Name,
                    false,
                    item.Comment,
                    item.ItemQuantity,
                    item.ItemCategoryId!.Value,
                    item.ManufacturerId,
                    new ItemTypes(item.ItemTypes.Select(t =>
                        {
                            var type = new ItemType(
                                Domain.Items.Models.ItemTypeId.New,
                                t.Name,
                                t.Availabilities.Select(av => av.StoreId == StoreId
                                    ? new ItemAvailability(StoreId.Value, Price.Value, av.DefaultSectionId)
                                    : av));
                            type.SetPredecessor(t);
                            return type;
                        }),
                        _itemTypeFactoryMock.Object),
                    null);

                ExpectedResult.SetPredecessor(item);
            }

            public void SetupExpectedResultWithOneTypeUpdated(Item item)
            {
                TestPropertyNotSetException.ThrowIfNull(Price);
                TestPropertyNotSetException.ThrowIfNull(StoreId);
                TestPropertyNotSetException.ThrowIfNull(ItemTypeId);

                ExpectedResult = new Item(
                    ItemId.New,
                    item.Name,
                    false,
                    item.Comment,
                    item.ItemQuantity,
                    item.ItemCategoryId!.Value,
                    item.ManufacturerId,
                    new ItemTypes(item.ItemTypes.Select(t =>
                        {
                            var type = new ItemType(
                                Domain.Items.Models.ItemTypeId.New,
                                t.Name,
                                t.Availabilities.Select(av => t.Id == ItemTypeId.Value
                                    ? new ItemAvailability(StoreId.Value, Price.Value, av.DefaultSectionId)
                                    : av));
                            type.SetPredecessor(t);
                            return type;
                        }),
                        _itemTypeFactoryMock.Object),
                    null);
                ExpectedResult.SetPredecessor(item);
            }
        }
    }

    private abstract class UpdateFixture : ItemFixture
    {
        public ValidatorMock ValidatorMock { get; } = new(MockBehavior.Strict);
        public DateTimeServiceMock DateTimeServiceMock { get; } = new(MockBehavior.Strict);
        public Item? ExpectedResult { get; protected set; }
        public DateTimeOffset? ExpectedUpdatedOn { get; private set; }

        protected void SetupValidatingItemCategorySuccess(ItemCategoryId itemCategoryId)
        {
            ValidatorMock.SetupValidateAsync(itemCategoryId);
        }

        protected void SetupValidatingManufacturerSuccess(ManufacturerId manufacturerId)
        {
            ValidatorMock.SetupValidateAsync(manufacturerId);
        }

        protected void SetupValidatingAvailabilitiesSuccess(IEnumerable<IItemAvailability> availabilities)
        {
            ValidatorMock.SetupValidateAsync(availabilities);
        }

        public void SetupDateTimeServiceReturningExpectedUpdatedOn()
        {
            ExpectedUpdatedOn = DateTimeOffset.UtcNow;
            DateTimeServiceMock.SetupUtcNow(ExpectedUpdatedOn.Value);
        }
    }

    private abstract class ItemFixture
    {
        protected readonly ItemBuilder Builder = new();

        public Item CreateSut()
        {
            return Builder.Create();
        }
    }
}