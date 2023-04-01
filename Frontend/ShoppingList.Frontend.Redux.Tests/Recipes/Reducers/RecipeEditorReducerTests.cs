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
                            new List<EditedIngredient>(0),
                            new SortedSet<EditedPreparationStep>()),
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

    private abstract class RecipeEditorReducerFixture
    {
        public RecipeState ExpectedState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
        public RecipeState InitialState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
    }
}