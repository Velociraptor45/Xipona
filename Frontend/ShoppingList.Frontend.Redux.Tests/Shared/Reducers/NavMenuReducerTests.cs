using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Shared.Reducers;

public class NavMenuReducerTests
{
    public class OnToggleMobileNavMenuExpansion
    {
        private readonly OnToggleMobileNavMenuExpansionFixture _fixture;

        public OnToggleMobileNavMenuExpansion()
        {
            _fixture = new OnToggleMobileNavMenuExpansionFixture();
        }

        [Fact]
        public void OnToggleMobileNavMenuExpansion_WithNotExpanded_ShouldSetExpanded()
        {
            // Arrange
            _fixture.SetupInitialStateNotExpanded();
            _fixture.SetupExpectedStateExpanded();

            // Act
            var result = NavMenuReducer.OnToggleMobileNavMenuExpansion(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnToggleMobileNavMenuExpansion_WithExpanded_ShouldSetNotExpanded()
        {
            // Arrange
            _fixture.SetupInitialStateExpanded();
            _fixture.SetupExpectedStateNotExpanded();

            // Act
            var result = NavMenuReducer.OnToggleMobileNavMenuExpansion(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnToggleMobileNavMenuExpansionFixture : NavMenuReducerFixture
        {
            public void SetupInitialStateNotExpanded()
            {
                SetupInitialState(false);
            }

            public void SetupInitialStateExpanded()
            {
                SetupInitialState(true);
            }

            private void SetupInitialState(bool isExpanded)
            {
                InitialState = ExpectedState with
                {
                    IsMobileNavMenuExpanded = isExpanded
                };
            }

            public void SetupExpectedStateNotExpanded()
            {
                SetupExpectedState(false);
            }

            public void SetupExpectedStateExpanded()
            {
                SetupExpectedState(true);
            }

            private void SetupExpectedState(bool isExpanded)
            {
                ExpectedState = ExpectedState with
                {
                    IsMobileNavMenuExpanded = isExpanded
                };
            }
        }
    }

    public class OnNavMenuItemClicked
    {
        private readonly OnNavMenuItemClickedFixture _fixture;

        public OnNavMenuItemClicked()
        {
            _fixture = new OnNavMenuItemClickedFixture();
        }

        [Fact]
        public void OnNavMenuItemClicked_WithNotExpanded_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateNotExpanded();
            _fixture.SetupExpectedState();

            // Act
            var result = NavMenuReducer.OnNavMenuItemClicked(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnNavMenuItemClicked_WithExpanded_ShouldSetNotExpanded()
        {
            // Arrange
            _fixture.SetupInitialStateExpanded();
            _fixture.SetupExpectedState();

            // Act
            var result = NavMenuReducer.OnNavMenuItemClicked(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnNavMenuItemClickedFixture : NavMenuReducerFixture
        {
            public void SetupInitialStateNotExpanded()
            {
                SetupInitialState(false);
            }

            public void SetupInitialStateExpanded()
            {
                SetupInitialState(true);
            }

            private void SetupInitialState(bool isExpanded)
            {
                InitialState = ExpectedState with
                {
                    IsMobileNavMenuExpanded = isExpanded
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    IsMobileNavMenuExpanded = false
                };
            }
        }
    }

    private abstract class NavMenuReducerFixture
    {
        public SharedState ExpectedState { get; protected set; } = new DomainTestBuilder<SharedState>().Create();
        public SharedState InitialState { get; protected set; } = new DomainTestBuilder<SharedState>().Create();
    }
}