using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
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

    public class OnItemCategoryDropdownClosed
    {
        private readonly OnItemCategoryDropdownClosedFixture _fixture;

        public OnItemCategoryDropdownClosed()
        {
            _fixture = new OnItemCategoryDropdownClosedFixture();
        }

        [Fact]
        public void OnItemCategoryDropdownClosed_WithValidData_ShouldChangeInput()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategorySelectorReducer.OnItemCategoryDropdownClosed(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnItemCategoryDropdownClosed_WithInvalidIngredient_ShouldNotChangeInput()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionWithInvalidIngredient();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategorySelectorReducer.OnItemCategoryDropdownClosed(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnItemCategoryDropdownClosed_WithRecipeNull_ShouldNotChangeInput()
        {
            // Arrange
            _fixture.SetupExpectedStateRecipeNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategorySelectorReducer.OnItemCategoryDropdownClosed(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnItemCategoryDropdownClosedFixture : ItemCategorySelectorReducerFixture
        {
            public ItemCategoryDropdownClosedAction? Action { get; private set; }

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

            public void SetupExpectedState()
            {
                var ingredients = ExpectedState.Editor.Recipe!.Ingredients.ToList();
                ingredients[0] = ingredients.First() with
                {
                    ItemCategorySelector = ingredients.First().ItemCategorySelector with
                    {
                        Input = string.Empty
                    }
                };

                ExpectedState = ExpectedState with
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
                Action = new ItemCategoryDropdownClosedAction(ingredient);
            }

            public void SetupActionWithInvalidIngredient()
            {
                var ingredient = new DomainTestBuilder<EditedIngredient>().Create();
                Action = new ItemCategoryDropdownClosedAction(ingredient);
            }

            public void SetupActionForRecipeNull()
            {
                Action = new DomainTestBuilder<ItemCategoryDropdownClosedAction>().Create();
            }
        }
    }

    public class OnSelectedItemCategoryChanged
    {
        private readonly OnSelectedItemCategoryChangedFixture _fixture;

        public OnSelectedItemCategoryChanged()
        {
            _fixture = new OnSelectedItemCategoryChangedFixture();
        }

        [Fact]
        public void OnSelectedItemCategoryChanged_WithValidData_ShouldChangeItemCategory()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategorySelectorReducer.OnSelectedItemCategoryChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSelectedItemCategoryChanged_WithInvalidIngredient_ShouldNotChangeItemCategory()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionWithInvalidIngredient();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategorySelectorReducer.OnSelectedItemCategoryChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSelectedItemCategoryChanged_WithRecipeNull_ShouldNotChangeItemCategory()
        {
            // Arrange
            _fixture.SetupExpectedStateRecipeNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategorySelectorReducer.OnSelectedItemCategoryChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSelectedItemCategoryChangedFixture : ItemCategorySelectorReducerFixture
        {
            public SelectedItemCategoryChangedAction? Action { get; private set; }

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
                    ItemCategoryId = Guid.NewGuid(),
                    ItemCategorySelector = ingredients.First().ItemCategorySelector with
                    {
                        ItemCategories =
                            ingredients.First().ItemCategorySelector.ItemCategories
                                .Concat(new DomainTestBuilder<ItemCategorySearchResult>().CreateMany(2))
                                .ToList(),
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

            public void SetupExpectedState()
            {
                var ingredients = ExpectedState.Editor.Recipe!.Ingredients.ToList();
                ingredients[0] = ingredients.First() with
                {
                    ItemCategoryId = ingredients.First().ItemCategorySelector.ItemCategories.First().Id,
                    ItemCategorySelector = ingredients.First().ItemCategorySelector with
                    {
                        ItemCategories = new List<ItemCategorySearchResult>
                        {
                            ingredients.First().ItemCategorySelector.ItemCategories.First()
                        },
                        Input = string.Empty
                    }
                };

                ExpectedState = ExpectedState with
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
                var itemCategoryId = ExpectedState.Editor.Recipe!.Ingredients.First().ItemCategoryId;
                Action = new SelectedItemCategoryChangedAction(ingredient.Key, itemCategoryId);
            }

            public void SetupActionWithInvalidIngredient()
            {
                var itemCategoryId = ExpectedState.Editor.Recipe!.Ingredients.First().ItemCategoryId;
                Action = new SelectedItemCategoryChangedAction(Guid.NewGuid(), itemCategoryId);
            }

            public void SetupActionForRecipeNull()
            {
                Action = new DomainTestBuilder<SelectedItemCategoryChangedAction>().Create();
            }
        }
    }

    public class OnLoadInitialItemCategoryFinished
    {
        private readonly OnLoadInitialItemCategoryFinishedFixture _fixture;

        public OnLoadInitialItemCategoryFinished()
        {
            _fixture = new OnLoadInitialItemCategoryFinishedFixture();
        }

        [Fact]
        public void OnLoadInitialItemCategoryFinished_WithValidData_ShouldSetSearchResult()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategorySelectorReducer.OnLoadInitialItemCategoryFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnLoadInitialItemCategoryFinished_WithInvalidIngredient_ShouldNotSetSearchResult()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionWithInvalidIngredient();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategorySelectorReducer.OnLoadInitialItemCategoryFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnLoadInitialItemCategoryFinished_WithRecipeNull_ShouldNotSetSearchResult()
        {
            // Arrange
            _fixture.SetupExpectedStateRecipeNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategorySelectorReducer.OnLoadInitialItemCategoryFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadInitialItemCategoryFinishedFixture : ItemCategorySelectorReducerFixture
        {
            public LoadInitialItemCategoryFinishedAction? Action { get; private set; }

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
                        ItemCategories = new List<ItemCategorySearchResult>()
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

            public void SetupExpectedState()
            {
                var ingredients = ExpectedState.Editor.Recipe!.Ingredients.ToList();
                ingredients[0] = ingredients.First() with
                {
                    ItemCategorySelector = ingredients.First().ItemCategorySelector with
                    {
                        ItemCategories = new List<ItemCategorySearchResult>
                        {
                            ingredients.First().ItemCategorySelector.ItemCategories.First()
                        }
                    }
                };

                ExpectedState = ExpectedState with
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
                var itemCategory = ExpectedState.Editor.Recipe!.Ingredients.First().ItemCategorySelector
                    .ItemCategories.First();
                Action = new LoadInitialItemCategoryFinishedAction(ingredient.Key, itemCategory);
            }

            public void SetupActionWithInvalidIngredient()
            {
                var itemCategory = ExpectedState.Editor.Recipe!.Ingredients.First().ItemCategorySelector
                    .ItemCategories.First();
                Action = new LoadInitialItemCategoryFinishedAction(Guid.NewGuid(), itemCategory);
            }

            public void SetupActionForRecipeNull()
            {
                Action = new DomainTestBuilder<LoadInitialItemCategoryFinishedAction>().Create();
            }
        }
    }

    private abstract class ItemCategorySelectorReducerFixture
    {
        public RecipeState ExpectedState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
        public RecipeState InitialState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
    }
}