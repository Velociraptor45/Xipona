using Moq;
using Moq.Contrib.InOrder;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions.Editor;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions.Editor.AddToShoppingListModal;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.Effects;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Shared.Ports;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Recipes.Effects;

public class RecipeEditorEffectsTests
{
    public class HandleInitializeRecipe
    {
        private readonly HandleInitializeRecipeFixture _fixture = new();

        [Fact]
        public async Task HandleInitializeRecipe_WithQuantityTypesAlreadyInState_ShouldNotLoadQuantityTypes()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupActionForLoadingRecipe();
                _fixture.SetupStateWithQuantityTypes();
                _fixture.SetupDispatchingLoadRecipeTagsAction();
                _fixture.SetupDispatchingLoadRecipeAction();
            });

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await _fixture.CreateSut().HandleInitializeRecipe(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleInitializeRecipe_WithApiCallSuccessful_WithRecipeId_ShouldLoadQuantityTypesAndDispatchActionForExistingRecipe()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupActionForLoadingRecipe();
                _fixture.SetupQuantityTypes();
                _fixture.SetupStateWithoutQuantityTypes();
                _fixture.SetupDispatchingLoadRecipeTagsAction();
                _fixture.SetupGettingQuantityTypes();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingLoadRecipeAction();
            });

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await _fixture.CreateSut().HandleInitializeRecipe(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleInitializeRecipe_WithApiCallSuccessful_WithEmptyRecipeId_ShouldLoadQuantityTypesAndDispatchActionForNewRecipe()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupActionForNewRecipe();
                _fixture.SetupQuantityTypes();
                _fixture.SetupStateWithoutQuantityTypes();
                _fixture.SetupDispatchingLoadRecipeTagsAction();
                _fixture.SetupGettingQuantityTypes();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingSetNewRecipeAction();
            });

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await _fixture.CreateSut().HandleInitializeRecipe(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleInitializeRecipe_WithWithApiException_ShouldDispatchApiExceptionAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupActionForLoadingRecipe();
                _fixture.SetupQuantityTypes();
                _fixture.SetupStateWithoutQuantityTypes();
                _fixture.SetupDispatchingLoadRecipeTagsAction();
                _fixture.SetupGettingQuantityTypesThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingLoadRecipeAction();
            });

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await _fixture.CreateSut().HandleInitializeRecipe(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleInitializeRecipe_WithWithHttpRequestException_ShouldDispatchErrorAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupActionForLoadingRecipe();
                _fixture.SetupQuantityTypes();
                _fixture.SetupStateWithoutQuantityTypes();
                _fixture.SetupDispatchingLoadRecipeTagsAction();
                _fixture.SetupGettingQuantityTypesThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorNotificationAction();
                _fixture.SetupDispatchingLoadRecipeAction();
            });

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await _fixture.CreateSut().HandleInitializeRecipe(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleInitializeRecipeFixture : RecipeEditorEffectsFixture
        {
            private readonly Guid _recipeId = Guid.NewGuid();
            private IReadOnlyCollection<IngredientQuantityType>? _quantityTypes;

            public InitializeRecipeAction? Action { get; private set; }

            public void SetupStateWithoutQuantityTypes()
            {
                State = State with { IngredientQuantityTypes = new List<IngredientQuantityType>() };
            }

            public void SetupStateWithQuantityTypes()
            {
                State = State with
                {
                    IngredientQuantityTypes = new DomainTestBuilder<IngredientQuantityType>().CreateMany(2).ToList()
                };
            }

            public void SetupGettingQuantityTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);
                TestPropertyNotSetException.ThrowIfNull(_quantityTypes);
                ApiClientMock.SetupGetAllIngredientQuantityTypes(_quantityTypes);
            }

            public void SetupQuantityTypes()
            {
                _quantityTypes = new DomainTestBuilder<IngredientQuantityType>().CreateMany(2).ToList();
            }

            public void SetupGettingQuantityTypesThrowsApiException()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);
                ApiClientMock.SetupGetAllIngredientQuantityTypesThrowing(
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingQuantityTypesThrowsHttpRequestException()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);
                ApiClientMock.SetupGetAllIngredientQuantityTypesThrowing(
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_quantityTypes);
                SetupDispatchingAction(new LoadIngredientQuantityTypesFinishedAction(_quantityTypes));
            }

            public void SetupDispatchingLoadRecipeTagsAction()
            {
                SetupDispatchingAnyAction<LoadRecipeTagsAction>();
            }

            public void SetupDispatchingSetNewRecipeAction()
            {
                SetupDispatchingAnyAction<SetNewRecipeAction>();
            }

            public void SetupDispatchingLoadRecipeAction()
            {
                SetupDispatchingAction(new LoadRecipeForEditingAction(_recipeId));
            }

            public void SetupActionForLoadingRecipe()
            {
                Action = new InitializeRecipeAction(_recipeId);
            }

            public void SetupActionForNewRecipe()
            {
                Action = new InitializeRecipeAction(Guid.Empty);
            }
        }
    }

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
                _fixture.SetupDispatchingExceptionNotificationAction();
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
                _fixture.SetupDispatchingErrorNotificationAction();
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
        public async Task HandleModifyRecipeAction_WithValidationErrors_ShouldNotDoAnything()
        {
            // Arrange
            _fixture.SetupRecipe();
            _fixture.SetupState();
            _fixture.SetupValidationErrors();
            var queue = CallQueue.Create(_ => { });

            // Act
            await _fixture.CreateSut().HandleModifyRecipeAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

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
                _fixture.SetupSuccessNotification();
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
                _fixture.SetupDispatchingExceptionNotificationAction();
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
                _fixture.SetupDispatchingErrorNotificationAction();
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
                SetupDispatchingAction(new LeaveRecipeEditorAction(true));
            }

            public void SetupSuccessNotification()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                ShoppingListNotificationServiceMock.SetupNotifySuccess($"Successfully modified recipe {_recipe.Name}");
            }
        }
    }

    public class HandleCreateRecipeAction
    {
        private readonly HandleCreateRecipeActionFixture _fixture = new();

        [Fact]
        public async Task HandleCreateRecipeAction_WithValidationErrors_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupRecipe();
            _fixture.SetupState();
            _fixture.SetupValidationErrors();
            var queue = CallQueue.Create(_ => { });

            // Act
            await _fixture.CreateSut().HandleCreateRecipeAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

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
                _fixture.SetupSuccessNotification();
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
                _fixture.SetupDispatchingExceptionNotificationAction();
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
                _fixture.SetupDispatchingErrorNotificationAction();
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
                SetupDispatchingAction(new LeaveRecipeEditorAction(true));
            }

            public void SetupSuccessNotification()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                ShoppingListNotificationServiceMock.SetupNotifySuccess($"Successfully created recipe {_recipe.Name}");
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
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleCreateNewRecipeTagAction(_fixture.DispatcherMock.Object);

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
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleCreateNewRecipeTagAction(_fixture.DispatcherMock.Object);

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
                _fixture.SetupDispatchingExceptionNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleCreateNewRecipeTagAction(_fixture.DispatcherMock.Object);

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
                _fixture.SetupDispatchingErrorNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleCreateNewRecipeTagAction(_fixture.DispatcherMock.Object);

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

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeTag);
                SetupDispatchingAction(new CreateNewRecipeTagFinishedAction(_recipeTag));
            }
        }
    }

    public class HandleLoadAddToShoppingListAction
    {
        private readonly HandleLoadAddToShoppingListActionFixture _fixture = new();

        [Fact]
        public async Task HandleLoadAddToShoppingListAction_WithRecipeNull_ShouldDoNothing()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupRecipeNull();
            });

            // Act
            await _fixture.CreateSut().HandleLoadAddToShoppingListAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadAddToShoppingListAction_WithApiCallSuccessful_ShouldDispatchCorrectActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupRecipeTagInput();
                _fixture.SetupState();
                _fixture.SetupGettingItemAmounts();
                _fixture.SetupDispatchingFinishedAction();
            });

            // Act
            await _fixture.CreateSut().HandleLoadAddToShoppingListAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadAddToShoppingListAction_WithWithApiException_ShouldDispatchCorrectActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupRecipeTagInput();
                _fixture.SetupState();
                _fixture.SetupGettingItemAmountsThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });

            // Act
            await _fixture.CreateSut().HandleLoadAddToShoppingListAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadAddToShoppingListAction_WithWithHttpRequestException_ShouldDispatchCorrectActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupRecipeTagInput();
                _fixture.SetupState();
                _fixture.SetupGettingItemAmountsThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorNotificationAction();
            });

            // Act
            await _fixture.CreateSut().HandleLoadAddToShoppingListAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadAddToShoppingListActionFixture : RecipeEditorEffectsFixture
        {
            private Guid? _recipeId;
            private IReadOnlyCollection<AddToShoppingListItem>? _itemAmounts;

            public void SetupGettingItemAmounts()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeId);

                _itemAmounts = new DomainTestBuilder<AddToShoppingListItem>().CreateMany(3).ToList();
                ApiClientMock.SetupGetItemAmountsForOneServingAsync(_recipeId.Value, _itemAmounts);
            }

            public void SetupRecipeTagInput()
            {
                _recipeId = Guid.NewGuid();
            }

            public void SetupRecipeNull()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupGettingItemAmountsThrowsApiException()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeId);
                ApiClientMock.SetupGetItemAmountsForOneServingAsyncThrowing(
                    _recipeId.Value,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingItemAmountsThrowsHttpRequestException()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeId);
                ApiClientMock.SetupGetItemAmountsForOneServingAsyncThrowing(
                    _recipeId.Value,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupState()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeId);
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Recipe = State.Editor.Recipe! with
                        {
                            Id = _recipeId.Value
                        }
                    }
                };
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemAmounts);
                SetupDispatchingAction(new LoadAddToShoppingListFinishedAction(_itemAmounts));
            }
        }
    }

    public class HandleAddItemsToShoppingListAction
    {
        private readonly HandleAddItemsToShoppingListActionFixture _fixture = new();

        [Fact]
        public async Task HandleAddItemsToShoppingListAction_WithApiCallSuccessful_ShouldDispatchCorrectActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupItems();
                _fixture.SetupState();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupAddingToShoppingList();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingCloseAction();
                _fixture.SetupSuccessNotification();
            });

            // Act
            await _fixture.CreateSut().HandleAddItemsToShoppingListAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleAddItemsToShoppingListAction_WithAddToShoppingListNull_ShouldDoNothing()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStateWithAddToShoppingListNull();
            });

            // Act
            await _fixture.CreateSut().HandleAddItemsToShoppingListAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleAddItemsToShoppingListAction_WithWithApiException_ShouldDispatchCorrectActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupItems();
                _fixture.SetupState();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupAddingToShoppingListThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
            });

            // Act
            await _fixture.CreateSut().HandleAddItemsToShoppingListAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleAddItemsToShoppingListAction_WithWithHttpRequestException_ShouldDispatchCorrectActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupItems();
                _fixture.SetupState();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupAddingToShoppingListThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
            });

            // Act
            await _fixture.CreateSut().HandleAddItemsToShoppingListAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleAddItemsToShoppingListActionFixture : RecipeEditorEffectsFixture
        {
            private IReadOnlyCollection<AddToShoppingListItem>? _items;

            public void SetupAddingToShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_items);
                ApiClientMock.SetupAddItemsToShoppingListsAsync(_items.Take(2));
            }

            public void SetupItems()
            {
                _items = new List<AddToShoppingListItem>
                {
                    new DomainTestBuilder<AddToShoppingListItem>().Create() with { AddToShoppingList = true },
                    new DomainTestBuilder<AddToShoppingListItem>().Create() with { AddToShoppingList = true },
                    new DomainTestBuilder<AddToShoppingListItem>().Create() with { AddToShoppingList = false },
                };
            }

            public void SetupAddingToShoppingListThrowsApiException()
            {
                TestPropertyNotSetException.ThrowIfNull(_items);
                ApiClientMock.SetupAddItemsToShoppingListsAsyncThrowing(
                    _items.Take(2),
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupAddingToShoppingListThrowsHttpRequestException()
            {
                TestPropertyNotSetException.ThrowIfNull(_items);
                ApiClientMock.SetupAddItemsToShoppingListsAsyncThrowing(
                    _items.Take(2),
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupState()
            {
                TestPropertyNotSetException.ThrowIfNull(_items);

                State = State with
                {
                    Editor = State.Editor with
                    {
                        AddToShoppingList = State.Editor.AddToShoppingList! with
                        {
                            Items = _items
                        }
                    }
                };
            }

            public void SetupStateWithAddToShoppingListNull()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        AddToShoppingList = null
                    }
                };
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<AddItemsToShoppingListStartedAction>();
            }

            public void SetupDispatchingFinishedAction()
            {
                SetupDispatchingAction<AddItemsToShoppingListFinishedAction>();
            }

            public void SetupDispatchingCloseAction()
            {
                SetupDispatchingAction<AddToShoppingListModalClosedAction>();
            }

            public void SetupSuccessNotification()
            {
                ShoppingListNotificationServiceMock.SetupNotifySuccess("Successfully added items to shopping lists");
            }
        }
    }

    private abstract class RecipeEditorEffectsFixture : RecipeEffectsFixtureBase
    {
        protected ShoppingListNotificationServiceMock ShoppingListNotificationServiceMock { get; } =
            new(MockBehavior.Strict);

        public RecipeEditorEffects CreateSut()
        {
            SetupStateReturningState();
            return new RecipeEditorEffects(ApiClientMock.Object, RecipeStateMock.Object, NavigationManagerMock.Object,
                ShoppingListNotificationServiceMock.Object);
        }

        public void SetupValidationErrors()
        {
            State = State with
            {
                Editor = State.Editor with
                {
                    ValidationResult = new DomainTestBuilder<EditorValidationResult>().Create()
                }
            };
        }
    }
}