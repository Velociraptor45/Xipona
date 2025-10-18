using Moq;
using Moq.Contrib.InOrder;
using Xipona.Frontend.Redux.ShoppingList.Actions;
using Xipona.Frontend.Redux.ShoppingList.Actions.ItemDiscounts;
using Xipona.Frontend.Redux.ShoppingList.Effects;
using Xipona.Frontend.Redux.ShoppingList.States;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.Shared.Ports;
using RestEase;

namespace Xipona.Frontend.Redux.Tests.ShoppingLists.Effects;
public class ItemDiscountEffectsTests
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

        private sealed class HandleSaveDiscountActionFixture : ItemDiscountEffectsFixture
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
                    ItemDiscountDialog = State.ItemDiscountDialog with
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
                    ItemDiscountDialog = State.ItemDiscountDialog with
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

            public void SetupAddingDiscount(IQueueComponent component)
            {
                ApiClientMock.SetupAddItemDiscountAsync(_shoppingListId, _itemId, _itemTypeId, _discount, component);
            }

            public void SetupAddingDiscountThrowsApiException(IQueueComponent component)
            {
                ApiClientMock.SetupAddItemDiscountAsyncThrowing(_shoppingListId, _itemId, _itemTypeId, _discount,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupAddingDiscountThrowsHttpRequestException(IQueueComponent component)
            {
                ApiClientMock.SetupAddItemDiscountAsyncThrowing(_shoppingListId, _itemId, _itemTypeId, _discount,
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
                ShoppingListNotificationServiceMock.SetupNotifySuccess($"Successfully discounted {_itemName}", 2f, component);
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

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupRemovingDiscount(x0);
                _fixture.SetupDispatchingFinishAction(x0);
                _fixture.SetupDispatchingCloseDialogAction(x0);
                _fixture.SetupDispatchingReloadAction(x0);
                _fixture.SetupSuccessNotification(x0);
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

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupRemovingDiscountThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
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

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupRemovingDiscountThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleRemoveDiscountAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleRemoveDiscountActionFixture : ItemDiscountEffectsFixture
        {
            private readonly Guid _shoppingListId = Guid.NewGuid();
            private readonly Guid _itemId = Guid.NewGuid();
            private readonly Guid _itemTypeId = Guid.NewGuid();
            private readonly string _itemName = new DomainTestBuilder<string>().Create();

            public void SetupStateWithoutItem()
            {
                State = State with
                {
                    ItemDiscountDialog = State.ItemDiscountDialog with
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
                    ItemDiscountDialog = State.ItemDiscountDialog with
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

            public void SetupRemovingDiscount(IQueueComponent component)
            {
                ApiClientMock.SetupRemoveItemDiscountAsync(_shoppingListId, _itemId, _itemTypeId, component);
            }

            public void SetupRemovingDiscountThrowsApiException(IQueueComponent component)
            {
                ApiClientMock.SetupRemoveItemDiscountAsyncThrowing(_shoppingListId, _itemId, _itemTypeId,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupRemovingDiscountThrowsHttpRequestException(IQueueComponent component)
            {
                ApiClientMock.SetupRemoveItemDiscountAsyncThrowing(_shoppingListId, _itemId, _itemTypeId,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingStartAction(IQueueComponent component)
            {
                DispatcherMock.SetupDispatch(new RemoveDiscountStartedAction(), component);
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                DispatcherMock.SetupDispatch(new RemoveDiscountFinishedAction(), component);
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
                    .SetupNotifySuccess($"Successfully removed discount from {_itemName}", 2f, component);
            }
        }
    }

    private abstract class ItemDiscountEffectsFixture : ShoppingListEffectsFixtureBase
    {
        protected ShoppingListNotificationServiceMock ShoppingListNotificationServiceMock { get; } =
            new(MockBehavior.Strict);

        public ItemDiscountEffects CreateSut()
        {
            SetupStateReturningState();
            return new ItemDiscountEffects(ApiClientMock.Object, ShoppingListStateMock.Object,
                ShoppingListNotificationServiceMock.Object);
        }
    }
}
