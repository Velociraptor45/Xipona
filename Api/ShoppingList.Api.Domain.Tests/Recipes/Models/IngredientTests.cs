using Force.DeepCloner;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Recipes.Models;

public class IngredientTests
{
    public class Modify
    {
        private readonly ModifyFixture _fixture;

        public Modify()
        {
            _fixture = new ModifyFixture();
        }

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
        public void ChangeDefaultItem_WithNoType_ShouldUpdateItemId()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupDefaultItemId();
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
        public void ChangeDefaultItem_WithType_ShouldUpdateItemAndItemTypeId()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupDefaultItemId();
            _fixture.SetupDefaultItemTypeId();
            _fixture.SetupNewTypeId();
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
            public Item? NewItem { get; private set; }
            public Ingredient? ExpectedResult { get; private set; }

            public void SetupNewItem()
            {
                TestPropertyNotSetException.ThrowIfNull(DefaultItemId);

                NewItem = ItemMother.Initial().WithPredecessorId(DefaultItemId.Value).Create();
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

                var types = new List<IItemType>
                {
                    ItemTypeMother.Initial().Create(),
                    ItemTypeMother.Initial().WithId(_newTypeId.Value).WithPredecessorId(DefaultItemTypeId.Value).Create(),
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

            public void SetupExpectedResult(IIngredient sut)
            {
                TestPropertyNotSetException.ThrowIfNull(NewItem);

                ExpectedResult = new Ingredient(
                    IngredientId.New,
                    sut.ItemCategoryId,
                    sut.QuantityType,
                    sut.Quantity,
                    new IngredientShoppingListProperties(
                        NewItem.Id, null, sut.ShoppingListProperties!.DefaultStoreId,
                        sut.ShoppingListProperties!.AddToShoppingListByDefault));
            }

            public void SetupExpectedResultWithType(IIngredient sut)
            {
                TestPropertyNotSetException.ThrowIfNull(NewItem);
                TestPropertyNotSetException.ThrowIfNull(_newTypeId);

                ExpectedResult = new Ingredient(
                    IngredientId.New,
                    sut.ItemCategoryId,
                    sut.QuantityType,
                    sut.Quantity,
                    new IngredientShoppingListProperties(
                        NewItem.Id, _newTypeId.Value, sut.ShoppingListProperties!.DefaultStoreId,
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

    public abstract class IngredientFixture
    {
        protected IngredientId? Id;
        protected ItemId? DefaultItemId;
        protected ItemTypeId? DefaultItemTypeId;

        public void SetupId()
        {
            Id = IngredientId.New;
        }

        public void SetupDefaultItemId()
        {
            DefaultItemId = ItemId.New;
        }

        public void SetupDefaultItemTypeId()
        {
            DefaultItemTypeId = ItemTypeId.New;
        }

        public Ingredient CreateSut()
        {
            var builder = new IngredientBuilder();

            if (Id is not null)
                builder.WithId(Id.Value);

            if (DefaultItemId is not null)
            {
                var shoppingListProperties = new IngredientShoppingListPropertiesBuilder()
                    .WithDefaultItemId(DefaultItemId.Value)
                    .WithDefaultItemTypeId(DefaultItemTypeId)
                    .Create();
                builder.WithShoppingListProperties(shoppingListProperties);
            }

            return builder.Create();
        }
    }
}