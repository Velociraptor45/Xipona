using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Configurations;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.SearchBar;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Effects;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.ShoppingLists.Effects;

public class ShoppingListSearchBarEffectsTests
{
    public class HandleItemForShoppingListSearchInputChangedAction
    {
        private HandleItemForShoppingListSearchInputChangedActionFixture _fixture;

        public HandleItemForShoppingListSearchInputChangedAction()
        {
            _fixture = new HandleItemForShoppingListSearchInputChangedActionFixture();
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        public async Task HandleItemForShoppingListSearchInputChangedAction_WithEmptyInput_ShouldNotDispatchAction(
            string input)
        {
            // Arrange
            _fixture.SetupAction(input);
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleItemForShoppingListSearchInputChangedAction(_fixture.Action, _fixture.DispatcherMock.Object);
            await Task.Delay(10);

            // Assert
            _fixture.VerifyNotDispatchingSearchAction();
        }

        [Fact]
        public async Task HandleItemForShoppingListSearchInputChangedAction_WithValidInput_ShouldDispatchAction()
        {
            // Arrange
            _fixture.SetupAction();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleItemForShoppingListSearchInputChangedAction(_fixture.Action, _fixture.DispatcherMock.Object);
            await Task.Delay(10);

            // Assert
            _fixture.VerifyDispatchingSearchAction();
        }

        [Fact]
        public async Task HandleItemForShoppingListSearchInputChangedAction_WithRepeatedInvokation_ShouldDispatchAction()
        {
            // Arrange
            _fixture.SetupAction();
            _fixture.SetupLongSearchDelay();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleItemForShoppingListSearchInputChangedAction(_fixture.Action, _fixture.DispatcherMock.Object);
            await sut.HandleItemForShoppingListSearchInputChangedAction(_fixture.Action, _fixture.DispatcherMock.Object);
            await Task.Delay(250);

            // Assert
            _fixture.VerifyDispatchingSearchAction();
        }

        private sealed class HandleItemForShoppingListSearchInputChangedActionFixture :
            ShoppingListSearchBarEffectsFixture
        {
            public ItemForShoppingListSearchInputChangedAction? Action { get; private set; }

            public void SetupAction()
            {
                SetupAction(new DomainTestBuilder<string>().Create());
            }

            public void SetupAction(string input)
            {
                Action = new ItemForShoppingListSearchInputChangedAction(input);
            }

            public void SetupLongSearchDelay()
            {
                SearchDelayInMilliseconds = 100;
            }

            public void VerifyDispatchingSearchAction()
            {
                VerifyDispatchingAction<SearchItemForShoppingListAction>();
            }

            public void VerifyNotDispatchingSearchAction()
            {
                VerifyNotDispatchingAction<SearchItemForShoppingListAction>();
            }
        }
    }

    private abstract class ShoppingListSearchBarEffectsFixture : ShoppingListEffectsFixtureBase
    {
        protected int SearchDelayInMilliseconds = 1;

        public ShoppingListSearchBarEffects CreateSut()
        {
            var config = new ShoppingListConfiguration()
            {
                SearchDelayAfterInput = TimeSpan.FromMilliseconds(SearchDelayInMilliseconds),
            };

            return new ShoppingListSearchBarEffects(ApiClientMock.Object, ShoppingListStateMock.Object, config);
        }
    }
}