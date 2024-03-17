using Force.DeepCloner;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.Recipes.Models;

public class IngredientTests
{
    public class Modify
    {
        private readonly ModifyFixture _fixture = new();

        [Fact]
        public async Task Modify_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupExpectedResult();
            _fixture.SetupModification();
            _fixture.SetupItemCategoryValidationSuccess();
            _fixture.SetupItemValidationSuccess();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

            // Act
            var result = await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public async Task Modify_WithInvalidItemCategory_ShouldThrow()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupExpectedResult();
            _fixture.SetupModification();
            _fixture.SetupItemCategoryValidationFailure();
            _fixture.SetupItemValidationSuccess();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedException);

            // Act
            var func = async () => await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            await func.Should().ThrowExactlyAsync<InvalidOperationException>()
                .WithMessage(_fixture.ExpectedException.Message);
        }

        [Fact]
        public async Task Modify_WithInvalidItemId_ShouldThrow()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupExpectedResult();
            _fixture.SetupModification();
            _fixture.SetupItemCategoryValidationSuccess();
            _fixture.SetupItemValidationFailure();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedException);

            // Act
            var func = async () => await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            await func.Should().ThrowExactlyAsync<InvalidOperationException>()
                .WithMessage(_fixture.ExpectedException.Message);
        }

        [Fact]
        public async Task Modify_WithoutDefaultItem_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupExpectedResultWithoutDefaultItem();
            _fixture.SetupModification();
            _fixture.SetupItemCategoryValidationSuccess();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

            // Act
            var result = await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class ModifyFixture : IngredientFixture
        {
            public ValidatorMock ValidatorMock { get; } = new(MockBehavior.Strict);
            public Ingredient? ExpectedResult { get; private set; }
            public IngredientModification? Modification { get; private set; }
            public InvalidOperationException? ExpectedException { get; private set; }

            public void SetupExpectedResult()
            {
                TestPropertyNotSetException.ThrowIfNull(Id);

                ExpectedResult = new IngredientBuilder()
                    .WithId(Id.Value)
                    .Create();
            }

            public void SetupExpectedResultWithoutDefaultItem()
            {
                TestPropertyNotSetException.ThrowIfNull(Id);

                ExpectedResult = new IngredientBuilder()
                    .WithId(Id.Value)
                    .WithoutShoppingListProperties()
                    .Create();
            }

            public void SetupModification()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                Modification = new IngredientModification(
                    ExpectedResult.Id,
                    ExpectedResult.ItemCategoryId,
                    ExpectedResult.QuantityType,
                    ExpectedResult.Quantity,
                    ExpectedResult.ShoppingListProperties);
            }

            public void SetupItemCategoryValidationSuccess()
            {
                TestPropertyNotSetException.ThrowIfNull(Modification);

                ValidatorMock.SetupValidateAsync(Modification.ItemCategoryId);
            }

            public void SetupItemValidationSuccess()
            {
                TestPropertyNotSetException.ThrowIfNull(Modification);
                TestPropertyNotSetException.ThrowIfNull(Modification.ShoppingListProperties);

                ValidatorMock.SetupValidateAsync(Modification.ShoppingListProperties.DefaultItemId,
                    Modification.ShoppingListProperties.DefaultItemTypeId);
            }

            public void SetupItemCategoryValidationFailure()
            {
                TestPropertyNotSetException.ThrowIfNull(Modification);

                ExpectedException = new InvalidOperationException("injected item category");

                ValidatorMock.SetupValidateAsyncAnd(Modification.ItemCategoryId)
                    .Throws(ExpectedException);
            }

            public void SetupItemValidationFailure()
            {
                TestPropertyNotSetException.ThrowIfNull(Modification);
                TestPropertyNotSetException.ThrowIfNull(Modification.ShoppingListProperties);

                ExpectedException = new InvalidOperationException("injected item");

                ValidatorMock.SetupValidateAsyncAnd(Modification.ShoppingListProperties.DefaultItemId,
                        Modification.ShoppingListProperties.DefaultItemTypeId)
                    .Throws(ExpectedException);
            }
        }
    }

    public class ChangeDefaultItem
    {
        private readonly ChangeDefaultItemFixture _fixture = new();

        [Fact]
        public void ChangeDefaultItem_WithNoType_WithDifferentStore_ShouldUpdateItemIdAndStoreId()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupDefaultItemId();
            _fixture.SetupNewItem();
            _fixture.SetupNewExpectedStoreId();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.ChangeDefaultItem(_fixture.NewItem.PredecessorId!.Value, _fixture.NewItem);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt.Excluding(info => info.Path == "Id"));
        }

        [Fact]
        public void ChangeDefaultItem_WithNoType_WithSameStore_ShouldUpdateItemId()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupDefaultItemId();
            _fixture.SetupOldExpectedStoreId();
            _fixture.SetupNewItem();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.ChangeDefaultItem(_fixture.NewItem.PredecessorId!.Value, _fixture.NewItem);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt.Excluding(info => info.Path == "Id"));
        }

        [Fact]
        public void ChangeDefaultItem_WithType_WithDifferentStore_ShouldUpdateItemAndItemTypeId()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupDefaultItemId();
            _fixture.SetupDefaultItemTypeId();
            _fixture.SetupNewTypeId();
            _fixture.SetupNewItemWithTypes();
            _fixture.SetupNewTypeExpectedStoreId();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultWithType(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.ChangeDefaultItem(_fixture.NewItem.PredecessorId!.Value, _fixture.NewItem);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt.Excluding(info => info.Path == "Id"));
        }

        [Fact]
        public void ChangeDefaultItem_WithType_WithSameStore_ShouldUpdateItemAndItemTypeId()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupDefaultItemId();
            _fixture.SetupDefaultItemTypeId();
            _fixture.SetupNewTypeId();
            _fixture.SetupOldExpectedStoreId();
            _fixture.SetupNewItemWithTypes();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultWithType(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.ChangeDefaultItem(_fixture.NewItem.PredecessorId!.Value, _fixture.NewItem);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt.Excluding(info => info.Path == "Id"));
        }

        [Fact]
        public void ChangeDefaultItem_WithTypeRemoved_ShouldRemoveShoppingListProperties()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupDefaultItemId();
            _fixture.SetupDefaultItemTypeId();
            _fixture.SetupNewItemWithRemovedType();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultWithRemovedType(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.ChangeDefaultItem(_fixture.NewItem.PredecessorId!.Value, _fixture.NewItem);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt.Excluding(info => info.Path == "Id"));
        }

        [Fact]
        public void ChangeDefaultItem_WithNewItemNotMatching_ShouldReturnOriginal()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupDefaultItemId();
            _fixture.SetupNotMatchingNewItem();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultAsOriginal(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.ChangeDefaultItem(_fixture.NewItem.PredecessorId!.Value, _fixture.NewItem);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void ChangeDefaultItem_WithNoShoppingListProperties_ShouldReturnOriginal()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupShoppingListPropertiesNull();
            _fixture.SetupNotMatchingNewItem();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultAsOriginal(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.ChangeDefaultItem(_fixture.NewItem.PredecessorId!.Value, _fixture.NewItem);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class ChangeDefaultItemFixture : IngredientFixture
        {
            private ItemTypeId? _newTypeId;
            private StoreId? _expectedStoreId;
            public Item? NewItem { get; private set; }
            public Ingredient? ExpectedResult { get; private set; }

            public void SetupNewItem()
            {
                TestPropertyNotSetException.ThrowIfNull(DefaultItemId);

                var availabilities = ItemAvailabilityMother.ForStore(DefaultStoreId ?? StoreId.New).CreateMany(1);

                NewItem = ItemMother.Initial()
                    .WithAvailabilities(availabilities)
                    .WithPredecessorId(DefaultItemId.Value)
                    .Create();
            }

            public void SetupNotMatchingNewItem()
            {
                NewItem = ItemMother.Initial().WithPredecessorId(ItemId.New).Create();
            }

            public void SetupNewItemWithTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(DefaultItemId);
                TestPropertyNotSetException.ThrowIfNull(DefaultItemTypeId);
                TestPropertyNotSetException.ThrowIfNull(_newTypeId);

                var availabilities = ItemAvailabilityMother.ForStore(DefaultStoreId ?? StoreId.New).CreateMany(1);

                var types = new List<IItemType>
                {
                    ItemTypeMother.Initial().Create(),
                    ItemTypeMother.Initial().WithId(_newTypeId.Value)
                        .WithAvailabilities(availabilities)
                        .WithPredecessorId(DefaultItemTypeId.Value)
                        .Create(),
                    ItemTypeMother.Initial().Create()
                };

                NewItem = ItemMother.InitialWithTypes()
                    .WithTypes(new ItemTypes(types, new ItemTypeFactoryMock(MockBehavior.Strict).Object))
                    .WithPredecessorId(DefaultItemId.Value)
                    .Create();
            }

            public void SetupNewItemWithRemovedType()
            {
                TestPropertyNotSetException.ThrowIfNull(DefaultItemId);

                var types = new List<IItemType>
                {
                    ItemTypeMother.Initial().Create(),
                    ItemTypeMother.Initial().Create(),
                    ItemTypeMother.Initial().Create()
                };

                NewItem = ItemMother.InitialWithTypes()
                    .WithTypes(new ItemTypes(types, new ItemTypeFactoryMock(MockBehavior.Strict).Object))
                    .WithPredecessorId(DefaultItemId.Value)
                    .Create();
            }

            public void SetupNewTypeId()
            {
                _newTypeId = ItemTypeId.New;
            }

            public void SetupOldExpectedStoreId()
            {
                SetupDefaultStoreId();
                _expectedStoreId = DefaultStoreId;
            }

            public void SetupNewExpectedStoreId()
            {
                TestPropertyNotSetException.ThrowIfNull(NewItem);
                _expectedStoreId = NewItem.Availabilities.First().StoreId;
            }

            public void SetupNewTypeExpectedStoreId()
            {
                TestPropertyNotSetException.ThrowIfNull(NewItem);
                _expectedStoreId = NewItem.ItemTypes.ElementAt(1).Availabilities.First().StoreId;
            }

            public void SetupExpectedResult(IIngredient sut)
            {
                TestPropertyNotSetException.ThrowIfNull(NewItem);
                TestPropertyNotSetException.ThrowIfNull(_expectedStoreId);

                ExpectedResult = new Ingredient(
                    IngredientId.New,
                    sut.ItemCategoryId,
                    sut.QuantityType,
                    sut.Quantity,
                    new IngredientShoppingListProperties(
                        NewItem.Id, null, _expectedStoreId.Value,
                        sut.ShoppingListProperties!.AddToShoppingListByDefault));
            }

            public void SetupExpectedResultWithType(IIngredient sut)
            {
                TestPropertyNotSetException.ThrowIfNull(NewItem);
                TestPropertyNotSetException.ThrowIfNull(_newTypeId);
                TestPropertyNotSetException.ThrowIfNull(_expectedStoreId);

                ExpectedResult = new Ingredient(
                    IngredientId.New,
                    sut.ItemCategoryId,
                    sut.QuantityType,
                    sut.Quantity,
                    new IngredientShoppingListProperties(
                        NewItem.Id, _newTypeId.Value, _expectedStoreId.Value,
                        sut.ShoppingListProperties!.AddToShoppingListByDefault));
            }

            public void SetupExpectedResultWithRemovedType(IIngredient sut)
            {
                ExpectedResult = new Ingredient(
                    IngredientId.New,
                    sut.ItemCategoryId,
                    sut.QuantityType,
                    sut.Quantity,
                    null);
            }

            public void SetupExpectedResultAsOriginal(Ingredient sut)
            {
                ExpectedResult = sut.DeepClone();
            }
        }
    }

    public class ChangeDefaultStore
    {
        private readonly ChangeDefaultStoreFixture _fixture = new();

        [Fact]
        public void ChangeDefaultStore_WithNoShoppingListProperties_ShouldThrow()
        {
            // Arrange
            _fixture.SetupItemWithoutTypes();
            _fixture.SetupShoppingListPropertiesNull();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            var func = () => sut.ChangeDefaultStore(_fixture.Item);

            // Assert
            func.Should()
                .ThrowDomainException(ErrorReasonCode.CannotChangeStoreOfIngredientWithoutShoppingListProperties);
        }

        [Fact]
        public void ChangeDefaultStore_WithNoTypes_ShouldUpdateStoreId()
        {
            // Arrange
            _fixture.SetupNoDefaultItemTypeId();
            _fixture.SetupItemWithoutTypes();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.ChangeDefaultStore(_fixture.Item);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void ChangeDefaultStore_WithTypes_ShouldUpdateStoreId()
        {
            // Arrange
            _fixture.SetupDefaultItemTypeId();
            _fixture.SetupItemWithTypes();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.ChangeDefaultStore(_fixture.Item);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class ChangeDefaultStoreFixture : IngredientFixture
        {
            private ItemAvailability? _availability;
            public Item? Item { get; private set; }
            public Ingredient? ExpectedResult { get; private set; }

            public void SetupItemWithoutTypes()
            {
                _availability = ItemAvailabilityMother.Initial().Create();
                Item = ItemMother.Initial().WithAvailability(_availability).Create();
            }

            public void SetupItemWithTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(DefaultItemTypeId);

                _availability = ItemAvailabilityMother.Initial().Create();
                var types = new List<IItemType>
                {
                    ItemTypeMother.Initial().WithId(DefaultItemTypeId.Value).WithAvailability(_availability).Create()
                };

                Item = ItemMother.InitialWithTypes()
                    .WithTypes(new ItemTypes(types, new ItemTypeFactoryMock(MockBehavior.Strict).Object))
                    .Create();
            }

            public void SetupExpectedResult(Ingredient sut)
            {
                TestPropertyNotSetException.ThrowIfNull(_availability);

                var props = new IngredientShoppingListPropertiesBuilder(sut.ShoppingListProperties!)
                    .WithDefaultStoreId(_availability.StoreId)
                    .Create();
                ExpectedResult = new IngredientBuilder(sut)
                    .WithShoppingListProperties(props)
                    .Create();
            }
        }
    }

    public class ModifyAfterAvailabilitiesChanged
    {
        private readonly ModifyAfterAvailabilitiesChangedFixture _fixture = new();

        [Fact]
        public void ModifyAfterAvailabilitiesChanged_WithPropertiesNull_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupShoppingListPropertiesNull();
            _fixture.SetupNewAvailabilitiesNotContainingOldStore();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultForNotChangingStore(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewAvailabilities);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.ModifyAfterAvailabilitiesChanged(_fixture.NewAvailabilities);

            // Assert
            result.Should().Be(sut);
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void ModifyAfterAvailabilitiesChanged_WithNewAvailabilitiesContainingOldStore_ShouldNotUpdateStoreId()
        {
            // Arrange
            _fixture.SetupDefaultStoreId();
            _fixture.SetupNewAvailabilitiesContainingOldStore();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultForNotChangingStore(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewAvailabilities);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.ModifyAfterAvailabilitiesChanged(_fixture.NewAvailabilities);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void ModifyAfterAvailabilitiesChanged_WithNewAvailabilitiesNotContainingOldStore_ShouldUpdateStoreId()
        {
            // Arrange
            _fixture.SetupDefaultStoreId();
            _fixture.SetupNewAvailabilitiesNotContainingOldStore();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultForChangingStore(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewAvailabilities);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.ModifyAfterAvailabilitiesChanged(_fixture.NewAvailabilities);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class ModifyAfterAvailabilitiesChangedFixture : IngredientFixture
        {
            public IReadOnlyCollection<ItemAvailability>? NewAvailabilities { get; private set; }
            public Ingredient? ExpectedResult { get; private set; }

            public void SetupNewAvailabilitiesContainingOldStore()
            {
                TestPropertyNotSetException.ThrowIfNull(DefaultStoreId);

                NewAvailabilities = ItemAvailabilityMother.ForStore(DefaultStoreId.Value).CreateMany(1).ToList();
            }

            public void SetupNewAvailabilitiesNotContainingOldStore()
            {
                NewAvailabilities = ItemAvailabilityMother.Initial().CreateMany(1).ToList();
            }

            public void SetupExpectedResultForChangingStore(Ingredient sut)
            {
                TestPropertyNotSetException.ThrowIfNull(NewAvailabilities);

                var props = new IngredientShoppingListPropertiesBuilder(sut.ShoppingListProperties!)
                    .WithDefaultStoreId(NewAvailabilities.First().StoreId)
                    .Create();
                ExpectedResult = new IngredientBuilder(sut)
                    .WithShoppingListProperties(props)
                    .Create();
            }

            public void SetupExpectedResultForNotChangingStore(Ingredient sut)
            {
                ExpectedResult = new IngredientBuilder(sut).Create();
            }
        }
    }

    public abstract class IngredientFixture
    {
        private readonly IngredientShoppingListPropertiesBuilder _shoppingListPropertiesBuilder = new();
        protected IngredientId? Id;
        protected ItemId? DefaultItemId;
        protected ItemTypeId? DefaultItemTypeId;
        protected StoreId? DefaultStoreId;
        private bool _noShoppingListProperties;
        protected IngredientBuilder Builder = new();

        public void SetupId()
        {
            Id = IngredientId.New;
        }

        public void SetupDefaultItemId()
        {
            DefaultItemId = ItemId.New;
            _shoppingListPropertiesBuilder.WithDefaultItemId(DefaultItemId.Value);
            _shoppingListPropertiesBuilder.WithDefaultItemTypeId(null);
        }

        public void SetupDefaultItemTypeId()
        {
            DefaultItemTypeId = ItemTypeId.New;
            _shoppingListPropertiesBuilder.WithDefaultItemTypeId(DefaultItemTypeId.Value);
        }

        public void SetupNoDefaultItemTypeId()
        {
            _shoppingListPropertiesBuilder.WithDefaultItemTypeId(null);
        }

        public void SetupDefaultStoreId()
        {
            DefaultStoreId = StoreId.New;
            _shoppingListPropertiesBuilder.WithDefaultStoreId(DefaultStoreId.Value);
        }

        public void SetupShoppingListPropertiesNull()
        {
            _noShoppingListProperties = true;
        }

        public Ingredient CreateSut()
        {
            if (Id is not null)
                Builder.WithId(Id.Value);

            if (_noShoppingListProperties)
            {
                Builder.WithoutShoppingListProperties();
            }
            else
            {
                Builder.WithShoppingListProperties(_shoppingListPropertiesBuilder.Create());
            }

            return Builder.Create();
        }
    }
}