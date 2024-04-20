using FluentAssertions;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions.Editor;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.Reducers;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Recipes.Reducers;

public class RecipeEditorReducerTests
{
    public class OnCreateRecipe
    {
        private readonly OnCreateRecipeFixture _fixture = new();

        [Fact]
        public void OnCreateRecipe_WithRecipeNull_ShouldSetRecipeNull()
        {
            // Arrange
            _fixture.SetupExpectedStateWithRecipeNull();
            _fixture.SetupInitialStateEqualToExpectedState();

            // Act
            var result = RecipeEditorReducer.OnCreateRecipe(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateRecipe_WithValidData_ShouldSetValidationResultWithoutErrors()
        {
            // Arrange
            _fixture.SetupExpectedStateWithoutValidationErrors();
            _fixture.SetupInitialStateWithValidationErrors();

            // Act
            var result = RecipeEditorReducer.OnCreateRecipe(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateRecipe_WithRecipeNameEmpty_ShouldSetValidationResultWithNameEmpty()
        {
            // Arrange
            _fixture.SetupExpectedStateWithNameEmpty();
            _fixture.SetupInitialStateWithNameEmpty();

            // Act
            var result = RecipeEditorReducer.OnCreateRecipe(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateRecipe_WithIngredientItemCategoryEmpty_ShouldSetValidationResultWithIngredientItemCategoryEmpty()
        {
            // Arrange
            _fixture.SetupExpectedStateWithIngredientItemCategoryEmpty();
            _fixture.SetupInitialStateWithItemCategoryEmpty();

            // Act
            var result = RecipeEditorReducer.OnCreateRecipe(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnCreateRecipeFixture : OnSaveRecipeFixture
        {
        }
    }

    public class OnModifyRecipe
    {
        private readonly OnModifyRecipeFixture _fixture = new();

        [Fact]
        public void OnModifyRecipe_WithRecipeNull_ShouldSetRecipeNull()
        {
            // Arrange
            _fixture.SetupExpectedStateWithRecipeNull();
            _fixture.SetupInitialStateEqualToExpectedState();

            // Act
            var result = RecipeEditorReducer.OnModifyRecipe(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnModifyRecipe_WithValidData_ShouldSetValidationResultWithoutErrors()
        {
            // Arrange
            _fixture.SetupExpectedStateWithoutValidationErrors();
            _fixture.SetupInitialStateWithValidationErrors();

            // Act
            var result = RecipeEditorReducer.OnModifyRecipe(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnModifyRecipe_WithRecipeNameEmpty_ShouldSetValidationResultWithNameEmpty()
        {
            // Arrange
            _fixture.SetupExpectedStateWithNameEmpty();
            _fixture.SetupInitialStateWithNameEmpty();

            // Act
            var result = RecipeEditorReducer.OnModifyRecipe(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnModifyRecipe_WithIngredientItemCategoryEmpty_ShouldSetValidationResultWithIngredientItemCategoryEmpty()
        {
            // Arrange
            _fixture.SetupExpectedStateWithIngredientItemCategoryEmpty();
            _fixture.SetupInitialStateWithItemCategoryEmpty();

            // Act
            var result = RecipeEditorReducer.OnModifyRecipe(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnModifyRecipeFixture : OnSaveRecipeFixture
        {
        }
    }

    private abstract class OnSaveRecipeFixture : RecipeEditorReducerFixture
    {
        public void SetupInitialStateEqualToExpectedState()
        {
            InitialState = ExpectedState;
        }

        public void SetupInitialStateWithNameEmpty()
        {
            InitialState = ExpectedState with
            {
                Editor = ExpectedState.Editor with
                {
                    ValidationResult = ExpectedState.Editor.ValidationResult with
                    {
                        Name = null
                    }
                }
            };
        }

        public void SetupInitialStateWithItemCategoryEmpty()
        {
            InitialState = ExpectedState with
            {
                Editor = ExpectedState.Editor with
                {
                    ValidationResult = ExpectedState.Editor.ValidationResult with
                    {
                        IngredientItemCategory = new Dictionary<Guid, string>(0)
                    }
                }
            };
        }

        public void SetupInitialStateWithValidationErrors()
        {
            InitialState = ExpectedState with
            {
                Editor = ExpectedState.Editor with
                {
                    ValidationResult = new DomainTestBuilder<EditorValidationResult>().Create()
                }
            };
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

        public void SetupExpectedStateWithoutValidationErrors()
        {
            ExpectedState = ExpectedState with
            {
                Editor = ExpectedState.Editor with
                {
                    ValidationResult = new()
                }
            };
        }

        public void SetupExpectedStateWithNameEmpty()
        {
            ExpectedState = ExpectedState with
            {
                Editor = ExpectedState.Editor with
                {
                    Recipe = ExpectedState.Editor.Recipe! with
                    {
                        Name = string.Empty
                    },
                    ValidationResult = new()
                    {
                        Name = "Name must not be empty"
                    }
                }
            };
        }

        public void SetupExpectedStateWithIngredientItemCategoryEmpty()
        {
            var ingredients = ExpectedState.Editor.Recipe!.Ingredients.ToList();
            ingredients[0] = ingredients[0] with
            {
                ItemCategoryId = Guid.Empty
            };

            ExpectedState = ExpectedState with
            {
                Editor = ExpectedState.Editor with
                {
                    Recipe = ExpectedState.Editor.Recipe with
                    {
                        Ingredients = ingredients
                    },
                    ValidationResult = new()
                    {
                        IngredientItemCategory = new Dictionary<Guid, string>
                            {
                                { ingredients[0].Key, "Item category must not be empty" }
                            }
                    }
                }
            };
        }
    }

    public class OnSetNewRecipe
    {
        private readonly OnSetNewRecipeFixture _fixture = new();

        [Fact]
        public void OnSetNewRecipe_WithValidData_ShouldSetRecipeAndLeaveEditMode()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnSetNewRecipe(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState,
                opt => opt.Excluding(info =>
                    info.Path == "Editor.Recipe.Ingredients[0].Key"
                    || info.Path == "Editor.Recipe.PreparationSteps[0].Key"));
        }

        private sealed class OnSetNewRecipeFixture : RecipeEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = new DomainTestBuilder<EditedRecipe>().Create(),
                        IsInEditMode = false,
                        ValidationResult = new DomainTestBuilder<EditorValidationResult>().Create()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = new EditedRecipe(
                            Guid.Empty,
                            string.Empty,
                            1,
                            new List<EditedIngredient>
                            {
                                new(
                                    Guid.NewGuid(),
                                    Guid.Empty,
                                    string.Empty,
                                    Guid.Empty,
                                    ExpectedState.IngredientQuantityTypes.First().Id,
                                    1,
                                    null,
                                    null,
                                    null,
                                    null,
                                    new ItemCategorySelector(
                                        new List<ItemCategorySearchResult>(0),
                                        string.Empty),
                                    new ItemSelector(new List<SearchItemByItemCategoryResult>(0)))
                            },
                            new SortedSet<EditedPreparationStep>
                            {
                                new(Guid.NewGuid(), Guid.Empty, string.Empty, 0)
                            },
                            new List<Guid>(0)),
                        IsInEditMode = true,
                        ValidationResult = new()
                    }
                };
            }
        }
    }

    public class OnLoadRecipeForEditingFinished
    {
        private readonly OnLoadRecipeForEditingFinishedFixture _fixture = new();

        [Fact]
        public void OnLoadRecipeForEditingFinished_WithValidData_ShouldSetRecipeAndLeaveEditMode()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnLoadRecipeForEditingFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadRecipeForEditingFinishedFixture : RecipeEditorReducerFixture
        {
            public LoadRecipeForEditingFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = new DomainTestBuilder<EditedRecipe>().Create(),
                        IsInEditMode = true,
                        ValidationResult = new DomainTestBuilder<EditorValidationResult>().Create()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsInEditMode = false,
                        ValidationResult = new()
                    }
                };
            }

            public void SetupAction()
            {
                Action = new LoadRecipeForEditingFinishedAction(ExpectedState.Editor.Recipe!);
            }
        }
    }

    public class OnRecipeNameChanged
    {
        private readonly OnRecipeNameChangedFixture _fixture = new();

        [Fact]
        public void OnRecipeNameChanged_WithValidData_ShouldSetName()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnRecipeNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnRecipeNameChanged_WithEmptyName_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithNameEmpty();
            _fixture.SetupInitialStateWithNameEmpty();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnRecipeNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnRecipeNameChanged_WithRecipeNull_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateWithRecipeNull();
            _fixture.SetupExpectedStateWithRecipeNull();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnRecipeNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnRecipeNameChangedFixture : RecipeEditorReducerFixture
        {
            public RecipeNameChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            Name = string.Empty
                        },
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            Name = new DomainTestBuilder<string>().Create()
                        }
                    }
                };
            }

            public void SetupInitialStateWithNameEmpty()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            Name = new DomainTestBuilder<string>().Create()
                        },
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            Name = null
                        }
                    }
                };
            }

            public void SetupInitialStateWithRecipeNull()
            {
                InitialState = ExpectedState with
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
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            Name = null
                        }
                    }
                };
            }

            public void SetupExpectedStateWithNameEmpty()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            Name = string.Empty
                        },
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            Name = "Name must not be empty"
                        }
                    }
                };
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

            public void SetupAction()
            {
                Action = new RecipeNameChangedAction(ExpectedState.Editor.Recipe!.Name);
            }

            public void SetupActionForRecipeNull()
            {
                Action = new DomainTestBuilder<RecipeNameChangedAction>().Create();
            }
        }
    }

    public class OnToggleEditMode
    {
        private readonly OnToggleEditModeFixture _fixture = new();

        [Fact]
        public void OnToggleEditMode_WithEditorInEditMode_ShouldSetNotInEditMode()
        {
            // Arrange
            _fixture.SetupInitialStateInEditMode();
            _fixture.SetupExpectedStateNotInEditMode();

            // Act
            var result = RecipeEditorReducer.OnToggleEditMode(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnToggleEditMode_WithEditorNotInEditMode_ShouldSetInEditMode()
        {
            // Arrange
            _fixture.SetupInitialStateNotInEditMode();
            _fixture.SetupExpectedStateInEditMode();

            // Act
            var result = RecipeEditorReducer.OnToggleEditMode(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnToggleEditModeFixture : RecipeEditorReducerFixture
        {
            public void SetupInitialStateInEditMode()
            {
                SetupInitialState(true);
            }

            public void SetupInitialStateNotInEditMode()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isInEditMode)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsInEditMode = isInEditMode
                    }
                };
            }

            public void SetupExpectedStateInEditMode()
            {
                SetupExpectedState(true);
            }

            public void SetupExpectedStateNotInEditMode()
            {
                SetupExpectedState(false);
            }

            private void SetupExpectedState(bool isInEditMode)
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsInEditMode = isInEditMode
                    }
                };
            }
        }
    }

    public class OnRecipeTagInputChanged
    {
        private readonly OnRecipeTagInputChangedFixture _fixture = new();

        [Fact]
        public void OnRecipeTagInputChanged_WithValidData_ShouldSetInput()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnRecipeTagInputChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnRecipeTagInputChangedFixture : RecipeEditorReducerFixture
        {
            public RecipeTagInputChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        RecipeTagCreateInput = new DomainTestBuilder<string>().Create()
                    }
                };
            }

            public void SetupAction()
            {
                Action = new RecipeTagInputChangedAction(ExpectedState.Editor.RecipeTagCreateInput);
            }
        }
    }

    public class OnRecipeTagsDropdownClosed
    {
        private readonly OnRecipeTagsDropdownClosedFixture _fixture = new();

        [Fact]
        public void OnRecipeTagsDropdownClosed_ShouldClearRecipeTagInput()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnRecipeTagsDropdownClosed(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnRecipeTagsDropdownClosedFixture : RecipeEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        RecipeTagCreateInput = new DomainTestBuilder<string>().Create()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        RecipeTagCreateInput = string.Empty
                    }
                };
            }
        }
    }

    public class OnRecipeTagsChanged
    {
        private readonly OnRecipeTagsChangedFixture _fixture = new();

        [Fact]
        public void OnRecipeTagsChanged_WithValidData_ShouldSetRecipeTagIds()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnRecipeTagsChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnRecipeTagsChanged_WithRecipeNull_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateWithRecipeNull();
            _fixture.SetupExpectedStateWithRecipeNull();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnRecipeTagsChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnRecipeTagsChangedFixture : RecipeEditorReducerFixture
        {
            public RecipeTagsChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            RecipeTagIds = new DomainTestBuilder<Guid>().CreateMany(2).ToList()
                        }
                    }
                };
            }

            public void SetupInitialStateWithRecipeNull()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
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

            public void SetupAction()
            {
                Action = new RecipeTagsChangedAction(ExpectedState.Editor.Recipe!.RecipeTagIds);
            }

            public void SetupActionForRecipeNull()
            {
                Action = new DomainTestBuilder<RecipeTagsChangedAction>().Create();
            }
        }
    }

    public class OnCreateNewRecipeTagFinished
    {
        private readonly OnCreateNewRecipeTagFinishedFixture _fixture = new();

        [Fact]
        public void OnCreateNewRecipeTagFinished_WithValidData_ShouldSetRecipeTagIds()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnCreateNewRecipeTagFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateNewRecipeTagFinished_WithRecipeNull_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateWithRecipeNull();
            _fixture.SetupExpectedStateWithRecipeNull();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnCreateNewRecipeTagFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnCreateNewRecipeTagFinishedFixture : RecipeEditorReducerFixture
        {
            public CreateNewRecipeTagFinishedAction? Action { get; private set; }

            public void SetupExpectedState()
            {
                var tag = ExpectedState.RecipeTags.ElementAt(1);
                var recipeTagIds = ExpectedState.Editor.Recipe!.RecipeTagIds.ToList();
                recipeTagIds.RemoveAt(recipeTagIds.Count - 1);
                recipeTagIds.Add(tag.Id);

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            RecipeTagIds = recipeTagIds
                        }
                    }
                };
            }

            public void SetupInitialState()
            {
                var allTags = ExpectedState.RecipeTags.ToList();
                allTags.RemoveAt(1);

                var recipeTagIds = ExpectedState.Editor.Recipe!.RecipeTagIds.ToList();
                recipeTagIds.RemoveAt(recipeTagIds.Count - 1);

                InitialState = ExpectedState with
                {
                    RecipeTags = allTags,
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            RecipeTagIds = recipeTagIds
                        }
                    }
                };
            }

            public void SetupInitialStateWithRecipeNull()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
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

            public void SetupAction()
            {
                Action = new CreateNewRecipeTagFinishedAction(ExpectedState.RecipeTags.ElementAt(1));
            }

            public void SetupActionForRecipeNull()
            {
                Action = new DomainTestBuilder<CreateNewRecipeTagFinishedAction>().Create();
            }
        }
    }

    public class OnRecipeNumberOfServingsChanged
    {
        private readonly OnRecipeNumberOfServingsChangedFixture _fixture = new();

        [Fact]
        public void OnRecipeNumberOfServingsChanged_WithValidData_ShouldSetNumberOfServings()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnRecipeNumberOfServingsChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnRecipeNumberOfServingsChanged_WithRecipeNull_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateWithRecipeNull();
            _fixture.SetupExpectedStateWithRecipeNull();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnRecipeNumberOfServingsChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnRecipeNumberOfServingsChangedFixture : RecipeEditorReducerFixture
        {
            public RecipeNumberOfServingsChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            NumberOfServings = new DomainTestBuilder<int>().Create()
                        }
                    }
                };
            }

            public void SetupInitialStateWithRecipeNull()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
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

            public void SetupAction()
            {
                Action = new RecipeNumberOfServingsChangedAction(ExpectedState.Editor.Recipe!.NumberOfServings);
            }

            public void SetupActionForRecipeNull()
            {
                Action = new DomainTestBuilder<RecipeNumberOfServingsChangedAction>().Create();
            }
        }
    }

    public class OnModifyRecipeStarted
    {
        private readonly OnModifyRecipeStartedFixture _fixture = new();

        [Fact]
        public void OnModifyRecipeStarted_WithNotSaving_ShouldSetSaving()
        {
            // Arrange
            _fixture.SetupInitialStateNotSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnModifyRecipeStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnModifyRecipeStarted_WithSaving_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnModifyRecipeStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.InitialState);
        }

        private sealed class OnModifyRecipeStartedFixture : RecipeEditorReducerFixture
        {
            public void SetupInitialStateNotSaving()
            {
                SetupInitialState(false);
            }

            public void SetupInitialStateSaving()
            {
                SetupInitialState(true);
            }

            private void SetupInitialState(bool isSaving)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = isSaving,
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = true
                    }
                };
            }
        }
    }

    public class OnModifyRecipeFinished
    {
        private readonly OnModifyRecipeFinishedFixture _fixture = new();

        [Fact]
        public void OnModifyRecipeFinished_WithSaving_ShouldSetNotSaving()
        {
            // Arrange
            _fixture.SetupInitialStateSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnModifyRecipeFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnModifyRecipeFinished_WithNotSaving_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateNotSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnModifyRecipeFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.InitialState);
        }

        private sealed class OnModifyRecipeFinishedFixture : RecipeEditorReducerFixture
        {
            public void SetupInitialStateNotSaving()
            {
                SetupInitialState(false);
            }

            public void SetupInitialStateSaving()
            {
                SetupInitialState(true);
            }

            private void SetupInitialState(bool isSaving)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = isSaving,
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = false
                    }
                };
            }
        }
    }

    public class OnCreateRecipeStarted
    {
        private readonly OnCreateRecipeStartedFixture _fixture = new();

        [Fact]
        public void OnCreateRecipeStarted_WithNotSaving_ShouldSetSaving()
        {
            // Arrange
            _fixture.SetupInitialStateNotSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnCreateRecipeStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateRecipeStarted_WithSaving_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnCreateRecipeStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.InitialState);
        }

        private sealed class OnCreateRecipeStartedFixture : RecipeEditorReducerFixture
        {
            public void SetupInitialStateNotSaving()
            {
                SetupInitialState(false);
            }

            public void SetupInitialStateSaving()
            {
                SetupInitialState(true);
            }

            private void SetupInitialState(bool isSaving)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = isSaving,
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = true
                    }
                };
            }
        }
    }

    public class OnCreateRecipeFinished
    {
        private readonly OnCreateRecipeFinishedFixture _fixture = new();

        [Fact]
        public void OnCreateRecipeFinished_WithSaving_ShouldSetNotSaving()
        {
            // Arrange
            _fixture.SetupInitialStateSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnCreateRecipeFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateRecipeFinished_WithNotSaving_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateNotSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnCreateRecipeFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.InitialState);
        }

        private sealed class OnCreateRecipeFinishedFixture : RecipeEditorReducerFixture
        {
            public void SetupInitialStateNotSaving()
            {
                SetupInitialState(false);
            }

            public void SetupInitialStateSaving()
            {
                SetupInitialState(true);
            }

            private void SetupInitialState(bool isSaving)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = isSaving,
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = false
                    }
                };
            }
        }
    }

    private abstract class RecipeEditorReducerFixture
    {
        public RecipeState ExpectedState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
        public RecipeState InitialState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
    }
}