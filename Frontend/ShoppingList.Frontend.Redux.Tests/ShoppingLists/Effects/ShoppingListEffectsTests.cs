using Moq;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions;
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
            _fixture.SetupExpectedStoresEmpty();
            _fixture.SetupFindingStoresForShoppingList();
            var sut = _fixture.CreatSut();

            // Act
            await sut.HandleLoadAllActiveStoresAction(_fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyDispatchingLoadFinishedAction();
        }

        [Fact]
        public async Task HandleLoadAllActiveStoresAction_WithoutStores_ShouldNotDispatchChangeAction()
        {
            // Arrange
            _fixture.SetupExpectedStoresEmpty();
            _fixture.SetupFindingStoresForShoppingList();
            var sut = _fixture.CreatSut();

            // Act
            await sut.HandleLoadAllActiveStoresAction(_fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyNotDispatchingChangeAction();
        }

        [Fact]
        public async Task HandleLoadAllActiveStoresAction_WithStores_ShouldDispatchFinishedChangeAction()
        {
            // Arrange
            _fixture.SetupExpectedStores();
            _fixture.SetupFindingStoresForShoppingList();
            var sut = _fixture.CreatSut();

            // Act
            await sut.HandleLoadAllActiveStoresAction(_fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyDispatchingLoadFinishedAction();
        }

        private sealed class HandleLoadAllActiveStoresActionFixture : ShoppingListEffectsFixture
        {
            public IReadOnlyCollection<ShoppingListStore>? ExpectedStoresForShoppingList { get; private set; }

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

            public void VerifyNotDispatchingChangeAction()
            {
                DispatcherMock.Verify(m => m.Dispatch(It.IsAny<SelectedStoreChangedAction>()), Times.Never);
            }

            public void VerifyDispatchingLoadFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedStoresForShoppingList);
                DispatcherMock.Verify(m => m.Dispatch(
                    It.Is<LoadAllActiveStoresFinishedAction>(a => a.Stores.Stores.IsEquivalentTo(ExpectedStoresForShoppingList))));
            }
        }
    }

    private abstract class ShoppingListEffectsFixture
    {
        protected readonly ShoppingListStateMock ShoppingListStateMock = new(MockBehavior.Strict);
        protected readonly ApiClientMock ApiClientMock = new(MockBehavior.Strict);
        protected readonly CommandQueueMock CommandQueueMock = new(MockBehavior.Strict);

        public ShoppingListEffects CreatSut()
        {
            return new ShoppingListEffects(ApiClientMock.Object, CommandQueueMock.Object, ShoppingListStateMock.Object);
        }

        public DispatcherMock DispatcherMock { get; } = new();
    }
}