using Moq;
using Moq.Contrib.InOrder;
using ProjectHermes.Xipona.Frontend.Redux.Stores.Actions.Editor;
using ProjectHermes.Xipona.Frontend.Redux.Stores.Effects;
using ProjectHermes.Xipona.Frontend.Redux.Stores.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Shared.Ports;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Stores.Effects;

public class StoreEditorEffectsTests
{
    public class HandleLoadStoreForEditingAction
    {
        private readonly HandleLoadStoreForEditingFixture _fixture = new();

        [Fact]
        public async Task HandleLoadStoreForEditingAction_WithValidStoreId_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupAction();
                _fixture.SetupGettingStore();
                _fixture.SetupDispatchingFinishAction();
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLoadStoreForEditingAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadStoreForEditingAction_WithApiException_ShouldDispatchErrorActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupAction();
                _fixture.SetupGettingStoreThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLoadStoreForEditingAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadStoreForEditingAction_WithHttpRequestException_ShouldDispatchErrorActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupAction();
                _fixture.SetupGettingStoreThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorNotificationAction();
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLoadStoreForEditingAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadStoreForEditingFixture : StoreEditorEffectsFixture
        {
            private EditedStore? _store;
            private readonly Guid _storeId = Guid.NewGuid();

            public LoadStoreForEditingAction? Action { get; private set; }

            public void SetupGettingStore()
            {
                _store = new DomainTestBuilder<EditedStore>().Create();
                ApiClientMock.SetupGetStoreByIdAsync(_storeId, _store);
            }

            public void SetupGettingStoreThrowsApiException()
            {
                ApiClientMock.SetupGetStoreByIdAsyncThrowing(_storeId, new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingStoreThrowsHttpRequestException()
            {
                ApiClientMock.SetupGetStoreByIdAsyncThrowing(_storeId, new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupAction()
            {
                Action = new LoadStoreForEditingAction(_storeId);
            }

            public void SetupDispatchingFinishAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);
                SetupDispatchingAction(new LoadStoreForEditingFinishedAction(_store));
            }
        }
    }

    public class HandleSaveStoreAction
    {
        private readonly HandleSaveStoreActionFixture _fixture = new();

        [Fact]
        public async Task HandleSaveStoreAction_WithStoreNull_ShouldNotDispatchAnything()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStoreNull();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveStoreAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSaveStoreAction_WithValidationErrors_ShouldNotDispatchAnything()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStoreId();
                _fixture.SetupValidationErrors();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveStoreAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSaveStoreAction_WithValidStoreId_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStoreId();
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupModifyingStore();
                _fixture.SetupSuccessModifyNotification();
                _fixture.SetupDispatchingFinishAction();
                _fixture.SetupDispatchingLeaveAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveStoreAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSaveStoreAction_WithStoreId_WithApiException_ShouldDispatchErrorActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStoreId();
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupModifyingStoreThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFinishAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveStoreAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSaveStoreAction_WithStoreId_WithHttpRequestException_ShouldDispatchErrorActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStoreId();
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupModifyingStoreThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorNotificationAction();
                _fixture.SetupDispatchingFinishAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveStoreAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSaveStoreAction_WithStoreIdEmpty_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStoreIdEmpty();
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupCreatingStore();
                _fixture.SetupSuccessCreateNotification();
                _fixture.SetupDispatchingFinishAction();
                _fixture.SetupDispatchingLeaveAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveStoreAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSaveStoreAction_WithStoreIdEmpty_WithApiException_ShouldDispatchErrorActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStoreIdEmpty();
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupCreatingStoreThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFinishAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveStoreAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSaveStoreAction_WithStoreIdEmpty_WithHttpRequestException_ShouldDispatchErrorActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStoreIdEmpty();
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupCreatingStoreThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorNotificationAction();
                _fixture.SetupDispatchingFinishAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveStoreAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSaveStoreActionFixture : StoreEditorEffectsFixture
        {
            private readonly string _storeName = new DomainTestBuilder<string>().Create();
            private Guid? _storeId;

            public void SetupStoreNull()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Store = null
                    }
                };
            }

            public void SetupStoreId()
            {
                _storeId = Guid.NewGuid();
                SetupStoreIdInState();
            }

            public void SetupStoreIdEmpty()
            {
                _storeId = Guid.Empty;
                SetupStoreIdInState();
            }

            private void SetupStoreIdInState()
            {
                TestPropertyNotSetException.ThrowIfNull(_storeId);
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Store = State.Editor.Store! with
                        {
                            Id = _storeId.Value,
                            Name = _storeName
                        }
                    }
                };
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

            public void SetupCreatingStore()
            {
                ApiClientMock.SetupCreateStoreAsync(State.Editor.Store!);
            }

            public void SetupCreatingStoreThrowsApiException()
            {
                ApiClientMock.SetupCreateStoreAsyncThrowing(State.Editor.Store!, new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupCreatingStoreThrowsHttpRequestException()
            {
                ApiClientMock.SetupCreateStoreAsyncThrowing(State.Editor.Store!, new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupModifyingStore()
            {
                ApiClientMock.SetupModifyStoreAsync(State.Editor.Store!);
            }

            public void SetupModifyingStoreThrowsApiException()
            {
                ApiClientMock.SetupModifyStoreAsyncThrowing(State.Editor.Store!, new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupModifyingStoreThrowsHttpRequestException()
            {
                ApiClientMock.SetupModifyStoreAsyncThrowing(State.Editor.Store!, new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingStartAction()
            {
                SetupDispatchingAction<SaveStoreStartedAction>();
            }

            public void SetupDispatchingFinishAction()
            {
                SetupDispatchingAction<SaveStoreFinishedAction>();
            }

            public void SetupDispatchingLeaveAction()
            {
                SetupDispatchingAction<LeaveStoreEditorAction>();
            }

            public void SetupSuccessCreateNotification()
            {
                ShoppingListNotificationServiceMock.SetupNotifySuccess($"Successfully created store {_storeName}");
            }

            public void SetupSuccessModifyNotification()
            {
                ShoppingListNotificationServiceMock.SetupNotifySuccess($"Successfully modified store {_storeName}");
            }
        }
    }

    public class HandleDeleteStoreConfirmedAction
    {
        private readonly HandleDeleteStoreConfirmedActionFixture _fixture = new();

        [Fact]
        public async Task HandleDeleteStoreConfirmedActionAction_WithStoreNull_ShouldDispatchAnything()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStoreNull();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleDeleteStoreConfirmedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleDeleteStoreConfirmedActionAction_WithValidStoreId_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStore();
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupDeletingStore();
                _fixture.SetupDispatchingFinishAction();
                _fixture.SetupDispatchingCloseDialogAction();
                _fixture.SetupDispatchingLeaveAction();
                _fixture.SetupSuccessNotification();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleDeleteStoreConfirmedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleDeleteStoreConfirmedActionAction_WithStoreId_WithApiException_ShouldDispatchErrorActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupDeletingStoreThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFinishAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleDeleteStoreConfirmedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleDeleteStoreConfirmedActionAction_WithStoreId_WithHttpRequestException_ShouldDispatchErrorActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupDeletingStoreThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorNotificationAction();
                _fixture.SetupDispatchingFinishAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleDeleteStoreConfirmedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleDeleteStoreConfirmedActionFixture : StoreEditorEffectsFixture
        {
            private readonly string _storeName = new DomainTestBuilder<string>().Create();

            public void SetupStore()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Store = State.Editor.Store! with
                        {
                            Name = _storeName
                        }
                    }
                };
            }

            public void SetupStoreNull()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Store = null
                    }
                };
            }

            public void SetupDeletingStore()
            {
                ApiClientMock.SetupDeleteStoreAsync(State.Editor.Store!.Id);
            }

            public void SetupDeletingStoreThrowsApiException()
            {
                ApiClientMock.SetupDeleteStoreAsyncThrowing(State.Editor.Store!.Id,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupDeletingStoreThrowsHttpRequestException()
            {
                ApiClientMock.SetupDeleteStoreAsyncThrowing(State.Editor.Store!.Id,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingStartAction()
            {
                SetupDispatchingAction<DeleteStoreStartedAction>();
            }

            public void SetupDispatchingFinishAction()
            {
                SetupDispatchingAction<DeleteStoreFinishedAction>();
            }

            public void SetupDispatchingCloseDialogAction()
            {
                SetupDispatchingAction<CloseDeleteStoreDialogAction>();
            }

            public void SetupDispatchingLeaveAction()
            {
                SetupDispatchingAction<LeaveStoreEditorAction>();
            }

            public void SetupSuccessNotification()
            {
                ShoppingListNotificationServiceMock.SetupNotifySuccess($"Successfully deleted store {_storeName}");
            }
        }
    }

    private abstract class StoreEditorEffectsFixture : StoreEffectsFixtureBase
    {
        protected ShoppingListNotificationServiceMock ShoppingListNotificationServiceMock { get; } =
            new(MockBehavior.Strict);

        public StoreEditorEffects CreateSut()
        {
            SetupStateReturningState();
            return new StoreEditorEffects(ApiClientMock.Object, StoreStateMock.Object, NavigationManagerMock.Object,
                ShoppingListNotificationServiceMock.Object);
        }
    }
}