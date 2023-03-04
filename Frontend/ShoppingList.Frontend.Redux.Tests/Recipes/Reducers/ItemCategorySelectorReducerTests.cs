using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients.ItemCategorySelectors;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Recipes.Reducers;

public class ItemCategorySelectorReducerTests
{
    public class OnItemCategoryInputChanged
    {
        private readonly OnItemCategoryInputChangedFixture _fixture;

        public OnItemCategoryInputChanged()
        {
            _fixture = new OnItemCategoryInputChangedFixture();
        }

        [Fact]
        public void OnItemCategoryInputChanged_WithValidData_ShouldChangeInput()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategorySelectorReducer.OnItemCategoryInputChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnItemCategoryInputChanged_WithInvalidIngredient_ShouldNotChangeInput()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionWithInvalidIngredient();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategorySelectorReducer.OnItemCategoryInputChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnItemCategoryInputChanged_WithRecipeNull_ShouldNotChangeInput()
        {
            // Arrange
            _fixture.SetupExpectedStateRecipeNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategorySelectorReducer.OnItemCategoryInputChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnItemCategoryInputChangedFixture : ItemCategorySelectorReducerFixture
        {
            public ItemCategoryInputChangedAction? Action { get; private set; }

            public void SetupExpectedStateRecipeNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialState()
            {
                var ingredients = ExpectedState.Editor.Recipe!.Ingredients.ToList();
                ingredients[0] = ingredients.First() with
                {
                    ItemCategorySelector = ingredients.First().ItemCategorySelector with
                    {
                        Input = new DomainTestBuilder<string>().Create()
                    }
                };

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe with
                        {
                            Ingredients = ingredients
                        }
                    }
                };
            }

            public void SetupAction()
            {
                var ingredient = InitialState.Editor.Recipe!.Ingredients.First();
                var input = ExpectedState.Editor.Recipe!.Ingredients.First().ItemCategorySelector.Input;
                Action = new ItemCategoryInputChangedAction(ingredient, input);
            }

            public void SetupActionWithInvalidIngredient()
            {
                var ingredient = new DomainTestBuilder<EditedIngredient>().Create();
                var input = ExpectedState.Editor.Recipe!.Ingredients.First().ItemCategorySelector.Input;
                Action = new ItemCategoryInputChangedAction(ingredient, input);
            }

            public void SetupActionForRecipeNull()
            {
                Action = new DomainTestBuilder<ItemCategoryInputChangedAction>().Create();
            }
        }
    }

    private abstract class ItemCategorySelectorReducerFixture
    {
        public RecipeState ExpectedState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
        public RecipeState InitialState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
    }
}