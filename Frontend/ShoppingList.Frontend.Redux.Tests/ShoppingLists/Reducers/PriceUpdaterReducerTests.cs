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
        private readonly OnPriceOnPriceUpdaterChangedFixture _fixture;

        public OnPriceOnPriceUpdaterChanged()
        {
            _fixture = new OnPriceOnPriceUpdaterChangedFixture();
        }

        [Fact]
        public void OnPriceOnPriceUpdaterChanged_ShouldUpdateState()
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

    public class OnUpdatePriceForAllTypesOnPriceUpdaterChangedChanged
    {
        private readonly OnUpdatePriceForAllTypesOnPriceUpdaterChangedChangedFixture _fixture;

        public OnUpdatePriceForAllTypesOnPriceUpdaterChangedChanged()
        {
            _fixture = new OnUpdatePriceForAllTypesOnPriceUpdaterChangedChangedFixture();
        }

        [Fact]
        public void OnUpdatePriceForAllTypesOnPriceUpdaterChangedChanged_ShouldUpdateState()
        {
            // Arrange
            _fixture.SetupAction();
            _fixture.SetupInitialState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ChangeAction);
            TestPropertyNotSetException.ThrowIfNull(_fixture.InitialState);

            // Act
            var result = PriceUpdaterReducer.OnUpdatePriceForAllTypesOnPriceUpdaterChanged(_fixture.InitialState, _fixture.ChangeAction);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnUpdatePriceForAllTypesOnPriceUpdaterChangedChangedFixture : PriceUpdaterReducerFixture
        {
            public UpdatePriceForAllTypesOnPriceUpdaterChangedAction? ChangeAction { get; private set; }
            public ShoppingListState? InitialState { get; private set; }

            public void SetupAction()
            {
                ChangeAction = new UpdatePriceForAllTypesOnPriceUpdaterChangedAction(ExpectedState.PriceUpdate.UpdatePriceForAllTypes);
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    PriceUpdate = ExpectedState.PriceUpdate with
                    {
                        UpdatePriceForAllTypes = !ExpectedState.PriceUpdate.UpdatePriceForAllTypes
                    }
                };
            }
        }
    }

    public class OnOpenPriceUpdaterChanged
    {
        private readonly OnOpenPriceUpdaterChangedFixture _fixture;

        public OnOpenPriceUpdaterChanged()
        {
            _fixture = new OnOpenPriceUpdaterChangedFixture();
        }

        [Fact]
        public void OnOpenPriceUpdaterChanged_ShouldUpdateState()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupAction();
            _fixture.SetupInitialState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ChangeAction);
            TestPropertyNotSetException.ThrowIfNull(_fixture.InitialState);

            // Act
            var result = PriceUpdaterReducer.OnOpenPriceUpdater(_fixture.InitialState, _fixture.ChangeAction);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnOpenPriceUpdaterChangedFixture : PriceUpdaterReducerFixture
        {
            public OpenPriceUpdaterAction? ChangeAction { get; private set; }
            public ShoppingListState? InitialState { get; private set; }

            public void SetupAction()
            {
                ChangeAction = new OpenPriceUpdaterAction(ExpectedState.PriceUpdate.Item!);
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    PriceUpdate = ExpectedState.PriceUpdate with
                    {
                        Item = null,
                        Price = new DomainTestBuilder<float>().Create(),
                        UpdatePriceForAllTypes = false,
                        IsOpen = false,
                        IsSaving = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    PriceUpdate = ExpectedState.PriceUpdate with
                    {
                        Price = ExpectedState.PriceUpdate.Item!.PricePerQuantity,
                        UpdatePriceForAllTypes = true,
                        IsOpen = true,
                        IsSaving = false
                    }
                };
            }
        }
    }

    public class OnClosePriceUpdaterChanged
    {
        private readonly OnClosePriceUpdaterChangedFixture _fixture;

        public OnClosePriceUpdaterChanged()
        {
            _fixture = new OnClosePriceUpdaterChangedFixture();
        }

        [Fact]
        public void OnClosePriceUpdaterChanged_ShouldUpdateState()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupAction();
            _fixture.SetupInitialState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ChangeAction);
            TestPropertyNotSetException.ThrowIfNull(_fixture.InitialState);

            // Act
            var result = PriceUpdaterReducer.OnClosePriceUpdater(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnClosePriceUpdaterChangedFixture : PriceUpdaterReducerFixture
        {
            public ClosePriceUpdaterAction? ChangeAction { get; private set; }
            public ShoppingListState? InitialState { get; private set; }

            public void SetupAction()
            {
                ChangeAction = new ClosePriceUpdaterAction();
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    PriceUpdate = ExpectedState.PriceUpdate with
                    {
                        Item = new DomainTestBuilder<ShoppingListItem>().Create(),
                        IsOpen = true,
                        IsSaving = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    PriceUpdate = ExpectedState.PriceUpdate with
                    {
                        Item = null,
                        IsOpen = false,
                        IsSaving = false
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