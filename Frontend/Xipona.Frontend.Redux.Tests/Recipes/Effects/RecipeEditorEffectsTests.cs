using Moq;
using Moq.Contrib.InOrder;
using Xipona.Frontend.Redux.Recipes.Actions;
using Xipona.Frontend.Redux.Recipes.Actions.Editor;
using Xipona.Frontend.Redux.Recipes.Actions.Editor.AddToShoppingListModal;
using Xipona.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
using Xipona.Frontend.Redux.Recipes.Effects;
using Xipona.Frontend.Redux.Recipes.States;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.Shared.Ports;
using Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace Xipona.Frontend.Redux.Tests.Recipes.Effects;

public class RecipeEditorEffectsTests
{
    public class HandleInitializeRecipe
    {
        private readonly HandleInitializeRecipeFixture _fixture = new();

        [Fact]
        public async Task HandleInitializeRecipe_WithQuantityTypesAlreadyInState_ShouldNotLoadQuantityTypes()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupActionForLoadingRecipe();
                _fixture.SetupStateWithQuantityTypes();
                _fixture.SetupDispatchingLoadRecipeTagsAction(x0);
                _fixture.SetupDispatchingLoadRecipeAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupActionForLoadingRecipe();
                _fixture.SetupQuantityTypes();
                _fixture.SetupStateWithoutQuantityTypes();
                _fixture.SetupDispatchingLoadRecipeTagsAction(x0);
                _fixture.SetupGettingQuantityTypes(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingLoadRecipeAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupActionForNewRecipe();
                _fixture.SetupQuantityTypes();
                _fixture.SetupStateWithoutQuantityTypes();
                _fixture.SetupDispatchingLoadRecipeTagsAction(x0);
                _fixture.SetupGettingQuantityTypes(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingSetNewRecipeAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupActionForLoadingRecipe();
                _fixture.SetupQuantityTypes();
                _fixture.SetupStateWithoutQuantityTypes();
                _fixture.SetupDispatchingLoadRecipeTagsAction(x0);
                _fixture.SetupGettingQuantityTypesThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingLoadRecipeAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupActionForLoadingRecipe();
                _fixture.SetupQuantityTypes();
                _fixture.SetupStateWithoutQuantityTypes();
                _fixture.SetupDispatchingLoadRecipeTagsAction(x0);
                _fixture.SetupGettingQuantityTypesThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingLoadRecipeAction(x0);
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

            public void SetupGettingQuantityTypes(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(Action);
                TestPropertyNotSetException.ThrowIfNull(_quantityTypes);
                ApiClientMock.SetupGetAllIngredientQuantityTypes(_quantityTypes, component);
            }

            public void SetupQuantityTypes()
            {
                _quantityTypes = new DomainTestBuilder<IngredientQuantityType>().CreateMany(2).ToList();
            }

            public void SetupGettingQuantityTypesThrowsApiException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(Action);
                ApiClientMock.SetupGetAllIngredientQuantityTypesThrowing(
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupGettingQuantityTypesThrowsHttpRequestException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(Action);
                ApiClientMock.SetupGetAllIngredientQuantityTypesThrowing(
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_quantityTypes);
                SetupDispatchingAction(new LoadIngredientQuantityTypesFinishedAction(_quantityTypes), component);
            }

            public void SetupDispatchingLoadRecipeTagsAction(IQueueComponent component)
            {
                SetupDispatchingAnyAction<LoadRecipeTagsAction>(component);
            }

            public void SetupDispatchingSetNewRecipeAction(IQueueComponent component)
            {
                SetupDispatchingAnyAction<SetNewRecipeAction>(component);
            }

            public void SetupDispatchingLoadRecipeAction(IQueueComponent component)
            {
                SetupDispatchingAction(new LoadRecipeForEditingAction(_recipeId), component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupAction();
                _fixture.SetupRecipe();
                _fixture.SetupGettingRecipeById(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupAction();
                _fixture.SetupRecipe();
                _fixture.SetupGettingRecipeByIdThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupAction();
                _fixture.SetupRecipe();
                _fixture.SetupGettingRecipeByIdThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
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

            public void SetupGettingRecipeById(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(Action);
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                ApiClientMock.SetupGetRecipeByIdAsync(Action.RecipeId, _recipe, component);
            }

            public void SetupRecipe()
            {
                _recipe = new DomainTestBuilder<EditedRecipe>().Create();
            }

            public void SetupGettingRecipeByIdThrowsApiException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(Action);
                ApiClientMock.SetupGetRecipeByIdAsyncThrowing(
                    Action.RecipeId,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupGettingRecipeByIdThrowsHttpRequestException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(Action);
                ApiClientMock.SetupGetRecipeByIdAsyncThrowing(
                    Action.RecipeId,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                SetupDispatchingAction(new LoadRecipeForEditingFinishedAction(_recipe), component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupRecipe();
                _fixture.SetupState();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupModifyingRecipe(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingLeaveAction(x0);
                _fixture.SetupSuccessNotification(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupRecipe();
                _fixture.SetupState();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupModifyingRecipeThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupRecipe();
                _fixture.SetupState();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupModifyingRecipeThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
            });

            // Act
            await _fixture.CreateSut().HandleModifyRecipeAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleModifyRecipeActionFixture : RecipeEditorEffectsFixture
        {
            private EditedRecipe? _recipe;

            public void SetupModifyingRecipe(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                ApiClientMock.SetupModifyRecipeAsync(_recipe, component);
            }

            public void SetupRecipe()
            {
                _recipe = new DomainTestBuilder<EditedRecipe>().Create();
            }

            public void SetupModifyingRecipeThrowsApiException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                ApiClientMock.SetupModifyRecipeAsyncThrowing(
                    _recipe,
                    new DomainTestBuilder<ApiException>().Create(),
                    component);
            }

            public void SetupModifyingRecipeThrowsHttpRequestException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                ApiClientMock.SetupModifyRecipeAsyncThrowing(
                    _recipe,
                    new DomainTestBuilder<HttpRequestException>().Create(),
                    component);
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

            public void SetupDispatchingStartedAction(IQueueComponent component)
            {
                SetupDispatchingAction<ModifyRecipeStartedAction>(component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                SetupDispatchingAction<ModifyRecipeFinishedAction>(component);
            }

            public void SetupDispatchingLeaveAction(IQueueComponent component)
            {
                SetupDispatchingAction(new LeaveRecipeEditorAction(true), component);
            }

            public void SetupSuccessNotification(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                ShoppingListNotificationServiceMock
                    .SetupNotifySuccess($"Successfully modified recipe {_recipe.Name}", 2f, component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupRecipe();
                _fixture.SetupState();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupCreatingRecipe(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingLeaveAction(x0);
                _fixture.SetupSuccessNotification(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupRecipe();
                _fixture.SetupState();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupCreatingRecipeThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupRecipe();
                _fixture.SetupState();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupCreatingRecipeThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
            });

            // Act
            await _fixture.CreateSut().HandleCreateRecipeAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleCreateRecipeActionFixture : RecipeEditorEffectsFixture
        {
            private EditedRecipe? _recipe;

            public void SetupCreatingRecipe(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                ApiClientMock.SetupCreateRecipeAsync(_recipe, _recipe, component);
            }

            public void SetupRecipe()
            {
                _recipe = new DomainTestBuilder<EditedRecipe>().Create();
            }

            public void SetupCreatingRecipeThrowsApiException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                ApiClientMock.SetupCreateRecipeAsyncThrowing(
                    _recipe,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupCreatingRecipeThrowsHttpRequestException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                ApiClientMock.SetupCreateRecipeAsyncThrowing(
                    _recipe,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
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

            public void SetupDispatchingStartedAction(IQueueComponent component)
            {
                SetupDispatchingAction<CreateRecipeStartedAction>(component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                SetupDispatchingAction<CreateRecipeFinishedAction>(component);
            }

            public void SetupDispatchingLeaveAction(IQueueComponent component)
            {
                SetupDispatchingAction(new LeaveRecipeEditorAction(true), component);
            }

            public void SetupSuccessNotification(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);
                ShoppingListNotificationServiceMock
                    .SetupNotifySuccess($"Successfully created recipe {_recipe.Name}", 2f, component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupRecipeTagInput();
                _fixture.SetupState();
                _fixture.SetupCreatingRecipeTag(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupRecipeTagInput();
                _fixture.SetupState();
                _fixture.SetupCreatingRecipeTagThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupRecipeTagInput();
                _fixture.SetupState();
                _fixture.SetupCreatingRecipeTagThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
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

            public void SetupCreatingRecipeTag(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeTagInput);

                _recipeTag = new DomainTestBuilder<RecipeTag>().Create();
                ApiClientMock.SetupCreateRecipeTagAsync(_recipeTagInput, _recipeTag, component);
            }

            public void SetupRecipeTagInput()
            {
                _recipeTagInput = new DomainTestBuilder<string>().Create();
            }

            public void SetupRecipeTagInputEmpty()
            {
                _recipeTagInput = string.Empty;
            }

            public void SetupCreatingRecipeTagThrowsApiException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeTagInput);
                ApiClientMock.SetupCreateRecipeTagAsyncThrowing(
                    _recipeTagInput,
                    new DomainTestBuilder<ApiException>().Create(),
                    component);
            }

            public void SetupCreatingRecipeTagThrowsHttpRequestException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeTagInput);
                ApiClientMock.SetupCreateRecipeTagAsyncThrowing(
                    _recipeTagInput,
                    new DomainTestBuilder<HttpRequestException>().Create(),
                    component);
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

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeTag);
                SetupDispatchingAction(new CreateNewRecipeTagFinishedAction(_recipeTag), component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupRecipeTagInput();
                _fixture.SetupState();
                _fixture.SetupGettingItemAmounts(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupRecipeTagInput();
                _fixture.SetupState();
                _fixture.SetupGettingItemAmountsThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupRecipeTagInput();
                _fixture.SetupState();
                _fixture.SetupGettingItemAmountsThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
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

            public void SetupGettingItemAmounts(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeId);

                _itemAmounts = new DomainTestBuilder<AddToShoppingListItem>().CreateMany(3).ToList();
                ApiClientMock.SetupGetItemAmountsForOneServingAsync(_recipeId.Value, _itemAmounts, component);
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

            public void SetupGettingItemAmountsThrowsApiException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeId);
                ApiClientMock.SetupGetItemAmountsForOneServingAsyncThrowing(
                    _recipeId.Value,
                    new DomainTestBuilder<ApiException>().Create(),
                    component);
            }

            public void SetupGettingItemAmountsThrowsHttpRequestException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeId);
                ApiClientMock.SetupGetItemAmountsForOneServingAsyncThrowing(
                    _recipeId.Value,
                    new DomainTestBuilder<HttpRequestException>().Create(), 
                    component);
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

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_itemAmounts);
                SetupDispatchingAction(new LoadAddToShoppingListFinishedAction(_itemAmounts), component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupItems();
                _fixture.SetupState();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupAddingToShoppingList(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingCloseAction(x0);
                _fixture.SetupSuccessNotification(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupItems();
                _fixture.SetupState();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupAddingToShoppingListThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupItems();
                _fixture.SetupState();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupAddingToShoppingListThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
            });

            // Act
            await _fixture.CreateSut().HandleAddItemsToShoppingListAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleAddItemsToShoppingListActionFixture : RecipeEditorEffectsFixture
        {
            private IReadOnlyCollection<AddToShoppingListItem>? _items;

            public void SetupAddingToShoppingList(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_items);
                ApiClientMock.SetupAddItemsToShoppingListsAsync(_items.Take(2), component);
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

            public void SetupAddingToShoppingListThrowsApiException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_items);
                ApiClientMock.SetupAddItemsToShoppingListsAsyncThrowing(
                    _items.Take(2),
                    new DomainTestBuilder<ApiException>().Create(),
                    component);
            }

            public void SetupAddingToShoppingListThrowsHttpRequestException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_items);
                ApiClientMock.SetupAddItemsToShoppingListsAsyncThrowing(
                    _items.Take(2),
                    new DomainTestBuilder<HttpRequestException>().Create(),
                    component);
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

            public void SetupDispatchingStartedAction(IQueueComponent component)
            {
                SetupDispatchingAction<AddItemsToShoppingListStartedAction>(component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                SetupDispatchingAction<AddItemsToShoppingListFinishedAction>(component);
            }

            public void SetupDispatchingCloseAction(IQueueComponent component)
            {
                SetupDispatchingAction<AddToShoppingListModalClosedAction>(component);
            }

            public void SetupSuccessNotification(IQueueComponent component)
            {
                ShoppingListNotificationServiceMock
                    .SetupNotifySuccess("Successfully added items to shopping lists", 2f, component);
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