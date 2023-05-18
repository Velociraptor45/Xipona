using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Recipes.Reducers;

public class RecipeEditorReducerTests
{
    public class OnSetNewRecipe
    {
        private readonly OnSetNewRecipeFixture _fixture;

        public OnSetNewRecipe()
        {
            _fixture = new OnSetNewRecipeFixture();
        }

        [Fact]
        public void OnSetNewRecipe_WithValidData_ShouldSetRecipeAndLeaveEditMode()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnSetNewRecipe(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
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
                        IsInEditMode = true
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
                            new List<EditedIngredient>(0),
                            new SortedSet<EditedPreparationStep>(),
                            new List<Guid>(0)),
                        IsInEditMode = false
                    }
                };
            }
        }
    }

    public class OnLoadRecipeForEditingFinished
    {
        private readonly OnLoadRecipeForEditingFinishedFixture _fixture;

        public OnLoadRecipeForEditingFinished()
        {
            _fixture = new OnLoadRecipeForEditingFinishedFixture();
        }

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
                        IsInEditMode = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsInEditMode = false
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
        private readonly OnRecipeNameChangedFixture _fixture;

        public OnRecipeNameChanged()
        {
            _fixture = new OnRecipeNameChangedFixture();
        }

        [Fact]
        public void OnRecipeNameChanged_WithValidData_ShouldSetName()
        {
            // Arrange
            _fixture.SetupInitialState();
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
                            Name = new DomainTestBuilder<string>().Create()
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
        private readonly OnToggleEditModeFixture _fixture;

        public OnToggleEditMode()
        {
            _fixture = new OnToggleEditModeFixture();
        }

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

            public void SetupInitialState(bool isInEditMode)
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

            public void SetupExpectedState(bool isInEditMode)
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
        private readonly OnRecipeTagInputChangedFixture _fixture;

        public OnRecipeTagInputChanged()
        {
            _fixture = new OnRecipeTagInputChangedFixture();
        }

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
        private readonly OnRecipeTagsDropdownClosedFixture _fixture;

        public OnRecipeTagsDropdownClosed()
        {
            _fixture = new OnRecipeTagsDropdownClosedFixture();
        }

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
        private readonly OnRecipeTagsChangedFixture _fixture;

        public OnRecipeTagsChanged()
        {
            _fixture = new OnRecipeTagsChangedFixture();
        }

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
        private readonly OnCreateNewRecipeTagFinishedFixture _fixture;

        public OnCreateNewRecipeTagFinished()
        {
            _fixture = new OnCreateNewRecipeTagFinishedFixture();
        }

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
        private readonly OnRecipeNumberOfServingsChangedFixture _fixture;

        public OnRecipeNumberOfServingsChanged()
        {
            _fixture = new OnRecipeNumberOfServingsChangedFixture();
        }

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

    private abstract class RecipeEditorReducerFixture
    {
        public RecipeState ExpectedState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
        public RecipeState InitialState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
    }
}