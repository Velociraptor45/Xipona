using Moq;
using Moq.Contrib.InOrder;
using Xipona.Frontend.Redux.Shared.Configurations;
using Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using Xipona.Frontend.Redux.ShoppingList.Actions.Items;
using Xipona.Frontend.Redux.ShoppingList.Actions.PriceUpdater;
using Xipona.Frontend.Redux.ShoppingList.Effects;
using Xipona.Frontend.Redux.ShoppingList.States;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace Xipona.Frontend.Redux.Tests.ShoppingLists.Effects;

public class ShoppingListItemEffectsTests
{
    public class HandleOpenPriceUpdaterAction
    {
        private readonly HandleOpenPriceUpdaterActionFixture _fixture = new();

        [Fact]
        public async Task HandleOpenPriceUpdaterAction_WithSuccessfulApiCall_ShouldDispatchFinishAction()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingItemTypePrices(x0);
                _fixture.SetupDispatchingFinishAction(x0);
            });
            _fixture.SetupAction();

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleOpenPriceUpdaterAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleOpenPriceUpdaterAction_WithApiException_ShouldCallEndpointAndDispatchActionInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingItemTypePricesThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
            });
            _fixture.SetupAction();

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleOpenPriceUpdaterAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleOpenPriceUpdaterAction_WithHttpRequestException_ShouldCallEndpointAndDispatchActionInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingItemTypePricesThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
            });
            _fixture.SetupAction();

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleOpenPriceUpdaterAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleOpenPriceUpdaterAction_WithItemWithoutItemType_ShouldNotCallEndpointAndNotDispatchAction()
        {
            // Arrange
            _fixture.SetupItemWithoutItemType();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ => { });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleOpenPriceUpdaterAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleOpenPriceUpdaterActionFixture : ShoppingListItemEffectsFixture
        {
            private List<ItemTypePrice>? _expectedPrices;
            public OpenPriceUpdaterAction? Action { get; private set; }

            public void SetupItemWithoutItemType()
            {
                State = State with
                {
                    PriceUpdate = State.PriceUpdate with
                    {
                        Item = State.PriceUpdate.Item! with
                        {
                            TypeId = null
                        }
                    }
                };
            }

            public void SetupGettingItemTypePrices(IQueueComponent component)
            {
                _expectedPrices = new DomainTestBuilder<ItemTypePrice>().CreateMany(2).ToList();
                ApiClientMock.SetupGetItemTypePricesAsync(State.PriceUpdate.Item!.Id.ActualId!.Value,
                    State.SelectedStoreId, _expectedPrices, component);
            }

            public void SetupGettingItemTypePricesThrowsApiException(IQueueComponent component)
            {
                ApiClientMock.SetupGetItemTypePricesAsyncThrowing(State.PriceUpdate.Item!.Id.ActualId!.Value,
                    State.SelectedStoreId, new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupGettingItemTypePricesThrowsHttpRequestException(IQueueComponent component)
            {
                ApiClientMock.SetupGetItemTypePricesAsyncThrowing(State.PriceUpdate.Item!.Id.ActualId!.Value,
                    State.SelectedStoreId, new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupAction()
            {
                Action = new OpenPriceUpdaterAction(State.PriceUpdate.Item!);
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedPrices);
                SetupDispatchingAction(new LoadingPriceUpdaterPricesFinishedAction(_expectedPrices), component);
            }
        }
    }

    public class HandleChangeItemQuantityAction
    {
        private readonly HandleChangeItemQuantityActionFixture _fixture = new();

        [Fact]
        public async Task HandleChangeItemQuantityAction_WithChangeTypeDiff_QuantityAtLeast1_ShouldChangeQuantityAsync()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupSelectedItem();
                _fixture.SetupActionWithChangeTypeDiffAndQuantityAtLeast1();
                _fixture.SetupEnqueuingRequest(x0);
                _fixture.SetupDispatchingFinishAction(x0);
            });

            _fixture.SetupStateReturningState();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleChangeItemQuantityAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyEnqueuingRequest();
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleChangeItemQuantityAction_WithChangeTypeDiff_QuantityBelow1_ShouldChangeQuantityAsync()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupSelectedItem();
                _fixture.SetupActionWithChangeTypeDiffAndQuantityBelow1();
                _fixture.SetupEnqueuingRequest(x0);
                _fixture.SetupDispatchingFinishAction(x0);
            });

            _fixture.SetupStateReturningState();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleChangeItemQuantityAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyEnqueuingRequest();
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleChangeItemQuantityAction_WithChangeTypeAbsolute_QuantityAtLeast1_ShouldChangeQuantityAsync()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupSelectedItem();
                _fixture.SetupActionWithChangeTypeAbsoluteAndQuantityAtLeast1();
                _fixture.SetupEnqueuingRequest(x0);
                _fixture.SetupDispatchingFinishAction(x0);
            });

            _fixture.SetupStateReturningState();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleChangeItemQuantityAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyEnqueuingRequest();
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleChangeItemQuantityAction_WithChangeTypeAbsolute_QuantityBelow1_ShouldChangeQuantityAsync()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupSelectedItem();
                _fixture.SetupActionWithChangeTypeAbsoluteAndQuantityBelow1();
                _fixture.SetupEnqueuingRequest(x0);
                _fixture.SetupDispatchingFinishAction(x0);
            });

            _fixture.SetupStateReturningState();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleChangeItemQuantityAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyEnqueuingRequest();
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleChangeItemQuantityAction_WithInvalidItemId_ShouldChangeQuantityAsync()
        {
            // Arrange
            _fixture.SetupActionWithInvalidItemId();

            _fixture.SetupStateReturningState();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleChangeItemQuantityAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyNotEnqueuingRequest();
            _fixture.VerifyNotDispatchingFinishAction();
        }

        private sealed class HandleChangeItemQuantityActionFixture : ShoppingListItemEffectsFixture
        {
            private float? _expectedQuantity;
            private ShoppingListItem? _item;
            private ChangeItemQuantityOnShoppingListRequest? _expectedRequest;
            public ChangeItemQuantityAction? Action { get; private set; }

            public void SetupSelectedItem()
            {
                _item = State.ShoppingList!.Items.ElementAt(5);
            }

            public void SetupActionWithInvalidItemId()
            {
                Action = new DomainTestBuilder<ChangeItemQuantityAction>().Create();
            }

            public void SetupActionWithChangeTypeDiffAndQuantityAtLeast1()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);

                var quantity = new DomainTestBuilder<float>().Create();
                _expectedQuantity = _item.Quantity + quantity;
                Action = new ChangeItemQuantityAction(_item.Id, _item.TypeId, quantity,
                    ChangeItemQuantityAction.ChangeType.Diff, _item.Name);
            }

            public void SetupActionWithChangeTypeDiffAndQuantityBelow1()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);

                _expectedQuantity = 1;
                Action = new ChangeItemQuantityAction(_item.Id, _item.TypeId, -_item.Quantity + 0.99f,
                    ChangeItemQuantityAction.ChangeType.Diff, _item.Name);
            }

            public void SetupActionWithChangeTypeAbsoluteAndQuantityAtLeast1()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);

                _expectedQuantity = new DomainTestBuilder<float>().Create();
                Action = new ChangeItemQuantityAction(_item.Id, _item.TypeId, _expectedQuantity.Value,
                    ChangeItemQuantityAction.ChangeType.Absolute, _item.Name);
            }

            public void SetupActionWithChangeTypeAbsoluteAndQuantityBelow1()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);

                _expectedQuantity = 1;
                Action = new ChangeItemQuantityAction(_item.Id, _item.TypeId, 0.99f,
                    ChangeItemQuantityAction.ChangeType.Absolute, _item.Name);
            }

            public void SetupEnqueuingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_item);
                TestPropertyNotSetException.ThrowIfNull(_expectedQuantity);

                _expectedRequest = new ChangeItemQuantityOnShoppingListRequest(Guid.NewGuid(), State.ShoppingList!.Id,
                    _item.Id, _item.TypeId, _expectedQuantity.Value, _item.Name);
                CommandQueueMock.SetupEnqueue(_expectedRequest, component);
            }

            public void VerifyEnqueuingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRequest);
                CommandQueueMock.VerifyEnqueue(_expectedRequest, Times.Once);
            }

            public void VerifyNotEnqueuingRequest()
            {
                CommandQueueMock.VerifyNoEnqueue<ChangeItemQuantityOnShoppingListRequest>();
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_item);
                TestPropertyNotSetException.ThrowIfNull(_expectedQuantity);

                SetupDispatchingAction(new ChangeItemQuantityFinishedAction(_item.Id, _item.TypeId, _expectedQuantity.Value), component);
            }

            public void VerifyNotDispatchingFinishAction()
            {
                VerifyNotDispatchingAction<ChangeItemQuantityFinishedAction>();
            }
        }
    }

    private abstract class ShoppingListItemEffectsFixture : ShoppingListEffectsFixtureBase
    {
        public ShoppingListItemEffects CreateSut()
        {
            SetupStateReturningState();
            return new ShoppingListItemEffects(CommandQueueMock.Object, ShoppingListStateMock.Object, ApiClientMock.Object,
                NavigationManagerMock.Object, new ShoppingListConfiguration());
        }
    }
}