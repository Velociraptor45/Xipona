using Moq;
using Moq.Contrib.InOrder;
using Xipona.Frontend.Redux.Shared.Ports.Requests.Items;
using Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using Xipona.Frontend.Redux.Shared.States;
using Xipona.Frontend.Redux.ShoppingList.Actions;
using Xipona.Frontend.Redux.ShoppingList.Actions.InitialStoreCreator;
using Xipona.Frontend.Redux.ShoppingList.Actions.Persistence;
using Xipona.Frontend.Redux.ShoppingList.Actions.PriceUpdater;
using Xipona.Frontend.Redux.ShoppingList.Actions.Summary;
using Xipona.Frontend.Redux.ShoppingList.Actions.TemporaryItemCreator;
using Xipona.Frontend.Redux.ShoppingList.Effects;
using Xipona.Frontend.Redux.ShoppingList.States;
using Xipona.Frontend.Redux.ShoppingList.States.Comparer;
using Xipona.Frontend.Redux.Stores.States;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.Shared.Ports;
using Xipona.Frontend.TestTools.Exceptions;
using Xipona.Frontend.TestTools.Extensions;
using RestEase;

namespace Xipona.Frontend.Redux.Tests.ShoppingLists.Effects;

public class ShoppingListEffectsTests
{
    public class HandleLoadQuantityTypesAction
    {
        private readonly HandleLoadQuantityTypesActionFixture _fixture = new();

        [Fact]
        public async Task HandleLoadQuantityTypesAction_WithQuantityTypesInState_ShouldNotDoAnything()
        {
            // Arrange
            _fixture.SetupStateContainingQuantityTypes();
            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesAction_WithSuccessfulRequest_ShouldDispatchFinishedAction()
        {
            // Arrange
            _fixture.SetupStateContainingNoQuantityTypes();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupExpectedQuantityTypes();
                _fixture.SetupGettingQuantityTypes(x0);
                _fixture.SetupDispatchingLoadFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesAction_WithApiException_ShouldCallEndpointAndDispatchActionInCorrectOrder()
        {
            // Arrange
            _fixture.SetupStateContainingNoQuantityTypes();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingQuantityTypesThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesAction_WithHttpException_ShouldCallEndpointAndDispatchActionInCorrectOrder()
        {
            // Arrange
            _fixture.SetupStateContainingNoQuantityTypes();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingQuantityTypesThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadQuantityTypesActionFixture : ShoppingListEffectsFixture
        {
            private IReadOnlyCollection<QuantityType>? _expectedQuantityTypes;
            private LoadQuantityTypesFinishedAction? _expectedLoadFinishedAction;

            public void SetupExpectedQuantityTypes()
            {
                _expectedQuantityTypes = new DomainTestBuilder<QuantityType>().CreateMany(2).ToList();
            }

            public void SetupGettingQuantityTypes(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedQuantityTypes);
                ApiClientMock.SetupGetAllQuantityTypesAsync(_expectedQuantityTypes, component);
            }

            public void SetupGettingQuantityTypesThrowsApiException(IQueueComponent component)
            {
                ApiClientMock.SetupGetAllQuantityTypesAsyncThrowing(
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupGettingQuantityTypesThrowsHttpRequestException(IQueueComponent component)
            {
                ApiClientMock.SetupGetAllQuantityTypesAsyncThrowing(
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingLoadFinishedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedQuantityTypes);

                _expectedLoadFinishedAction = new LoadQuantityTypesFinishedAction(_expectedQuantityTypes);
                SetupDispatchingAction(_expectedLoadFinishedAction, component);
            }

            public void SetupStateContainingNoQuantityTypes()
            {
                State = State with { QuantityTypes = new List<QuantityType>() };
            }

            public void SetupStateContainingQuantityTypes()
            {
                State = State with { QuantityTypes = new List<QuantityType> { new DomainTestBuilder<QuantityType>().Create() } };
            }
        }
    }

    public class HandleLoadQuantityTypesInPacketAction
    {
        private readonly HandleLoadQuantityTypesInPacketActionFixture _fixture = new();

        [Fact]
        public async Task HandleLoadQuantityTypesInPacketAction_WithQuantityTypesInState_ShouldNotDoAnything()
        {
            // Arrange
            _fixture.SetupStateContainingQuantityTypesInPacket();
            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesInPacketAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesInPacketAction_WithSuccessfulRequest_ShouldDispatchFinishedAction()
        {
            // Arrange
            _fixture.SetupStateContainingNoQuantityTypesInPacket();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupExpectedQuantityTypesInPacket();
                _fixture.SetupGettingQuantityTypesInPacket(x0);
                _fixture.SetupDispatchingLoadFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesInPacketAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesInPacketAction_WithWithApiException_ShouldCallEndpointAndDispatchActionInCorrectOrder()
        {
            // Arrange
            _fixture.SetupStateContainingNoQuantityTypesInPacket();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingQuantityTypesInPacketThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesInPacketAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesInPacketAction_WithWithHttpException_ShouldCallEndpointAndDispatchActionInCorrectOrder()
        {
            // Arrange
            _fixture.SetupStateContainingNoQuantityTypesInPacket();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingQuantityTypesInPacketThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesInPacketAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadQuantityTypesInPacketActionFixture : ShoppingListEffectsFixture
        {
            private IReadOnlyCollection<QuantityTypeInPacket>? _expectedQuantityTypesInPacket;
            private LoadQuantityTypesInPacketFinishedAction? _expectedLoadFinishedAction;

            public void SetupExpectedQuantityTypesInPacket()
            {
                _expectedQuantityTypesInPacket = new DomainTestBuilder<QuantityTypeInPacket>().CreateMany(2).ToList();
            }

            public void SetupGettingQuantityTypesInPacket(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedQuantityTypesInPacket);
                ApiClientMock.SetupGetAllQuantityTypesInPacketAsync(_expectedQuantityTypesInPacket, component);
            }

            public void SetupGettingQuantityTypesInPacketThrowsApiException(IQueueComponent component)
            {
                ApiClientMock.SetupGetAllQuantityTypesInPacketAsyncThrowing(
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupGettingQuantityTypesInPacketThrowsHttpRequestException(IQueueComponent component)
            {
                ApiClientMock.SetupGetAllQuantityTypesInPacketAsyncThrowing(
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingLoadFinishedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedQuantityTypesInPacket);

                _expectedLoadFinishedAction = new LoadQuantityTypesInPacketFinishedAction(_expectedQuantityTypesInPacket);
                SetupDispatchingAction(_expectedLoadFinishedAction, component);
            }

            public void SetupStateContainingNoQuantityTypesInPacket()
            {
                State = State with { QuantityTypesInPacket = new List<QuantityTypeInPacket>() };
            }

            public void SetupStateContainingQuantityTypesInPacket()
            {
                State = State with
                {
                    QuantityTypesInPacket = new List<QuantityTypeInPacket>
                    {
                        new DomainTestBuilder<QuantityTypeInPacket>().Create()
                    }
                };
            }
        }
    }

    public class HandleLoadAllActiveStoresAction
    {
        private readonly HandleLoadAllActiveStoresActionFixture _fixture = new();

        [Fact]
        public async Task HandleLoadAllActiveStoresAction_WithStateContainingStores_ShouldNotDoAnything()
        {
            // Arrange
            _fixture.SetupStateContainingStores();
            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadAllActiveStoresAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadAllActiveStoresAction_WithoutFindingStores_ShouldDispatchNoStoresFoundAction()
        {
            // Arrange
            _fixture.SetupStateContainingNoStores();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupExpectedStoresEmpty();
                _fixture.SetupFindingStoresForShoppingList(x0);
                _fixture.SetupDispatchingNoStoresFoundAction(x0);
                _fixture.SetupDispatchingLoadFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadAllActiveStoresAction(_fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyNotDispatchingChangeAction();
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadAllActiveStoresAction_WithStores_ShouldCallEndpointAndDispatchActionInCorrectOrder()
        {
            // Arrange
            _fixture.SetupStateContainingNoStores();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupExpectedStores();
                _fixture.SetupFindingStoresForShoppingList(x0);
                _fixture.SetupDispatchingLoadFinishedAction(x0);
                _fixture.SetupDispatchingChangeAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadAllActiveStoresAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadAllActiveStoresAction_WithApiException_ShouldDispatchExceptionNotificationAction()
        {
            // Arrange
            _fixture.SetupStateContainingNoStores();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupExpectedStores();
                _fixture.SetupFindingStoresForShoppingListThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadAllActiveStoresAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadAllActiveStoresAction_WithHttpRequestException_ShouldDispatchErrorNotificationAction()
        {
            // Arrange
            _fixture.SetupStateContainingNoStores();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupExpectedStores();
                _fixture.SetupFindingStoresForShoppingListThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadAllActiveStoresAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadAllActiveStoresActionFixture : ShoppingListEffectsFixture
        {
            private IReadOnlyCollection<ShoppingListStore>? _expectedStoresForShoppingList;
            private LoadAllActiveStoresFinishedAction? _expectedLoadFinishedAction;
            private SelectedStoreChangedAction? _expectedStoreChangeAction;

            public void SetupExpectedStoresEmpty()
            {
                _expectedStoresForShoppingList = [];
            }

            public void SetupExpectedStores()
            {
                _expectedStoresForShoppingList =
                [
                    new DomainTestBuilder<ShoppingListStore>().Create() with
                    {
                        Name = "Bstore" + new DomainTestBuilder<string>().Create()
                    },
                    new DomainTestBuilder<ShoppingListStore>().Create() with
                    {
                        Name = "Astore" + new DomainTestBuilder<string>().Create()
                    }
                ];
            }

            public void SetupFindingStoresForShoppingList(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedStoresForShoppingList);
                ApiClientMock.SetupGetAllActiveStoresForShoppingListAsync(_expectedStoresForShoppingList, component);
            }

            public void SetupFindingStoresForShoppingListThrowsApiException(IQueueComponent component)
            {
                ApiClientMock.SetupGetAllActiveStoresForShoppingListAsyncThrowing(
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupFindingStoresForShoppingListThrowsHttpRequestException(IQueueComponent component)
            {
                ApiClientMock.SetupGetAllActiveStoresForShoppingListAsyncThrowing(
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingChangeAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedStoresForShoppingList);
                _expectedStoreChangeAction = new SelectedStoreChangedAction(_expectedStoresForShoppingList.Last().Id);
                SetupDispatchingAction(_expectedStoreChangeAction, component);
            }

            public void VerifyNotDispatchingChangeAction()
            {
                DispatcherMock.Verify(m => m.Dispatch(It.IsAny<SelectedStoreChangedAction>()), Times.Never);
            }

            public void SetupDispatchingLoadFinishedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedStoresForShoppingList);

                _expectedLoadFinishedAction = new LoadAllActiveStoresFinishedAction(_expectedStoresForShoppingList);
                SetupDispatchingAction(_expectedLoadFinishedAction, component);
            }

            public void SetupDispatchingNoStoresFoundAction(IQueueComponent component)
            {
                SetupDispatchingAction<NoStoresFoundAction>(component);
            }

            public void SetupStateContainingNoStores()
            {
                State = State with
                {
                    Stores = State.Stores with
                    {
                        Stores = new List<ShoppingListStore>()
                    }
                };
            }

            public void SetupStateContainingStores()
            {
                State = State with
                {
                    Stores = State.Stores with
                    {
                        Stores = new DomainTestBuilder<ShoppingListStore>().CreateMany(2).ToList()
                    }
                };
            }
        }
    }

    public class HandleSelectedStoreChangedAction
    {
        private readonly HandleSelectedStoreChangedActionFixture _fixture = new();

        [Fact]
        public async Task HandleSelectedStoreChangedAction_WithValidStoreId_ShouldDispatchFinishedAction()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupAction();
                _fixture.SetupExpectedShoppingList();
                _fixture.SetupGettingQuantityTypesInPacket(x0);
                _fixture.SetupDispatchingLoadFinishedAction(x0);
                _fixture.SetupDispatchingResetEditModeAction(x0);
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSelectedStoreChangedAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSelectedStoreChangedAction_WithWithApiException_ShouldCallEndpointAndDispatchActionInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupAction();
                _fixture.SetupGettingQuantityTypesInPacketThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSelectedStoreChangedAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSelectedStoreChangedAction_WithWithHttpException_ShouldCallEndpointAndDispatchActionInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupAction();
                _fixture.SetupGettingQuantityTypesInPacketThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingLoadFromLocalStorageAction(x0);
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSelectedStoreChangedAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSelectedStoreChangedActionFixture : ShoppingListEffectsFixture
        {
            private readonly Guid _storeId = Guid.NewGuid();
            private ShoppingListModel? _expectedShoppingList;
            private LoadShoppingListFinishedAction? _expectedLoadFinishedAction;

            public SelectedStoreChangedAction? Action { get; private set; }

            public void SetupExpectedShoppingList()
            {
                _expectedShoppingList = new DomainTestBuilder<ShoppingListModel>().Create();
            }

            public void SetupGettingQuantityTypesInPacket(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedShoppingList);
                ApiClientMock.SetupGetActiveShoppingListByStoreIdAsync(_storeId, _expectedShoppingList, component);
            }

            public void SetupGettingQuantityTypesInPacketThrowsApiException(IQueueComponent component)
            {
                ApiClientMock.SetupGetActiveShoppingListByStoreIdAsyncThrowing(_storeId,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupGettingQuantityTypesInPacketThrowsHttpRequestException(IQueueComponent component)
            {
                ApiClientMock.SetupGetActiveShoppingListByStoreIdAsyncThrowing(_storeId,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingLoadFinishedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedShoppingList);

                _expectedLoadFinishedAction = new LoadShoppingListFinishedAction(_expectedShoppingList);
                SetupDispatchingAction(_expectedLoadFinishedAction, component);
            }

            public void SetupDispatchingLoadFromLocalStorageAction(IQueueComponent component)
            {
                SetupDispatchingAction(new LoadShoppingListFromLocalStorageAction(_storeId), component);
            }

            public void SetupDispatchingResetEditModeAction(IQueueComponent component)
            {
                SetupDispatchingAction<ResetEditModeAction>(component);
            }

            public void SetupAction()
            {
                Action = new SelectedStoreChangedAction(_storeId);
            }
        }
    }

    public class HandleReloadCurrentShoppingListAction
    {
        private readonly HandleReloadCurrentShoppingListActionFixture _fixture = new();

        [Fact]
        public async Task HandleReloadCurrentShoppingListAction_WithValidStoreId_ShouldDispatchFinishedAction()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupStoreId();
                _fixture.SetupExpectedShoppingList();
                _fixture.SetupGettingQuantityTypesInPacket(x0);
                _fixture.SetupDispatchingLoadFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleReloadCurrentShoppingListAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleReloadCurrentShoppingListAction_WithWithApiException_ShouldCallEndpointAndDispatchActionInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupStoreId();
                _fixture.SetupGettingQuantityTypesInPacketThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleReloadCurrentShoppingListAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleReloadCurrentShoppingListAction_WithWithHttpException_ShouldCallEndpointAndDispatchActionInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupStoreId();
                _fixture.SetupGettingQuantityTypesInPacketThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingLoadFromLocalStorageAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleReloadCurrentShoppingListAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleReloadCurrentShoppingListActionFixture : ShoppingListEffectsFixture
        {
            private readonly Guid _storeId = Guid.NewGuid();
            private ShoppingListModel? _expectedShoppingList;
            private LoadShoppingListFinishedAction? _expectedLoadFinishedAction;

            public void SetupExpectedShoppingList()
            {
                _expectedShoppingList = new DomainTestBuilder<ShoppingListModel>().Create();
            }

            public void SetupStoreId()
            {
                State = State with { SelectedStoreId = _storeId };
            }

            public void SetupGettingQuantityTypesInPacket(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedShoppingList);
                ApiClientMock.SetupGetActiveShoppingListByStoreIdAsync(_storeId, _expectedShoppingList, component);
            }

            public void SetupGettingQuantityTypesInPacketThrowsApiException(IQueueComponent component)
            {
                ApiClientMock.SetupGetActiveShoppingListByStoreIdAsyncThrowing(_storeId,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupGettingQuantityTypesInPacketThrowsHttpRequestException(IQueueComponent component)
            {
                ApiClientMock.SetupGetActiveShoppingListByStoreIdAsyncThrowing(_storeId,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingLoadFinishedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedShoppingList);

                _expectedLoadFinishedAction = new LoadShoppingListFinishedAction(_expectedShoppingList);
                SetupDispatchingAction(_expectedLoadFinishedAction, component);
            }

            public void SetupDispatchingLoadFromLocalStorageAction(IQueueComponent component)
            {
                SetupDispatchingAction(new LoadShoppingListFromLocalStorageAction(_storeId), component);
            }
        }
    }

    public class HandleSaveTemporaryItemAction
    {
        private readonly HandleSaveTemporaryItemActionFixture _fixture = new();

        [Fact]
        public async Task HandleSaveTemporaryItemAction_WithUnit_WithSuccessfulEnqueue_ShouldDispatchFinishedAndCloseAction()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupQuantityType();
                _fixture.SetupQuantityTypeUnitInTemporaryItemCreator();
                _fixture.SetupItemForQuantityTypeUnit();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupDispatchingAddItemAction(x0);
                _fixture.SetupEnqueuingRequest(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingCloseAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveTemporaryItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSaveTemporaryItemAction_WithWeight_WithSuccessfulEnqueue_ShouldDispatchFinishedAndCloseAction()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupQuantityType();
                _fixture.SetupQuantityTypeWeightInTemporaryItemCreator();
                _fixture.SetupItemForQuantityTypeWeight();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupDispatchingAddItemAction(x0);
                _fixture.SetupEnqueuingRequest(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingCloseAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveTemporaryItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSaveTemporaryItemActionFixture : ShoppingListEffectsFixture
        {
            private AddTemporaryItemAction? _expectedFinishedAction;
            private ShoppingListItem? _item;
            private QuantityType? _quantityType;

            public void SetupQuantityType()
            {
                State = State with
                {
                    QuantityTypes = new List<QuantityType>
                    {
                        new DomainTestBuilder<QuantityType>().Create(),
                        new DomainTestBuilder<QuantityType>().Create() with { Id = 1 },
                        new DomainTestBuilder<QuantityType>().Create() with { Id = 0 },
                    }
                };
            }

            public void SetupQuantityTypeUnitInTemporaryItemCreator()
            {
                _quantityType = State.QuantityTypes.Last();
                State = State with
                {
                    TemporaryItemCreator = State.TemporaryItemCreator with
                    {
                        SelectedQuantityTypeId = _quantityType.Id
                    }
                };
            }

            public void SetupQuantityTypeWeightInTemporaryItemCreator()
            {
                _quantityType = State.QuantityTypes.ElementAt(1);
                State = State with
                {
                    TemporaryItemCreator = State.TemporaryItemCreator with
                    {
                        SelectedQuantityTypeId = _quantityType.Id
                    }
                };
            }

            public void SetupItemForQuantityTypeUnit()
            {
                TestPropertyNotSetException.ThrowIfNull(_quantityType);

                _item = new ShoppingListItem(
                    ShoppingListItemId.FromOfflineId(Guid.NewGuid()),
                    TypeId: null,
                    State.TemporaryItemCreator.ItemName,
                    IsTemporary: true,
                    State.TemporaryItemCreator.Price,
                    _quantityType,
                    QuantityInPacket: 1,
                    State.QuantityTypesInPacket.First(),
                    ItemCategory: "",
                    Manufacturer: "",
                    IsInBasket: false,
                    Quantity: _quantityType.DefaultQuantity,
                    false,
                    false);
            }

            public void SetupItemForQuantityTypeWeight()
            {
                TestPropertyNotSetException.ThrowIfNull(_quantityType);

                _item = new ShoppingListItem(
                    ShoppingListItemId.FromOfflineId(Guid.NewGuid()),
                    TypeId: null,
                    State.TemporaryItemCreator.ItemName,
                    IsTemporary: true,
                    State.TemporaryItemCreator.Price,
                    _quantityType,
                    QuantityInPacket: null,
                    QuantityInPacketType: null,
                    ItemCategory: "",
                    Manufacturer: "",
                    IsInBasket: false,
                    Quantity: _quantityType.DefaultQuantity,
                    false,
                    false);
            }

            public void SetupEnqueuingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_quantityType);

                var request = new AddTemporaryItemToShoppingListRequest(
                    Guid.NewGuid(),
                    State.ShoppingList!.Id,
                    State.TemporaryItemCreator.ItemName,
                    _quantityType.Id,
                    _quantityType.DefaultQuantity,
                    State.TemporaryItemCreator.Price,
                    State.TemporaryItemCreator.Section!.Id, Guid.NewGuid());

                CommandQueueMock.SetupEnqueue(req => req.IsRequestEquivalentTo(request, new List<string> { "TemporaryId" }),
                    component);
            }

            public void SetupDispatchingStartedAction(IQueueComponent component)
            {
                SetupDispatchingAction<SaveTemporaryItemStartedAction>(component);
            }

            public void SetupDispatchingAddItemAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_item);

                _expectedFinishedAction = new AddTemporaryItemAction(_item, State.TemporaryItemCreator.Section!);
                SetupDispatchingAction<AddTemporaryItemAction>(
                    action => action.IsEquivalentTo(_expectedFinishedAction, new List<string> { "Item.Id.OfflineId.Value" }),
                    component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                SetupDispatchingAction<SaveTemporaryItemFinishedAction>(component);
            }

            public void SetupDispatchingCloseAction(IQueueComponent component)
            {
                SetupDispatchingAnyAction<CloseTemporaryItemCreatorAction>(component);
            }
        }
    }

    public class HandleSavePriceUpdateAction
    {
        private readonly HandleSavePriceUpdateActionFixture _fixture = new();

        [Fact]
        public async Task HandleSavePriceUpdateAction_WithUpdatingAllTypes_ShouldCallApiAndDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupPriceUpdateForAllTypes();
            _fixture.SetupExpectedRequestForAllTypes();

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupUpdatingItemPrice(x0);
                _fixture.SetupDispatchingFinishAction(x0);
                _fixture.SetupDispatchingCloseAction(x0);
                _fixture.SetupDispatchingReloadShoppingListAction(x0);
                _fixture.SetupSuccessNotification(x0);
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSavePriceUpdateAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSavePriceUpdateAction_WithUpdatingOneType_ShouldCallApiAndDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupPriceUpdateForOneType();
            _fixture.SetupExpectedRequestForOneType();

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupUpdatingItemPrice(x0);
                _fixture.SetupDispatchingFinishAction(x0);
                _fixture.SetupDispatchingCloseAction(x0);
                _fixture.SetupDispatchingReloadShoppingListAction(x0);
                _fixture.SetupSuccessNotification(x0);
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSavePriceUpdateAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSavePriceUpdateAction_WithApiException_ShouldDispatchExceptionNotificationAction()
        {
            // Arrange
            _fixture.SetupPriceUpdateForOneType();
            _fixture.SetupExpectedRequestForOneType();

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupUpdatingItemPriceThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSavePriceUpdateAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSavePriceUpdateAction_WithHttpRequestException_ShouldDispatchErrorNotificationAction()
        {
            // Arrange
            _fixture.SetupPriceUpdateForOneType();
            _fixture.SetupExpectedRequestForOneType();

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupUpdatingItemPriceThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSavePriceUpdateAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSavePriceUpdateActionFixture : ShoppingListEffectsFixture
        {
            private UpdateItemPriceRequest? _expectedRequest;

            public void SetupPriceUpdateForAllTypes()
            {
                State = State with
                {
                    PriceUpdate = State.PriceUpdate with
                    {
                        UpdatePriceForAllTypes = true,
                        Item = State.PriceUpdate.Item! with
                        {
                            TypeId = Guid.NewGuid()
                        }
                    }
                };
            }

            public void SetupPriceUpdateForOneType()
            {
                State = State with
                {
                    PriceUpdate = State.PriceUpdate with
                    {
                        UpdatePriceForAllTypes = false,
                        Item = State.PriceUpdate.Item! with
                        {
                            TypeId = Guid.NewGuid()
                        }
                    }
                };
            }

            public void SetupExpectedRequestForAllTypes()
            {
                _expectedRequest = new UpdateItemPriceRequest(
                    State.PriceUpdate.Item!.Id.ActualId!.Value,
                    null,
                    State.SelectedStoreId,
                    State.PriceUpdate.Price);
            }

            public void SetupExpectedRequestForOneType()
            {
                _expectedRequest = new UpdateItemPriceRequest(
                    State.PriceUpdate.Item!.Id.ActualId!.Value,
                    State.PriceUpdate.Item.TypeId,
                    State.SelectedStoreId,
                    State.PriceUpdate.Price);
            }

            public void SetupDispatchingStartAction(IQueueComponent component)
            {
                SetupDispatchingAction<SavePriceUpdateStartedAction>(component);
            }

            public void SetupDispatchingCloseAction(IQueueComponent component)
            {
                SetupDispatchingAction<ClosePriceUpdaterAction>(component);
            }

            public void SetupDispatchingReloadShoppingListAction(IQueueComponent component)
            {
                SetupDispatchingAction<ReloadCurrentShoppingListAction>(component);
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                SetupDispatchingAction<SavePriceUpdateFinishedAction>(component);
            }

            public void SetupUpdatingItemPrice(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRequest);
                ApiClientMock.SetupUpdateItemPriceAsync(_expectedRequest, component);
            }

            public void SetupUpdatingItemPriceThrowsApiException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRequest);
                ApiClientMock.SetupUpdateItemPriceAsyncThrowing(
                    _expectedRequest,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupUpdatingItemPriceThrowsHttpRequestException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRequest);
                ApiClientMock.SetupUpdateItemPriceAsyncThrowing(
                    _expectedRequest,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupSuccessNotification(IQueueComponent component)
            {
                ShoppingListNotificationServiceMock.SetupNotifySuccess("Successfully updated item price", 2f, component);
            }
        }
    }

    public class HandleFinishShoppingListAction
    {
        private readonly HandleFinishShoppingListActionFixture _fixture = new();

        public static IEnumerable<object[]> GetTestDates()
        {
            yield return [new DateTimeOffset(2020, 04, 30, 02, 45, 23, TimeSpan.FromHours(-2))];
            yield return [new DateTimeOffset(2020, 04, 30, 02, 45, 23, TimeSpan.FromHours(6))];
            yield return [new DateTimeOffset(2020, 04, 30, 02, 45, 23, TimeSpan.Zero)];
            yield return [DateTimeOffset.UtcNow];
        }

        [Theory]
        [MemberData(nameof(GetTestDates))]
        public async Task HandleFinishShoppingListAction_ShouldCallEndpointAndDispatchActionsInCorrectOrder(
            DateTimeOffset expectedFinishedAt)
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupExpectedFinishRequest(expectedFinishedAt);
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupFinishingList(x0);
                _fixture.SetupDispatchingFinishAction(x0);
                _fixture.SetupDispatchingReloadShoppingListAction(x0);
                _fixture.SetupDispatchingResetEditModeAction(x0);
                _fixture.SetupSuccessNotification(x0);
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleFinishShoppingListAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleFinishShoppingListAction_WithApiException_ShouldDispatchExceptionNotificationAction()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupExpectedFinishRequest(DateTimeOffset.UtcNow);
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupFinishingListThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishAction(x0);
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleFinishShoppingListAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleFinishShoppingListAction_WithHttpRequestException_ShouldDispatchErrorNotificationAction()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupExpectedFinishRequest(DateTimeOffset.UtcNow);
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupFinishingListThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishAction(x0);
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleFinishShoppingListAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleFinishShoppingListActionFixture : ShoppingListEffectsFixture
        {
            private FinishListRequest? _expectedFinishRequest;

            public void SetupExpectedFinishRequest(DateTimeOffset? expectedFinishDate)
            {
                _expectedFinishRequest = new DomainTestBuilder<FinishListRequest>()
                    .FillConstructorWith("finishedAt", expectedFinishDate)
                    .Create();
                State = State with
                {
                    ShoppingList = State.ShoppingList! with
                    {
                        Id = _expectedFinishRequest.ShoppingListId
                    },
                    Summary = State.Summary with
                    {
                        FinishedAt = new DateTime(
                            expectedFinishDate!.Value.DateTime
                                .Add(-expectedFinishDate.Value.Offset)
                                .Ticks,
                            DateTimeKind.Utc)
                    }
                };
            }

            public void SetupDispatchingStartAction(IQueueComponent component)
            {
                SetupDispatchingAction<FinishShoppingListStartedAction>(component);
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                SetupDispatchingAction<FinishShoppingListFinishedAction>(component);
            }

            public void SetupDispatchingReloadShoppingListAction(IQueueComponent component)
            {
                SetupDispatchingAction<ReloadCurrentShoppingListAction>(component);
            }

            public void SetupDispatchingResetEditModeAction(IQueueComponent component)
            {
                SetupDispatchingAction<ResetEditModeAction>(component);
            }

            public void SetupFinishingList(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedFinishRequest);
                ApiClientMock.SetupFinishListAsync(_expectedFinishRequest, component);
            }

            public void SetupFinishingListThrowsApiException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedFinishRequest);
                ApiClientMock.SetupFinishListAsyncThrowing(
                    _expectedFinishRequest,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupFinishingListThrowsHttpRequestException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedFinishRequest);
                ApiClientMock.SetupFinishListAsyncThrowing(
                    _expectedFinishRequest,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupSuccessNotification(IQueueComponent component)
            {
                ShoppingListNotificationServiceMock.SetupNotifySuccess("Finished shopping list", 2f, component);
            }
        }
    }

    public class HandleCreateInitialStoreAction
    {
        private readonly HandleCreateInitialStoreActionFixture _fixture = new();

        [Fact]
        public async Task HandleCreateInitialStoreAction_WithValidName_ShouldCallEndpointAndDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupStoreName();
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupCreatingStoreSucceeded(x0);
                _fixture.SetupDispatchingFinishAction(x0);
                _fixture.SetupDispatchingLoadAllActiveStoresAction(x0);
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleCreateInitialStoreAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateInitialStoreAction_WithApiException_ShouldDispatchExceptionNotificationAction()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupStoreName();
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupCreatingStoreThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishAction(x0);
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleCreateInitialStoreAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateInitialStoreAction_WithHttpRequestException_ShouldDispatchErrorNotificationAction()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupStoreName();
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupCreatingStoreThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishAction(x0);
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleCreateInitialStoreAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleCreateInitialStoreActionFixture : ShoppingListEffectsFixture
        {
            private EditedStore? _expectedStore;

            public void SetupStoreName()
            {
                var name = new DomainTestBuilder<string>().Create();
                State = State with
                {
                    InitialStoreCreator = State.InitialStoreCreator with
                    {
                        Name = name
                    }
                };
                _expectedStore = new EditedStore(
                    Guid.Empty,
                    name,
                    new SortedSet<EditedSection>(new SortingIndexComparer())
                    {
                        new(Guid.Empty, Guid.Empty, "Default", true, 0)
                    });
            }

            public void SetupCreatingStoreSucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedStore);
                ApiClientMock.SetupCreateStoreAsync(_expectedStore, component);
            }

            public void SetupCreatingStoreThrowsApiException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedStore);
                ApiClientMock.SetupCreateStoreAsyncThrowing(_expectedStore,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupCreatingStoreThrowsHttpRequestException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedStore);
                ApiClientMock.SetupCreateStoreAsyncThrowing(_expectedStore,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingStartAction(IQueueComponent component)
            {
                SetupDispatchingAction<CreateInitialStoreStartedAction>(component);
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                SetupDispatchingAction<CreateInitialStoreFinishedAction>(component);
            }

            public void SetupDispatchingLoadAllActiveStoresAction(IQueueComponent component)
            {
                SetupDispatchingAction<LoadAllActiveStoresAction>(component);
            }
        }
    }

    private abstract class ShoppingListEffectsFixture : ShoppingListEffectsFixtureBase
    {
        protected ShoppingListNotificationServiceMock ShoppingListNotificationServiceMock { get; } =
            new(MockBehavior.Strict);

        public ShoppingListEffects CreateSut()
        {
            SetupStateReturningState();
            return new ShoppingListEffects(ApiClientMock.Object, CommandQueueMock.Object, ShoppingListStateMock.Object,
                ShoppingListNotificationServiceMock.Object);
        }
    }
}