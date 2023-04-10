using Moq.Contrib.InOrder;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Effects;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;
using RestEase;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Recipes.Effects;

public class RecipeEditorEffectsTests
{
    public class HandleLoadRecipeForEditing
    {
        private readonly HandleLoadRecipeForEditingFixture _fixture = new();

        [Fact]
        public async Task HandleLoadRecipeForEditing_WithApiCallSuccessful_ShouldDispatchFinishAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupAction();
                _fixture.SetupRecipe();
                _fixture.SetupGettingRecipeById();
                _fixture.SetupDispatchingFinishedAction();
            });

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);
            
            // Act
            await _fixture.CreateSut().HandleLoadRecipeForEditing(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadRecipeForEditing_WithWithApiException_ShouldDispatchApiExceptionAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupAction();
                _fixture.SetupRecipe();
                _fixture.SetupGettingRecipeByIdThrowsApiException();
                _fixture.SetupDispatchingApiExceptionAction();
            });

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await _fixture.CreateSut().HandleLoadRecipeForEditing(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadRecipeForEditing_WithWithHttpRequestException_ShouldDispatchErrorAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupAction();
                _fixture.SetupRecipe();
                _fixture.SetupGettingRecipeByIdThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorAction();
            });
            
            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await _fixture.CreateSut().HandleLoadRecipeForEditing(_fixture.Action, _fixture.DispatcherMock.Object);
            
            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadRecipeForEditingFixture : RecipeEditorEffectsFixture
        {
            private EditedRecipe? _recipe;


            public LoadRecipeForEditingAction? Action { get; private set; }


            public void SetupGettingRecipeById()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                ApiClientMock.SetupGetRecipeByIdAsync(Action.RecipeId, _recipe);
            }

            public void SetupRecipe()
            {
                _recipe = new DomainTestBuilder<EditedRecipe>().Create();
            }

            public void SetupGettingRecipeByIdThrowsApiException()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);
                ApiClientMock.SetupGetRecipeByIdAsyncThrowing(
                    Action.RecipeId,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingRecipeByIdThrowsHttpRequestException()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);
                ApiClientMock.SetupGetRecipeByIdAsyncThrowing(
                    Action.RecipeId,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingApiExceptionAction()
            {
                SetupDispatchingAnyAction<DisplayApiExceptionNotificationAction>();
            }

            public void SetupDispatchingErrorAction()
            {
                SetupDispatchingAnyAction<DisplayErrorNotificationAction>();
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                SetupDispatchingAction(new LoadRecipeForEditingFinishedAction(_recipe));
            }

            public void SetupAction()
            {
                Action = new DomainTestBuilder<LoadRecipeForEditingAction>().Create();
            }
        }
    }

    public class HandleModifyRecipeAction
    {
        private readonly HandleModifyRecipeActionFixture _fixture = new();

        [Fact]
        public async Task HandleModifyRecipeAction_WithApiCallSuccessful_ShouldDispatchCorrectActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupRecipe();
                _fixture.SetupState();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupModifyingRecipe();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingLeaveAction();
            });
            
            // Act
            await _fixture.CreateSut().HandleModifyRecipeAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleModifyRecipeAction_WithWithApiException_ShouldDispatchCorrectActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupRecipe();
                _fixture.SetupState();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupModifyingRecipeThrowsApiException();
                _fixture.SetupDispatchingApiExceptionAction();
                _fixture.SetupDispatchingFinishedAction();
            });

            // Act
            await _fixture.CreateSut().HandleModifyRecipeAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleModifyRecipeAction_WithWithHttpRequestException_ShouldDispatchCorrectActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupRecipe();
                _fixture.SetupState();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupModifyingRecipeThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorAction();
                _fixture.SetupDispatchingFinishedAction();
            });

            // Act
            await _fixture.CreateSut().HandleModifyRecipeAction(_fixture.DispatcherMock.Object);
            
            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleModifyRecipeActionFixture : RecipeEditorEffectsFixture
        {
            private EditedRecipe? _recipe;

            public void SetupModifyingRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                ApiClientMock.SetupModifyRecipeAsync(_recipe);
            }

            public void SetupRecipe()
            {
                _recipe = new DomainTestBuilder<EditedRecipe>().Create();
            }

            public void SetupModifyingRecipeThrowsApiException()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                ApiClientMock.SetupModifyRecipeAsyncThrowing(
                    _recipe,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupModifyingRecipeThrowsHttpRequestException()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                ApiClientMock.SetupModifyRecipeAsyncThrowing(
                    _recipe,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupState()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Recipe = _recipe
                    }
                };
            }

            public void SetupDispatchingApiExceptionAction()
            {
                SetupDispatchingAnyAction<DisplayApiExceptionNotificationAction>();
            }

            public void SetupDispatchingErrorAction()
            {
                SetupDispatchingAnyAction<DisplayErrorNotificationAction>();
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<ModifyRecipeStartedAction>();
            }

            public void SetupDispatchingFinishedAction()
            {
                SetupDispatchingAction<ModifyRecipeFinishedAction>();
            }

            public void SetupDispatchingLeaveAction()
            {
                SetupDispatchingAction<LeaveRecipeEditorAction>();
            }
        }
    }

    public class HandleCreateRecipeAction
    {
        private readonly HandleCreateRecipeActionFixture _fixture = new();

        [Fact]
        public async Task HandleCreateRecipeAction_WithApiCallSuccessful_ShouldDispatchCorrectActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupRecipe();
                _fixture.SetupState();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupCreatingRecipe();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingLeaveAction();
            });
            
            // Act
            await _fixture.CreateSut().HandleCreateRecipeAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateRecipeAction_WithWithApiException_ShouldDispatchCorrectActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupRecipe();
                _fixture.SetupState();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupCreatingRecipeThrowsApiException();
                _fixture.SetupDispatchingApiExceptionAction();
                _fixture.SetupDispatchingFinishedAction();
            });

            // Act
            await _fixture.CreateSut().HandleCreateRecipeAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateRecipeAction_WithWithHttpRequestException_ShouldDispatchCorrectActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupRecipe();
                _fixture.SetupState();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupCreatingRecipeThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorAction();
                _fixture.SetupDispatchingFinishedAction();
            });

            // Act
            await _fixture.CreateSut().HandleCreateRecipeAction(_fixture.DispatcherMock.Object);
            
            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleCreateRecipeActionFixture : RecipeEditorEffectsFixture
        {
            private EditedRecipe? _recipe;

            public void SetupCreatingRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                ApiClientMock.SetupCreateRecipeAsync(_recipe, _recipe);
            }

            public void SetupRecipe()
            {
                _recipe = new DomainTestBuilder<EditedRecipe>().Create();
            }

            public void SetupCreatingRecipeThrowsApiException()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                ApiClientMock.SetupCreateRecipeAsyncThrowing(
                    _recipe,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupCreatingRecipeThrowsHttpRequestException()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                ApiClientMock.SetupCreateRecipeAsyncThrowing(
                    _recipe,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupState()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Recipe = _recipe
                    }
                };
            }

            public void SetupDispatchingApiExceptionAction()
            {
                SetupDispatchingAnyAction<DisplayApiExceptionNotificationAction>();
            }

            public void SetupDispatchingErrorAction()
            {
                SetupDispatchingAnyAction<DisplayErrorNotificationAction>();
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<CreateRecipeStartedAction>();
            }

            public void SetupDispatchingFinishedAction()
            {
                SetupDispatchingAction<CreateRecipeFinishedAction>();
            }

            public void SetupDispatchingLeaveAction()
            {
                SetupDispatchingAction<LeaveRecipeEditorAction>();
            }
        }
    }

    public class HandleCreateNewRecipeTagAction
    {
        private readonly HandleCreateNewRecipeTagActionFixture _fixture = new();

        [Fact]
        public async Task HandleCreateNewRecipeTagAction_WithRecipeTagInputEmpty_ShouldDoNothing()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupRecipeTagInputEmpty();
                _fixture.SetupState();
            });

            // Act
            await _fixture.CreateSut().HandleCreateNewRecipeTagAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateNewRecipeTagAction_WithApiCallSuccessful_ShouldDispatchCorrectActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupRecipeTagInput();
                _fixture.SetupState();
                _fixture.SetupCreatingRecipeTag();
                _fixture.SetupDispatchingFinishedAction();
            });
            
            // Act
            await _fixture.CreateSut().HandleCreateNewRecipeTagAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateNewRecipeTagAction_WithWithApiException_ShouldDispatchCorrectActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupRecipeTagInput();
                _fixture.SetupState();
                _fixture.SetupCreatingRecipeTagThrowsApiException();
                _fixture.SetupDispatchingApiExceptionAction();
            });

            // Act
            await _fixture.CreateSut().HandleCreateNewRecipeTagAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateNewRecipeTagAction_WithWithHttpRequestException_ShouldDispatchCorrectActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupRecipeTagInput();
                _fixture.SetupState();
                _fixture.SetupCreatingRecipeTagThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorAction();
            });

            // Act
            await _fixture.CreateSut().HandleCreateNewRecipeTagAction(_fixture.DispatcherMock.Object);
            
            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleCreateNewRecipeTagActionFixture : RecipeEditorEffectsFixture
        {
            private string? _recipeTagInput;
            private RecipeTag? _recipeTag;

            public void SetupCreatingRecipeTag()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeTagInput);

                _recipeTag = new DomainTestBuilder<RecipeTag>().Create();
                ApiClientMock.SetupCreateRecipeTagAsync(_recipeTagInput, _recipeTag);
            }

            public void SetupRecipeTagInput()
            {
                _recipeTagInput = new DomainTestBuilder<string>().Create();
            }

            public void SetupRecipeTagInputEmpty()
            {
                _recipeTagInput = string.Empty;
            }

            public void SetupCreatingRecipeTagThrowsApiException()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeTagInput);
                ApiClientMock.SetupCreateRecipeTagAsyncThrowing(
                    _recipeTagInput,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupCreatingRecipeTagThrowsHttpRequestException()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeTagInput);
                ApiClientMock.SetupCreateRecipeTagAsyncThrowing(
                    _recipeTagInput,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupState()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeTagInput);
                State = State with
                {
                    Editor = State.Editor with
                    {
                        RecipeTagCreateInput = _recipeTagInput
                    }
                };
            }

            public void SetupDispatchingApiExceptionAction()
            {
                SetupDispatchingAnyAction<DisplayApiExceptionNotificationAction>();
            }

            public void SetupDispatchingErrorAction()
            {
                SetupDispatchingAnyAction<DisplayErrorNotificationAction>();
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeTag);
                SetupDispatchingAction(new CreateNewRecipeTagFinishedAction(_recipeTag));
            }
        }
    }

    private abstract class RecipeEditorEffectsFixture : RecipeEffectsFixtureBase
    {
        public RecipeEditorEffects CreateSut()
        {
            SetupStateReturningState();
            return new RecipeEditorEffects(ApiClientMock.Object, RecipeStateMock.Object, NavigationManagerMock.Object);
        }
    }
}