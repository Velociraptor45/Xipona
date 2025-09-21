using Moq;
using Moq.Contrib.InOrder;
using Xipona.Frontend.Redux.ItemCategories.Actions;
using Xipona.Frontend.Redux.ItemCategories.Effects;
using Xipona.Frontend.Redux.ItemCategories.States;
using Xipona.Frontend.Redux.Shared.Ports.Requests.ItemCategories;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.Shared.Ports;
using Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace Xipona.Frontend.Redux.Tests.ItemCategories.Effects;

public class ItemCategoryEditorEffectsTests
{
    public class HandleLoadItemCategoryForEditingAction
    {
        private readonly HandleLoadItemCategoryForEditingActionFixture _fixture = new();

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithValidId_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupItemCategory();
            _fixture.SetupAction();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupGettingItemCategorySucceeded(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLoadItemCategoryForEditingAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupAction();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupGettingItemCategoryFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLoadItemCategoryForEditingAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupAction();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupGettingItemCategoryFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLoadItemCategoryForEditingAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadItemCategoryForEditingActionFixture : ItemCategoryEditorEffectsFixture
        {
            private Guid? _id;
            private EditedItemCategory? _itemCategory;
            public LoadItemCategoryForEditingAction? Action { get; private set; }

            public void SetupId()
            {
                _id = Guid.NewGuid();
            }

            public void SetupItemCategory()
            {
                _itemCategory = new DomainTestBuilder<EditedItemCategory>().Create();
            }

            public void SetupGettingItemCategorySucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);

                ApiClientMock.SetupGetItemCategoryByIdAsync(_id.Value, _itemCategory, component);
            }

            public void SetupGettingItemCategoryFailedWithErrorInApi(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupGetItemCategoryByIdAsyncThrowing(_id.Value,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupGettingItemCategoryFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupGetItemCategoryByIdAsyncThrowing(_id.Value,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                Action = new LoadItemCategoryForEditingAction(_id.Value);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);

                SetupDispatchingAction(new LoadItemCategoryForEditingFinishedAction(_itemCategory), component);
            }

            public void SetupDispatchingStartedAction(IQueueComponent component)
            {
                SetupDispatchingAction<LoadItemCategoryForEditingStartedAction>(component);
            }
        }
    }

    public class HandleSaveItemCategoryAction
    {
        private readonly HandleSaveItemCategoryActionFixture _fixture = new();

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithEmptyId_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupEmptyId();
            _fixture.SetupItemCategoryName();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupCreateItemCategorySucceeded(x0);
                _fixture.SetupSuccessCreateNotification(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingLeaveAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithEmptyId_WithWithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupEmptyId();
            _fixture.SetupItemCategoryName();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupCreateItemCategoryFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithEmptyId_WithWithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupEmptyId();
            _fixture.SetupItemCategoryName();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupCreateItemCategoryFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithFilledId_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupItemCategoryName();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupModifyItemCategorySucceeded(x0);
                _fixture.SetupUpdateSearchResultsAfterSaveAction(x0);
                _fixture.SetupSuccessModifyNotification(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingLeaveAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithFilledId_WithWithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupItemCategoryName();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupModifyItemCategoryFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithFilledId_WithWithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupId();
            _fixture.SetupItemCategoryName();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupModifyItemCategoryFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSaveItemCategoryActionFixture : ItemCategoryEditorEffectsFixture
        {
            private Guid? _id;
            private string? _manufacturerName;

            public void SetupId()
            {
                _id = Guid.NewGuid();
                State = State with
                {
                    Editor = State.Editor with
                    {
                        ItemCategory = State.Editor.ItemCategory! with
                        {
                            Id = _id.Value
                        }
                    }
                };
            }

            public void SetupEmptyId()
            {
                _id = Guid.Empty;
                State = State with
                {
                    Editor = State.Editor with
                    {
                        ItemCategory = State.Editor.ItemCategory! with
                        {
                            Id = _id.Value
                        }
                    }
                };
            }

            public void SetupItemCategoryName()
            {
                _manufacturerName = new DomainTestBuilder<string>().Create();
                State = State with
                {
                    Editor = State.Editor with
                    {
                        ItemCategory = State.Editor.ItemCategory! with
                        {
                            Name = _manufacturerName
                        }
                    }
                };
            }

            public void SetupCreateItemCategorySucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupCreateItemCategoryAsync(_manufacturerName,
                    new DomainTestBuilder<EditedItemCategory>().Create(), component);
            }

            public void SetupCreateItemCategoryFailedWithErrorInApi(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupCreateItemCategoryAsyncThrowing(_manufacturerName,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupCreateItemCategoryFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupCreateItemCategoryAsyncThrowing(_manufacturerName,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupModifyItemCategorySucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupModifyItemCategoryAsync(new ModifyItemCategoryRequest(_id.Value, _manufacturerName),
                    component);
            }

            public void SetupModifyItemCategoryFailedWithErrorInApi(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupModifyItemCategoryAsyncThrowing(new ModifyItemCategoryRequest(_id.Value, _manufacturerName),
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupModifyItemCategoryFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupModifyItemCategoryAsyncThrowing(new ModifyItemCategoryRequest(_id.Value, _manufacturerName),
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupSuccessCreateNotification(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ShoppingListNotificationServiceMock
                    .SetupNotifySuccess($"Successfully created item category {_manufacturerName}", 2f, component);
            }

            public void SetupSuccessModifyNotification(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ShoppingListNotificationServiceMock
                    .SetupNotifySuccess($"Successfully modified item category {_manufacturerName}", 2f, component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                SetupDispatchingAction<SaveItemCategoryFinishedAction>(component);
            }

            public void SetupUpdateSearchResultsAfterSaveAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                SetupDispatchingAction(new UpdateItemCategorySearchResultsAfterSaveAction(_id.Value, _manufacturerName), component);
            }

            public void SetupDispatchingStartedAction(IQueueComponent component)
            {
                SetupDispatchingAction<SaveItemCategoryStartedAction>(component);
            }

            public void SetupDispatchingLeaveAction(IQueueComponent component)
            {
                SetupDispatchingAction(new LeaveItemCategoryEditorAction(true), component);
            }
        }
    }

    public class HandleDeleteItemCategoryAction
    {
        private readonly HandleDeleteItemCategoryActionFixture _fixture = new();

        [Fact]
        public async Task HandleDeleteItemCategoryAction_WithValidId_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupId();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupGettingItemCategorySucceeded(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupCloseDialogAction(x0);
                _fixture.SetupSuccessNotification(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleDeleteItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleDeleteItemCategoryAction_WithWithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupId();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupGettingItemCategoryFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleDeleteItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleDeleteItemCategoryAction_WithWithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupId();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupGettingItemCategoryFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleDeleteItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleDeleteItemCategoryActionFixture : ItemCategoryEditorEffectsFixture
        {
            private Guid? _id;

            public void SetupId()
            {
                _id = Guid.NewGuid();
                State = State with
                {
                    Editor = State.Editor with
                    {
                        ItemCategory = State.Editor.ItemCategory! with
                        {
                            Id = _id.Value
                        }
                    }
                };
            }

            public void SetupGettingItemCategorySucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupDeleteItemCategoryAsync(_id.Value, component);
            }

            public void SetupGettingItemCategoryFailedWithErrorInApi(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupDeleteItemCategoryAsyncThrowing(_id.Value,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupGettingItemCategoryFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupDeleteItemCategoryAsyncThrowing(_id.Value,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                SetupDispatchingAction<DeleteItemCategoryFinishedAction>(component);
            }

            public void SetupDispatchingStartedAction(IQueueComponent component)
            {
                SetupDispatchingAction<DeleteItemCategoryStartedAction>(component);
            }

            public void SetupCloseDialogAction(IQueueComponent component)
            {
                SetupDispatchingAction(new CloseDeleteItemCategoryDialogAction(true), component);
            }

            public void SetupSuccessNotification(IQueueComponent component)
            {
                ShoppingListNotificationServiceMock
                    .SetupNotifySuccess($"Successfully deleted item category {State.Editor.ItemCategory!.Name}", 2f, component);
            }
        }
    }

    private abstract class ItemCategoryEditorEffectsFixture : ItemCategoryEffectsFixtureBase
    {
        protected readonly ShoppingListNotificationServiceMock ShoppingListNotificationServiceMock =
            new(MockBehavior.Strict);

        public ItemCategoryEditorEffects CreateSut()
        {
            SetupStateReturningState();
            return new ItemCategoryEditorEffects(ApiClientMock.Object, NavigationManagerMock.Object,
                ItemCategoryStateMock.Object, ShoppingListNotificationServiceMock.Object);
        }
    }
}