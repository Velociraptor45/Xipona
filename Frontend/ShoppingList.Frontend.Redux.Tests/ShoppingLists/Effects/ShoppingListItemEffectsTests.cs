using Moq;
using Moq.Contrib.InOrder;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Configurations;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.Items;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Effects;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.ShoppingLists.Effects;

public class ShoppingListItemEffectsTests
{
    public class HandleChangeItemQuantityAction
    {
        private readonly HandleChangeItemQuantityActionFixture _fixture;

        public HandleChangeItemQuantityAction()
        {
            _fixture = new HandleChangeItemQuantityActionFixture();
        }

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
            return new ShoppingListItemEffects(CommandQueueMock.Object, ShoppingListStateMock.Object,
                NavigationManagerMock.Object, new ShoppingListConfiguration());
        }
    }
}