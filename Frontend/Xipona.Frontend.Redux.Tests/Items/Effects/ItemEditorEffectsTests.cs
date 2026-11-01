using Moq;
using Moq.Contrib.InOrder;
using Xipona.Frontend.Redux.Items.Actions.Editor;
using Xipona.Frontend.Redux.Items.Actions.Editor.Availabilities;
using Xipona.Frontend.Redux.Items.Effects;
using Xipona.Frontend.Redux.Items.States;
using Xipona.Frontend.Redux.Shared.Ports.Requests.Items;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.Shared.Ports;
using Xipona.Frontend.TestTools.Exceptions;
using RestEase;
using Xipona.Frontend.Redux.Items.Actions;
using Xipona.Frontend.Redux.Items.Actions.Editor.Saving;

namespace Xipona.Frontend.Redux.Tests.Items.Effects;

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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingLoadItemAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingSettingNewItemAction(x0);
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

            public void SetupDispatchingSettingNewItemAction(IQueueComponent component)
            {
                SetupDispatchingAction<SetNewItemAction>(component);
            }

            public void SetupDispatchingLoadItemAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(State.Editor.ItemId);
                SetupDispatchingAction(new LoadItemForEditingAction(State.Editor.ItemId.Value), component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingLoadItemAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingSettingNewItemAction(x0);
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

            public void SetupDispatchingSettingNewItemAction(IQueueComponent component)
            {
                SetupDispatchingAction<SetNewItemAction>(component);
            }

            public void SetupDispatchingLoadItemAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(State.Editor.ItemId);
                SetupDispatchingAction(new LoadItemForEditingAction(State.Editor.ItemId.Value), component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingLoadItemAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingSettingNewItemAction(x0);
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

            public void SetupDispatchingSettingNewItemAction(IQueueComponent component)
            {
                SetupDispatchingAction<SetNewItemAction>(component);
            }

            public void SetupDispatchingLoadItemAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(State.Editor.ItemId);
                SetupDispatchingAction(new LoadItemForEditingAction(State.Editor.ItemId.Value), component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingLoadItemAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingSettingNewItemAction(x0);
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

            public void SetupDispatchingSettingNewItemAction(IQueueComponent component)
            {
                SetupDispatchingAction<SetNewItemAction>(component);
            }

            public void SetupDispatchingLoadItemAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(State.Editor.ItemId);
                SetupDispatchingAction(new LoadItemForEditingAction(State.Editor.ItemId.Value), component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupGettingItem(x0);
                _fixture.SetupDispatchingFinishAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupGettingItemFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupGettingItemFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
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

            public void SetupGettingItem(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(Action);
                TestPropertyNotSetException.ThrowIfNull(_item);

                ApiClientMock.SetupGetItemByIdAsync(Action.ItemId, _item, component);
            }

            public void SetupGettingItemFailedWithErrorInApi(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                var exception = new DomainTestBuilder<ApiException>().Create();
                ApiClientMock.SetupGetItemByIdAsyncThrowing(Action.ItemId, exception, component);
            }

            public void SetupGettingItemFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                var exception = new DomainTestBuilder<HttpRequestException>().Create();
                ApiClientMock.SetupGetItemByIdAsyncThrowing(Action.ItemId, exception, component);
            }

            public void SetupDispatchingStartAction(IQueueComponent component)
            {
                SetupDispatchingAction<LoadItemForEditingStartedAction>(component);
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_item);

                SetupDispatchingAction(new LoadItemForEditingFinishedAction(_item), component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingAddedItemAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingAddedItemTypeAction(x0);
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

            public void SetupDispatchingAddedItemAction(IQueueComponent component)
            {
                SetupDispatchingAction<StoreAddedToItemAction>(component);
            }

            public void SetupDispatchingAddedItemTypeAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                var itemType = Action.Available as EditedItemType;
                SetupDispatchingAction(new StoreAddedToItemTypeAction(itemType!.Key), component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingItemAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingItemTypeAction(x0);
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

            public void SetupDispatchingItemAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                SetupDispatchingAction(new StoreOfItemChangedAction(Action.Availability, Action.StoreId), component);
            }

            public void SetupDispatchingItemTypeAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                var itemType = Action.Available as EditedItemType;
                SetupDispatchingAction(new StoreOfItemTypeChangedAction(itemType!, Action.Availability, Action.StoreId),
                    component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingItemAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingItemTypeAction(x0);
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

            public void SetupDispatchingItemAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                SetupDispatchingAction(new PriceOfItemChangedAction(Action.Availability, Action.Price), component);
            }

            public void SetupDispatchingItemTypeAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                var itemType = Action.Available as EditedItemType;
                SetupDispatchingAction(new PriceOfItemTypeChangedAction(itemType!, Action.Availability, Action.Price), component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingItemAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingItemTypeAction(x0);
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

            public void SetupDispatchingItemAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                SetupDispatchingAction(new DefaultSectionOfItemChangedAction(Action.Availability, Action.DefaultSectionId),
                    component);
            }

            public void SetupDispatchingItemTypeAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                var itemType = Action.Available as EditedItemType;
                SetupDispatchingAction(new DefaultSectionOfItemTypeChangedAction(itemType!, Action.Availability, Action.DefaultSectionId),
                    component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingItemAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingItemTypeAction(x0);
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

            public void SetupDispatchingItemAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                SetupDispatchingAction(new StoreOfItemRemovedAction(Action.Availability), component);
            }

            public void SetupDispatchingItemTypeAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                var itemType = Action.Available as EditedItemType;
                SetupDispatchingAction(new StoreOfItemTypeRemovedAction(itemType!, Action.Availability), component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupCreatingItem(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingLeaveAction(x0);
                _fixture.SetupSuccessNotification(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupCreatingItem(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingLeaveAction(x0);
                _fixture.SetupSuccessNotification(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupCreatingItemFailed(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupCreatingItemWithTypes(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingLeaveAction(x0);
                _fixture.SetupSuccessNotification(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupCreatingItemWithTypes(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingLeaveAction(x0);
                _fixture.SetupSuccessNotification(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupCreatingItemWithTypesFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupCreatingItemWithTypesFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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

            public void SetupCreatingItem(IQueueComponent component)
            {
                ApiClientMock.SetupCreateItemAsync(State.Editor.Item!, component);
            }

            public void SetupCreatingItemFailed(IQueueComponent component)
            {
                ApiClientMock.SetupCreateItemAsyncThrowing(State.Editor.Item!,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupCreatingItemWithTypes(IQueueComponent component)
            {
                ApiClientMock.SetupCreateItemWithTypesAsync(State.Editor.Item!, component);
            }

            public void SetupCreatingItemWithTypesFailedWithErrorInApi(IQueueComponent component)
            {
                ApiClientMock.SetupCreateItemWithTypesAsyncThrowing(State.Editor.Item!,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupCreatingItemWithTypesFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                ApiClientMock.SetupCreateItemWithTypesAsyncThrowing(State.Editor.Item!,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingStartedAction(IQueueComponent component)
            {
                SetupDispatchingAction<CreateItemStartedAction>(component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                SetupDispatchingAction<CreateItemFinishedAction>(component);
            }

            public void SetupDispatchingLeaveAction(IQueueComponent component)
            {
                SetupDispatchingAction(new LeaveItemViewAction(true), component);
            }

            public void SetupSuccessNotification(IQueueComponent component)
            {
                ShoppingListNotificationServiceMock
                    .SetupNotifySuccess($"Successfully created item {_itemName}", 2f, component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupUpdatingItem(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingLeaveAction(x0);
                _fixture.SetupSuccessNotification(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupUpdatingItemFailed(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupUpdatingItemWithTypes(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingLeaveAction(x0);
                _fixture.SetupSuccessNotification(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupUpdatingItemWithTypesFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupUpdatingItemWithTypesFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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

            public void SetupUpdatingItem(IQueueComponent component)
            {
                ApiClientMock.SetupUpdateItemAsync(State.Editor.Item!, component);
            }

            public void SetupUpdatingItemFailed(IQueueComponent component)
            {
                ApiClientMock.SetupUpdateItemAsyncThrowing(State.Editor.Item!,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupUpdatingItemWithTypes(IQueueComponent component)
            {
                ApiClientMock.SetupUpdateItemWithTypesAsync(State.Editor.Item!, component);
            }

            public void SetupUpdatingItemWithTypesFailedWithErrorInApi(IQueueComponent component)
            {
                ApiClientMock.SetupUpdateItemWithTypesAsyncThrowing(State.Editor.Item!,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupUpdatingItemWithTypesFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                ApiClientMock.SetupUpdateItemWithTypesAsyncThrowing(State.Editor.Item!,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingStartedAction(IQueueComponent component)
            {
                SetupDispatchingAction<UpdateItemStartedAction>(component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                SetupDispatchingAction<UpdateItemFinishedAction>(component);
            }

            public void SetupDispatchingLeaveAction(IQueueComponent component)
            {
                SetupDispatchingAction(new LeaveItemViewAction(true), component);
            }

            public void SetupSuccessNotification(IQueueComponent component)
            {
                ShoppingListNotificationServiceMock
                    .SetupNotifySuccess($"Successfully updated item {_itemName}", 2f, component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupModifyingItem(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingLeaveAction(x0);
                _fixture.SetupSuccessNotification(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupModifyingItemFailed(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupModifyingItemWithTypes(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingLeaveAction(x0);
                _fixture.SetupSuccessNotification(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupModifyingItemWithTypesFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupModifyingItemWithTypesFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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

            public void SetupModifyingItem(IQueueComponent component)
            {
                ApiClientMock.SetupModifyItemAsync(State.Editor.Item!, component);
            }

            public void SetupModifyingItemFailed(IQueueComponent component)
            {
                ApiClientMock.SetupModifyItemAsyncThrowing(State.Editor.Item!,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupModifyingItemWithTypes(IQueueComponent component)
            {
                ApiClientMock.SetupModifyItemWithTypesAsync(State.Editor.Item!, component);
            }

            public void SetupModifyingItemWithTypesFailedWithErrorInApi(IQueueComponent component)
            {
                ApiClientMock.SetupModifyItemWithTypesAsyncThrowing(State.Editor.Item!,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupModifyingItemWithTypesFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                ApiClientMock.SetupModifyItemWithTypesAsyncThrowing(State.Editor.Item!,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingStartedAction(IQueueComponent component)
            {
                SetupDispatchingAction<ModifyItemStartedAction>(component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                SetupDispatchingAction<ModifyItemFinishedAction>(component);
            }

            public void SetupDispatchingLeaveAction(IQueueComponent component)
            {
                SetupDispatchingAction(new LeaveItemViewAction(true), component);
            }

            public void SetupSuccessNotification(IQueueComponent component)
            {
                ShoppingListNotificationServiceMock
                    .SetupNotifySuccess($"Successfully modified item {_itemName}", 2f, component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupMakingItemPermanent(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingLeaveAction(x0);
                _fixture.SetupSuccessNotification(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupMakingItemPermanentFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupMakingItemPermanentFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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

            public void SetupMakingItemPermanent(IQueueComponent component)
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
                ApiClientMock.SetupMakeTemporaryItemPermanent(request, component);
            }

            public void SetupMakingItemPermanentFailedWithErrorInApi(IQueueComponent component)
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
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupMakingItemPermanentFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
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
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingStartedAction(IQueueComponent component)
            {
                SetupDispatchingAction<MakeItemPermanentStartedAction>(component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                SetupDispatchingAction<MakeItemPermanentFinishedAction>(component);
            }

            public void SetupDispatchingLeaveAction(IQueueComponent component)
            {
                SetupDispatchingAction(new LeaveItemViewAction(true), component);
            }

            public void SetupSuccessNotification(IQueueComponent component)
            {
                ShoppingListNotificationServiceMock
                    .SetupNotifySuccess($"Successfully made item {_itemName} permanent", 2f, component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupMakingItemPermanent(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingCloseDialogAction(x0);
                _fixture.SetupSuccessNotification(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupMakingItemPermanentFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupMakingItemPermanentFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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

            public void SetupMakingItemPermanent(IQueueComponent component)
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
                ApiClientMock.SetupDeleteItemAsync(item.Id, component);
            }

            public void SetupMakingItemPermanentFailedWithErrorInApi(IQueueComponent component)
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
                ApiClientMock.SetupDeleteItemAsyncThrowing(item.Id, new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupMakingItemPermanentFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
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
                ApiClientMock.SetupDeleteItemAsyncThrowing(item.Id, new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingStartedAction(IQueueComponent component)
            {
                SetupDispatchingAction<DeleteItemStartedAction>(component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                SetupDispatchingAction<DeleteItemFinishedAction>(component);
            }

            public void SetupDispatchingCloseDialogAction(IQueueComponent component)
            {
                SetupDispatchingAction(new CloseDeleteItemDialogAction(true), component);
            }

            public void SetupSuccessNotification(IQueueComponent component)
            {
                ShoppingListNotificationServiceMock
                    .SetupNotifySuccess($"Successfully deleted item {_itemName}", 2f, component);
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