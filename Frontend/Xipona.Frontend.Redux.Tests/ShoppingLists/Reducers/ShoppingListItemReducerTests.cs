using FluentAssertions;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.PriceUpdater;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Reducers;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.ShoppingLists.Reducers;

public class ShoppingListItemReducerTests
{
    public class OnLoadingPriceUpdaterPricesFinished
    {
        private readonly OnLoadingPriceUpdaterPricesFinishedFixture _fixture = new();

        [Fact]
        public void OnLoadingPriceUpdaterPricesFinished_WithPriceThatMatchesSelectedItem_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupActionWithPriceThatMatchesSelectedItem();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnLoadingPriceUpdaterPricesFinished(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnLoadingPriceUpdaterPricesFinished_WithoutPriceThatMatchesSelectedItem_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupActionWithoutPriceThatMatchesSelectedItem();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnLoadingPriceUpdaterPricesFinished(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadingPriceUpdaterPricesFinishedFixture : ShoppingListItemReducerFixture
        {
            public LoadingPriceUpdaterPricesFinishedAction? Action { get; set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    PriceUpdate = ExpectedState.PriceUpdate with
                    {
                        OtherItemTypePrices = InitialState.PriceUpdate.OtherItemTypePrices
                    }
                };
            }

            public void SetupActionWithPriceThatMatchesSelectedItem()
            {
                var prices = ExpectedState.PriceUpdate.OtherItemTypePrices.ToList();
                var additionalPrice = new DomainTestBuilder<ItemTypePrice>().Create() with
                {
                    ItemTypeId = ExpectedState.PriceUpdate.Item!.TypeId!.Value
                };
                prices.Add(additionalPrice);
                Action = new LoadingPriceUpdaterPricesFinishedAction(prices);
            }

            public void SetupActionWithoutPriceThatMatchesSelectedItem()
            {
                Action = new LoadingPriceUpdaterPricesFinishedAction(ExpectedState.PriceUpdate.OtherItemTypePrices);
            }
        }
    }

    private abstract class ShoppingListItemReducerFixture
    {
        public ShoppingListState ExpectedState { get; protected set; } = new DomainTestBuilder<ShoppingListState>().Create();
        public ShoppingListState InitialState { get; protected set; } = new DomainTestBuilder<ShoppingListState>().Create();
    }
}