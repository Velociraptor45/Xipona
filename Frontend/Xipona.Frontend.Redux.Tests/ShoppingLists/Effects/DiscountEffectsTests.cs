using Moq;
using Moq.Contrib.InOrder;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Discounts;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Effects;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Shared.Ports;
using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.ShoppingLists.Effects;
public class DiscountEffectsTests
{
    public class HandleSaveDiscountAction
    {
        private readonly HandleSaveDiscountActionFixture _fixture = new();

        [Fact]
        public async Task HandleSaveDiscountAction_WithoutItem_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupStateWithoutItem();

            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveDiscountAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

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

        private sealed class HandleSaveDiscountActionFixture : DiscountEffectsFixture
        {
            private readonly Guid _shoppingListId = Guid.NewGuid();
            private readonly Guid _itemId = Guid.NewGuid();
            private readonly Guid _itemTypeId = Guid.NewGuid();
            private readonly decimal _discount = new DomainTestBuilder<decimal>().Create();
            private readonly string _itemName = new DomainTestBuilder<string>().Create();

            public void SetupStateWithoutItem()
            {
                State = State with
                {
                    DiscountDialog = State.DiscountDialog with
                    {
                        Item = null
                    }
                };
            }

            public void SetupState()
            {
                State = State with
                {
                    ShoppingList = new DomainTestBuilder<ShoppingListModel>().Create() with
                    {
                        Id = _shoppingListId
                    },
                    DiscountDialog = State.DiscountDialog with
                    {
                        Item = new DomainTestBuilder<ShoppingListItem>().Create() with
                        {
                            Id = ShoppingListItemId.FromActualId(_itemId),
                            TypeId = _itemTypeId,
                            Name = _itemName
                        },
                        Discount = _discount
                    }
                };
            }

            public void SetupAddingDiscount()
            {
                ApiClientMock.SetupAddItemDiscountAsync(_shoppingListId, _itemId, _itemTypeId, _discount);
            }

            public void SetupAddingDiscountThrowsApiException()
            {
                ApiClientMock.SetupAddItemDiscountAsyncThrowing(_shoppingListId, _itemId, _itemTypeId, _discount,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupAddingDiscountThrowsHttpRequestException()
            {
                ApiClientMock.SetupAddItemDiscountAsyncThrowing(_shoppingListId, _itemId, _itemTypeId, _discount,
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
                ShoppingListNotificationServiceMock.SetupNotifySuccess($"Successfully discounted {_itemName}");
            }
        }
    }

    public class HandleRemoveDiscountAction
    {
        private readonly HandleRemoveDiscountActionFixture _fixture = new();

        [Fact]
        public async Task HandleRemoveDiscountAction_WithoutItem_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupStateWithoutItem();

            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleRemoveDiscountAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

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
                _fixture.SetupDispatchingCloseDialogAction();
                _fixture.SetupDispatchingReloadAction();
                _fixture.SetupSuccessNotification();
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleRemoveDiscountAction(_fixture.DispatcherMock.Object);

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
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleRemoveDiscountAction(_fixture.DispatcherMock.Object);

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
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleRemoveDiscountAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleRemoveDiscountActionFixture : DiscountEffectsFixture
        {
            private readonly Guid _shoppingListId = Guid.NewGuid();
            private readonly Guid _itemId = Guid.NewGuid();
            private readonly Guid _itemTypeId = Guid.NewGuid();
            private readonly string _itemName = new DomainTestBuilder<string>().Create();

            public void SetupStateWithoutItem()
            {
                State = State with
                {
                    DiscountDialog = State.DiscountDialog with
                    {
                        Item = null
                    }
                };
            }

            public void SetupState()
            {
                State = State with
                {
                    ShoppingList = new DomainTestBuilder<ShoppingListModel>().Create() with
                    {
                        Id = _shoppingListId
                    },
                    DiscountDialog = State.DiscountDialog with
                    {
                        Item = new DomainTestBuilder<ShoppingListItem>().Create() with
                        {
                            Id = ShoppingListItemId.FromActualId(_itemId),
                            TypeId = _itemTypeId,
                            Name = _itemName
                        }
                    }
                };
            }

            public void SetupRemovingDiscount()
            {
                ApiClientMock.SetupRemoveItemDiscountAsync(_shoppingListId, _itemId, _itemTypeId);
            }

            public void SetupRemovingDiscountThrowsApiException()
            {
                ApiClientMock.SetupRemoveItemDiscountAsyncThrowing(_shoppingListId, _itemId, _itemTypeId,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupRemovingDiscountThrowsHttpRequestException()
            {
                ApiClientMock.SetupRemoveItemDiscountAsyncThrowing(_shoppingListId, _itemId, _itemTypeId,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingStartAction()
            {
                DispatcherMock.SetupDispatch(new RemoveDiscountStartedAction());
            }

            public void SetupDispatchingFinishAction()
            {
                DispatcherMock.SetupDispatch(new RemoveDiscountFinishedAction());
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
                ShoppingListNotificationServiceMock.SetupNotifySuccess($"Successfully removed discount from {_itemName}");
            }
        }
    }

    private abstract class DiscountEffectsFixture : ShoppingListEffectsFixtureBase
    {
        protected ShoppingListNotificationServiceMock ShoppingListNotificationServiceMock { get; } =
            new(MockBehavior.Strict);

        public DiscountEffects CreateSut()
        {
            SetupStateReturningState();
            return new DiscountEffects(ApiClientMock.Object, ShoppingListStateMock.Object,
                ShoppingListNotificationServiceMock.Object);
        }
    }
}
