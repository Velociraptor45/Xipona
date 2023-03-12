using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

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

    public class OnLoadStoreForEditingFinished
    {
        private readonly OnLoadStoreForEditingFinishedFixture _fixture;

        public OnLoadStoreForEditingFinished()
        {
            _fixture = new OnLoadStoreForEditingFinishedFixture();
        }

        [Fact]
        public void OnLoadStoreForEditingFinished_WithValidData_ShouldSetStore()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnLoadStoreForEditingFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadStoreForEditingFinishedFixture : StoreEditorReducerFixture
        {
            public LoadStoreForEditingFinishedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupAction()
            {
                Action = new LoadStoreForEditingFinishedAction(ExpectedState.Editor.Store!);
            }
        }
    }

    public class OnStoreNameChanged
    {
        private readonly OnStoreNameChangedFixture _fixture;

        public OnStoreNameChanged()
        {
            _fixture = new OnStoreNameChangedFixture();
        }

        [Fact]
        public void OnStoreNameChanged_WithValidData_ShouldChangeName()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnStoreNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnStoreNameChanged_WithStoreNull_ShouldNotChangeName()
        {
            // Arrange
            _fixture.SetupExpectedStateStoreNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForStoreNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnStoreNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnStoreNameChangedFixture : StoreEditorReducerFixture
        {
            public StoreNameChangedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupExpectedStateStoreNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Store = null
                    }
                };
            }

            public void SetupAction()
            {
                Action = new StoreNameChangedAction(ExpectedState.Editor.Store!.Name);
            }

            public void SetupActionForStoreNull()
            {
                Action = new DomainTestBuilder<StoreNameChangedAction>().Create();
            }
        }
    }

    private abstract class StoreEditorReducerFixture
    {
        public StoreState ExpectedState { get; protected set; } = new DomainTestBuilder<StoreState>().Create();
        public StoreState InitialState { get; protected set; } = new DomainTestBuilder<StoreState>().Create();
    }
}