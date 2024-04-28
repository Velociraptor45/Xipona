using FluentAssertions;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.InitialStoreCreator;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Reducers;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.ShoppingLists.Reducers;

public class InitialStoreCreatorReducerTests
{
    public class OnNoStoresFound
    {
        private readonly OnNoStoresFoundFixture _fixture = new();

        [Fact]
        public void OnNoStoresFound_WithValidData_ShouldOpenInitialStoreDialog()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = InitialStoreCreatorReducer.OnNoStoresFound(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnNoStoresFoundFixture : InitialStoreCreatorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    InitialStoreCreator = ExpectedState.InitialStoreCreator with
                    {
                        IsOpen = false
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    InitialStoreCreator = ExpectedState.InitialStoreCreator with
                    {
                        IsOpen = true
                    }
                };
            }
        }
    }

    public class OnInitialStoreNameChanged
    {
        private readonly OnInitialStoreNameChangedFixture _fixture = new();

        [Fact]
        public void OnInitialStoreNameChanged_WithValidData_ShouldChangeName()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = InitialStoreCreatorReducer.OnInitialStoreNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnInitialStoreNameChangedFixture : InitialStoreCreatorReducerFixture
        {
            public InitialStoreNameChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    InitialStoreCreator = ExpectedState.InitialStoreCreator with
                    {
                        Name = new DomainTestBuilder<string>().Create()
                    }
                };
            }

            public void SetupAction()
            {
                Action = new InitialStoreNameChangedAction(ExpectedState.InitialStoreCreator.Name);
            }
        }
    }

    private abstract class InitialStoreCreatorReducerFixture
    {
        public ShoppingListState ExpectedState { get; protected set; } = new DomainTestBuilder<ShoppingListState>().Create();
        public ShoppingListState InitialState { get; protected set; } = new DomainTestBuilder<ShoppingListState>().Create();
    }
}