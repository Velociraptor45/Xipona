using Moq;
using Moq.Contrib.InOrder;
using Xipona.Frontend.Redux.Stores.Actions.Editor;
using Xipona.Frontend.Redux.Stores.Effects;
using Xipona.Frontend.Redux.Stores.States;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.Shared.Ports;
using Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace Xipona.Frontend.Redux.Tests.Stores.Effects;

public class StoreEditorEffectsTests
{
    public class HandleLoadStoreForEditingAction
    {
        private readonly HandleLoadStoreForEditingFixture _fixture = new();

        [Fact]
        public async Task HandleLoadStoreForEditingAction_WithValidStoreId_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupAction();
                _fixture.SetupGettingStore(x0);
                _fixture.SetupDispatchingFinishAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupAction();
                _fixture.SetupGettingStoreThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupAction();
                _fixture.SetupGettingStoreThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
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

            public void SetupGettingStore(IQueueComponent component)
            {
                _store = new DomainTestBuilder<EditedStore>().Create();
                ApiClientMock.SetupGetStoreByIdAsync(_storeId, _store, component);
            }

            public void SetupGettingStoreThrowsApiException(IQueueComponent component)
            {
                ApiClientMock.SetupGetStoreByIdAsyncThrowing(_storeId,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupGettingStoreThrowsHttpRequestException(IQueueComponent component)
            {
                ApiClientMock.SetupGetStoreByIdAsyncThrowing(_storeId,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupAction()
            {
                Action = new LoadStoreForEditingAction(_storeId);
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_store);
                SetupDispatchingAction(new LoadStoreForEditingFinishedAction(_store), component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupStoreId();
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupModifyingStore(x0);
                _fixture.SetupSuccessModifyNotification(x0);
                _fixture.SetupDispatchingFinishAction(x0);
                _fixture.SetupDispatchingLeaveAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupStoreId();
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupModifyingStoreThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupStoreId();
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupModifyingStoreThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupStoreIdEmpty();
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupCreatingStore(x0);
                _fixture.SetupSuccessCreateNotification(x0);
                _fixture.SetupDispatchingFinishAction(x0);
                _fixture.SetupDispatchingLeaveAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupStoreIdEmpty();
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupCreatingStoreThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupStoreIdEmpty();
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupCreatingStoreThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishAction(x0);
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

            public void SetupCreatingStore(IQueueComponent component)
            {
                ApiClientMock.SetupCreateStoreAsync(State.Editor.Store!, component);
            }

            public void SetupCreatingStoreThrowsApiException(IQueueComponent component)
            {
                ApiClientMock.SetupCreateStoreAsyncThrowing(State.Editor.Store!,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupCreatingStoreThrowsHttpRequestException(IQueueComponent component)
            {
                ApiClientMock.SetupCreateStoreAsyncThrowing(State.Editor.Store!,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupModifyingStore(IQueueComponent component)
            {
                ApiClientMock.SetupModifyStoreAsync(State.Editor.Store!, component);
            }

            public void SetupModifyingStoreThrowsApiException(IQueueComponent component)
            {
                ApiClientMock.SetupModifyStoreAsyncThrowing(State.Editor.Store!,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupModifyingStoreThrowsHttpRequestException(IQueueComponent component)
            {
                ApiClientMock.SetupModifyStoreAsyncThrowing(State.Editor.Store!,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingStartAction(IQueueComponent component)
            {
                SetupDispatchingAction<SaveStoreStartedAction>(component);
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                SetupDispatchingAction<SaveStoreFinishedAction>(component);
            }

            public void SetupDispatchingLeaveAction(IQueueComponent component)
            {
                SetupDispatchingAction<LeaveStoreEditorAction>(component);
            }

            public void SetupSuccessCreateNotification(IQueueComponent component)
            {
                ShoppingListNotificationServiceMock
                    .SetupNotifySuccess($"Successfully created store {_storeName}", 2f, component);
            }

            public void SetupSuccessModifyNotification(IQueueComponent component)
            {
                ShoppingListNotificationServiceMock
                    .SetupNotifySuccess($"Successfully modified store {_storeName}", 2f, component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupStore();
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupDeletingStore(x0);
                _fixture.SetupDispatchingFinishAction(x0);
                _fixture.SetupDispatchingCloseDialogAction(x0);
                _fixture.SetupDispatchingLeaveAction(x0);
                _fixture.SetupSuccessNotification(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupDeletingStoreThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupDeletingStoreThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishAction(x0);
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

            public void SetupDeletingStore(IQueueComponent component)
            {
                ApiClientMock.SetupDeleteStoreAsync(State.Editor.Store!.Id, component);
            }

            public void SetupDeletingStoreThrowsApiException(IQueueComponent component)
            {
                ApiClientMock.SetupDeleteStoreAsyncThrowing(State.Editor.Store!.Id,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupDeletingStoreThrowsHttpRequestException(IQueueComponent component)
            {
                ApiClientMock.SetupDeleteStoreAsyncThrowing(State.Editor.Store!.Id,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingStartAction(IQueueComponent component)
            {
                SetupDispatchingAction<DeleteStoreStartedAction>(component);
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                SetupDispatchingAction<DeleteStoreFinishedAction>(component);
            }

            public void SetupDispatchingCloseDialogAction(IQueueComponent component)
            {
                SetupDispatchingAction<CloseDeleteStoreDialogAction>(component);
            }

            public void SetupDispatchingLeaveAction(IQueueComponent component)
            {
                SetupDispatchingAction<LeaveStoreEditorAction>(component);
            }

            public void SetupSuccessNotification(IQueueComponent component)
            {
                ShoppingListNotificationServiceMock
                    .SetupNotifySuccess($"Successfully deleted store {_storeName}", 2f, component);
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