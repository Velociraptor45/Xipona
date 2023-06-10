using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models.Factories;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Recipes.Models;

public class RecipeTests
{
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