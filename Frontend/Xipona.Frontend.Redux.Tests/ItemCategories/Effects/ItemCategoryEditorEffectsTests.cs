using Moq;
using Moq.Contrib.InOrder;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.Actions;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.Effects;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ItemCategories;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Shared.Ports;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.ItemCategories.Effects;

public class ItemCategoryEditorEffectsTests
{
    public class HandleLoadItemCategoryForEditingAction
    {
        private readonly HandleLoadItemCategoryForEditingActionFixture _fixture = new();

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithValidId_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupId();
                _fixture.SetupItemCategory();
                _fixture.SetupAction();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupGettingItemCategorySucceeded();
                _fixture.SetupDispatchingFinishedAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupId();
                _fixture.SetupAction();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupGettingItemCategoryFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupId();
                _fixture.SetupAction();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupGettingItemCategoryFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
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

            public void SetupGettingItemCategorySucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);

                ApiClientMock.SetupGetItemCategoryByIdAsync(_id.Value, _itemCategory);
            }

            public void SetupGettingItemCategoryFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupGetItemCategoryByIdAsyncThrowing(_id.Value,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingItemCategoryFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupGetItemCategoryByIdAsyncThrowing(_id.Value,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                Action = new LoadItemCategoryForEditingAction(_id.Value);
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);

                SetupDispatchingAction(new LoadItemCategoryForEditingFinishedAction(_itemCategory));
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<LoadItemCategoryForEditingStartedAction>();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupEmptyId();
                _fixture.SetupItemCategoryName();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupCreateItemCategorySucceeded();
                _fixture.SetupSuccessCreateNotification();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingLeaveAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupEmptyId();
                _fixture.SetupItemCategoryName();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupCreateItemCategoryFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupEmptyId();
                _fixture.SetupItemCategoryName();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupCreateItemCategoryFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupId();
                _fixture.SetupItemCategoryName();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupModifyItemCategorySucceeded();
                _fixture.SetupUpdateSearchResultsAfterSaveAction();
                _fixture.SetupSuccessModifyNotification();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingLeaveAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupId();
                _fixture.SetupItemCategoryName();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupModifyItemCategoryFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupId();
                _fixture.SetupItemCategoryName();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupModifyItemCategoryFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
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

            public void SetupCreateItemCategorySucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupCreateItemCategoryAsync(_manufacturerName,
                    new DomainTestBuilder<EditedItemCategory>().Create());
            }

            public void SetupCreateItemCategoryFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupCreateItemCategoryAsyncThrowing(_manufacturerName,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupCreateItemCategoryFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupCreateItemCategoryAsyncThrowing(_manufacturerName,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupModifyItemCategorySucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupModifyItemCategoryAsync(new ModifyItemCategoryRequest(_id.Value, _manufacturerName));
            }

            public void SetupModifyItemCategoryFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupModifyItemCategoryAsyncThrowing(new ModifyItemCategoryRequest(_id.Value, _manufacturerName),
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupModifyItemCategoryFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupModifyItemCategoryAsyncThrowing(new ModifyItemCategoryRequest(_id.Value, _manufacturerName),
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupSuccessCreateNotification()
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ShoppingListNotificationServiceMock.SetupNotifySuccess($"Successfully created item category {_manufacturerName}");
            }

            public void SetupSuccessModifyNotification()
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ShoppingListNotificationServiceMock.SetupNotifySuccess($"Successfully modified item category {_manufacturerName}");
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                SetupDispatchingAction<SaveItemCategoryFinishedAction>();
            }

            public void SetupUpdateSearchResultsAfterSaveAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                SetupDispatchingAction(new UpdateItemCategorySearchResultsAfterSaveAction(_id.Value, _manufacturerName));
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<SaveItemCategoryStartedAction>();
            }

            public void SetupDispatchingLeaveAction()
            {
                SetupDispatchingAction(new LeaveItemCategoryEditorAction(true));
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupId();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupGettingItemCategorySucceeded();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupCloseDialogAction();
                _fixture.SetupSuccessNotification();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupId();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupGettingItemCategoryFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupId();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupGettingItemCategoryFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
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

            public void SetupGettingItemCategorySucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupDeleteItemCategoryAsync(_id.Value);
            }

            public void SetupGettingItemCategoryFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupDeleteItemCategoryAsyncThrowing(_id.Value,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingItemCategoryFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupDeleteItemCategoryAsyncThrowing(_id.Value,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingFinishedAction()
            {
                SetupDispatchingAction<DeleteItemCategoryFinishedAction>();
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<DeleteItemCategoryStartedAction>();
            }

            public void SetupCloseDialogAction()
            {
                SetupDispatchingAction(new CloseDeleteItemCategoryDialogAction(true));
            }

            public void SetupSuccessNotification()
            {
                ShoppingListNotificationServiceMock.SetupNotifySuccess($"Successfully deleted item category {State.Editor.ItemCategory!.Name}");
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