using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Models.Factories;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.Recipes.Models;

public class RecipeTests
{
    public class ModifyAsync
    {
        private readonly ModifyAsyncFixture _fixture = new();

        [Fact]
        public async Task ModifyAsync_WithModifyingExisting_ShouldModifyRecipe()
        {
            // Arrange
            _fixture.SetupExistingIngredient();
            _fixture.SetupExistingPreparationStep();

            _fixture.SetupIngredientExpectedResultModifyingExisting();
            _fixture.SetupPreparationStepExpectedResultModifyingExisting();
            _fixture.SetupExpectedRecipeTagIds();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedRecipe(sut);

            _fixture.SetupIngredientModification();
            _fixture.SetupPreparationStepModification();
            _fixture.SetupRecipeModification();

            _fixture.SetupIngredientValidationSuccess();
            _fixture.SetupRecipeTagIdValidationSuccess();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedRecipe);

            // Act
            await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            sut.Should().BeEquivalentTo(_fixture.ExpectedRecipe);
        }

        [Fact]
        public async Task ModifyAsync_WithAdding_ShouldModifyRecipe()
        {
            // Arrange
            _fixture.SetupExistingIngredientEmpty();
            _fixture.SetupExistingPreparationStepEmpty();
            _fixture.SetupExistingRecipeTagsEmpty();

            _fixture.SetupIngredientExpectedResultNew();
            _fixture.SetupPreparationStepExpectedResultNew();
            _fixture.SetupExpectedRecipeTagIds();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedRecipe(sut);

            _fixture.SetupIngredientModificationNew();
            _fixture.SetupPreparationStepModificationNew();
            _fixture.SetupRecipeModification();

            _fixture.SetupIngredientValidationSuccess();
            _fixture.SetupRecipeTagIdValidationSuccess();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedRecipe);

            // Act
            await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            sut.Should().BeEquivalentTo(_fixture.ExpectedRecipe,
                opt => opt.Excluding(info => info.Path == "Ingredients[0].Id" || info.Path == "PreparationSteps[0].Id"));
        }

        [Fact]
        public async Task ModifyAsync_WithDeleting_ShouldModifyRecipe()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedRecipe(sut);

            _fixture.SetupRecipeModification();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedRecipe);

            // Act
            await sut.ModifyAsync(_fixture.Modification, _fixture.ValidatorMock.Object);

            // Assert
            sut.Should().BeEquivalentTo(_fixture.ExpectedRecipe);
        }

        private sealed class ModifyAsyncFixture : RecipeFixture
        {
            private readonly RecipeId _recipeId = RecipeId.New;
            private Ingredient? _existingIngredient;
            private PreparationStep? _existingPreparationStep;
            private IngredientModification? _ingredientModification;
            private PreparationStepModification? _preparationSetupModification;
            private Ingredient? _expectedIngredient;
            private PreparationStep? _expectedPreparationStep;

            private readonly IngredientFactory _ingredientFactory;
            private readonly PreparationStepFactory _preparationStepFactoryMock = new();

            public ModifyAsyncFixture()
            {
                _ingredientFactory = new(ValidatorMock.Object);
                RecipeBuilder.WithId(_recipeId);
            }

            public Recipe? ExpectedRecipe { get; private set; }
            public RecipeModification? Modification { get; private set; }
            public readonly ValidatorMock ValidatorMock = new(MockBehavior.Strict);
            private IList<RecipeTagId>? _expectedTagIds;

            public void SetupExistingIngredient()
            {
                _existingIngredient = new IngredientBuilder().Create();
                RecipeBuilder.WithIngredients(
                    new Ingredients(_existingIngredient.ToMonoList(), _ingredientFactory));
            }

            public void SetupExistingPreparationStep()
            {
                _existingPreparationStep = new PreparationStepBuilder().Create();
                RecipeBuilder.WithSteps(
                    new PreparationSteps(_existingPreparationStep.ToMonoList(), _preparationStepFactoryMock));
            }

            public void SetupExistingIngredientEmpty()
            {
                _existingIngredient = new IngredientBuilder().Create();
                RecipeBuilder.WithIngredients(
                    new Ingredients(_existingIngredient.ToMonoList(), _ingredientFactory));
            }

            public void SetupExistingPreparationStepEmpty()
            {
                _existingPreparationStep = new PreparationStepBuilder().Create();
                RecipeBuilder.WithSteps(
                    new PreparationSteps(_existingPreparationStep.ToMonoList(), _preparationStepFactoryMock));
            }

            public void SetupExistingRecipeTagsEmpty()
            {
                RecipeBuilder.WithTags(new Domain.Recipes.Models.RecipeTags(Enumerable.Empty<RecipeTagId>()));
            }

            public void SetupIngredientModification()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedIngredient);

                _ingredientModification = new IngredientModification(
                    _expectedIngredient.Id,
                    _expectedIngredient.ItemCategoryId,
                    _expectedIngredient.QuantityType,
                    _expectedIngredient.Quantity,
                    _expectedIngredient.ShoppingListProperties);
            }

            public void SetupPreparationStepModification()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedPreparationStep);

                _preparationSetupModification = new PreparationStepModification(
                    _expectedPreparationStep.Id,
                    _expectedPreparationStep.Instruction,
                    _expectedPreparationStep.SortingIndex);
            }

            public void SetupIngredientModificationNew()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedIngredient);

                _ingredientModification = new IngredientModification(
                    null,
                    _expectedIngredient.ItemCategoryId,
                    _expectedIngredient.QuantityType,
                    _expectedIngredient.Quantity,
                    _expectedIngredient.ShoppingListProperties);
            }

            public void SetupPreparationStepModificationNew()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedPreparationStep);

                _preparationSetupModification = new PreparationStepModification(
                    null,
                    _expectedPreparationStep.Instruction,
                    _expectedPreparationStep.SortingIndex);
            }

            public void SetupRecipeModification()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedRecipe);

                var ingredients = _ingredientModification?.ToMonoList() ?? Enumerable.Empty<IngredientModification>();
                var preparationSteps = _preparationSetupModification?.ToMonoList() ??
                                       Enumerable.Empty<PreparationStepModification>();
                var tags = _expectedTagIds ?? Enumerable.Empty<RecipeTagId>();

                Modification = new RecipeModification(
                    ExpectedRecipe.Id,
                    ExpectedRecipe.Name,
                    ExpectedRecipe.NumberOfServings,
                    ingredients,
                    preparationSteps,
                    tags,
                    ExpectedRecipe.SideDishId);
            }

            public void SetupIngredientValidationSuccess()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredientModification);

                ValidatorMock.SetupValidateAsync(_ingredientModification.ItemCategoryId);
                ValidatorMock.SetupValidateAsync(_ingredientModification.ShoppingListProperties!.DefaultItemId,
                    _ingredientModification.ShoppingListProperties.DefaultItemTypeId);
            }

            public void SetupRecipeTagIdValidationSuccess()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedTagIds);
                ValidatorMock.SetupValidateAsync(_expectedTagIds);
            }

            public void SetupIngredientExpectedResultModifyingExisting()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingIngredient);

                _expectedIngredient = new IngredientBuilder().WithId(_existingIngredient.Id).Create();
            }

            public void SetupPreparationStepExpectedResultModifyingExisting()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingPreparationStep);

                _expectedPreparationStep = new PreparationStepBuilder().WithId(_existingPreparationStep.Id).Create();
            }

            public void SetupIngredientExpectedResultNew()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingIngredient);

                _expectedIngredient = new IngredientBuilder().Create();
            }

            public void SetupPreparationStepExpectedResultNew()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingPreparationStep);

                _expectedPreparationStep = new PreparationStepBuilder().Create();
            }

            public void SetupExpectedRecipeTagIds()
            {
                _expectedTagIds = new List<RecipeTagId>
                {
                    RecipeTagId.New,
                    RecipeTagId.New,
                    RecipeTagId.New,
                };
            }

            public void SetupExpectedRecipe(Recipe sut)
            {
                var ingredients = _expectedIngredient?.ToMonoList() ?? Enumerable.Empty<Ingredient>();
                var preparationSteps = _expectedPreparationStep?.ToMonoList() ?? Enumerable.Empty<PreparationStep>();
                var tags = _expectedTagIds ?? Enumerable.Empty<RecipeTagId>();

                ExpectedRecipe = new RecipeBuilder()
                    .WithId(_recipeId)
                    .WithIngredients(new Ingredients(ingredients, _ingredientFactory))
                    .WithSteps(new PreparationSteps(preparationSteps, _preparationStepFactoryMock))
                    .WithTags(new Domain.Recipes.Models.RecipeTags(tags))
                    .WithCreatedAt(sut.CreatedAt)
                    .Create();
            }
        }
    }

    public class RemoveDefaultItem
    {
        private readonly RemoveDefaultItemFixture _fixture = new();

        [Fact]
        public void RemoveDefaultItem_WithItemType_ShouldRemoveDefaultItem()
        {
            // Arrange
            _fixture.SetupItemTypeId();
            _fixture.SetupIngredientWithItemType();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedIngredients);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);

            // Act
            sut.RemoveDefaultItem(_fixture.ItemId, _fixture.ItemTypeId);

            // Assert
            sut.Ingredients.Should().BeEquivalentTo(_fixture.ExpectedIngredients);
        }

        [Fact]
        public void RemoveDefaultItem_WithItemType_WithTwoItemTypesOfSameItem_ShouldRemoveDefaultItemOfOneIngredient()
        {
            // Arrange
            _fixture.SetupItemTypeId();
            _fixture.SetupIngredientWithTwoItemTypesOfSameItem();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedIngredients);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);

            // Act
            sut.RemoveDefaultItem(_fixture.ItemId, _fixture.ItemTypeId);

            // Assert
            sut.Ingredients.Should().BeEquivalentTo(_fixture.ExpectedIngredients);
        }

        [Fact]
        public void RemoveDefaultItem_WithoutItemType_WithIngredientWithoutItemType_ShouldRemoveDefaultItem()
        {
            // Arrange
            _fixture.SetupIngredientWithoutItemType();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedIngredients);

            // Act
            sut.RemoveDefaultItem(_fixture.ItemId, null);

            // Assert
            sut.Ingredients.Should().BeEquivalentTo(_fixture.ExpectedIngredients);
        }

        [Fact]
        public void RemoveDefaultItem_WithoutItemType_WithIngredientWithItemType_ShouldRemoveDefaultItem()
        {
            // Arrange
            _fixture.SetupIngredientWithRandomItemType();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedIngredients);

            // Act
            sut.RemoveDefaultItem(_fixture.ItemId, null);

            // Assert
            sut.Ingredients.Should().BeEquivalentTo(_fixture.ExpectedIngredients);
        }

        private sealed class RemoveDefaultItemFixture : RecipeFixture
        {
            public ItemId ItemId { get; } = ItemId.New;
            public ItemTypeId? ItemTypeId { get; private set; }
            public IReadOnlyCollection<Ingredient>? ExpectedIngredients { get; private set; }

            public void SetupItemTypeId()
            {
                ItemTypeId = Domain.Items.Models.ItemTypeId.New;
            }

            public void SetupIngredientWithoutItemType()
            {
                var shoppingListProperties1 = new IngredientShoppingListPropertiesBuilder()
                    .WithDefaultItemId(ItemId)
                    .WithoutDefaultItemTypeId()
                    .Create();

                var shoppingListProperties2 = new IngredientShoppingListPropertiesBuilder()
                    .WithoutDefaultItemTypeId()
                    .Create();

                var ingredients = new List<Ingredient>
                {
                    new IngredientBuilder().WithShoppingListProperties(shoppingListProperties1).Create(),
                    new IngredientBuilder().WithShoppingListProperties(shoppingListProperties2).Create(),
                };

                RecipeBuilder.WithIngredients(new Ingredients(ingredients,
                    new IngredientFactoryMock(MockBehavior.Strict).Object));

                ExpectedIngredients = new List<Ingredient>
                {
                    RemoveShoppingListProperties(ingredients.First()),
                    ingredients.Last()
                };
            }

            public void SetupIngredientWithItemType()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemTypeId);

                var shoppingListProperties = new IngredientShoppingListPropertiesBuilder()
                    .WithDefaultItemId(ItemId)
                    .WithDefaultItemTypeId(ItemTypeId)
                    .Create();

                var ingredients = new List<Ingredient>
                {
                    new IngredientBuilder().WithShoppingListProperties(shoppingListProperties).Create(),
                    new IngredientBuilder().Create(),
                };
                RecipeBuilder.WithIngredients(new Ingredients(ingredients,
                    new IngredientFactoryMock(MockBehavior.Strict).Object));

                ExpectedIngredients = new List<Ingredient>
                {
                    RemoveShoppingListProperties(ingredients.First()),
                    ingredients.Last()
                };
            }

            public void SetupIngredientWithRandomItemType()
            {
                var shoppingListProperties = new IngredientShoppingListPropertiesBuilder()
                    .WithDefaultItemId(ItemId)
                    .Create();

                var ingredients = new List<Ingredient>
                {
                    new IngredientBuilder().WithShoppingListProperties(shoppingListProperties).Create(),
                    new IngredientBuilder().Create(),
                };
                RecipeBuilder.WithIngredients(new Ingredients(ingredients,
                    new IngredientFactoryMock(MockBehavior.Strict).Object));

                ExpectedIngredients = new List<Ingredient>
                {
                    RemoveShoppingListProperties(ingredients.First()),
                    ingredients.Last()
                };
            }

            public void SetupIngredientWithTwoItemTypesOfSameItem()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemTypeId);

                var shoppingListProperties1 = new IngredientShoppingListPropertiesBuilder()
                    .WithDefaultItemId(ItemId)
                    .WithDefaultItemTypeId(ItemTypeId)
                    .Create();
                var shoppingListProperties2 = new IngredientShoppingListPropertiesBuilder()
                    .WithDefaultItemId(ItemId)
                    .Create();

                var ingredients = new List<Ingredient>
                {
                    new IngredientBuilder().WithShoppingListProperties(shoppingListProperties1).Create(),
                    new IngredientBuilder().WithShoppingListProperties(shoppingListProperties2).Create(),
                };
                RecipeBuilder.WithIngredients(new Ingredients(ingredients,
                    new IngredientFactoryMock(MockBehavior.Strict).Object));

                ExpectedIngredients = new List<Ingredient>
                {
                    RemoveShoppingListProperties(ingredients.First()),
                    ingredients.Last()
                };
            }

            private Ingredient RemoveShoppingListProperties(Ingredient ingredient)
            {
                return new Ingredient(
                    ingredient.Id,
                    ingredient.ItemCategoryId,
                    ingredient.QuantityType,
                    ingredient.Quantity,
                    null);
            }
        }
    }

    public class ModifyIngredientsAfterAvailabilityWasDeleted
    {
        private readonly ModifyIngredientsAfterAvailabilityWasDeletedFixture _fixture = new();

        [Fact]
        public void ModifyIngredientsAfterAvailabilityWasDeleted_WithItemWithoutTypes_ShouldModifyIngredient()
        {
            // Arrange
            _fixture.SetupExpectedIngredient();
            _fixture.SetupItem();
            _fixture.SetupIngredient();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedIngredient);

            // Act
            sut.ModifyIngredientsAfterAvailabilityWasDeleted(_fixture.ItemId, _fixture.ItemTypeId, _fixture.Item,
                _fixture.StoreId);

            // Assert
            sut.Ingredients.Should().HaveCount(1);
            sut.Ingredients.First().Should().BeEquivalentTo(_fixture.ExpectedIngredient);
        }

        [Fact]
        public void ModifyIngredientsAfterAvailabilityWasDeleted_WithItemWithTypes_ShouldModifyIngredient()
        {
            // Arrange
            _fixture.SetupItemTypeId();
            _fixture.SetupExpectedIngredient();
            _fixture.SetupItemWithTypes();
            _fixture.SetupIngredient();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedIngredient);

            // Act
            sut.ModifyIngredientsAfterAvailabilityWasDeleted(_fixture.ItemId, _fixture.ItemTypeId, _fixture.Item,
                _fixture.StoreId);

            // Assert
            sut.Ingredients.Should().HaveCount(1);
            sut.Ingredients.First().Should().BeEquivalentTo(_fixture.ExpectedIngredient);
        }

        [Fact]
        public void ModifyIngredientsAfterAvailabilityWasDeleted_WithoutIngredientNotForItem_ShouldNotModifyIngredient()
        {
            // Arrange
            _fixture.SetupExpectedIngredientWithoutItemReference();
            _fixture.SetupItem();
            _fixture.SetupIngredientEqualsExpectedIngredient();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedIngredient);

            // Act
            sut.ModifyIngredientsAfterAvailabilityWasDeleted(_fixture.ItemId, _fixture.ItemTypeId, _fixture.Item,
                _fixture.StoreId);

            // Assert
            sut.Ingredients.Should().HaveCount(1);
            sut.Ingredients.First().Should().BeEquivalentTo(_fixture.ExpectedIngredient);
        }

        [Fact]
        public void ModifyIngredientsAfterAvailabilityWasDeleted_WithoutShoppingListPropertiesOnIngredient_ShouldNotModifyIngredient()
        {
            // Arrange
            _fixture.SetupExpectedIngredientWithoutShoppingListProperties();
            _fixture.SetupItem();
            _fixture.SetupIngredientEqualsExpectedIngredient();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            sut.ModifyIngredientsAfterAvailabilityWasDeleted(_fixture.ItemId, _fixture.ItemTypeId, _fixture.Item,
                _fixture.StoreId);

            // Assert
            sut.Ingredients.Should().HaveCount(1);
            sut.Ingredients.First().Should().BeEquivalentTo(_fixture.ExpectedIngredient);
        }

        private sealed class ModifyIngredientsAfterAvailabilityWasDeletedFixture : RecipeFixture
        {
            public ItemId ItemId { get; } = ItemId.New;
            public ItemTypeId? ItemTypeId { get; private set; }
            public StoreId StoreId { get; } = StoreId.New;
            public IItem? Item { get; private set; }
            public Ingredient? ExpectedIngredient { get; private set; }

            public void SetupExpectedIngredient()
            {
                var properties = new IngredientShoppingListPropertiesBuilder()
                    .WithDefaultItemId(ItemId)
                    .WithDefaultItemTypeId(ItemTypeId)
                    .WithDefaultStoreId(StoreId)
                    .Create();
                ExpectedIngredient = new IngredientBuilder().WithShoppingListProperties(properties).Create();
            }

            public void SetupExpectedIngredientWithoutItemReference()
            {
                ExpectedIngredient = new IngredientBuilder().Create();
            }

            public void SetupExpectedIngredientWithoutShoppingListProperties()
            {
                ExpectedIngredient = new IngredientBuilder().WithoutShoppingListProperties().Create();
            }

            public void SetupItemTypeId()
            {
                ItemTypeId = Domain.Items.Models.ItemTypeId.New;
            }

            public void SetupItem()
            {
                var availabilities = new List<ItemAvailability>
                {
                    ItemAvailabilityMother.ForStore(StoreId).Create(),
                    ItemAvailabilityMother.Initial().Create()
                };
                Item = ItemMother.Initial().WithAvailabilities(availabilities).WithId(ItemId).Create();
            }

            public void SetupItemWithTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemTypeId);

                var availabilities = new List<ItemAvailability>
                {
                    ItemAvailabilityMother.ForStore(StoreId).Create(),
                    ItemAvailabilityMother.Initial().Create()
                };
                var types = new List<ItemType>
                {
                    ItemTypeMother.Initial().WithAvailabilities(availabilities).WithId(ItemTypeId.Value).Create(),
                    ItemTypeMother.Initial().Create(),
                };

                Item = ItemMother.InitialWithTypes()
                    .WithTypes(new ItemTypes(types, new ItemTypeFactoryMock(MockBehavior.Strict).Object))
                    .WithId(ItemId).Create();
            }

            public void SetupIngredient()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedIngredient);
                TestPropertyNotSetException.ThrowIfNull(ExpectedIngredient.ShoppingListProperties);

                var ingredient = new Ingredient(
                    ExpectedIngredient.Id,
                    ExpectedIngredient.ItemCategoryId,
                    ExpectedIngredient.QuantityType,
                    ExpectedIngredient.Quantity,
                    new IngredientShoppingListProperties(
                        ExpectedIngredient.ShoppingListProperties.DefaultItemId,
                        ExpectedIngredient.ShoppingListProperties.DefaultItemTypeId,
                        StoreId,
                        ExpectedIngredient.ShoppingListProperties.AddToShoppingListByDefault));

                RecipeBuilder.WithIngredients(new Ingredients(ingredient.ToMonoList(),
                    new IngredientFactoryMock(MockBehavior.Strict).Object));
            }

            public void SetupIngredientEqualsExpectedIngredient()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedIngredient);

                RecipeBuilder.WithIngredients(new Ingredients(ExpectedIngredient.ToMonoList(),
                    new IngredientFactoryMock(MockBehavior.Strict).Object));
            }
        }
    }

    public abstract class RecipeFixture
    {
        protected RecipeBuilder RecipeBuilder { get; } = new();

        public Recipe CreateSut()
        {
            return RecipeBuilder.Create();
        }
    }
}