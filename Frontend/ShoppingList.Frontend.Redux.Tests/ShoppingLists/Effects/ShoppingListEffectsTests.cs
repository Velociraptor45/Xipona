using Moq;
using Moq.Contrib.InOrder;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.PriceUpdater;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.Summary;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Effects;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.ShoppingLists.Effects;

public class ShoppingListEffectsTests
{
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

                ExpectedLoadFinishedAction =
                    new LoadAllActiveStoresFinishedAction(new AllActiveStores(ExpectedStoresForShoppingList));
                SetupDispatchingAction(ExpectedLoadFinishedAction);
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
            return new ShoppingListEffects(ApiClientMock.Object, CommandQueueMock.Object, ShoppingListStateMock.Object);
        }
    }
}