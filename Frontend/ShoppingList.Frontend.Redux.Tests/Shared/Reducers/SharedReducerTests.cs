using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Shared.Reducers;

public class SharedReducerTests
{
    public class OnApplicationInitialized
    {
        private readonly OnApplicationInitializedFixture _fixture = new();

        [Fact]
        public void OnApplicationInitialized_WithIsNotMobile_ShouldSetIsMobile()
        {
            // Arrange
            _fixture.SetupInitialStateWithNotMobile();
            _fixture.SetupExpectedStateWithMobile();
            _fixture.SetupActionWithMobile();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = SharedReducer.OnApplicationInitialized(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnApplicationInitialized_WithIsMobile_ShouldSetIsNotMobile()
        {
            // Arrange
            _fixture.SetupInitialStateWithMobile();
            _fixture.SetupExpectedStateWithNotMobile();
            _fixture.SetupActionWithNotMobile();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = SharedReducer.OnApplicationInitialized(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnApplicationInitializedFixture : SharedReducerFixture
        {
            public ApplicationInitializedAction? Action { get; private set; }

            public void SetupInitialStateWithNotMobile()
            {
                SetupInitialState(false);
            }

            public void SetupInitialStateWithMobile()
            {
                SetupInitialState(true);
            }

            private void SetupInitialState(bool isMobile)
            {
                InitialState = ExpectedState with { IsMobile = isMobile };
            }

            public void SetupExpectedStateWithNotMobile()
            {
                SetupExpectedState(false);
            }

            public void SetupExpectedStateWithMobile()
            {
                SetupExpectedState(true);
            }

            private void SetupExpectedState(bool isMobile)
            {
                ExpectedState = ExpectedState with { IsMobile = isMobile };
            }

            public void SetupActionWithNotMobile()
            {
                SetupAction(false);
            }

            public void SetupActionWithMobile()
            {
                SetupAction(true);
            }

            private void SetupAction(bool isMobile)
            {
                Action = new ApplicationInitializedAction(isMobile);
            }
        }
    }

    private abstract class SharedReducerFixture
    {
        public SharedState ExpectedState { get; protected set; } = new DomainTestBuilder<SharedState>().Create();
        public SharedState InitialState { get; protected set; } = new DomainTestBuilder<SharedState>().Create();
    }
}