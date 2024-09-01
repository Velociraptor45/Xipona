using FluentAssertions;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.PriceUpdater;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Reducers;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.ShoppingLists.Reducers;

public class PriceUpdaterReducerTests
{
    public class OnPriceOnPriceUpdaterChanged
    {
        private readonly OnPriceOnPriceUpdaterChangedFixture _fixture = new();

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
                        Price = new DomainTestBuilder<decimal>().Create()
                    }
                };
            }
        }
    }

    public class OnUpdatePriceForAllTypesOnPriceUpdaterChangedChanged
    {
        private readonly OnUpdatePriceForAllTypesOnPriceUpdaterChangedChangedFixture _fixture = new();

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
        private readonly OnOpenPriceUpdaterChangedFixture _fixture = new();

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
                        Price = new DomainTestBuilder<decimal>().Create(),
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
        private readonly OnClosePriceUpdaterChangedFixture _fixture = new();

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
                        IsSaving = false,
                        OtherItemTypePrices = []
                    }
                };
            }
        }
    }

    public class OnSavePriceUpdateFinished
    {
        private readonly OnSavePriceUpdateFinishedFixture _fixture = new();

        [Fact]
        public void OnSavePriceUpdateFinished_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.InitialState);

            // Act
            var result = PriceUpdaterReducer.OnSavePriceUpdateFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSavePriceUpdateFinishedFixture : PriceUpdaterReducerFixture
        {
            public ShoppingListState? InitialState { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    PriceUpdate = ExpectedState.PriceUpdate with
                    {
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