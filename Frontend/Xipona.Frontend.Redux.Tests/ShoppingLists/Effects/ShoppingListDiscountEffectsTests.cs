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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupAddingDiscount(x0);
                _fixture.SetupDispatchingFinishAction(x0);
                _fixture.SetupDispatchingCloseDialogAction(x0);
                _fixture.SetupDispatchingReloadAction(x0);
                _fixture.SetupSuccessNotification(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupAddingDiscountThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupAddingDiscountThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
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

            public void SetupAddingDiscount(IQueueComponent component)
            {
                ApiClientMock.SetupAddShoppingListDiscountAsync(_shoppingListId, _discount, _type, component);
            }

            public void SetupAddingDiscountThrowsApiException(IQueueComponent component)
            {
                ApiClientMock.SetupAddShoppingListDiscountAsyncThrowing(_shoppingListId, _discount, _type,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupAddingDiscountThrowsHttpRequestException(IQueueComponent component)
            {
                ApiClientMock.SetupAddShoppingListDiscountAsyncThrowing(_shoppingListId, _discount, _type,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingStartAction(IQueueComponent component)
            {
                DispatcherMock.SetupDispatch(new SaveDiscountStartedAction(), component);
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                DispatcherMock.SetupDispatch(new SaveDiscountFinishedAction(), component);
            }

            public void SetupDispatchingCloseDialogAction(IQueueComponent component)
            {
                DispatcherMock.SetupDispatch(new CloseDiscountDialogAction(), component);
            }

            public void SetupDispatchingReloadAction(IQueueComponent component)
            {
                DispatcherMock.SetupDispatch(new ReloadCurrentShoppingListAction(), component);
            }

            public void SetupSuccessNotification(IQueueComponent component)
            {
                ShoppingListNotificationServiceMock
                    .SetupNotifySuccess("Successfully added discount", 2f, component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupRemovingDiscount(x0);
                _fixture.SetupDispatchingFinishAction(x0);
                _fixture.SetupSuccessNotification(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupRemovingDiscountThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFailedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupRemovingDiscountThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFailedAction(x0);
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
            public readonly RemoveDiscountAction Action;
            private readonly Guid _discountId = Guid.NewGuid();

            public HandleRemoveDiscountActionFixture()
            {
                Action = new RemoveDiscountAction(_discountId);
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

            public void SetupRemovingDiscount(IQueueComponent component)
            {
                ApiClientMock.SetupRemoveShoppingListDiscountAsync(_shoppingListId, _discountId, component);
            }

            public void SetupRemovingDiscountThrowsApiException(IQueueComponent component)
            {
                ApiClientMock.SetupRemoveShoppingListDiscountAsyncThrowing(_shoppingListId, _discountId,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupRemovingDiscountThrowsHttpRequestException(IQueueComponent component)
            {
                ApiClientMock.SetupRemoveShoppingListDiscountAsyncThrowing(_shoppingListId, _discountId,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingStartAction(IQueueComponent component)
            {
                DispatcherMock.SetupDispatch(new RemoveDiscountStartedAction(_discountId), component);
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                DispatcherMock.SetupDispatch(new RemoveDiscountFinishedAction(_discountId), component);
            }

            public void SetupDispatchingFailedAction(IQueueComponent component)
            {
                DispatcherMock.SetupDispatch(new RemoveDiscountFailedAction(_discountId), component);
            }

            public void SetupSuccessNotification(IQueueComponent component)
            {
                ShoppingListNotificationServiceMock.SetupNotifySuccess("Successfully removed discount", 2f, component);
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
