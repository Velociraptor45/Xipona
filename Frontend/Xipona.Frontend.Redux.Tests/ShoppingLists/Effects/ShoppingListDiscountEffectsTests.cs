using Moq;
using Moq.Contrib.InOrder;
using Xipona.Frontend.Redux.ShoppingList.Actions;
using Xipona.Frontend.Redux.ShoppingList.Actions.ShoppingListDiscounts;
using Xipona.Frontend.Redux.ShoppingList.Effects;
using Xipona.Frontend.Redux.ShoppingList.States;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.Shared.Ports;
using RestEase;

namespace Xipona.Frontend.Redux.Tests.ShoppingLists.Effects;

public class ShoppingListDiscountEffectsTests
{
    public class HandleSaveDiscountAction
    {
        private readonly HandleSaveDiscountActionFixture _fixture = new();

        [Fact]
        public async Task HandleSaveDiscountAction_WithApiCallSuccessful_ShouldAddDiscount()
        {
            // Arrange
            _fixture.SetupState();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupAddingDiscount();
                _fixture.SetupDispatchingFinishAction();
                _fixture.SetupDispatchingCloseDialogAction();
                _fixture.SetupDispatchingReloadAction();
                _fixture.SetupSuccessNotification();
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveDiscountAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSaveDiscountAction_WithApiException_ShouldDispatchNotificationAction()
        {
            // Arrange
            _fixture.SetupState();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupAddingDiscountThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveDiscountAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSaveDiscountAction_WithHttpRequestException_ShouldDispatchNotificationAction()
        {
            // Arrange
            _fixture.SetupState();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupAddingDiscountThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorNotificationAction();
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveDiscountAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSaveDiscountActionFixture : ShoppingListDiscountEffectsFixture
        {
            private readonly Guid _shoppingListId = Guid.NewGuid();
            private readonly decimal _discount = new DomainTestBuilder<decimal>().Create();
            private readonly ShoppingListDiscountType _type = new DomainTestBuilder<ShoppingListDiscountType>().Create();

            public void SetupState()
            {
                State = State with
                {
                    ShoppingList = new DomainTestBuilder<ShoppingListModel>().Create() with
                    {
                        Id = _shoppingListId
                    },
                    ShoppingListDiscountDialog = new ShoppingListDiscountDialog(_discount, _type, true, false)
                };
            }

            public void SetupAddingDiscount()
            {
                ApiClientMock.SetupAddShoppingListDiscountAsync(_shoppingListId, _discount, _type);
            }

            public void SetupAddingDiscountThrowsApiException()
            {
                ApiClientMock.SetupAddShoppingListDiscountAsyncThrowing(_shoppingListId, _discount, _type,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupAddingDiscountThrowsHttpRequestException()
            {
                ApiClientMock.SetupAddShoppingListDiscountAsyncThrowing(_shoppingListId, _discount, _type,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingStartAction()
            {
                DispatcherMock.SetupDispatch(new SaveDiscountStartedAction());
            }

            public void SetupDispatchingFinishAction()
            {
                DispatcherMock.SetupDispatch(new SaveDiscountFinishedAction());
            }

            public void SetupDispatchingCloseDialogAction()
            {
                DispatcherMock.SetupDispatch(new CloseDiscountDialogAction());
            }

            public void SetupDispatchingReloadAction()
            {
                DispatcherMock.SetupDispatch(new ReloadCurrentShoppingListAction());
            }

            public void SetupSuccessNotification()
            {
                ShoppingListNotificationServiceMock.SetupNotifySuccess("Successfully added discount");
            }
        }
    }

    public class HandleRemoveDiscountAction
    {
        private readonly HandleRemoveDiscountActionFixture _fixture = new();

        [Fact]
        public async Task HandleRemoveDiscountAction_WithApiCallSuccessful_ShouldRemoveDiscount()
        {
            // Arrange
            _fixture.SetupState();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupRemovingDiscount();
                _fixture.SetupDispatchingFinishAction();
                _fixture.SetupSuccessNotification();
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleRemoveDiscountAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleRemoveDiscountAction_WithApiException_ShouldDispatchNotificationAction()
        {
            // Arrange
            _fixture.SetupState();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupRemovingDiscountThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFailedAction();
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleRemoveDiscountAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleRemoveDiscountAction_WithHttpRequestException_ShouldDispatchNotificationAction()
        {
            // Arrange
            _fixture.SetupState();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupRemovingDiscountThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorNotificationAction();
                _fixture.SetupDispatchingFailedAction();
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleRemoveDiscountAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleRemoveDiscountActionFixture : ShoppingListDiscountEffectsFixture
        {
            private readonly Guid _shoppingListId = Guid.NewGuid();
            public readonly Guid DiscountId = Guid.NewGuid();
            public readonly RemoveDiscountAction Action;

            public HandleRemoveDiscountActionFixture()
            {
                Action = new RemoveDiscountAction(DiscountId);
            }

            public void SetupState()
            {
                State = State with
                {
                    ShoppingList = new DomainTestBuilder<ShoppingListModel>().Create() with
                    {
                        Id = _shoppingListId
                    }
                };
            }

            public void SetupRemovingDiscount()
            {
                ApiClientMock.SetupRemoveShoppingListDiscountAsync(_shoppingListId, DiscountId);
            }

            public void SetupRemovingDiscountThrowsApiException()
            {
                ApiClientMock.SetupRemoveShoppingListDiscountAsyncThrowing(_shoppingListId, DiscountId,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupRemovingDiscountThrowsHttpRequestException()
            {
                ApiClientMock.SetupRemoveShoppingListDiscountAsyncThrowing(_shoppingListId, DiscountId,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingStartAction()
            {
                DispatcherMock.SetupDispatch(new RemoveDiscountStartedAction(DiscountId));
            }

            public void SetupDispatchingFinishAction()
            {
                DispatcherMock.SetupDispatch(new RemoveDiscountFinishedAction(DiscountId));
            }

            public void SetupDispatchingFailedAction()
            {
                DispatcherMock.SetupDispatch(new RemoveDiscountFailedAction(DiscountId));
            }

            public void SetupSuccessNotification()
            {
                ShoppingListNotificationServiceMock.SetupNotifySuccess("Successfully removed discount");
            }
        }
    }

    private abstract class ShoppingListDiscountEffectsFixture : ShoppingListEffectsFixtureBase
    {
        protected ShoppingListNotificationServiceMock ShoppingListNotificationServiceMock { get; } =
            new(MockBehavior.Strict);

        public ShoppingListDiscountEffects CreateSut()
        {
            SetupStateReturningState();
            return new ShoppingListDiscountEffects(ApiClientMock.Object, ShoppingListStateMock.Object,
                ShoppingListNotificationServiceMock.Object);
        }
    }
}
