using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Stores.Reducers;

public class StoreEditorReducerTests
{
    public class OnSetNewStore
    {
        private readonly OnSetNewStoreFixture _fixture;

        public OnSetNewStore()
        {
            _fixture = new OnSetNewStoreFixture();
        }

        [Fact]
        public void OnSetNewStore_WithValidData_ShouldSetStore()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupExpectedState();

            // Act
            var result = StoreEditorReducer.OnSetNewStore(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSetNewStoreFixture : StoreEditorReducerFixture
        {
            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Store = new EditedStore(
                            Guid.Empty,
                            string.Empty,
                            new SortedSet<EditedSection>())
                    }
                };
            }
        }
    }

    private abstract class StoreEditorReducerFixture
    {
        public StoreState ExpectedState { get; protected set; } = new DomainTestBuilder<StoreState>().Create();
        public StoreState InitialState { get; protected set; } = new DomainTestBuilder<StoreState>().Create();
    }
}