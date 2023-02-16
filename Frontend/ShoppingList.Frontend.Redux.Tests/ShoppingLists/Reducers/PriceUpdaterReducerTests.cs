using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.PriceUpdater;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.ShoppingLists.Reducers;

public class PriceUpdaterReducerTests
{
    public class OnPriceOnPriceUpdaterChanged
    {
        private OnPriceOnPriceUpdaterChangedFixture _fixture;

        public OnPriceOnPriceUpdaterChanged()
        {
            _fixture = new OnPriceOnPriceUpdaterChangedFixture();
        }

        [Fact]
        public void OnPriceOnPriceUpdaterChanged_ShouldUpdatePrice()
        {
            // Arrange
            _fixture.SetupAction();
            _fixture.SetupInitialState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ChangeAction);
            TestPropertyNotSetException.ThrowIfNull(_fixture.InitialState);

            // Act
            var result = PriceUpdaterReducer.OnPriceOnPriceUpdaterChanged(_fixture.InitialState, _fixture.ChangeAction);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnPriceOnPriceUpdaterChangedFixture : PriceUpdaterReducerFixture
        {
            public PriceOnPriceUpdaterChangedAction? ChangeAction { get; private set; }
            public ShoppingListState? InitialState { get; private set; }

            public void SetupAction()
            {
                ChangeAction = new PriceOnPriceUpdaterChangedAction(ExpectedState.PriceUpdate.Price);
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    PriceUpdate = ExpectedState.PriceUpdate with
                    {
                        Price = new DomainTestBuilder<float>().Create()
                    }
                };
            }
        }
    }

    private abstract class PriceUpdaterReducerFixture
    {
        public ShoppingListState ExpectedState { get; protected set; } =
            new DomainTestBuilder<ShoppingListState>().Create();
    }
}