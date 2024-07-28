using Moq;
using Moq.Contrib.InOrder;
using ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Editor;
using ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Editor.Availabilities;
using ProjectHermes.Xipona.Frontend.Redux.Items.Effects;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.Items;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Shared.Ports;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Items.Effects;

public class ItemEditorEffectsTests
{
    public class HandleSetEditorItemIdAction
    {
        private readonly HandleSetEditorItemIdActionFixture _fixture = new();

        [Fact]
        public async Task HandleSetEditorItemIdAction_WithStoresNotLoaded_ShouldNotDispatchAnything()
        {
            // Arrange
            var queue = CallQueue.Create(_ => { });
            _fixture.SetupItemIdSet();
            _fixture.SetupStateWithStoresNotLoaded();
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSetEditorItemIdAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSetEditorItemIdAction_WithQuantityTypesInPacketNotLoaded_ShouldNotDispatchAnything()
        {
            // Arrange
            var queue = CallQueue.Create(_ => { });
            _fixture.SetupItemIdSet();
            _fixture.SetupStateWithQuantityTypesInPacketNotLoaded();
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSetEditorItemIdAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSetEditorItemIdAction_WithQuantityTypesNotLoaded_ShouldNotDispatchAnything()
        {
            // Arrange
            var queue = CallQueue.Create(_ => { });
            _fixture.SetupItemIdSet();
            _fixture.SetupStateWithQuantityTypesNotLoaded();
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSetEditorItemIdAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSetEditorItemIdAction_WithAllLoaded_WithIdSet_ShouldDispatchLoadItemAction()
        {
            // Arrange
            _fixture.SetupItemIdSet();
            _fixture.SetupStateWithAllLoaded();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingLoadItemAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSetEditorItemIdAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSetEditorItemIdAction_WithAllLoaded_WithIdSetEmpty_ShouldDispatchNewItemAction()
        {
            // Arrange
            _fixture.SetupItemIdSetEmpty();
            _fixture.SetupStateWithAllLoaded();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingSettingNewItemAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSetEditorItemIdAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSetEditorItemIdActionFixture : ItemEditorEffectsFixture
        {
            public void SetupItemIdSet()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        ItemId = Guid.NewGuid()
                    }
                };
            }

            public void SetupItemIdSetEmpty()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        ItemId = Guid.Empty
                    }
                };
            }

            public void SetupStateWithStoresNotLoaded()
            {
                State = State with
                {
                    Initialization = State.Initialization with
                    {
                        QuantityTypesLoaded = true,
                        QuantityTypesInPacketLoaded = true,
                        StoresLoaded = false
                    }
                };
            }

            public void SetupStateWithQuantityTypesNotLoaded()
            {
                State = State with
                {
                    Initialization = State.Initialization with
                    {
                        QuantityTypesLoaded = false,
                        QuantityTypesInPacketLoaded = true,
                        StoresLoaded = true
                    }
                };
            }

            public void SetupStateWithQuantityTypesInPacketNotLoaded()
            {
                State = State with
                {
                    Initialization = State.Initialization with
                    {
                        QuantityTypesLoaded = true,
                        QuantityTypesInPacketLoaded = false,
                        StoresLoaded = true
                    }
                };
            }

            public void SetupStateWithAllLoaded()
            {
                State = State with
                {
                    Initialization = State.Initialization with
                    {
                        QuantityTypesLoaded = true,
                        QuantityTypesInPacketLoaded = true,
                        StoresLoaded = true
                    }
                };
            }

            public void SetupDispatchingSettingNewItemAction()
            {
                SetupDispatchingAction<SetNewItemAction>();
            }

            public void SetupDispatchingLoadItemAction()
            {
                TestPropertyNotSetException.ThrowIfNull(State.Editor.ItemId);
                SetupDispatchingAction(new LoadItemForEditingAction(State.Editor.ItemId.Value));
            }
        }
    }

    public class HandleLoadQuantityTypesFinishedAction
    {
        private readonly HandleLoadQuantityTypesFinishedActionFixture _fixture = new();

        [Fact]
        public async Task HandleLoadQuantityTypesFinishedAction_WithStoresNotLoaded_ShouldNotDispatchAnything()
        {
            // Arrange
            var queue = CallQueue.Create(_ => { });
            _fixture.SetupItemIdSet();
            _fixture.SetupStateWithStoresNotLoaded();
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesFinishedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesFinishedAction_WithQuantityTypesInPacketNotLoaded_ShouldNotDispatchAnything()
        {
            // Arrange
            var queue = CallQueue.Create(_ => { });
            _fixture.SetupItemIdSet();
            _fixture.SetupStateWithQuantityTypesInPacketNotLoaded();
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesFinishedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesFinishedAction_WithAllLoaded_WithIdNotSet_ShouldNotDispatchAnything()
        {
            // Arrange
            _fixture.SetupItemIdNotSet();
            _fixture.SetupStateWithAllLoaded();
            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesFinishedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesFinishedAction_WithAllLoaded_WithIdSet_ShouldDispatchLoadItemAction()
        {
            // Arrange
            _fixture.SetupItemIdSet();
            _fixture.SetupStateWithAllLoaded();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingLoadItemAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesFinishedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesFinishedAction_WithAllLoaded_WithIdSetEmpty_ShouldDispatchNewItemAction()
        {
            // Arrange
            _fixture.SetupItemIdSetEmpty();
            _fixture.SetupStateWithAllLoaded();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingSettingNewItemAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesFinishedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadQuantityTypesFinishedActionFixture : ItemEditorEffectsFixture
        {
            public void SetupItemIdSet()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        ItemId = Guid.NewGuid()
                    }
                };
            }

            public void SetupItemIdSetEmpty()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        ItemId = Guid.Empty
                    }
                };
            }

            public void SetupItemIdNotSet()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        ItemId = null
                    }
                };
            }

            public void SetupStateWithStoresNotLoaded()
            {
                State = State with
                {
                    Initialization = State.Initialization with
                    {
                        QuantityTypesLoaded = true,
                        QuantityTypesInPacketLoaded = true,
                        StoresLoaded = false
                    }
                };
            }

            public void SetupStateWithQuantityTypesInPacketNotLoaded()
            {
                State = State with
                {
                    Initialization = State.Initialization with
                    {
                        QuantityTypesLoaded = true,
                        QuantityTypesInPacketLoaded = false,
                        StoresLoaded = true
                    }
                };
            }

            public void SetupStateWithAllLoaded()
            {
                State = State with
                {
                    Initialization = State.Initialization with
                    {
                        QuantityTypesLoaded = true,
                        QuantityTypesInPacketLoaded = true,
                        StoresLoaded = true
                    }
                };
            }

            public void SetupDispatchingSettingNewItemAction()
            {
                SetupDispatchingAction<SetNewItemAction>();
            }

            public void SetupDispatchingLoadItemAction()
            {
                TestPropertyNotSetException.ThrowIfNull(State.Editor.ItemId);
                SetupDispatchingAction(new LoadItemForEditingAction(State.Editor.ItemId.Value));
            }
        }
    }

    public class HandleLoadQuantityTypesInPacketFinishedAction
    {
        private readonly HandleLoadQuantityTypesInPacketFinishedActionFixture _fixture = new();

        [Fact]
        public async Task HandleLoadQuantityTypesInPacketFinishedAction_WithStoresNotLoaded_ShouldNotDispatchAnything()
        {
            // Arrange
            var queue = CallQueue.Create(_ => { });
            _fixture.SetupItemIdSet();
            _fixture.SetupStateWithStoresNotLoaded();
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesInPacketFinishedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesInPacketFinishedAction_WithQuantityTypesNotLoaded_ShouldNotDispatchAnything()
        {
            // Arrange
            var queue = CallQueue.Create(_ => { });
            _fixture.SetupItemIdSet();
            _fixture.SetupStateWithQuantityTypesNotLoaded();
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesInPacketFinishedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesInPacketFinishedAction_WithAllLoaded_WithIdNotSet_ShouldNotDispatchAnything()
        {
            // Arrange
            _fixture.SetupItemIdNotSet();
            _fixture.SetupStateWithAllLoaded();
            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesInPacketFinishedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesInPacketFinishedAction_WithAllLoaded_WithIdSet_ShouldDispatchLoadItemAction()
        {
            // Arrange
            _fixture.SetupItemIdSet();
            _fixture.SetupStateWithAllLoaded();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingLoadItemAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesInPacketFinishedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesInPacketFinishedAction_WithAllLoaded_WithIdSetEmpty_ShouldDispatchNewItemAction()
        {
            // Arrange
            _fixture.SetupItemIdSetEmpty();
            _fixture.SetupStateWithAllLoaded();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingSettingNewItemAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesInPacketFinishedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadQuantityTypesInPacketFinishedActionFixture : ItemEditorEffectsFixture
        {
            public void SetupItemIdSet()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        ItemId = Guid.NewGuid()
                    }
                };
            }

            public void SetupItemIdSetEmpty()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        ItemId = Guid.Empty
                    }
                };
            }

            public void SetupItemIdNotSet()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        ItemId = null
                    }
                };
            }

            public void SetupStateWithStoresNotLoaded()
            {
                State = State with
                {
                    Initialization = State.Initialization with
                    {
                        QuantityTypesLoaded = true,
                        QuantityTypesInPacketLoaded = true,
                        StoresLoaded = false
                    }
                };
            }

            public void SetupStateWithQuantityTypesNotLoaded()
            {
                State = State with
                {
                    Initialization = State.Initialization with
                    {
                        QuantityTypesLoaded = false,
                        QuantityTypesInPacketLoaded = true,
                        StoresLoaded = true
                    }
                };
            }

            public void SetupStateWithAllLoaded()
            {
                State = State with
                {
                    Initialization = State.Initialization with
                    {
                        QuantityTypesLoaded = true,
                        QuantityTypesInPacketLoaded = true,
                        StoresLoaded = true
                    }
                };
            }

            public void SetupDispatchingSettingNewItemAction()
            {
                SetupDispatchingAction<SetNewItemAction>();
            }

            public void SetupDispatchingLoadItemAction()
            {
                TestPropertyNotSetException.ThrowIfNull(State.Editor.ItemId);
                SetupDispatchingAction(new LoadItemForEditingAction(State.Editor.ItemId.Value));
            }
        }
    }

    public class HandleLoadActiveStoresFinishedAction
    {
        private readonly HandleLoadActiveStoresFinishedActionFixture _fixture = new();

        [Fact]
        public async Task HandleLoadActiveStoresFinishedAction_WithQuantityTypesNotLoaded_ShouldNotDispatchAnything()
        {
            // Arrange
            var queue = CallQueue.Create(_ => { });
            _fixture.SetupItemIdSet();
            _fixture.SetupStateWithQuantityTypesNotLoaded();
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadActiveStoresFinishedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadActiveStoresFinishedAction_WithQuantityTypesInPacketNotLoaded_ShouldNotDispatchAnything()
        {
            // Arrange
            var queue = CallQueue.Create(_ => { });
            _fixture.SetupItemIdSet();
            _fixture.SetupStateWithQuantityTypesInPacketNotLoaded();
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadActiveStoresFinishedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadActiveStoresFinishedAction_WithAllLoaded_WithIdNotSet_ShouldNotDispatchAnything()
        {
            // Arrange
            _fixture.SetupItemIdNotSet();
            _fixture.SetupStateWithAllLoaded();
            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadActiveStoresFinishedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadActiveStoresFinishedAction_WithAllLoaded_WithIdSet_ShouldDispatchLoadItemAction()
        {
            // Arrange
            _fixture.SetupItemIdSet();
            _fixture.SetupStateWithAllLoaded();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingLoadItemAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadActiveStoresFinishedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadActiveStoresFinishedAction_WithAllLoaded_WithIdSetEmpty_ShouldDispatchNewItemAction()
        {
            // Arrange
            _fixture.SetupItemIdSetEmpty();
            _fixture.SetupStateWithAllLoaded();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingSettingNewItemAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadActiveStoresFinishedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadActiveStoresFinishedActionFixture : ItemEditorEffectsFixture
        {
            public void SetupItemIdSet()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        ItemId = Guid.NewGuid()
                    }
                };
            }

            public void SetupItemIdSetEmpty()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        ItemId = Guid.Empty
                    }
                };
            }

            public void SetupItemIdNotSet()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        ItemId = null
                    }
                };
            }

            public void SetupStateWithQuantityTypesNotLoaded()
            {
                State = State with
                {
                    Initialization = State.Initialization with
                    {
                        QuantityTypesLoaded = false,
                        QuantityTypesInPacketLoaded = true,
                        StoresLoaded = true
                    }
                };
            }

            public void SetupStateWithQuantityTypesInPacketNotLoaded()
            {
                State = State with
                {
                    Initialization = State.Initialization with
                    {
                        QuantityTypesLoaded = true,
                        QuantityTypesInPacketLoaded = false,
                        StoresLoaded = true
                    }
                };
            }

            public void SetupStateWithAllLoaded()
            {
                State = State with
                {
                    Initialization = State.Initialization with
                    {
                        QuantityTypesLoaded = true,
                        QuantityTypesInPacketLoaded = true,
                        StoresLoaded = true
                    }
                };
            }

            public void SetupDispatchingSettingNewItemAction()
            {
                SetupDispatchingAction<SetNewItemAction>();
            }

            public void SetupDispatchingLoadItemAction()
            {
                TestPropertyNotSetException.ThrowIfNull(State.Editor.ItemId);
                SetupDispatchingAction(new LoadItemForEditingAction(State.Editor.ItemId.Value));
            }
        }
    }

    public class HandleLoadItemForEditingAction
    {
        private readonly HandleLoadItemForEditingActionFixture _fixture = new();

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
            });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLoadItemForEditingAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadItemForEditingAction_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupAction();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupGettingItemFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLoadItemForEditingAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadItemForEditingAction_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupAction();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupGettingItemFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });

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

            public void SetupGettingItemFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                var exception = new DomainTestBuilder<ApiException>().Create();
                ApiClientMock.SetupGetItemByIdAsyncThrowing(Action.ItemId, exception);
            }

            public void SetupGettingItemFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                var exception = new DomainTestBuilder<HttpRequestException>().Create();
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
        }
    }

    public class HandleAddStoreAction
    {
        private readonly HandleAddStoreActionFixture _fixture = new();

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
                SetupDispatchingAction(new StoreAddedToItemTypeAction(itemType!.Key));
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
        private readonly HandleChangeStoreActionFixture _fixture = new();

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
        private readonly HandleChangePriceActionFixture _fixture = new();

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
                    new DomainTestBuilder<decimal>().Create());
            }

            public void SetupActionForItemType()
            {
                Action = new ChangePriceAction(
                    new DomainTestBuilder<EditedItemType>().Create(),
                    new DomainTestBuilder<EditedItemAvailability>().Create(),
                    new DomainTestBuilder<decimal>().Create());
            }
        }
    }

    public class HandleChangeDefaultSectionAction
    {
        private readonly HandleChangeDefaultSectionActionFixture _fixture = new();

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
        private readonly HandleRemoveStoreActionFixture _fixture = new();

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
        private readonly HandleCreateItemActionFixture _fixture = new();

        [Fact]
        public async Task HandleCreateItemAction_WithValidationErrors_ShouldNotDispatchAnything()
        {
            // Arrange
            _fixture.SetupItemWithoutTypes();
            _fixture.SetupValidationErrors();
            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleCreateItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
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
                _fixture.SetupSuccessNotification();
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
                _fixture.SetupSuccessNotification();
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
                _fixture.SetupDispatchingExceptionNotificationAction();
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
                _fixture.SetupSuccessNotification();
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
                _fixture.SetupSuccessNotification();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleCreateItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateItemAction_WithTypes_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupItemWithTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupCreatingItemWithTypesFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleCreateItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateItemAction_WithTypes_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupItemWithTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupCreatingItemWithTypesFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
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
            private readonly string _itemName = new DomainTestBuilder<string>().Create();

            public void SetupItemWithoutTypes()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = State.Editor.Item! with
                        {
                            Name = _itemName,
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
                            Name = _itemName,
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
                            Name = _itemName,
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
                            Name = _itemName,
                            ItemMode = ItemMode.NotDefined,
                            ItemTypes = new DomainTestBuilder<EditedItemType>().CreateMany(2).ToList()
                        }
                    }
                };
            }

            public void SetupCreatingItem()
            {
                ApiClientMock.SetupCreateItemAsync(State.Editor.Item!);
            }

            public void SetupCreatingItemFailed()
            {
                ApiClientMock.SetupCreateItemAsyncThrowing(State.Editor.Item!,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupCreatingItemWithTypes()
            {
                ApiClientMock.SetupCreateItemWithTypesAsync(State.Editor.Item!);
            }

            public void SetupCreatingItemWithTypesFailedWithErrorInApi()
            {
                ApiClientMock.SetupCreateItemWithTypesAsyncThrowing(State.Editor.Item!,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupCreatingItemWithTypesFailedWithErrorWhileTransmittingRequest()
            {
                ApiClientMock.SetupCreateItemWithTypesAsyncThrowing(State.Editor.Item!,
                    new DomainTestBuilder<HttpRequestException>().Create());
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
                SetupDispatchingAction(new LeaveItemEditorAction(true));
            }

            public void SetupSuccessNotification()
            {
                ShoppingListNotificationServiceMock.SetupNotifySuccess($"Successfully created item {_itemName}");
            }
        }
    }

    public class HandleUpdateItemAction
    {
        private readonly HandleUpdateItemActionFixture _fixture = new();

        [Fact]
        public async Task HandleUpdateItemAction_WithValidationErrors_ShouldNotDispatchAnything()
        {
            // Arrange
            _fixture.SetupItemWithoutTypes();
            _fixture.SetupValidationErrors();
            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleUpdateItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
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
                _fixture.SetupSuccessNotification();
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
                _fixture.SetupDispatchingExceptionNotificationAction();
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
                _fixture.SetupSuccessNotification();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleUpdateItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleUpdateItemAction_WithTypes_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupItemWithTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupUpdatingItemWithTypesFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleUpdateItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleUpdateItemAction_WithTypes_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupItemWithTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupUpdatingItemWithTypesFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
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
            private readonly string _itemName = new DomainTestBuilder<string>().Create();

            public void SetupItemWithoutTypes()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = State.Editor.Item! with
                        {
                            Name = _itemName,
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
                            Name = _itemName,
                            ItemMode = ItemMode.WithTypes
                        }
                    }
                };
            }

            public void SetupUpdatingItem()
            {
                ApiClientMock.SetupUpdateItemAsync(State.Editor.Item!);
            }

            public void SetupUpdatingItemFailed()
            {
                ApiClientMock.SetupUpdateItemAsyncThrowing(State.Editor.Item!, new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupUpdatingItemWithTypes()
            {
                ApiClientMock.SetupUpdateItemWithTypesAsync(State.Editor.Item!);
            }

            public void SetupUpdatingItemWithTypesFailedWithErrorInApi()
            {
                ApiClientMock.SetupUpdateItemWithTypesAsyncThrowing(State.Editor.Item!,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupUpdatingItemWithTypesFailedWithErrorWhileTransmittingRequest()
            {
                ApiClientMock.SetupUpdateItemWithTypesAsyncThrowing(State.Editor.Item!,
                    new DomainTestBuilder<HttpRequestException>().Create());
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
                SetupDispatchingAction(new LeaveItemEditorAction(true));
            }

            public void SetupSuccessNotification()
            {
                ShoppingListNotificationServiceMock.SetupNotifySuccess($"Successfully updated item {_itemName}");
            }
        }
    }

    public class HandleModifyItemAction
    {
        private readonly HandleModifyItemActionFixture _fixture = new();

        [Fact]
        public async Task HandleModifyItemAction_WithValidationErrors_ShouldNotDispatchAnything()
        {
            // Arrange
            _fixture.SetupItemWithoutTypes();
            _fixture.SetupValidationErrors();
            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleModifyItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
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
                _fixture.SetupSuccessNotification();
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
                _fixture.SetupDispatchingExceptionNotificationAction();
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
                _fixture.SetupSuccessNotification();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleModifyItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleModifyItemAction_WithTypes_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupItemWithTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupModifyingItemWithTypesFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleModifyItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleModifyItemAction_WithTypes_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupItemWithTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupModifyingItemWithTypesFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
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
            private readonly string _itemName = new DomainTestBuilder<string>().Create();

            public void SetupItemWithoutTypes()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = State.Editor.Item! with
                        {
                            Name = _itemName,
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
                            Name = _itemName,
                            ItemMode = ItemMode.WithTypes
                        }
                    }
                };
            }

            public void SetupModifyingItem()
            {
                ApiClientMock.SetupModifyItemAsync(State.Editor.Item!);
            }

            public void SetupModifyingItemFailed()
            {
                ApiClientMock.SetupModifyItemAsyncThrowing(State.Editor.Item!, new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupModifyingItemWithTypes()
            {
                ApiClientMock.SetupModifyItemWithTypesAsync(State.Editor.Item!);
            }

            public void SetupModifyingItemWithTypesFailedWithErrorInApi()
            {
                ApiClientMock.SetupModifyItemWithTypesAsyncThrowing(State.Editor.Item!, new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupModifyingItemWithTypesFailedWithErrorWhileTransmittingRequest()
            {
                ApiClientMock.SetupModifyItemWithTypesAsyncThrowing(State.Editor.Item!, new DomainTestBuilder<HttpRequestException>().Create());
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
                SetupDispatchingAction(new LeaveItemEditorAction(true));
            }

            public void SetupSuccessNotification()
            {
                ShoppingListNotificationServiceMock.SetupNotifySuccess($"Successfully modified item {_itemName}");
            }
        }
    }

    public class HandleMakeItemPermanentAction
    {
        private readonly HandleMakeItemPermanentActionFixture _fixture = new();

        [Fact]
        public async Task HandleMakeItemPermanentAction_WithValidationError_ShouldNotDispatchAnything()
        {
            // Arrange
            _fixture.SetupItemWithoutTypes();
            _fixture.SetupValidationErrors();
            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleMakeItemPermanentAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
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
                _fixture.SetupSuccessNotification();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleMakeItemPermanentAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleMakeItemPermanentAction_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupItemWithoutTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupMakingItemPermanentFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleMakeItemPermanentAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleMakeItemPermanentAction_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupItemWithoutTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupMakingItemPermanentFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
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
            private readonly string _itemName = new DomainTestBuilder<string>().Create();

            public void SetupItemWithoutTypes()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = State.Editor.Item! with
                        {
                            Name = _itemName,
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

            public void SetupMakingItemPermanentFailedWithErrorInApi()
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

            public void SetupMakingItemPermanentFailedWithErrorWhileTransmittingRequest()
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
                    new DomainTestBuilder<HttpRequestException>().Create());
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
                SetupDispatchingAction(new LeaveItemEditorAction(true));
            }

            public void SetupSuccessNotification()
            {
                ShoppingListNotificationServiceMock.SetupNotifySuccess($"Successfully made item {_itemName} permanent");
            }
        }
    }

    public class HandleDeleteItemAction
    {
        private readonly HandleDeleteItemActionFixture _fixture = new();

        [Fact]
        public async Task HandleDeleteItemAction_WithCallSuccessful_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupMakingItemPermanent();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingCloseDialogAction();
                _fixture.SetupSuccessNotification();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleDeleteItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleDeleteItemAction_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupMakingItemPermanentFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleDeleteItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleDeleteItemAction_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupMakingItemPermanentFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
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
            private readonly string _itemName = new DomainTestBuilder<string>().Create();

            public void SetupMakingItemPermanent()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = State.Editor.Item! with
                        {
                            Name = _itemName
                        }
                    }
                };
                var item = State.Editor.Item!;
                ApiClientMock.SetupDeleteItemAsync(item.Id);
            }

            public void SetupMakingItemPermanentFailedWithErrorInApi()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = State.Editor.Item! with
                        {
                            Name = _itemName
                        }
                    }
                };
                var item = State.Editor.Item!;
                ApiClientMock.SetupDeleteItemAsyncThrowing(item.Id, new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupMakingItemPermanentFailedWithErrorWhileTransmittingRequest()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = State.Editor.Item! with
                        {
                            Name = _itemName
                        }
                    }
                };
                var item = State.Editor.Item!;
                ApiClientMock.SetupDeleteItemAsyncThrowing(item.Id, new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<DeleteItemStartedAction>();
            }

            public void SetupDispatchingFinishedAction()
            {
                SetupDispatchingAction<DeleteItemFinishedAction>();
            }

            public void SetupDispatchingCloseDialogAction()
            {
                SetupDispatchingAction(new CloseDeleteItemDialogAction(true));
            }

            public void SetupSuccessNotification()
            {
                ShoppingListNotificationServiceMock.SetupNotifySuccess($"Successfully deleted item {_itemName}");
            }
        }
    }

    private abstract class ItemEditorEffectsFixture : ItemEffectsFixtureBase
    {
        protected ShoppingListNotificationServiceMock ShoppingListNotificationServiceMock { get; } =
            new(MockBehavior.Strict);

        public ItemEditorEffects CreateSut()
        {
            SetupStateReturningState();
            return new ItemEditorEffects(ApiClientMock.Object, ItemStateMock.Object, NavigationManagerMock.Object,
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