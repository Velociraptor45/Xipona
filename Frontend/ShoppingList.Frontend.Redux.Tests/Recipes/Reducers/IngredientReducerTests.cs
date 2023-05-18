using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Recipes.Reducers;

public class IngredientReducerTests
{
    public class OnLoadIngredientQuantityTypesFinished
    {
        private readonly OnLoadIngredientQuantityTypesFinishedFixture _fixture;

        public OnLoadIngredientQuantityTypesFinished()
        {
            _fixture = new OnLoadIngredientQuantityTypesFinishedFixture();
        }

        [Fact]
        public void ShouldSetIngredientQuantityTypes()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnLoadIngredientQuantityTypesFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadIngredientQuantityTypesFinishedFixture : IngredientReducerFixture
        {
            public LoadIngredientQuantityTypesFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    IngredientQuantityTypes = new DomainTestBuilder<IngredientQuantityType>().CreateMany(2).ToList()
                };
            }

            public void SetupAction()
            {
                Action = new LoadIngredientQuantityTypesFinishedAction(ExpectedState.IngredientQuantityTypes);
            }
        }
    }

    public class OnIngredientAdded
    {
        private readonly OnIngredientAddedFixture _fixture;

        public OnIngredientAdded()
        {
            _fixture = new OnIngredientAddedFixture();
        }

        [Fact]
        public void OnIngredientAdded_WithValidData_ShouldAddIngredient()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupExpectedState();

            // Act
            var result = IngredientReducer.OnIngredientAdded(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState,
                opt => opt.Excluding(info => info.Path == "Editor.Recipe.Ingredients[3].Key"));
        }

        [Fact]
        public void OnIngredientAdded_WithRecipeNull_ShouldNotAddIngredient()
        {
            // Arrange
            _fixture.SetupExpectedStateWithRecipeNull();
            _fixture.SetupInitialStateEqualsExpectedState();

            // Act
            var result = IngredientReducer.OnIngredientAdded(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnIngredientAddedFixture : IngredientReducerFixture
        {
            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupExpectedStateWithRecipeNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            Ingredients = ExpectedState.Editor.Recipe!.Ingredients
                                .Union(new List<EditedIngredient>
                                {
                                    new(
                                        Guid.NewGuid(),
                                        Guid.Empty,
                                        Guid.Empty,
                                        ExpectedState.IngredientQuantityTypes.First().Id,
                                        1,
                                        null,
                                        null,
                                        null,
                                        null,
                                        new ItemCategorySelector(
                                            new List<ItemCategorySearchResult>(),
                                            string.Empty),
                                        new ItemSelector(new List<SearchItemByItemCategoryResult>()))
                                })
                                .ToList()
                        }
                    }
                };
            }
        }
    }

    public class OnIngredientRemoved
    {
        private readonly OnIngredientRemovedFixture _fixture;

        public OnIngredientRemoved()
        {
            _fixture = new OnIngredientRemovedFixture();
        }

        [Fact]
        public void OnIngredientRemoved_WithValidData_ShouldRemoveIngredient()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnIngredientRemoved(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState,
                opt => opt.Excluding(info => info.Path == "Editor.Recipe.Ingredients[3].Key"));
        }

        [Fact]
        public void OnIngredientRemoved_WithRecipeNull_ShouldNotModifyState()
        {
            // Arrange
            _fixture.SetupExpectedStateWithRecipeNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnIngredientRemoved(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnIngredientRemovedFixture : IngredientReducerFixture
        {
            public IngredientRemovedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupExpectedStateWithRecipeNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            Ingredients = ExpectedState.Editor.Recipe!.Ingredients.Skip(1).ToList()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new IngredientRemovedAction(InitialState.Editor.Recipe!.Ingredients.First());
            }

            public void SetupActionForRecipeNull()
            {
                Action = new IngredientRemovedAction(new DomainTestBuilder<EditedIngredient>().Create());
            }
        }
    }

    public class OnIngredientQuantityChanged
    {
        private readonly OnIngredientQuantityChangedFixture _fixture;

        public OnIngredientQuantityChanged()
        {
            _fixture = new OnIngredientQuantityChangedFixture();
        }

        [Fact]
        public void OnIngredientQuantityChanged_WithValidData_ShouldChangeQuantity()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnIngredientQuantityChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnIngredientQuantityChanged_WithRecipeNull_ShouldNotModifyState()
        {
            // Arrange
            _fixture.SetupExpectedStateWithRecipeNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnIngredientQuantityChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnIngredientQuantityChanged_WithInvalidIngredientKey_ShouldNotModifyState()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForInvalidIngredientKey();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnIngredientQuantityChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnIngredientQuantityChangedFixture : IngredientReducerFixture
        {
            public IngredientQuantityChangedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupExpectedStateWithRecipeNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupInitialState()
            {
                var ingredients = ExpectedState.Editor.Recipe!.Ingredients.ToList();
                ingredients[^1] = ingredients[^1] with
                {
                    Quantity = new DomainTestBuilder<float>().Create()
                };

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            Ingredients = ingredients
                        }
                    }
                };
            }

            public void SetupAction()
            {
                var ingredient = ExpectedState.Editor.Recipe!.Ingredients.Last();
                Action = new IngredientQuantityChangedAction(ingredient.Key, ingredient.Quantity);
            }

            public void SetupActionForInvalidIngredientKey()
            {
                SetupRandomAction();
            }

            public void SetupActionForRecipeNull()
            {
                SetupRandomAction();
            }

            private void SetupRandomAction()
            {
                Action = new DomainTestBuilder<IngredientQuantityChangedAction>().Create();
            }
        }
    }

    public class OnIngredientQuantityTypeChanged
    {
        private readonly OnIngredientQuantityTypeChangedFixture _fixture;

        public OnIngredientQuantityTypeChanged()
        {
            _fixture = new OnIngredientQuantityTypeChangedFixture();
        }

        [Fact]
        public void OnIngredientQuantityTypeChanged_WithValidData_ShouldChangeQuantityTypeId()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnIngredientQuantityTypeChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnIngredientQuantityTypeChanged_WithRecipeNull_ShouldNotModifyState()
        {
            // Arrange
            _fixture.SetupExpectedStateWithRecipeNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnIngredientQuantityTypeChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnIngredientQuantityTypeChanged_WithInvalidIngredientKey_ShouldNotModifyState()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForInvalidIngredientKey();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnIngredientQuantityTypeChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnIngredientQuantityTypeChangedFixture : IngredientReducerFixture
        {
            public IngredientQuantityTypeChangedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupExpectedStateWithRecipeNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupInitialState()
            {
                var ingredients = ExpectedState.Editor.Recipe!.Ingredients.ToList();
                ingredients[^1] = ingredients[^1] with
                {
                    QuantityTypeId = new DomainTestBuilder<int>().Create()
                };

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            Ingredients = ingredients
                        }
                    }
                };
            }

            public void SetupAction()
            {
                var ingredient = ExpectedState.Editor.Recipe!.Ingredients.Last();
                Action = new IngredientQuantityTypeChangedAction(ingredient.Key, ingredient.QuantityTypeId);
            }

            public void SetupActionForInvalidIngredientKey()
            {
                SetupRandomAction();
            }

            public void SetupActionForRecipeNull()
            {
                SetupRandomAction();
            }

            private void SetupRandomAction()
            {
                Action = new DomainTestBuilder<IngredientQuantityTypeChangedAction>().Create();
            }
        }
    }

    public class OnSelectedItemChanged
    {
        private readonly OnSelectedItemChangedFixture _fixture;

        public OnSelectedItemChanged()
        {
            _fixture = new OnSelectedItemChangedFixture();
        }

        [Fact]
        public void OnSelectedItemChanged_WithValidData_ShouldChangeSelectedItem()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnSelectedItemChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSelectedItemChanged_WithSettingSameItem_ShouldNotModifyState()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnSelectedItemChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSelectedItemChanged_WithRecipeNull_ShouldNotModifyState()
        {
            // Arrange
            _fixture.SetupExpectedStateWithRecipeNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnSelectedItemChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSelectedItemChanged_WithInvalidIngredientKey_ShouldNotModifyState()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForInvalidIngredientKey();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnSelectedItemChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSelectedItemChangedFixture : IngredientReducerFixture
        {
            public SelectedItemChangedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupExpectedStateWithRecipeNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupInitialState()
            {
                var ingredients = ExpectedState.Editor.Recipe!.Ingredients.ToList();
                ingredients[^1] = ingredients[^1] with
                {
                    DefaultItemId = Guid.NewGuid(),
                    DefaultItemTypeId = Guid.NewGuid(),
                    DefaultStoreId = Guid.NewGuid(),
                    AddToShoppingListByDefault = false
                };

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            Ingredients = ingredients
                        }
                    }
                };
            }

            public void SetupExpectedState()
            {
                var ingredients = ExpectedState.Editor.Recipe!.Ingredients.ToList();
                var item = ingredients[^1].ItemSelector.Items.First();
                ingredients[^1] = ingredients[^1] with
                {
                    DefaultItemId = item.ItemId,
                    DefaultItemTypeId = item.ItemTypeId,
                    DefaultStoreId = item.Availabilities.First().StoreId,
                    AddToShoppingListByDefault = true
                };

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            Ingredients = ingredients
                        }
                    }
                };
            }

            public void SetupAction()
            {
                var ingredient = ExpectedState.Editor.Recipe!.Ingredients.Last();
                Action = new SelectedItemChangedAction(ingredient.Key, ingredient.DefaultItemId!.Value,
                    ingredient.DefaultItemTypeId);
            }

            public void SetupActionForInvalidIngredientKey()
            {
                SetupRandomAction();
            }

            public void SetupActionForRecipeNull()
            {
                SetupRandomAction();
            }

            private void SetupRandomAction()
            {
                Action = new DomainTestBuilder<SelectedItemChangedAction>().Create();
            }
        }
    }

    public class OnSelectedItemCleared
    {
        private readonly OnSelectedItemClearedFixture _fixture;

        public OnSelectedItemCleared()
        {
            _fixture = new OnSelectedItemClearedFixture();
        }

        [Fact]
        public void OnSelectedItemCleared_WithValidData_ShouldChangeSelectedItem()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnSelectedItemCleared(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSelectedItemCleared_WithRecipeNull_ShouldNotModifyState()
        {
            // Arrange
            _fixture.SetupExpectedStateWithRecipeNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnSelectedItemCleared(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSelectedItemCleared_WithInvalidIngredientKey_ShouldNotModifyState()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForInvalidIngredientKey();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnSelectedItemCleared(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSelectedItemClearedFixture : IngredientReducerFixture
        {
            public SelectedItemClearedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupExpectedStateWithRecipeNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupExpectedState()
            {
                var ingredients = ExpectedState.Editor.Recipe!.Ingredients.ToList();
                ingredients[^1] = ingredients[^1] with
                {
                    DefaultItemId = null,
                    DefaultItemTypeId = null,
                    DefaultStoreId = null,
                    AddToShoppingListByDefault = null
                };

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            Ingredients = ingredients
                        }
                    }
                };
            }

            public void SetupInitialState()
            {
                var ingredients = ExpectedState.Editor.Recipe!.Ingredients.ToList();
                ingredients[^1] = ingredients[^1] with
                {
                    DefaultItemId = Guid.NewGuid(),
                    DefaultItemTypeId = Guid.NewGuid(),
                    DefaultStoreId = Guid.NewGuid(),
                    AddToShoppingListByDefault = false
                };

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            Ingredients = ingredients
                        }
                    }
                };
            }

            public void SetupAction()
            {
                var ingredient = ExpectedState.Editor.Recipe!.Ingredients.Last();
                Action = new SelectedItemClearedAction(ingredient.Key);
            }

            public void SetupActionForInvalidIngredientKey()
            {
                SetupRandomAction();
            }

            public void SetupActionForRecipeNull()
            {
                SetupRandomAction();
            }

            private void SetupRandomAction()
            {
                Action = new DomainTestBuilder<SelectedItemClearedAction>().Create();
            }
        }
    }

    public class OnLoadItemsForItemCategoryFinished
    {
        private readonly OnLoadItemsForItemCategoryFinishedFixture _fixture;

        public OnLoadItemsForItemCategoryFinished()
        {
            _fixture = new OnLoadItemsForItemCategoryFinishedFixture();
        }

        [Fact]
        public void OnLoadItemsForItemCategoryFinished_WithValidData_ShouldChangeSelectedItem()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnLoadItemsForItemCategoryFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public void OnLoadItemsForItemCategoryFinished_WithRecipeNull_ShouldNotModifyState()
        {
            // Arrange
            _fixture.SetupExpectedStateWithRecipeNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnLoadItemsForItemCategoryFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnLoadItemsForItemCategoryFinished_WithInvalidIngredientKey_ShouldNotModifyState()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForInvalidIngredientKey();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnLoadItemsForItemCategoryFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadItemsForItemCategoryFinishedFixture : IngredientReducerFixture
        {
            public LoadItemsForItemCategoryFinishedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupExpectedStateWithRecipeNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupInitialState()
            {
                var ingredients = ExpectedState.Editor.Recipe!.Ingredients.ToList();
                ingredients[^1] = ingredients[^1] with
                {
                    ItemSelector = ingredients[^1].ItemSelector with
                    {
                        Items = new DomainTestBuilder<SearchItemByItemCategoryResult>().CreateMany(2).ToList()
                    }
                };

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            Ingredients = ingredients
                        }
                    }
                };
            }

            public void SetupExpectedState()
            {
                var ingredients = ExpectedState.Editor.Recipe!.Ingredients.ToList();
                ingredients[^1] = ingredients[^1] with
                {
                    ItemSelector = ingredients[^1].ItemSelector with
                    {
                        Items = new List<SearchItemByItemCategoryResult>
                        {
                            new DomainTestBuilder<SearchItemByItemCategoryResult>().Create() with
                            {
                                Name = $"A{new DomainTestBuilder<string>().Create()}"
                            },
                            new DomainTestBuilder<SearchItemByItemCategoryResult>().Create() with
                            {
                                Name = $"B{new DomainTestBuilder<string>().Create()}"
                            },
                            new DomainTestBuilder<SearchItemByItemCategoryResult>().Create() with
                            {
                                Name = $"Z{new DomainTestBuilder<string>().Create()}"
                            }
                        }
                    }
                };

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            Ingredients = ingredients
                        }
                    }
                };
            }

            public void SetupAction()
            {
                var ingredient = ExpectedState.Editor.Recipe!.Ingredients.Last();
                Action = new LoadItemsForItemCategoryFinishedAction(ingredient.Key,
                    ingredient.ItemSelector.Items.Reverse().ToList());
            }

            public void SetupActionForInvalidIngredientKey()
            {
                SetupRandomAction();
            }

            public void SetupActionForRecipeNull()
            {
                SetupRandomAction();
            }

            private void SetupRandomAction()
            {
                Action = new DomainTestBuilder<LoadItemsForItemCategoryFinishedAction>().Create();
            }
        }
    }

    public class OnIngredientDefaultStoreChanged
    {
        private readonly OnIngredientDefaultStoreChangedFixture _fixture;

        public OnIngredientDefaultStoreChanged()
        {
            _fixture = new OnIngredientDefaultStoreChangedFixture();
        }

        [Fact]
        public void OnIngredientDefaultStoreChanged_WithValidData_ShouldChangeDefaultStoreId()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnIngredientDefaultStoreChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnIngredientDefaultStoreChanged_WithRecipeNull_ShouldNotModifyState()
        {
            // Arrange
            _fixture.SetupExpectedStateWithRecipeNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnIngredientDefaultStoreChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnIngredientDefaultStoreChanged_WithInvalidIngredientKey_ShouldNotModifyState()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForInvalidIngredientKey();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnIngredientDefaultStoreChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnIngredientDefaultStoreChangedFixture : IngredientReducerFixture
        {
            public IngredientDefaultStoreChangedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupExpectedStateWithRecipeNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupInitialState()
            {
                var ingredients = ExpectedState.Editor.Recipe!.Ingredients.ToList();
                ingredients[^1] = ingredients[^1] with
                {
                    DefaultStoreId = Guid.NewGuid()
                };

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            Ingredients = ingredients
                        }
                    }
                };
            }

            public void SetupAction()
            {
                var ingredient = ExpectedState.Editor.Recipe!.Ingredients.Last();
                Action = new IngredientDefaultStoreChangedAction(ingredient.Key, ingredient.DefaultStoreId!.Value);
            }

            public void SetupActionForInvalidIngredientKey()
            {
                SetupRandomAction();
            }

            public void SetupActionForRecipeNull()
            {
                SetupRandomAction();
            }

            private void SetupRandomAction()
            {
                Action = new DomainTestBuilder<IngredientDefaultStoreChangedAction>().Create();
            }
        }
    }

    public class OnIngredientAddToShoppingListByDefaultChanged
    {
        private readonly OnIngredientAddToShoppingListByDefaultChangedFixture _fixture;

        public OnIngredientAddToShoppingListByDefaultChanged()
        {
            _fixture = new OnIngredientAddToShoppingListByDefaultChangedFixture();
        }

        [Fact]
        public void OnIngredientAddToShoppingListByDefaultChanged_WithValidData_ShouldChangeAddToShoppingListByDefault()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnIngredientAddToShoppingListByDefaultChanged(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnIngredientAddToShoppingListByDefaultChanged_WithRecipeNull_ShouldNotModifyState()
        {
            // Arrange
            _fixture.SetupExpectedStateWithRecipeNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnIngredientAddToShoppingListByDefaultChanged(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnIngredientAddToShoppingListByDefaultChanged_WithInvalidIngredientKey_ShouldNotModifyState()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForInvalidIngredientKey();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = IngredientReducer.OnIngredientAddToShoppingListByDefaultChanged(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnIngredientAddToShoppingListByDefaultChangedFixture : IngredientReducerFixture
        {
            public IngredientAddToShoppingListByDefaultChangedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupExpectedStateWithRecipeNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupInitialState()
            {
                var ingredients = ExpectedState.Editor.Recipe!.Ingredients.ToList();
                ingredients[^1] = ingredients[^1] with
                {
                    AddToShoppingListByDefault = !ingredients[^1].AddToShoppingListByDefault
                };

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            Ingredients = ingredients
                        }
                    }
                };
            }

            public void SetupAction()
            {
                var ingredient = ExpectedState.Editor.Recipe!.Ingredients.Last();
                Action = new IngredientAddToShoppingListByDefaultChangedAction(ingredient.Key,
                    ingredient.AddToShoppingListByDefault!.Value);
            }

            public void SetupActionForInvalidIngredientKey()
            {
                SetupRandomAction();
            }

            public void SetupActionForRecipeNull()
            {
                SetupRandomAction();
            }

            private void SetupRandomAction()
            {
                Action = new DomainTestBuilder<IngredientAddToShoppingListByDefaultChangedAction>().Create();
            }
        }
    }

    private abstract class IngredientReducerFixture
    {
        public RecipeState ExpectedState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
        public RecipeState InitialState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
    }
}