using Moq;
using Moq.Sequences;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.PriceUpdater;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Effects;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;
using ProjectHermes.ShoppingList.Frontend.TestTools.Extensions;

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
            using var seq = Sequence.Create();

            _fixture.SetupExpectedStoresEmpty();
            _fixture.SetupFindingStoresForShoppingList();
            _fixture.SetupDispatchingLoadFinishedAction();
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadAllActiveStoresAction(_fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyDispatchingLoadFinishedAction();
        }

        [Fact]
        public async Task HandleLoadAllActiveStoresAction_WithoutStores_ShouldNotDispatchChangeAction()
        {
            // Arrange
            using var seq = Sequence.Create();

            _fixture.SetupExpectedStoresEmpty();
            _fixture.SetupFindingStoresForShoppingList();
            _fixture.SetupDispatchingLoadFinishedAction();
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadAllActiveStoresAction(_fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyNotDispatchingChangeAction();
        }

        [Fact]
        public async Task HandleLoadAllActiveStoresAction_WithStores_ShouldCallEndpointAndDispatchActionInCorrectOrder()
        {
            // Arrange
            using var seq = Sequence.Create();

            _fixture.SetupExpectedStores();
            _fixture.SetupFindingStoresForShoppingList();
            _fixture.SetupDispatchingLoadFinishedAction();
            _fixture.SetupDispatchingChangeAction();
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadAllActiveStoresAction(_fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyDispatchingLoadFinishedAction();
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
                ExpectedStoresForShoppingList = new DomainTestBuilderBase<ShoppingListStore>().CreateMany(2).ToList();
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
                DispatcherMock
                    .Setup(m => m.Dispatch(
                        It.Is<SelectedStoreChangedAction>(a => a.IsEquivalentTo(ExpectedStoreChangeAction))))
                    .InSequence();
            }

            public void VerifyDispatchingChangeAction()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedStoreChangeAction);
                DispatcherMock
                    .Verify(m => m.Dispatch(
                        It.Is<SelectedStoreChangedAction>(a => a.IsEquivalentTo(ExpectedStoreChangeAction))),
                        Times.Once);
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
                DispatcherMock
                    .Setup(m => m.Dispatch(
                        It.Is<LoadAllActiveStoresFinishedAction>(a => a.IsEquivalentTo(ExpectedLoadFinishedAction))))
                    .InSequence();
            }

            public void VerifyDispatchingLoadFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedLoadFinishedAction);
                DispatcherMock.Verify(m => m.Dispatch(
                    It.Is<LoadAllActiveStoresFinishedAction>(a => a.IsEquivalentTo(ExpectedLoadFinishedAction))));
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

            using var seq = Sequence.Create();

            _fixture.SetupDispatchingStartAction();
            _fixture.SetupCallingEndpoint();
            _fixture.SetupDispatchingFinishAction();
            _fixture.SetupDispatchingCloseAction();

            _fixture.SetupStateReturningState();
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSavePriceUpdateAction(_fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyDispatchingStartAction();
            _fixture.VerifyCallingEndpoint();
            _fixture.VerifyDispatchingFinishAction();
            _fixture.VerifyDispatchingCloseAction();
        }

        [Fact]
        public async Task HandleSavePriceUpdateAction_WithUpdatingOneType_ShouldCallApiAndDispatchActionsInCorrectOrderAsync()
        {
            // Arrange
            _fixture.SetupPriceUpdateForOneType();
            _fixture.SetupExpectedRequestForOneType();

            using var seq = Sequence.Create();

            _fixture.SetupDispatchingStartAction();
            _fixture.SetupCallingEndpoint();
            _fixture.SetupDispatchingFinishAction();
            _fixture.SetupDispatchingCloseAction();

            _fixture.SetupStateReturningState();
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSavePriceUpdateAction(_fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyDispatchingStartAction();
            _fixture.VerifyCallingEndpoint();
            _fixture.VerifyDispatchingFinishAction();
            _fixture.VerifyDispatchingCloseAction();
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

            public void SetupDispatchingFinishAction()
            {
                ExpectedFinishAction = new SavePriceUpdateFinishedAction(
                    State.PriceUpdate.Item!.Id.ActualId!.Value,
                    State.PriceUpdate.Item.TypeId,
                    State.PriceUpdate.Price);
                DispatcherMock
                    .Setup(m => m.Dispatch(
                        It.Is<SavePriceUpdateFinishedAction>(a => a.IsEquivalentTo(ExpectedFinishAction))))
                    .InSequence();
            }

            public void SetupCallingEndpoint()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedRequest);
                ApiClientMock.SetupUpdateItemPriceAsync(ExpectedRequest);
            }

            public void VerifyDispatchingStartAction()
            {
                VerifyDispatchingAction<SavePriceUpdateStartedAction>();
            }

            public void VerifyDispatchingCloseAction()
            {
                VerifyDispatchingAction<ClosePriceUpdaterAction>();
            }

            public void VerifyDispatchingFinishAction()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedFinishAction);
                DispatcherMock.Setup(m =>
                    m.Dispatch(It.Is<SavePriceUpdateFinishedAction>(a => a.IsEquivalentTo(ExpectedFinishAction))));
            }

            public void VerifyCallingEndpoint()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedRequest);
                ApiClientMock.VerifyUpdateItemPriceAsync(ExpectedRequest, Times.Once);
            }
        }
    }

    private abstract class ShoppingListEffectsFixture
    {
        protected readonly ShoppingListStateMock ShoppingListStateMock = new(MockBehavior.Strict);
        protected readonly ApiClientMock ApiClientMock = new(MockBehavior.Strict);
        protected readonly CommandQueueMock CommandQueueMock = new(MockBehavior.Strict);
        protected ShoppingListState State;

        protected ShoppingListEffectsFixture()
        {
            State = new DomainTestBuilderBase<ShoppingListState>().Create();
        }

        public ShoppingListEffects CreateSut()
        {
            return new ShoppingListEffects(ApiClientMock.Object, CommandQueueMock.Object, ShoppingListStateMock.Object);
        }

        public DispatcherMock DispatcherMock { get; } = new(MockBehavior.Strict);

        protected void SetupDispatchingAction<TAction>() where TAction : new()
        {
            DispatcherMock
                .Setup(m => m.Dispatch(It.Is<TAction>(a => a.IsEquivalentTo(new TAction()))))
                .InSequence();
        }

        protected void VerifyDispatchingAction<TAction>() where TAction : new()
        {
            DispatcherMock.Verify(m => m.Dispatch(It.Is<TAction>(a => a.IsEquivalentTo(new TAction()))), Times.Once);
        }

        public void SetupStateReturningState()
        {
            ShoppingListStateMock.SetupValue(State);
        }
    }
}