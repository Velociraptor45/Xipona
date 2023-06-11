﻿using Moq;
using Moq.Contrib.InOrder;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.PriceUpdater;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.Summary;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.TemporaryItemCreator;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Effects;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;
using ProjectHermes.ShoppingList.Frontend.TestTools.Extensions;
using RestEase;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.ShoppingLists.Effects;

public class ShoppingListEffectsTests
{
    public class HandleLoadQuantityTypesAction
    {
        private readonly HandleLoadQuantityTypesActionFixture _fixture = new();

        [Fact]
        public async Task HandleLoadQuantityTypesAction_WithSuccessfulRequest_ShouldDispatchFinishedAction()
        {
            // Arrange
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingQuantityTypesThrowsApiException();
                _fixture.SetupDispatchingApiExceptionAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingQuantityTypesThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorAction();
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

            public void SetupDispatchingApiExceptionAction()
            {
                SetupDispatchingAnyAction<DisplayApiExceptionNotificationAction>();
            }

            public void SetupDispatchingErrorAction()
            {
                SetupDispatchingAnyAction<DisplayErrorNotificationAction>();
            }
        }
    }

    public class HandleLoadQuantityTypesInPacketAction
    {
        private readonly HandleLoadQuantityTypesInPacketActionFixture _fixture = new();

        [Fact]
        public async Task HandleLoadQuantityTypesInPacketAction_WithSuccessfulRequest_ShouldDispatchFinishedAction()
        {
            // Arrange
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingQuantityTypesInPacketThrowsApiException();
                _fixture.SetupDispatchingApiExceptionAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingQuantityTypesInPacketThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorAction();
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

            public void SetupDispatchingApiExceptionAction()
            {
                SetupDispatchingAnyAction<DisplayApiExceptionNotificationAction>();
            }

            public void SetupDispatchingErrorAction()
            {
                SetupDispatchingAnyAction<DisplayErrorNotificationAction>();
            }
        }
    }

    public class HandleLoadAllActiveStoresAction
    {
        private readonly HandleLoadAllActiveStoresActionFixture _fixture = new();

        [Fact]
        public async Task HandleLoadAllActiveStoresAction_WithoutStores_ShouldDispatchFinishedAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupExpectedStoresEmpty();
                _fixture.SetupFindingStoresForShoppingList();
                _fixture.SetupDispatchingLoadFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadAllActiveStoresAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadAllActiveStoresAction_WithoutStores_ShouldNotDispatchChangeAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupExpectedStoresEmpty();
                _fixture.SetupFindingStoresForShoppingList();
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

        private sealed class HandleLoadAllActiveStoresActionFixture : ShoppingListEffectsFixture
        {
            public IReadOnlyCollection<ShoppingListStore>? ExpectedStoresForShoppingList { get; private set; }
            public LoadAllActiveStoresFinishedAction? ExpectedLoadFinishedAction { get; private set; }
            public SelectedStoreChangedAction? ExpectedStoreChangeAction { get; private set; }

            public void SetupExpectedStoresEmpty()
            {
                ExpectedStoresForShoppingList = new List<ShoppingListStore>();
            }

            public void SetupExpectedStores()
            {
                ExpectedStoresForShoppingList = new DomainTestBuilder<ShoppingListStore>().CreateMany(2).ToList();
            }

            public void SetupFindingStoresForShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedStoresForShoppingList);
                ApiClientMock.SetupGetAllActiveStoresForShoppingListAsync(ExpectedStoresForShoppingList);
            }

            public void SetupDispatchingChangeAction()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedStoresForShoppingList);
                ExpectedStoreChangeAction = new SelectedStoreChangedAction(ExpectedStoresForShoppingList.First().Id);
                SetupDispatchingAction(ExpectedStoreChangeAction);
            }

            public void VerifyNotDispatchingChangeAction()
            {
                DispatcherMock.Verify(m => m.Dispatch(It.IsAny<SelectedStoreChangedAction>()), Times.Never);
            }

            public void SetupDispatchingLoadFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedStoresForShoppingList);

                ExpectedLoadFinishedAction = new LoadAllActiveStoresFinishedAction(ExpectedStoresForShoppingList);
                SetupDispatchingAction(ExpectedLoadFinishedAction);
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
                _fixture.SetupDispatchingApiExceptionAction();
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
                _fixture.SetupDispatchingErrorAction();
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
            private Guid _storeId = Guid.NewGuid();
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

            public void SetupAction()
            {
                Action = new SelectedStoreChangedAction(_storeId);
            }

            public void SetupDispatchingApiExceptionAction()
            {
                SetupDispatchingAnyAction<DisplayApiExceptionNotificationAction>();
            }

            public void SetupDispatchingErrorAction()
            {
                SetupDispatchingAnyAction<DisplayErrorNotificationAction>();
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
        private readonly HandleSavePriceUpdateActionFixture _fixture;

        public HandleSavePriceUpdateAction()
        {
            _fixture = new HandleSavePriceUpdateActionFixture();
        }

        [Fact]
        public async Task HandleSavePriceUpdateAction_WithUpdatingAllTypes_ShouldCallApiAndDispatchActionsInCorrectOrderAsync()
        {
            // Arrange
            _fixture.SetupPriceUpdateForAllTypes();
            _fixture.SetupExpectedRequestForAllTypes();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupCallingEndpoint();
                _fixture.SetupDispatchingFinishActionForAllTypes();
                _fixture.SetupDispatchingCloseAction();
            });

            _fixture.SetupStateReturningState();
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSavePriceUpdateAction(_fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyCallingEndpoint();
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSavePriceUpdateAction_WithUpdatingOneType_ShouldCallApiAndDispatchActionsInCorrectOrderAsync()
        {
            // Arrange
            _fixture.SetupPriceUpdateForOneType();
            _fixture.SetupExpectedRequestForOneType();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupCallingEndpoint();
                _fixture.SetupDispatchingFinishActionForOneType();
                _fixture.SetupDispatchingCloseAction();
            });

            _fixture.SetupStateReturningState();
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSavePriceUpdateAction(_fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyCallingEndpoint();
            queue.VerifyOrder();
        }

        private sealed class HandleSavePriceUpdateActionFixture : ShoppingListEffectsFixture
        {
            public UpdateItemPriceRequest? ExpectedRequest { get; private set; }
            public SavePriceUpdateFinishedAction? ExpectedFinishAction { get; private set; }

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
                ExpectedRequest = new UpdateItemPriceRequest(
                    State.PriceUpdate.Item!.Id.ActualId!.Value,
                    null,
                    State.SelectedStoreId,
                    State.PriceUpdate.Price);
            }

            public void SetupExpectedRequestForOneType()
            {
                ExpectedRequest = new UpdateItemPriceRequest(
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
                ExpectedFinishAction = new SavePriceUpdateFinishedAction(
                    State.PriceUpdate.Item!.Id.ActualId!.Value,
                    null,
                    State.PriceUpdate.Price);
                SetupDispatchingAction(ExpectedFinishAction);
            }

            public void SetupDispatchingFinishActionForOneType()
            {
                ExpectedFinishAction = new SavePriceUpdateFinishedAction(
                    State.PriceUpdate.Item!.Id.ActualId!.Value,
                    State.PriceUpdate.Item.TypeId,
                    State.PriceUpdate.Price);
                SetupDispatchingAction(ExpectedFinishAction);
            }

            public void SetupCallingEndpoint()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedRequest);
                ApiClientMock.SetupUpdateItemPriceAsync(ExpectedRequest);
            }

            public void VerifyCallingEndpoint()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedRequest);
                ApiClientMock.VerifyUpdateItemPriceAsync(ExpectedRequest, Times.Once);
            }
        }
    }

    public class HandleFinishShoppingListAction
    {
        private readonly HandleFinishShoppingListActionFixture _fixture;

        public HandleFinishShoppingListAction()
        {
            _fixture = new HandleFinishShoppingListActionFixture();
        }

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
                _fixture.SetupExpectedFinisheRequest(expectedFinishedAt);
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupFinishingList();
                _fixture.SetupDispatchingFinishAction();
                _fixture.SetupDispatchingStoreChangeAction();
            });

            _fixture.SetupStateReturningState();
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleFinishShoppingListAction(_fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyFinishingList();
            queue.VerifyOrder();
        }

        private sealed class HandleFinishShoppingListActionFixture : ShoppingListEffectsFixture
        {
            public FinishListRequest? ExpectedFinisheRequest { get; private set; }
            public SelectedStoreChangedAction? ExpectedStoreChangeAction { get; private set; }

            public void SetupExpectedFinisheRequest(DateTimeOffset? expectedFinishDate)
            {
                ExpectedFinisheRequest = new DomainTestBuilder<FinishListRequest>()
                    .FillConstructorWith("finishedAt", expectedFinishDate)
                    .Create();
                State = State with
                {
                    ShoppingList = State.ShoppingList! with
                    {
                        Id = ExpectedFinisheRequest.ShoppingListId
                    },
                    Summary = State.Summary with
                    {
                        FinishedAt = new DateTime(
                            ExpectedFinisheRequest.FinishedAt!.Value.DateTime
                                .Add(-ExpectedFinisheRequest.FinishedAt.Value.Offset)
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

            public void SetupDispatchingStoreChangeAction()
            {
                ExpectedStoreChangeAction = new SelectedStoreChangedAction(State.SelectedStoreId);
                SetupDispatchingAction(ExpectedStoreChangeAction);
            }

            public void SetupFinishingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedFinisheRequest);
                ApiClientMock.SetupFinishListAsync(ExpectedFinisheRequest);
            }

            public void VerifyFinishingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedFinisheRequest);
                ApiClientMock.VerifyFinishListAsync(ExpectedFinisheRequest, Times.Once);
            }
        }
    }

    private abstract class ShoppingListEffectsFixture : ShoppingListEffectsFixtureBase
    {
        public ShoppingListEffects CreateSut()
        {
            SetupStateReturningState();
            return new ShoppingListEffects(ApiClientMock.Object, CommandQueueMock.Object, ShoppingListStateMock.Object);
        }
    }
}