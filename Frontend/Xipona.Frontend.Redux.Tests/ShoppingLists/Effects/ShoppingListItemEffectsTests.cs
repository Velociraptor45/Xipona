using Moq;
using Moq.Contrib.InOrder;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Configurations;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Items;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.PriceUpdater;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Effects;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.ShoppingLists.Effects;

public class ShoppingListItemEffectsTests
{
    public class HandleOpenPriceUpdaterAction
    {
        private readonly HandleOpenPriceUpdaterActionFixture _fixture = new();

        [Fact]
        public async Task HandleOpenPriceUpdaterAction_WithSuccessfulApiCall_ShouldDispatchFinishAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingItemTypePrices();
                _fixture.SetupDispatchingFinishAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingItemTypePricesThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingItemTypePricesThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorNotificationAction();
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

            public void SetupGettingItemTypePrices()
            {
                _expectedPrices = new DomainTestBuilder<ItemTypePrice>().CreateMany(2).ToList();
                ApiClientMock.SetupGetItemTypePricesAsync(State.PriceUpdate.Item!.Id.ActualId!.Value,
                    State.SelectedStoreId, _expectedPrices);
            }

            public void SetupGettingItemTypePricesThrowsApiException()
            {
                ApiClientMock.SetupGetItemTypePricesAsyncThrowing(State.PriceUpdate.Item!.Id.ActualId!.Value,
                    State.SelectedStoreId, new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingItemTypePricesThrowsHttpRequestException()
            {
                ApiClientMock.SetupGetItemTypePricesAsyncThrowing(State.PriceUpdate.Item!.Id.ActualId!.Value,
                    State.SelectedStoreId, new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupAction()
            {
                Action = new OpenPriceUpdaterAction(State.PriceUpdate.Item!);
            }

            public void SetupDispatchingFinishAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedPrices);
                SetupDispatchingAction(new LoadingPriceUpdaterPricesFinishedAction(_expectedPrices));
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSelectedItem();
                _fixture.SetupActionWithChangeTypeDiffAndQuantityAtLeast1();
                _fixture.SetupEnqueuingRequest();
                _fixture.SetupDispatchingFinishAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSelectedItem();
                _fixture.SetupActionWithChangeTypeDiffAndQuantityBelow1();
                _fixture.SetupEnqueuingRequest();
                _fixture.SetupDispatchingFinishAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSelectedItem();
                _fixture.SetupActionWithChangeTypeAbsoluteAndQuantityAtLeast1();
                _fixture.SetupEnqueuingRequest();
                _fixture.SetupDispatchingFinishAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSelectedItem();
                _fixture.SetupActionWithChangeTypeAbsoluteAndQuantityBelow1();
                _fixture.SetupEnqueuingRequest();
                _fixture.SetupDispatchingFinishAction();
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
            public float? ExpectedQuantity { get; private set; }
            public ChangeItemQuantityAction? Action { get; private set; }
            public ShoppingListItem? Item { get; private set; }
            public ChangeItemQuantityOnShoppingListRequest? ExpectedRequest { get; private set; }

            public void SetupSelectedItem()
            {
                Item = State.ShoppingList!.Items.ElementAt(5);
            }

            public void SetupActionWithInvalidItemId()
            {
                Action = new DomainTestBuilder<ChangeItemQuantityAction>().Create();
            }

            public void SetupActionWithChangeTypeDiffAndQuantityAtLeast1()
            {
                TestPropertyNotSetException.ThrowIfNull(Item);

                var quantity = new DomainTestBuilder<float>().Create();
                ExpectedQuantity = Item.Quantity + quantity;
                Action = new ChangeItemQuantityAction(Item.Id, Item.TypeId, quantity,
                    ChangeItemQuantityAction.ChangeType.Diff, Item.Name);
            }

            public void SetupActionWithChangeTypeDiffAndQuantityBelow1()
            {
                TestPropertyNotSetException.ThrowIfNull(Item);

                ExpectedQuantity = 1;
                Action = new ChangeItemQuantityAction(Item.Id, Item.TypeId, -Item.Quantity + 0.99f,
                    ChangeItemQuantityAction.ChangeType.Diff, Item.Name);
            }

            public void SetupActionWithChangeTypeAbsoluteAndQuantityAtLeast1()
            {
                TestPropertyNotSetException.ThrowIfNull(Item);

                ExpectedQuantity = new DomainTestBuilder<float>().Create();
                Action = new ChangeItemQuantityAction(Item.Id, Item.TypeId, ExpectedQuantity.Value,
                    ChangeItemQuantityAction.ChangeType.Absolute, Item.Name);
            }

            public void SetupActionWithChangeTypeAbsoluteAndQuantityBelow1()
            {
                TestPropertyNotSetException.ThrowIfNull(Item);

                ExpectedQuantity = 1;
                Action = new ChangeItemQuantityAction(Item.Id, Item.TypeId, 0.99f,
                    ChangeItemQuantityAction.ChangeType.Absolute, Item.Name);
            }

            public void SetupEnqueuingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(Item);
                TestPropertyNotSetException.ThrowIfNull(ExpectedQuantity);

                ExpectedRequest = new ChangeItemQuantityOnShoppingListRequest(Guid.NewGuid(), State.ShoppingList!.Id,
                    Item.Id, Item.TypeId, ExpectedQuantity.Value, Item.Name);
                CommandQueueMock.SetupEnqueue(ExpectedRequest);
            }

            public void VerifyEnqueuingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedRequest);
                CommandQueueMock.VerifyEnqueue(ExpectedRequest, Times.Once);
            }

            public void VerifyNotEnqueuingRequest()
            {
                CommandQueueMock.VerifyNoEnqueue<ChangeItemQuantityOnShoppingListRequest>();
            }

            public void SetupDispatchingFinishAction()
            {
                TestPropertyNotSetException.ThrowIfNull(Item);
                TestPropertyNotSetException.ThrowIfNull(ExpectedQuantity);

                SetupDispatchingAction(new ChangeItemQuantityFinishedAction(Item.Id, Item.TypeId, ExpectedQuantity.Value));
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