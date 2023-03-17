using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States.Comparer;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.Actions.Editor.Sections;
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

    public class OnSectionDecremented
    {
        private readonly OnSectionDecrementedFixture _fixture;

        public OnSectionDecremented()
        {
            _fixture = new OnSectionDecrementedFixture();
        }

        [Fact]
        public void OnSectionDecremented_WithSectionDecrementable_ShouldDecrementSection()
        {
            // Arrange
            _fixture.SetupInitialStateForDecrementingLastSection();
            _fixture.SetupActionForDecrementingLastSection();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnSectionDecremented(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSectionDecremented_WithSectionNotDecrementable_ShouldNotChangeSectionOrder()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForDecrementingFirstSection();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnSectionDecremented(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSectionDecremented_WithInvalidSectionKey_ShouldNotChangeSectionOrder()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForInvalidSectionKey();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnSectionDecremented(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSectionDecremented_WithStoreNull_ShouldNotChangeSectionOrder()
        {
            // Arrange
            _fixture.SetupExpectedStateForStoreNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForStoreNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnSectionDecremented(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSectionDecrementedFixture : StoreEditorReducerFixture
        {
            public SectionDecrementedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialStateForDecrementingLastSection()
            {
                var sections = ExpectedState.Editor.Store!.Sections.ToList();
                var sortingIndexTmp = sections[1].SortingIndex;
                sections[1] = sections[1] with { SortingIndex = sections[2].SortingIndex };
                sections[2] = sections[2] with { SortingIndex = sortingIndexTmp };

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Store = ExpectedState.Editor.Store! with
                        {
                            Sections = new SortedSet<EditedSection>(sections, new SortingIndexComparer())
                        }
                    }
                };
            }

            public void SetupExpectedStateForStoreNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Store = null
                    }
                };
            }

            public void SetupActionForDecrementingLastSection()
            {
                var section = InitialState.Editor.Store!.Sections.Last();
                Action = new SectionDecrementedAction(section.Key);
            }

            public void SetupActionForDecrementingFirstSection()
            {
                var section = InitialState.Editor.Store!.Sections.First();
                Action = new SectionDecrementedAction(section.Key);
            }

            public void SetupActionForInvalidSectionKey()
            {
                Action = new SectionDecrementedAction(Guid.NewGuid());
            }

            public void SetupActionForStoreNull()
            {
                Action = new DomainTestBuilder<SectionDecrementedAction>().Create();
            }
        }
    }

    private abstract class StoreEditorReducerFixture
    {
        public StoreState ExpectedState { get; protected set; } = new DomainTestBuilder<StoreState>().Create();
        public StoreState InitialState { get; protected set; } = new DomainTestBuilder<StoreState>().Create();
    }
}