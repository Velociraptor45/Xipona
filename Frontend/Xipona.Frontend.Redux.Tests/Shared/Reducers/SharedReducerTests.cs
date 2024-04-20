using FluentAssertions;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Reducers;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Shared.Reducers;

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

    public class OnApiConnectionDied
    {
        private readonly OnApiConnectionDiedFixture _fixture = new();

        [Fact]
        public void OnApiConnectionDied_WithApplicationOnline_ShouldSetIsOnlineFalse()
        {
            // Arrange
            _fixture.SetupInitialStateWithIsOnline();
            _fixture.SetupExpectedStateWithIsNotOnline();

            // Act
            var result = SharedReducer.OnApiConnectionDied(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnApiConnectionDied_WithApplicationNotOnline_ShouldSetIsOnlineFalse()
        {
            // Arrange
            _fixture.SetupInitialStateWithIsNotOnline();
            _fixture.SetupExpectedStateWithIsNotOnline();

            // Act
            var result = SharedReducer.OnApiConnectionDied(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnApiConnectionDiedFixture : SharedReducerFixture
        {
            public void SetupInitialStateWithIsOnline()
            {
                InitialState = ExpectedState with { IsOnline = true };
            }

            public void SetupInitialStateWithIsNotOnline()
            {
                InitialState = ExpectedState with { IsOnline = false };
            }

            public void SetupExpectedStateWithIsNotOnline()
            {
                ExpectedState = ExpectedState with { IsOnline = false };
            }
        }
    }

    public class OnApiConnectionRecovered
    {
        private readonly OnApiConnectionRecoveredFixture _fixture = new();

        [Fact]
        public void OnApiConnectionRecovered_WithApplicationOnline_ShouldSetIsOnlineTrue()
        {
            // Arrange
            _fixture.SetupInitialStateWithIsOnline();
            _fixture.SetupExpectedStateWithIsOnline();

            // Act
            var result = SharedReducer.OnApiConnectionRecovered(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnApiConnectionRecovered_WithApplicationNotOnline_ShouldSetIsOnlineTrue()
        {
            // Arrange
            _fixture.SetupInitialStateWithIsNotOnline();
            _fixture.SetupExpectedStateWithIsOnline();

            // Act
            var result = SharedReducer.OnApiConnectionRecovered(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnApiConnectionRecoveredFixture : SharedReducerFixture
        {
            public void SetupInitialStateWithIsOnline()
            {
                InitialState = ExpectedState with { IsOnline = true };
            }

            public void SetupInitialStateWithIsNotOnline()
            {
                InitialState = ExpectedState with { IsOnline = false };
            }

            public void SetupExpectedStateWithIsOnline()
            {
                ExpectedState = ExpectedState with { IsOnline = true };
            }
        }
    }

    private abstract class SharedReducerFixture
    {
        public SharedState ExpectedState { get; protected set; } = new DomainTestBuilder<SharedState>().Create();
        public SharedState InitialState { get; protected set; } = new DomainTestBuilder<SharedState>().Create();
    }
}