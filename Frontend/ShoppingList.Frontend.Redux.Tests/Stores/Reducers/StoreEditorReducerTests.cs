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
            result.Should().BeEquivalentTo(_fixture.ExpectedState,
                opt => opt.Excluding(info => info.Path == "Editor.Store.Sections[0].Key"));
        }

        private sealed class OnSetNewStoreFixture : StoreEditorReducerFixture
        {
            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupExpectedState()
            {
                var sections = new List<EditedSection>
                {
                    new(Guid.NewGuid(), Guid.Empty, "Default", true, 0)
                };

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Store = new EditedStore(
                            Guid.Empty,
                            string.Empty,
                            new SortedSet<EditedSection>(sections, new SortingIndexComparer()))
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

    public class OnSaveStoreStarted
    {
        private readonly OnSaveStoreStartedFixture _fixture;

        public OnSaveStoreStarted()
        {
            _fixture = new OnSaveStoreStartedFixture();
        }

        [Fact]
        public void OnSaveStoreStarted_WithNotAlreadySaving_ShouldSave()
        {
            // Arrange
            _fixture.SetupInitialStateNotSaving();
            _fixture.SetupExpectedStateSaving();

            // Act
            var result = StoreEditorReducer.OnSaveStoreStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSaveStoreStarted_WithAlreadySaving_ShouldSave()
        {
            // Arrange
            _fixture.SetupInitialStateSaving();
            _fixture.SetupExpectedStateSaving();

            // Act
            var result = StoreEditorReducer.OnSaveStoreStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSaveStoreStartedFixture : StoreEditorReducerFixture
        {
            public void SetupInitialStateSaving()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = true
                    }
                };
            }

            public void SetupInitialStateNotSaving()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = false
                    }
                };
            }

            public void SetupExpectedStateSaving()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = true
                    }
                };
            }
        }
    }

    public class OnSaveStoreFinished
    {
        private readonly OnSaveStoreFinishedFixture _fixture;

        public OnSaveStoreFinished()
        {
            _fixture = new OnSaveStoreFinishedFixture();
        }

        [Fact]
        public void OnSaveStoreFinished_WithSaving_ShouldNotSave()
        {
            // Arrange
            _fixture.SetupInitialStateSaving();
            _fixture.SetupExpectedStateNotSaving();

            // Act
            var result = StoreEditorReducer.OnSaveStoreFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSaveStoreFinished_WithNotSaving_ShouldNotSave()
        {
            // Arrange
            _fixture.SetupInitialStateNotSaving();
            _fixture.SetupExpectedStateNotSaving();

            // Act
            var result = StoreEditorReducer.OnSaveStoreFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSaveStoreFinishedFixture : StoreEditorReducerFixture
        {
            public void SetupInitialStateSaving()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = true
                    }
                };
            }

            public void SetupInitialStateNotSaving()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = false
                    }
                };
            }

            public void SetupExpectedStateNotSaving()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = false
                    }
                };
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

    public class OnSectionIncremented
    {
        private readonly OnSectionIncrementedFixture _fixture;

        public OnSectionIncremented()
        {
            _fixture = new OnSectionIncrementedFixture();
        }

        [Fact]
        public void OnSectionIncremented_WithSectionIncrementable_ShouldIncrementSection()
        {
            // Arrange
            _fixture.SetupInitialStateForIncrementingFirstSection();
            _fixture.SetupActionForIncrementingFirstSection();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnSectionIncremented(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSectionIncremented_WithSectionNotIncrementable_ShouldNotChangeSectionOrder()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForIncrementingLastSection();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnSectionIncremented(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSectionIncremented_WithInvalidSectionKey_ShouldNotChangeSectionOrder()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForInvalidSectionKey();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnSectionIncremented(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSectionIncremented_WithStoreNull_ShouldNotChangeSectionOrder()
        {
            // Arrange
            _fixture.SetupExpectedStateForStoreNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForStoreNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnSectionIncremented(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSectionIncrementedFixture : StoreEditorReducerFixture
        {
            public SectionIncrementedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialStateForIncrementingFirstSection()
            {
                var sections = ExpectedState.Editor.Store!.Sections.ToList();
                var sortingIndexTmp = sections[0].SortingIndex;
                sections[0] = sections[0] with { SortingIndex = sections[1].SortingIndex };
                sections[1] = sections[1] with { SortingIndex = sortingIndexTmp };

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

            public void SetupActionForIncrementingLastSection()
            {
                var section = InitialState.Editor.Store!.Sections.Last();
                Action = new SectionIncrementedAction(section.Key);
            }

            public void SetupActionForIncrementingFirstSection()
            {
                var section = InitialState.Editor.Store!.Sections.First();
                Action = new SectionIncrementedAction(section.Key);
            }

            public void SetupActionForInvalidSectionKey()
            {
                Action = new SectionIncrementedAction(Guid.NewGuid());
            }

            public void SetupActionForStoreNull()
            {
                Action = new DomainTestBuilder<SectionIncrementedAction>().Create();
            }
        }
    }

    public class OnSectionRemoved
    {
        private readonly OnSectionRemovedFixture _fixture;

        public OnSectionRemoved()
        {
            _fixture = new OnSectionRemovedFixture();
        }

        [Fact]
        public void OnSectionRemoved_WithValidSection_ShouldRemoveSection()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnSectionRemoved(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSectionRemoved_WithInvalidSection_ShouldNotRemoveSection()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForInvalidSection();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnSectionRemoved(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSectionRemoved_WithStoreNull_ShouldNotRemoveSection()
        {
            // Arrange
            _fixture.SetupExpectedStateStoreNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForStoreNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnSectionRemoved(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSectionRemovedFixture : StoreEditorReducerFixture
        {
            public SectionRemovedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialState()
            {
                var sections = ExpectedState.Editor.Store!.Sections.ToList();
                var additionalSection = new DomainTestBuilder<EditedSection>().Create() with { SortingIndex = -1 };
                sections.Add(additionalSection);

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Store = ExpectedState.Editor.Store with
                        {
                            Sections = new SortedSet<EditedSection>(sections, new SortingIndexComparer())
                        }
                    }
                };
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
                Action = new SectionRemovedAction(InitialState.Editor.Store!.Sections.First());
            }

            public void SetupActionForInvalidSection()
            {
                Action = new SectionRemovedAction(new DomainTestBuilder<EditedSection>().Create());
            }

            public void SetupActionForStoreNull()
            {
                Action = new DomainTestBuilder<SectionRemovedAction>().Create();
            }
        }
    }

    public class OnSectionTextChanged
    {
        private readonly OnSectionTextChangedFixture _fixture;

        public OnSectionTextChanged()
        {
            _fixture = new OnSectionTextChangedFixture();
        }

        [Fact]
        public void OnSectionTextChanged_WithValidSection_ShouldChangeText()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnSectionTextChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSectionTextChanged_WithInvalidSection_ShouldNotChangeText()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForInvalidSection();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnSectionTextChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSectionTextChanged_WithStoreNull_ShouldNotChangeText()
        {
            // Arrange
            _fixture.SetupExpectedStateStoreNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForStoreNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnSectionTextChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSectionTextChangedFixture : StoreEditorReducerFixture
        {
            public SectionTextChangedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialState()
            {
                var sections = ExpectedState.Editor.Store!.Sections.ToList();
                sections[0] = sections[0] with { Name = new DomainTestBuilder<string>().Create() };

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Store = ExpectedState.Editor.Store with
                        {
                            Sections = new SortedSet<EditedSection>(sections, new SortingIndexComparer())
                        }
                    }
                };
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
                var section = ExpectedState.Editor.Store!.Sections.First();
                Action = new SectionTextChangedAction(section.Key, section.Name);
            }

            public void SetupActionForInvalidSection()
            {
                Action = new SectionTextChangedAction(Guid.NewGuid(), string.Empty);
            }

            public void SetupActionForStoreNull()
            {
                Action = new DomainTestBuilder<SectionTextChangedAction>().Create();
            }
        }
    }

    public class OnDefaultSectionChanged
    {
        private readonly OnDefaultSectionChangedFixture _fixture;

        public OnDefaultSectionChanged()
        {
            _fixture = new OnDefaultSectionChangedFixture();
        }

        [Fact]
        public void OnDefaultSectionChanged_WithValidSection_ShouldChangeDefaultSection()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnDefaultSectionChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnDefaultSectionChanged_WithInvalidSection_ShouldNotChangeDefaultSection()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForInvalidSection();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnDefaultSectionChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnDefaultSectionChanged_WithStoreNull_ShouldNotChangeDefaultSection()
        {
            // Arrange
            _fixture.SetupExpectedStateStoreNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForStoreNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnDefaultSectionChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnDefaultSectionChangedFixture : StoreEditorReducerFixture
        {
            public DefaultSectionChangedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialState()
            {
                var sections = ExpectedState.Editor.Store!.Sections.ToList();
                sections[0] = sections[0] with { IsDefaultSection = false };
                sections[2] = sections[2] with { IsDefaultSection = true };

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Store = ExpectedState.Editor.Store with
                        {
                            Sections = new SortedSet<EditedSection>(sections, new SortingIndexComparer())
                        }
                    }
                };
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
                var section = ExpectedState.Editor.Store!.Sections.First();
                Action = new DefaultSectionChangedAction(section.Key);
            }

            public void SetupActionForInvalidSection()
            {
                Action = new DefaultSectionChangedAction(Guid.NewGuid());
            }

            public void SetupActionForStoreNull()
            {
                Action = new DomainTestBuilder<DefaultSectionChangedAction>().Create();
            }
        }
    }

    public class OnSectionAdded
    {
        private readonly OnSectionAddedFixture _fixture;

        public OnSectionAdded()
        {
            _fixture = new OnSectionAddedFixture();
        }

        [Fact]
        public void OnSectionAdded_WithSectionsExisting_ShouldAddSection()
        {
            // Arrange
            _fixture.SetupInitialStateForSectionsExisting();
            _fixture.SetupExpectedStateForSectionsExisting();

            // Act
            var result = StoreEditorReducer.OnSectionAdded(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState,
                opt => opt.Excluding(info => info.Path == "Editor.Store.Sections[2].Key"));
        }

        [Fact]
        public void OnSectionAdded_WithNoSectionsExisting_ShouldAddSection()
        {
            // Arrange
            _fixture.SetupInitialStateForNoSectionsExisting();
            _fixture.SetupExpectedStateForNoSectionsExisting();

            // Act
            var result = StoreEditorReducer.OnSectionAdded(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState,
                opt => opt.Excluding(info => info.Path == "Editor.Store.Sections[0].Key"));
        }

        [Fact]
        public void OnSectionAdded_WithStoreNull_ShouldNotChangeDefaultSection()
        {
            // Arrange
            _fixture.SetupExpectedStateStoreNull();
            _fixture.SetupInitialStateEqualsExpectedState();

            // Act
            var result = StoreEditorReducer.OnSectionAdded(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSectionAddedFixture : StoreEditorReducerFixture
        {
            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialStateForSectionsExisting()
            {
                var sections = ExpectedState.Editor.Store!.Sections.ToList();
                sections.RemoveAt(2);

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Store = ExpectedState.Editor.Store with
                        {
                            Sections = new SortedSet<EditedSection>(sections, new SortingIndexComparer())
                        }
                    }
                };
            }

            public void SetupInitialStateForNoSectionsExisting()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Store = ExpectedState.Editor.Store! with
                        {
                            Sections = new SortedSet<EditedSection>(new SortingIndexComparer())
                        }
                    }
                };
            }

            public void SetupExpectedStateForSectionsExisting()
            {
                var sections = ExpectedState.Editor.Store!.Sections.ToList();
                sections[2] = sections[2] with
                {
                    Id = Guid.Empty,
                    Name = string.Empty,
                    IsDefaultSection = false
                };

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Store = ExpectedState.Editor.Store with
                        {
                            Sections = new SortedSet<EditedSection>(sections, new SortingIndexComparer())
                        }
                    }
                };
            }

            public void SetupExpectedStateForNoSectionsExisting()
            {
                var sections = new List<EditedSection>
                {
                    new(
                        Guid.NewGuid(),
                        Guid.Empty,
                        string.Empty,
                        false,
                        0
                    )
                };

                ExpectedState = ExpectedState with
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
        }
    }

    public class OnDeleteStoreButtonClicked
    {
        private readonly OnDeleteStoreButtonClickedFixture _fixture;

        public OnDeleteStoreButtonClicked()
        {
            _fixture = new OnDeleteStoreButtonClickedFixture();
        }

        [Fact]
        public void OnDeleteStoreButtonClicked_WithDeletionNoticeShowing_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateShowingDeletionNotice();
            _fixture.SetupExpectedState();

            // Act
            var result = StoreEditorReducer.OnDeleteStoreButtonClicked(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnDeleteStoreButtonClicked_WithDeletionNoticeNotShowing_ShouldShowDeletionNotice()
        {
            // Arrange
            _fixture.SetupInitialStateNotShowingDeletionNotice();
            _fixture.SetupExpectedState();

            // Act
            var result = StoreEditorReducer.OnDeleteStoreButtonClicked(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnDeleteStoreButtonClickedFixture : StoreEditorReducerFixture
        {
            public void SetupInitialStateShowingDeletionNotice()
            {
                SetupInitialState(true);
            }

            public void SetupInitialStateNotShowingDeletionNotice()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isShowingDeletionNotice)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsShowingDeletionNotice = isShowingDeletionNotice
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsShowingDeletionNotice = true
                    }
                };
            }
        }
    }

    public class OnDeleteStoreAborted
    {
        private readonly OnDeleteStoreAbortedFixture _fixture;

        public OnDeleteStoreAborted()
        {
            _fixture = new OnDeleteStoreAbortedFixture();
        }

        [Fact]
        public void OnDeleteStoreAborted_WithDeletionNoticeNotShowing_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateNotShowingDeletionNotice();
            _fixture.SetupExpectedState();

            // Act
            var result = StoreEditorReducer.OnDeleteStoreAborted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnDeleteStoreAborted_WithDeletionNoticeShowing_ShouldHideDeletionNotice()
        {
            // Arrange
            _fixture.SetupInitialStateShowingDeletionNotice();
            _fixture.SetupExpectedState();

            // Act
            var result = StoreEditorReducer.OnDeleteStoreAborted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnDeleteStoreAbortedFixture : StoreEditorReducerFixture
        {
            public void SetupInitialStateShowingDeletionNotice()
            {
                SetupInitialState(true);
            }

            public void SetupInitialStateNotShowingDeletionNotice()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isShowingDeletionNotice)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsShowingDeletionNotice = isShowingDeletionNotice
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsShowingDeletionNotice = false
                    }
                };
            }
        }
    }

    public class OnDeleteStoreFinished
    {
        private readonly OnDeleteStoreFinishedFixture _fixture;

        public OnDeleteStoreFinished()
        {
            _fixture = new OnDeleteStoreFinishedFixture();
        }

        [Fact]
        public void OnDeleteStoreFinished_WithDeletionNoticeNotShowing_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateNotShowingDeletionNotice();
            _fixture.SetupExpectedState();

            // Act
            var result = StoreEditorReducer.OnDeleteStoreFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnDeleteStoreFinished_WithDeletionNoticeShowing_ShouldHideDeletionNotice()
        {
            // Arrange
            _fixture.SetupInitialStateShowingDeletionNotice();
            _fixture.SetupExpectedState();

            // Act
            var result = StoreEditorReducer.OnDeleteStoreFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnDeleteStoreFinishedFixture : StoreEditorReducerFixture
        {
            public void SetupInitialStateShowingDeletionNotice()
            {
                SetupInitialState(true);
            }

            public void SetupInitialStateNotShowingDeletionNotice()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isShowingDeletionNotice)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsShowingDeletionNotice = isShowingDeletionNotice
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsShowingDeletionNotice = false
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