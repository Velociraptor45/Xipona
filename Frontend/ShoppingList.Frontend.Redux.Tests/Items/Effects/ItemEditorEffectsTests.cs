using Microsoft.Extensions.Logging;
using Moq.Contrib.InOrder;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Effects;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;
using RestEase;
using Xunit.Abstractions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Items.Effects;

public class ItemEditorEffectsTests
{
    public class HandleLoadItemForEditingAction
    {
        private readonly HandleLoadItemForEditingActionFixture _fixture;
        private readonly ILogger<CallQueue> _logger;

        public HandleLoadItemForEditingAction(ITestOutputHelper output)
        {
            _fixture = new HandleLoadItemForEditingActionFixture();
            _logger = output.BuildLoggerFor<CallQueue>();
        }

        [Fact]
        public async Task HandleLoadItemForEditingAction_WithSuccessfulCall_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupAction();
            _fixture.SetupReturnedItem();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupGettingItem();
                _fixture.SetupDispatchingFinishAction();
            }, _logger);

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLoadItemForEditingAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadItemForEditingAction_WithFailedCall_ShouldDispatchExceptionAction()
        {
            // Arrange
            _fixture.SetupAction();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupGettingItemFailed();
                _fixture.SetupDispatchingExceptionAction();
            }, _logger);

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLoadItemForEditingAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadItemForEditingActionFixture : ItemEditorEffectsFixture
        {
            private EditedItem? _item;
            public LoadItemForEditingAction? Action { get; private set; }

            public void SetupAction()
            {
                Action = new DomainTestBuilder<LoadItemForEditingAction>().Create();
            }

            public void SetupReturnedItem()
            {
                _item = new DomainTestBuilder<EditedItem>().Create();
            }

            public void SetupGettingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);
                TestPropertyNotSetException.ThrowIfNull(_item);

                ApiClientMock.SetupGetItemByIdAsync(Action.ItemId, _item);
            }

            public void SetupGettingItemFailed()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                var exception = new DomainTestBuilder<ApiException>().Create();
                ApiClientMock.SetupGetItemByIdAsyncThrowing(Action.ItemId, exception);
            }

            public void SetupDispatchingStartAction()
            {
                SetupDispatchingAction<LoadItemForEditingStartedAction>();
            }

            public void SetupDispatchingFinishAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);

                SetupDispatchingAction(new LoadItemForEditingFinishedAction(_item));
            }

            public void SetupDispatchingExceptionAction()
            {
                SetupDispatchingAnyAction<DisplayApiExceptionNotificationAction>();
            }
        }
    }

    public class HandleAddStoreAction
    {
        private readonly HandleAddStoreActionFixture _fixture;

        public HandleAddStoreAction()
        {
            _fixture = new HandleAddStoreActionFixture();
        }

        [Fact]
        public async Task HandleAddStoreAction_WithItem_ShouldDispatchCorrectActions()
        {
            // Arrange
            _fixture.SetupActionForItem();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingAddedItemAction();
            });

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await ItemEditorEffects.HandleAddStoreAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleAddStoreAction_WithItemType_ShouldDispatchCorrectActions()
        {
            // Arrange
            _fixture.SetupActionForItemType();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingAddedItemTypeAction();
            });

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await ItemEditorEffects.HandleAddStoreAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleAddStoreActionFixture : ItemEditorEffectsFixture
        {
            public AddStoreAction? Action { get; private set; }

            public void SetupDispatchingAddedItemAction()
            {
                SetupDispatchingAction<StoreAddedToItemAction>();
            }

            public void SetupDispatchingAddedItemTypeAction()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                var itemType = Action.Available as EditedItemType;
                SetupDispatchingAction(new StoreAddedToItemTypeAction(itemType!.Id));
            }

            public void SetupActionForItem()
            {
                Action = new AddStoreAction(new DomainTestBuilder<EditedItem>().Create());
            }

            public void SetupActionForItemType()
            {
                Action = new AddStoreAction(new DomainTestBuilder<EditedItemType>().Create());
            }
        }
    }

    public class HandleChangeStoreAction
    {
        private readonly HandleChangeStoreActionFixture _fixture;

        public HandleChangeStoreAction()
        {
            _fixture = new HandleChangeStoreActionFixture();
        }

        [Fact]
        public async Task HandleChangeStoreAction_WithItem_ShouldDispatchCorrectActions()
        {
            // Arrange
            _fixture.SetupActionForItem();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingItemAction();
            });

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await ItemEditorEffects.HandleChangeStoreAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleChangeStoreAction_WithItemType_ShouldDispatchCorrectActions()
        {
            // Arrange
            _fixture.SetupActionForItemType();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingItemTypeAction();
            });

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await ItemEditorEffects.HandleChangeStoreAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleChangeStoreActionFixture : ItemEditorEffectsFixture
        {
            public ChangeStoreAction? Action { get; private set; }

            public void SetupDispatchingItemAction()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                SetupDispatchingAction(new StoreOfItemChangedAction(Action.Availability, Action.StoreId));
            }

            public void SetupDispatchingItemTypeAction()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                var itemType = Action.Available as EditedItemType;
                SetupDispatchingAction(new StoreOfItemTypeChangedAction(itemType!, Action.Availability, Action.StoreId));
            }

            public void SetupActionForItem()
            {
                Action = new ChangeStoreAction(
                    new DomainTestBuilder<EditedItem>().Create(),
                    new DomainTestBuilder<EditedItemAvailability>().Create(),
                    Guid.NewGuid());
            }

            public void SetupActionForItemType()
            {
                Action = new ChangeStoreAction(
                    new DomainTestBuilder<EditedItemType>().Create(),
                    new DomainTestBuilder<EditedItemAvailability>().Create(),
                    Guid.NewGuid());
            }
        }
    }

    public class HandleChangePriceAction
    {
        private readonly HandleChangePriceActionFixture _fixture;

        public HandleChangePriceAction()
        {
            _fixture = new HandleChangePriceActionFixture();
        }

        [Fact]
        public async Task HandleChangePriceAction_WithItem_ShouldDispatchCorrectActions()
        {
            // Arrange
            _fixture.SetupActionForItem();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingItemAction();
            });

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await ItemEditorEffects.HandleChangePriceAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleChangePriceAction_WithItemType_ShouldDispatchCorrectActions()
        {
            // Arrange
            _fixture.SetupActionForItemType();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingItemTypeAction();
            });

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await ItemEditorEffects.HandleChangePriceAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleChangePriceActionFixture : ItemEditorEffectsFixture
        {
            public ChangePriceAction? Action { get; private set; }

            public void SetupDispatchingItemAction()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                SetupDispatchingAction(new PriceOfItemChangedAction(Action.Availability, Action.Price));
            }

            public void SetupDispatchingItemTypeAction()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                var itemType = Action.Available as EditedItemType;
                SetupDispatchingAction(new PriceOfItemTypeChangedAction(itemType!, Action.Availability, Action.Price));
            }

            public void SetupActionForItem()
            {
                Action = new ChangePriceAction(
                    new DomainTestBuilder<EditedItem>().Create(),
                    new DomainTestBuilder<EditedItemAvailability>().Create(),
                    new DomainTestBuilder<float>().Create());
            }

            public void SetupActionForItemType()
            {
                Action = new ChangePriceAction(
                    new DomainTestBuilder<EditedItemType>().Create(),
                    new DomainTestBuilder<EditedItemAvailability>().Create(),
                    new DomainTestBuilder<float>().Create());
            }
        }
    }

    public class HandleChangeDefaultSectionAction
    {
        private readonly HandleChangeDefaultSectionActionFixture _fixture;

        public HandleChangeDefaultSectionAction()
        {
            _fixture = new HandleChangeDefaultSectionActionFixture();
        }

        [Fact]
        public async Task HandleChangeDefaultSectionAction_WithItem_ShouldDispatchCorrectActions()
        {
            // Arrange
            _fixture.SetupActionForItem();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingItemAction();
            });

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await ItemEditorEffects.HandleChangeDefaultSectionAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleChangeDefaultSectionAction_WithItemType_ShouldDispatchCorrectActions()
        {
            // Arrange
            _fixture.SetupActionForItemType();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingItemTypeAction();
            });

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await ItemEditorEffects.HandleChangeDefaultSectionAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleChangeDefaultSectionActionFixture : ItemEditorEffectsFixture
        {
            public ChangeDefaultSectionAction? Action { get; private set; }

            public void SetupDispatchingItemAction()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                SetupDispatchingAction(new DefaultSectionOfItemChangedAction(Action.Availability, Action.DefaultSectionId));
            }

            public void SetupDispatchingItemTypeAction()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                var itemType = Action.Available as EditedItemType;
                SetupDispatchingAction(new DefaultSectionOfItemTypeChangedAction(itemType!, Action.Availability, Action.DefaultSectionId));
            }

            public void SetupActionForItem()
            {
                Action = new ChangeDefaultSectionAction(
                    new DomainTestBuilder<EditedItem>().Create(),
                    new DomainTestBuilder<EditedItemAvailability>().Create(),
                    Guid.NewGuid());
            }

            public void SetupActionForItemType()
            {
                Action = new ChangeDefaultSectionAction(
                    new DomainTestBuilder<EditedItemType>().Create(),
                    new DomainTestBuilder<EditedItemAvailability>().Create(),
                    Guid.NewGuid());
            }
        }
    }

    public class HandleRemoveStoreAction
    {
        private readonly HandleRemoveStoreActionFixture _fixture;

        public HandleRemoveStoreAction()
        {
            _fixture = new HandleRemoveStoreActionFixture();
        }

        [Fact]
        public async Task HandleRemoveStoreAction_WithItem_ShouldDispatchCorrectActions()
        {
            // Arrange
            _fixture.SetupActionForItem();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingItemAction();
            });

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await ItemEditorEffects.HandleRemoveStoreAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleRemoveStoreAction_WithItemType_ShouldDispatchCorrectActions()
        {
            // Arrange
            _fixture.SetupActionForItemType();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingItemTypeAction();
            });

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await ItemEditorEffects.HandleRemoveStoreAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleRemoveStoreActionFixture : ItemEditorEffectsFixture
        {
            public RemoveStoreAction? Action { get; private set; }

            public void SetupDispatchingItemAction()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                SetupDispatchingAction(new StoreOfItemRemovedAction(Action.Availability));
            }

            public void SetupDispatchingItemTypeAction()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                var itemType = Action.Available as EditedItemType;
                SetupDispatchingAction(new StoreOfItemTypeRemovedAction(itemType!, Action.Availability));
            }

            public void SetupActionForItem()
            {
                Action = new RemoveStoreAction(
                    new DomainTestBuilder<EditedItem>().Create(),
                    new DomainTestBuilder<EditedItemAvailability>().Create());
            }

            public void SetupActionForItemType()
            {
                Action = new RemoveStoreAction(
                    new DomainTestBuilder<EditedItemType>().Create(),
                    new DomainTestBuilder<EditedItemAvailability>().Create());
            }
        }
    }

    public class HandleCreateItemAction
    {
        private readonly HandleCreateItemActionFixture _fixture;

        public HandleCreateItemAction()
        {
            _fixture = new HandleCreateItemActionFixture();
        }

        [Fact]
        public async Task HandleCreateItemAction_WithoutTypes_CallSuccessful_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupItemWithoutTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupCreatingItem();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingLeaveAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleCreateItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateItemAction_WithoutTypesAndItemModeNotDefined_CallSuccessful_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupItemWithoutTypesAndItemModeNotDefined();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupCreatingItem();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingLeaveAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleCreateItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateItemAction_WithoutTypes_CallFailed_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupItemWithoutTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupCreatingItemFailed();
                _fixture.SetupDispatchingExceptionAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleCreateItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateItemAction_WithTypes_CallSuccessful_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupItemWithTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupCreatingItemWithTypes();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingLeaveAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleCreateItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateItemAction_WithTypesAndItemModeNotDefined_CallSuccessful_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupItemWithTypesAndItemModeNotDefined();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupCreatingItemWithTypes();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingLeaveAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleCreateItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateItemAction_WithTypes_CallFailed_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupItemWithTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupCreatingItemWithTypesFailed();
                _fixture.SetupDispatchingExceptionAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleCreateItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleCreateItemActionFixture : ItemEditorEffectsFixture
        {
            public void SetupItemWithoutTypes()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = State.Editor.Item! with
                        {
                            ItemMode = ItemMode.WithoutTypes
                        }
                    }
                };
            }

            public void SetupItemWithoutTypesAndItemModeNotDefined()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = State.Editor.Item! with
                        {
                            ItemMode = ItemMode.NotDefined,
                            ItemTypes = new List<EditedItemType>()
                        }
                    }
                };
            }

            public void SetupItemWithTypes()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = State.Editor.Item! with
                        {
                            ItemMode = ItemMode.WithTypes
                        }
                    }
                };
            }

            public void SetupItemWithTypesAndItemModeNotDefined()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = State.Editor.Item! with
                        {
                            ItemMode = ItemMode.NotDefined,
                            ItemTypes = new DomainTestBuilder<EditedItemType>().CreateMany(2).ToList()
                        }
                    }
                };
            }

            public void SetupCreatingItem()
            {
                var request = new CreateItemRequest(Guid.NewGuid(), State.Editor.Item!);
                ApiClientMock.SetupCreateItemAsync(request);
            }

            public void SetupCreatingItemFailed()
            {
                var request = new CreateItemRequest(Guid.NewGuid(), State.Editor.Item!);
                ApiClientMock.SetupCreateItemAsyncThrowing(request, new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupCreatingItemWithTypes()
            {
                var request = new CreateItemWithTypesRequest(Guid.NewGuid(), State.Editor.Item!);
                ApiClientMock.SetupCreateItemWithTypesAsync(request);
            }

            public void SetupCreatingItemWithTypesFailed()
            {
                var request = new CreateItemWithTypesRequest(Guid.NewGuid(), State.Editor.Item!);
                ApiClientMock.SetupCreateItemWithTypesAsyncThrowing(request, new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<CreateItemStartedAction>();
            }

            public void SetupDispatchingFinishedAction()
            {
                SetupDispatchingAction<CreateItemFinishedAction>();
            }

            public void SetupDispatchingLeaveAction()
            {
                SetupDispatchingAction<LeaveItemEditorAction>();
            }

            public void SetupDispatchingExceptionAction()
            {
                SetupDispatchingAnyAction<DisplayApiExceptionNotificationAction>();
            }
        }
    }

    public class HandleUpdateItemAction
    {
        private readonly HandleUpdateItemActionFixture _fixture;

        public HandleUpdateItemAction()
        {
            _fixture = new HandleUpdateItemActionFixture();
        }

        [Fact]
        public async Task HandleUpdateItemAction_WithoutTypes_CallSuccessful_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupItemWithoutTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupUpdatingItem();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingLeaveAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleUpdateItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleUpdateItemAction_WithoutTypes_CallFailed_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupItemWithoutTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupUpdatingItemFailed();
                _fixture.SetupDispatchingExceptionAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleUpdateItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleUpdateItemAction_WithTypes_CallSuccessful_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupItemWithTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupUpdatingItemWithTypes();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingLeaveAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleUpdateItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleUpdateItemAction_WithTypes_CallFailed_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupItemWithTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupUpdatingItemWithTypesFailed();
                _fixture.SetupDispatchingExceptionAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleUpdateItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleUpdateItemActionFixture : ItemEditorEffectsFixture
        {
            public void SetupItemWithoutTypes()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = State.Editor.Item! with
                        {
                            ItemMode = ItemMode.WithoutTypes
                        }
                    }
                };
            }

            public void SetupItemWithTypes()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = State.Editor.Item! with
                        {
                            ItemMode = ItemMode.WithTypes
                        }
                    }
                };
            }

            public void SetupUpdatingItem()
            {
                var request = new UpdateItemRequest(Guid.NewGuid(), State.Editor.Item!);
                ApiClientMock.SetupUpdateItemAsync(request);
            }

            public void SetupUpdatingItemFailed()
            {
                var request = new UpdateItemRequest(Guid.NewGuid(), State.Editor.Item!);
                ApiClientMock.SetupUpdateItemAsyncThrowing(request, new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupUpdatingItemWithTypes()
            {
                var request = new UpdateItemWithTypesRequest(Guid.NewGuid(), State.Editor.Item!);
                ApiClientMock.SetupUpdateItemWithTypesAsync(request);
            }

            public void SetupUpdatingItemWithTypesFailed()
            {
                var request = new UpdateItemWithTypesRequest(Guid.NewGuid(), State.Editor.Item!);
                ApiClientMock.SetupUpdateItemWithTypesAsyncThrowing(request, new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<UpdateItemStartedAction>();
            }

            public void SetupDispatchingFinishedAction()
            {
                SetupDispatchingAction<UpdateItemFinishedAction>();
            }

            public void SetupDispatchingLeaveAction()
            {
                SetupDispatchingAction<LeaveItemEditorAction>();
            }

            public void SetupDispatchingExceptionAction()
            {
                SetupDispatchingAnyAction<DisplayApiExceptionNotificationAction>();
            }
        }
    }

    public class HandleModifyItemAction
    {
        private readonly HandleModifyItemActionFixture _fixture;

        public HandleModifyItemAction()
        {
            _fixture = new HandleModifyItemActionFixture();
        }

        [Fact]
        public async Task HandleModifyItemAction_WithoutTypes_CallSuccessful_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupItemWithoutTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupModifyingItem();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingLeaveAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleModifyItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleModifyItemAction_WithoutTypes_CallFailed_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupItemWithoutTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupModifyingItemFailed();
                _fixture.SetupDispatchingExceptionAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleModifyItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleModifyItemAction_WithTypes_CallSuccessful_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupItemWithTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupModifyingItemWithTypes();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingLeaveAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleModifyItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleModifyItemAction_WithTypes_CallFailed_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupItemWithTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupModifyingItemWithTypesFailed();
                _fixture.SetupDispatchingExceptionAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleModifyItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleModifyItemActionFixture : ItemEditorEffectsFixture
        {
            public void SetupItemWithoutTypes()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = State.Editor.Item! with
                        {
                            ItemMode = ItemMode.WithoutTypes
                        }
                    }
                };
            }

            public void SetupItemWithTypes()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = State.Editor.Item! with
                        {
                            ItemMode = ItemMode.WithTypes
                        }
                    }
                };
            }

            public void SetupModifyingItem()
            {
                var request = new ModifyItemRequest(Guid.NewGuid(), State.Editor.Item!);
                ApiClientMock.SetupModifyItemAsync(request);
            }

            public void SetupModifyingItemFailed()
            {
                var request = new ModifyItemRequest(Guid.NewGuid(), State.Editor.Item!);
                ApiClientMock.SetupModifyItemAsyncThrowing(request, new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupModifyingItemWithTypes()
            {
                var request = new ModifyItemWithTypesRequest(Guid.NewGuid(), State.Editor.Item!);
                ApiClientMock.SetupModifyItemWithTypesAsync(request);
            }

            public void SetupModifyingItemWithTypesFailed()
            {
                var request = new ModifyItemWithTypesRequest(Guid.NewGuid(), State.Editor.Item!);
                ApiClientMock.SetupModifyItemWithTypesAsyncThrowing(request, new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<ModifyItemStartedAction>();
            }

            public void SetupDispatchingFinishedAction()
            {
                SetupDispatchingAction<ModifyItemFinishedAction>();
            }

            public void SetupDispatchingLeaveAction()
            {
                SetupDispatchingAction<LeaveItemEditorAction>();
            }

            public void SetupDispatchingExceptionAction()
            {
                SetupDispatchingAnyAction<DisplayApiExceptionNotificationAction>();
            }
        }
    }

    public class HandleMakeItemPermanentAction
    {
        private readonly HandleMakeItemPermanentActionFixture _fixture;

        public HandleMakeItemPermanentAction()
        {
            _fixture = new HandleMakeItemPermanentActionFixture();
        }

        [Fact]
        public async Task HandleMakeItemPermanentAction_WithCallSuccessful_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupItemWithoutTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupMakingItemPermanent();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingLeaveAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleMakeItemPermanentAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleMakeItemPermanentAction_WithCallFailed_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupItemWithoutTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupMakingItemPermanentFailed();
                _fixture.SetupDispatchingExceptionAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleMakeItemPermanentAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleMakeItemPermanentActionFixture : ItemEditorEffectsFixture
        {
            public void SetupItemWithoutTypes()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = State.Editor.Item! with
                        {
                            ItemMode = ItemMode.WithoutTypes,
                            ItemTypes = new List<EditedItemType>(),
                            Availabilities = new DomainTestBuilder<EditedItemAvailability>().CreateMany(2).ToList()
                        }
                    }
                };
            }

            public void SetupMakingItemPermanent()
            {
                var item = State.Editor.Item!;
                var request = new MakeTemporaryItemPermanentRequest(
                    item.Id,
                    item.Name,
                    item.Comment,
                    item.QuantityType.Id,
                    item.QuantityInPacket,
                    item.QuantityInPacketType?.Id,
                    item.ItemCategoryId!.Value,
                    item.ManufacturerId,
                    item.Availabilities);
                ApiClientMock.SetupMakeTemporaryItemPermanent(request);
            }

            public void SetupMakingItemPermanentFailed()
            {
                var item = State.Editor.Item!;
                var request = new MakeTemporaryItemPermanentRequest(
                    item.Id,
                    item.Name,
                    item.Comment,
                    item.QuantityType.Id,
                    item.QuantityInPacket,
                    item.QuantityInPacketType?.Id,
                    item.ItemCategoryId!.Value,
                    item.ManufacturerId,
                    item.Availabilities);
                ApiClientMock.SetupMakeTemporaryItemPermanentThrowing(request,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<MakeItemPermanentStartedAction>();
            }

            public void SetupDispatchingFinishedAction()
            {
                SetupDispatchingAction<MakeItemPermanentFinishedAction>();
            }

            public void SetupDispatchingLeaveAction()
            {
                SetupDispatchingAction<LeaveItemEditorAction>();
            }

            public void SetupDispatchingExceptionAction()
            {
                SetupDispatchingAnyAction<DisplayApiExceptionNotificationAction>();
            }
        }
    }

    public class HandleDeleteItemAction
    {
        private readonly HandleDeleteItemActionFixture _fixture;

        public HandleDeleteItemAction()
        {
            _fixture = new HandleDeleteItemActionFixture();
        }

        [Fact]
        public async Task HandleDeleteItemAction_WithCallSuccessful_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupMakingItemPermanent();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingLeaveAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleDeleteItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleDeleteItemAction_WithCallFailed_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupMakingItemPermanentFailed();
                _fixture.SetupDispatchingExceptionAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleDeleteItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleDeleteItemActionFixture : ItemEditorEffectsFixture
        {
            public void SetupMakingItemPermanent()
            {
                var item = State.Editor.Item!;
                var request = new DeleteItemRequest(Guid.NewGuid(), item.Id);
                ApiClientMock.SetupDeleteItemAsync(request);
            }

            public void SetupMakingItemPermanentFailed()
            {
                var item = State.Editor.Item!;
                var request = new DeleteItemRequest(Guid.NewGuid(), item.Id);
                ApiClientMock.SetupDeleteItemAsyncThrowing(request, new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<DeleteItemStartedAction>();
            }

            public void SetupDispatchingFinishedAction()
            {
                SetupDispatchingAction<DeleteItemFinishedAction>();
            }

            public void SetupDispatchingLeaveAction()
            {
                SetupDispatchingAction<LeaveItemEditorAction>();
            }

            public void SetupDispatchingExceptionAction()
            {
                SetupDispatchingAnyAction<DisplayApiExceptionNotificationAction>();
            }
        }
    }

    private abstract class ItemEditorEffectsFixture : ItemEffectsFixtureBase
    {
        public ItemEditorEffects CreateSut()
        {
            SetupStateReturningState();
            return new ItemEditorEffects(ApiClientMock.Object, ItemStateMock.Object, NavigationManagerMock.Object);
        }
    }
}