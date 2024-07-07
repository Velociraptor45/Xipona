using Moq;
using Moq.Contrib.InOrder;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.Items;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.InitialStoreCreator;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Persistence;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.PriceUpdater;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Summary;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.TemporaryItemCreator;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Effects;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States.Comparer;
using ProjectHermes.Xipona.Frontend.Redux.Stores.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Shared.Ports;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;
using ProjectHermes.Xipona.Frontend.TestTools.Extensions;
using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.ShoppingLists.Effects;

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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupExpectedQuantityTypes();
                _fixture.SetupGettingQuantityTypes();
                _fixture.SetupDispatchingLoadFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesAction_WithWithApiException_ShouldCallEndpointAndDispatchActionInCorrectOrder()
        {
            // Arrange
            _fixture.SetupStateContainingNoQuantityTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingQuantityTypesThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesAction_WithWithHttpException_ShouldCallEndpointAndDispatchActionInCorrectOrder()
        {
            // Arrange
            _fixture.SetupStateContainingNoQuantityTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingQuantityTypesThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorNotificationAction();
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

            public void SetupGettingQuantityTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedQuantityTypes);
                ApiClientMock.SetupGetAllQuantityTypesAsync(_expectedQuantityTypes);
            }

            public void SetupGettingQuantityTypesThrowsApiException()
            {
                ApiClientMock.SetupGetAllQuantityTypesAsyncThrowing(
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingQuantityTypesThrowsHttpRequestException()
            {
                ApiClientMock.SetupGetAllQuantityTypesAsyncThrowing(
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingLoadFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedQuantityTypes);

                _expectedLoadFinishedAction = new LoadQuantityTypesFinishedAction(_expectedQuantityTypes);
                SetupDispatchingAction(_expectedLoadFinishedAction);
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupExpectedQuantityTypesInPacket();
                _fixture.SetupGettingQuantityTypesInPacket();
                _fixture.SetupDispatchingLoadFinishedAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingQuantityTypesInPacketThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingQuantityTypesInPacketThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorNotificationAction();
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

            public void SetupGettingQuantityTypesInPacket()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedQuantityTypesInPacket);
                ApiClientMock.SetupGetAllQuantityTypesInPacketAsync(_expectedQuantityTypesInPacket);
            }

            public void SetupGettingQuantityTypesInPacketThrowsApiException()
            {
                ApiClientMock.SetupGetAllQuantityTypesInPacketAsyncThrowing(
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingQuantityTypesInPacketThrowsHttpRequestException()
            {
                ApiClientMock.SetupGetAllQuantityTypesInPacketAsyncThrowing(
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingLoadFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedQuantityTypesInPacket);

                _expectedLoadFinishedAction = new LoadQuantityTypesInPacketFinishedAction(_expectedQuantityTypesInPacket);
                SetupDispatchingAction(_expectedLoadFinishedAction);
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupExpectedStoresEmpty();
                _fixture.SetupFindingStoresForShoppingList();
                _fixture.SetupDispatchingNoStoresFoundAction();
                _fixture.SetupDispatchingLoadFinishedAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupExpectedStores();
                _fixture.SetupFindingStoresForShoppingList();
                _fixture.SetupDispatchingLoadFinishedAction();
                _fixture.SetupDispatchingChangeAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupExpectedStores();
                _fixture.SetupFindingStoresForShoppingListThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupExpectedStores();
                _fixture.SetupFindingStoresForShoppingListThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorNotificationAction();
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

            public void SetupFindingStoresForShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedStoresForShoppingList);
                ApiClientMock.SetupGetAllActiveStoresForShoppingListAsync(_expectedStoresForShoppingList);
            }

            public void SetupFindingStoresForShoppingListThrowsApiException()
            {
                ApiClientMock.SetupGetAllActiveStoresForShoppingListAsyncThrowing(
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupFindingStoresForShoppingListThrowsHttpRequestException()
            {
                ApiClientMock.SetupGetAllActiveStoresForShoppingListAsyncThrowing(
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingChangeAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedStoresForShoppingList);
                _expectedStoreChangeAction = new SelectedStoreChangedAction(_expectedStoresForShoppingList.Last().Id);
                SetupDispatchingAction(_expectedStoreChangeAction);
            }

            public void VerifyNotDispatchingChangeAction()
            {
                DispatcherMock.Verify(m => m.Dispatch(It.IsAny<SelectedStoreChangedAction>()), Times.Never);
            }

            public void SetupDispatchingLoadFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedStoresForShoppingList);

                _expectedLoadFinishedAction = new LoadAllActiveStoresFinishedAction(_expectedStoresForShoppingList);
                SetupDispatchingAction(_expectedLoadFinishedAction);
            }

            public void SetupDispatchingNoStoresFoundAction()
            {
                SetupDispatchingAction<NoStoresFoundAction>();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupAction();
                _fixture.SetupExpectedShoppingList();
                _fixture.SetupGettingQuantityTypesInPacket();
                _fixture.SetupDispatchingLoadFinishedAction();
                _fixture.SetupDispatchingResetEditModeAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupAction();
                _fixture.SetupGettingQuantityTypesInPacketThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupAction();
                _fixture.SetupGettingQuantityTypesInPacketThrowsHttpRequestException();
                _fixture.SetupDispatchingLoadFromLocalStorageAction();
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

            public void SetupGettingQuantityTypesInPacket()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedShoppingList);
                ApiClientMock.SetupGetActiveShoppingListByStoreIdAsync(_storeId, _expectedShoppingList);
            }

            public void SetupGettingQuantityTypesInPacketThrowsApiException()
            {
                ApiClientMock.SetupGetActiveShoppingListByStoreIdAsyncThrowing(_storeId,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingQuantityTypesInPacketThrowsHttpRequestException()
            {
                ApiClientMock.SetupGetActiveShoppingListByStoreIdAsyncThrowing(_storeId,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingLoadFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedShoppingList);

                _expectedLoadFinishedAction = new LoadShoppingListFinishedAction(_expectedShoppingList);
                SetupDispatchingAction(_expectedLoadFinishedAction);
            }

            public void SetupDispatchingLoadFromLocalStorageAction()
            {
                SetupDispatchingAction(new LoadShoppingListFromLocalStorageAction(_storeId));
            }

            public void SetupDispatchingResetEditModeAction()
            {
                SetupDispatchingAction<ResetEditModeAction>();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStoreId();
                _fixture.SetupExpectedShoppingList();
                _fixture.SetupGettingQuantityTypesInPacket();
                _fixture.SetupDispatchingLoadFinishedAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStoreId();
                _fixture.SetupGettingQuantityTypesInPacketThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStoreId();
                _fixture.SetupGettingQuantityTypesInPacketThrowsHttpRequestException();
                _fixture.SetupDispatchingLoadFromLocalStorageAction();
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

            public void SetupGettingQuantityTypesInPacket()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedShoppingList);
                ApiClientMock.SetupGetActiveShoppingListByStoreIdAsync(_storeId, _expectedShoppingList);
            }

            public void SetupGettingQuantityTypesInPacketThrowsApiException()
            {
                ApiClientMock.SetupGetActiveShoppingListByStoreIdAsyncThrowing(_storeId,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingQuantityTypesInPacketThrowsHttpRequestException()
            {
                ApiClientMock.SetupGetActiveShoppingListByStoreIdAsyncThrowing(_storeId,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingLoadFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedShoppingList);

                _expectedLoadFinishedAction = new LoadShoppingListFinishedAction(_expectedShoppingList);
                SetupDispatchingAction(_expectedLoadFinishedAction);
            }

            public void SetupDispatchingLoadFromLocalStorageAction()
            {
                SetupDispatchingAction(new LoadShoppingListFromLocalStorageAction(_storeId));
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupQuantityType();
                _fixture.SetupQuantityTypeUnitInTemporaryItemCreator();
                _fixture.SetupItemForQuantityTypeUnit();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupEnqueuingRequest();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingCloseAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupQuantityType();
                _fixture.SetupQuantityTypeWeightInTemporaryItemCreator();
                _fixture.SetupItemForQuantityTypeWeight();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupEnqueuingRequest();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingCloseAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveTemporaryItemAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSaveTemporaryItemActionFixture : ShoppingListEffectsFixture
        {
            private SaveTemporaryItemFinishedAction? _expectedFinishedAction;
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
                    false);
            }

            public void SetupEnqueuingRequest()
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

                CommandQueueMock.SetupEnqueue(req => req.IsRequestEquivalentTo(request, new List<string> { "TemporaryId" }));
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<SaveTemporaryItemStartedAction>();
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);

                _expectedFinishedAction = new SaveTemporaryItemFinishedAction(_item, State.TemporaryItemCreator.Section!);
                SetupDispatchingAction<SaveTemporaryItemFinishedAction>(
                    action => action.IsEquivalentTo(_expectedFinishedAction, new List<string> { "Item.Id.OfflineId.Value" }));
            }

            public void SetupDispatchingCloseAction()
            {
                SetupDispatchingAnyAction<CloseTemporaryItemCreatorAction>();
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

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupUpdatingItemPrice();
                _fixture.SetupDispatchingFinishActionForAllTypes();
                _fixture.SetupDispatchingCloseAction();
                _fixture.SetupSuccessNotification();
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

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupUpdatingItemPrice();
                _fixture.SetupDispatchingFinishActionForOneType();
                _fixture.SetupDispatchingCloseAction();
                _fixture.SetupSuccessNotification();
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

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupUpdatingItemPriceThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
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

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupUpdatingItemPriceThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorNotificationAction();
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
            private SavePriceUpdateFinishedAction? _expectedFinishAction;

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

            public void SetupDispatchingStartAction()
            {
                SetupDispatchingAction<SavePriceUpdateStartedAction>();
            }

            public void SetupDispatchingCloseAction()
            {
                SetupDispatchingAction<ClosePriceUpdaterAction>();
            }

            public void SetupDispatchingFinishActionForAllTypes()
            {
                _expectedFinishAction = new SavePriceUpdateFinishedAction(
                    State.PriceUpdate.Item!.Id.ActualId!.Value,
                    null,
                    State.PriceUpdate.Price);
                SetupDispatchingAction(_expectedFinishAction);
            }

            public void SetupDispatchingFinishActionForOneType()
            {
                _expectedFinishAction = new SavePriceUpdateFinishedAction(
                    State.PriceUpdate.Item!.Id.ActualId!.Value,
                    State.PriceUpdate.Item.TypeId,
                    State.PriceUpdate.Price);
                SetupDispatchingAction(_expectedFinishAction);
            }

            public void SetupUpdatingItemPrice()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRequest);
                ApiClientMock.SetupUpdateItemPriceAsync(_expectedRequest);
            }

            public void SetupUpdatingItemPriceThrowsApiException()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRequest);
                ApiClientMock.SetupUpdateItemPriceAsyncThrowing(
                    _expectedRequest,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupUpdatingItemPriceThrowsHttpRequestException()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRequest);
                ApiClientMock.SetupUpdateItemPriceAsyncThrowing(
                    _expectedRequest,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupSuccessNotification()
            {
                ShoppingListNotificationServiceMock.SetupNotifySuccess("Successfully updated item price");
            }
        }
    }

    public class HandleFinishShoppingListAction
    {
        private readonly HandleFinishShoppingListActionFixture _fixture = new();

        public static IEnumerable<object[]> GetTestDates()
        {
            yield return new object[] { new DateTimeOffset(2020, 04, 30, 02, 45, 23, TimeSpan.FromHours(-2)) };
            yield return new object[] { new DateTimeOffset(2020, 04, 30, 02, 45, 23, TimeSpan.FromHours(6)) };
            yield return new object[] { new DateTimeOffset(2020, 04, 30, 02, 45, 23, TimeSpan.Zero) };
            yield return new object[] { DateTimeOffset.UtcNow };
        }

        [Theory]
        [MemberData(nameof(GetTestDates))]
        public async Task HandleFinishShoppingListAction_ShouldCallEndpointAndDispatchActionsInCorrectOrder(
            DateTimeOffset expectedFinishedAt)
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupExpectedFinishRequest(expectedFinishedAt);
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupFinishingList();
                _fixture.SetupDispatchingFinishAction();
                _fixture.SetupDispatchingReloadShoppingListAction();
                _fixture.SetupDispatchingResetEditModeAction();
                _fixture.SetupSuccessNotification();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupExpectedFinishRequest(DateTimeOffset.UtcNow);
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupFinishingListThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFinishAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupExpectedFinishRequest(DateTimeOffset.UtcNow);
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupFinishingListThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorNotificationAction();
                _fixture.SetupDispatchingFinishAction();
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

            public void SetupDispatchingStartAction()
            {
                SetupDispatchingAction<FinishShoppingListStartedAction>();
            }

            public void SetupDispatchingFinishAction()
            {
                SetupDispatchingAction<FinishShoppingListFinishedAction>();
            }

            public void SetupDispatchingReloadShoppingListAction()
            {
                SetupDispatchingAction<ReloadCurrentShoppingListAction>();
            }

            public void SetupDispatchingResetEditModeAction()
            {
                SetupDispatchingAction<ResetEditModeAction>();
            }

            public void SetupFinishingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedFinishRequest);
                ApiClientMock.SetupFinishListAsync(_expectedFinishRequest);
            }

            public void SetupFinishingListThrowsApiException()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedFinishRequest);
                ApiClientMock.SetupFinishListAsyncThrowing(
                    _expectedFinishRequest,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupFinishingListThrowsHttpRequestException()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedFinishRequest);
                ApiClientMock.SetupFinishListAsyncThrowing(
                    _expectedFinishRequest,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupSuccessNotification()
            {
                ShoppingListNotificationServiceMock.SetupNotifySuccess("Finished shopping list");
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStoreName();
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupCreatingStoreSucceeded();
                _fixture.SetupDispatchingFinishAction();
                _fixture.SetupDispatchingLoadAllActiveStoresAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStoreName();
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupCreatingStoreThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFinishAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStoreName();
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupCreatingStoreThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorNotificationAction();
                _fixture.SetupDispatchingFinishAction();
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

            public void SetupCreatingStoreSucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedStore);
                ApiClientMock.SetupCreateStoreAsync(_expectedStore);
            }

            public void SetupCreatingStoreThrowsApiException()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedStore);
                ApiClientMock.SetupCreateStoreAsyncThrowing(_expectedStore,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupCreatingStoreThrowsHttpRequestException()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedStore);
                ApiClientMock.SetupCreateStoreAsyncThrowing(_expectedStore,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingStartAction()
            {
                SetupDispatchingAction<CreateInitialStoreStartedAction>();
            }

            public void SetupDispatchingFinishAction()
            {
                SetupDispatchingAction<CreateInitialStoreFinishedAction>();
            }

            public void SetupDispatchingLoadAllActiveStoresAction()
            {
                SetupDispatchingAction<LoadAllActiveStoresAction>();
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