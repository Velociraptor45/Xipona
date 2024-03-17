using FluentAssertions;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States.Comparer;
using ProjectHermes.Xipona.Frontend.Redux.Stores.Actions.Editor;
using ProjectHermes.Xipona.Frontend.Redux.Stores.Actions.Editor.Sections;
using ProjectHermes.Xipona.Frontend.Redux.Stores.Reducers;
using ProjectHermes.Xipona.Frontend.Redux.Stores.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Stores.Reducers;

public class StoreEditorReducerTests
{
    public class OnSaveStore
    {
        private readonly OnSaveStoreFixture _fixture = new();

        [Fact]
        public void OnSaveStore_WithStoreNull_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupExpectedStateStoreNull();
            _fixture.SetupInitialStateEqualsExpectedState();

            // Act
            var result = StoreEditorReducer.OnSaveStore(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSaveStore_WithAllValuesValid_ShouldNotSetValidationErrors()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();

            // Act
            var result = StoreEditorReducer.OnSaveStore(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSaveStore_WithStoreNameEmpty_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithStoreNameValidationError();
            _fixture.SetupInitialStateWithoutValidationErrors();

            // Act
            var result = StoreEditorReducer.OnSaveStore(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSaveStore_WithSectionNameEmpty_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithSectionNameValidationError();
            _fixture.SetupInitialStateWithoutValidationErrors();

            // Act
            var result = StoreEditorReducer.OnSaveStore(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSaveStoreFixture : StoreEditorReducerFixture
        {
            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialStateWithoutValidationErrors()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ValidationResult = new EditorValidationResult()
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

            public void SetupExpectedStateWithStoreNameValidationError()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Store = ExpectedState.Editor.Store! with
                        {
                            Name = string.Empty
                        },
                        ValidationResult = new EditorValidationResult
                        {
                            Name = "Name must not be empty"
                        }
                    }
                };
            }

            public void SetupExpectedStateWithSectionNameValidationError()
            {
                var key = Guid.NewGuid();
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Store = ExpectedState.Editor.Store! with
                        {
                            Sections = new SortedSet<EditedSection>(
                                new[]
                                {
                                    new EditedSection(key, Guid.Empty, string.Empty, false, 0),
                                    new EditedSection(Guid.NewGuid(), Guid.Empty, "Default", true, 1)
                                }, new SortingIndexComparer())
                        },
                        ValidationResult = new EditorValidationResult
                        {
                            SectionNames = new Dictionary<Guid, string>
                            {
                                { key, "Section name must not be empty" }
                            }
                        }
                    }
                };
            }
        }
    }

    public class OnSetNewStore
    {
        private readonly OnSetNewStoreFixture _fixture = new();

        [Fact]
        public void OnSetNewStore_WithoutExistingValidationErrors_ShouldSetStore()
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

        [Fact]
        public void OnSetNewStore_WithExistingValidationErrors_ShouldClearValidationErrors()
        {
            // Arrange
            _fixture.SetupInitialStateWithValidationErrors();
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

            public void SetupInitialStateWithValidationErrors()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ValidationResult = new EditorValidationResult
                        {
                            Name = "Name must not be empty"
                        }
                    }
                };
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
                            new SortedSet<EditedSection>(sections, new SortingIndexComparer())),
                        ValidationResult = new EditorValidationResult()
                    }
                };
            }
        }
    }

    public class OnLoadStoreForEditingFinished
    {
        private readonly OnLoadStoreForEditingFinishedFixture _fixture = new();

        [Fact]
        public void OnLoadStoreForEditingFinished_WithoutExistingValidationErrors_ShouldSetStore()
        {
            // Arrange
            _fixture.SetupExpectedStoreWithoutValidationErrors();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnLoadStoreForEditingFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnLoadStoreForEditingFinished_WithExistingValidationErrors_ShouldClearValidationErrors()
        {
            // Arrange
            _fixture.SetupExpectedStoreWithoutValidationErrors();
            _fixture.SetupInitialStateWithValidationErrors();
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

            public void SetupInitialStateWithValidationErrors()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ValidationResult = new EditorValidationResult
                        {
                            Name = "Name must not be empty"
                        }
                    }
                };
            }

            public void SetupExpectedStoreWithoutValidationErrors()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ValidationResult = new EditorValidationResult()
                    }
                };
            }

            public void SetupAction()
            {
                Action = new LoadStoreForEditingFinishedAction(ExpectedState.Editor.Store!);
            }
        }
    }

    public class OnStoreNameChanged
    {
        private readonly OnStoreNameChangedFixture _fixture = new();

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
        public void OnStoreNameChanged_WithStoreNameEmpty_ShouldHaveValidationError()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupExpectedStateStoreNameEmpty();
            _fixture.SetupActionForStoreNameEmpty();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnStoreNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnStoreNameChanged_WithExistingValidationErrorAndStoreNameNotEmpty_ShouldHaveNoValidationError()
        {
            // Arrange
            _fixture.SetupInitialStateWithValidationError();
            _fixture.SetupExpectedStatePreviousValidationError();
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

            public void SetupInitialStateWithValidationError()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ValidationResult = new EditorValidationResult
                        {
                            Name = "Name must not be empty"
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

            public void SetupExpectedStateStoreNameEmpty()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Store = ExpectedState.Editor.Store! with
                        {
                            Name = string.Empty
                        },
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            Name = "Name must not be empty"
                        }
                    }
                };
            }

            public void SetupExpectedStatePreviousValidationError()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ValidationResult = new EditorValidationResult()
                    }
                };
            }

            public void SetupAction()
            {
                Action = new StoreNameChangedAction(ExpectedState.Editor.Store!.Name);
            }

            public void SetupActionForStoreNameEmpty()
            {
                Action = new StoreNameChangedAction(string.Empty);
            }

            public void SetupActionForStoreNull()
            {
                Action = new DomainTestBuilder<StoreNameChangedAction>().Create();
            }
        }
    }

    public class OnSaveStoreStarted
    {
        private readonly OnSaveStoreStartedFixture _fixture = new();

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
        private readonly OnSaveStoreFinishedFixture _fixture = new();

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
        private readonly OnSectionDecrementedFixture _fixture = new();

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
        private readonly OnSectionIncrementedFixture _fixture = new();

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
        private readonly OnSectionRemovedFixture _fixture = new();

        [Fact]
        public void OnSectionRemoved_WithRemovingDefaultSection_ShouldRemoveSection()
        {
            // Arrange
            _fixture.SetupInitialStateForRemovingDefaultSection();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnSectionRemoved(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSectionRemoved_WithRemovingNonDefaultSection_ShouldRemoveSection()
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
        public void OnSectionRemoved_WithOnlyOneSection_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupExpectedStateWithOneSection();
            _fixture.SetupInitialStateEqualsExpectedState();
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

            public void SetupInitialStateForRemovingDefaultSection()
            {
                var sections = ExpectedState.Editor.Store!.Sections.ToList();
                var additionalSection = new DomainTestBuilder<EditedSection>().Create() with
                {
                    IsDefaultSection = true,
                    SortingIndex = -1
                };
                sections.Add(additionalSection);
                sections[0] = sections[0] with { IsDefaultSection = false };

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

            public void SetupExpectedStateWithOneSection()
            {
                var section = new DomainTestBuilder<EditedSection>().Create() with
                {
                    IsDefaultSection = true,
                    SortingIndex = -1
                };

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Store = ExpectedState.Editor.Store! with
                        {
                            Sections = new SortedSet<EditedSection>(new[] { section }, new SortingIndexComparer())
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
        private readonly OnSectionTextChangedFixture _fixture = new();

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

        [Fact]
        public void OnSectionTextChanged_WithSectionNameEmpty_ShouldHaveValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithSectionNameValidationError();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnSectionTextChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSectionTextChanged_WithExistingValidationErrorAndNameNotEmpty_ShouldRemoveValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithoutValidationError();
            _fixture.SetupInitialStateWithSectionNameValidationError();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreEditorReducer.OnSectionTextChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSectionTextChanged_WithExistingValidationErrorInDifferentSectionAndNameNotEmpty_ShouldNotValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithDifferentSectionNameValidationError();
            _fixture.SetupInitialStateWithDifferentSectionNameValidationError();
            _fixture.SetupAction();

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
                SetupInitialState(new DomainTestBuilder<string>().Create(), new Dictionary<Guid, string>(0));
            }

            public void SetupInitialStateWithDifferentSectionNameValidationError()
            {
                SetupInitialState(new DomainTestBuilder<string>().Create(),
                    ExpectedState.Editor.ValidationResult.SectionNames);
            }

            public void SetupInitialStateWithSectionNameValidationError()
            {
                SetupInitialState(string.Empty, new Dictionary<Guid, string>
                {
                    { ExpectedState.Editor.Store!.Sections.First().Key, "Section name must not be empty" }
                });
            }

            private void SetupInitialState(string name, IReadOnlyDictionary<Guid, string> sectionNameErrors)
            {
                var sections = ExpectedState.Editor.Store!.Sections.ToList();
                sections[0] = sections[0] with { Name = name };

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Store = ExpectedState.Editor.Store with
                        {
                            Sections = new SortedSet<EditedSection>(sections, new SortingIndexComparer())
                        },
                        ValidationResult = new EditorValidationResult
                        {
                            SectionNames = sectionNameErrors
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

            public void SetupExpectedStateWithoutValidationError()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ValidationResult = new EditorValidationResult()
                    }
                };
            }

            public void SetupExpectedStateWithSectionNameValidationError()
            {
                var key = Guid.NewGuid();
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Store = ExpectedState.Editor.Store! with
                        {
                            Sections = new SortedSet<EditedSection>(
                                new[]
                                {
                                    new EditedSection(key, Guid.Empty, string.Empty, false, 0),
                                    new EditedSection(Guid.NewGuid(), Guid.Empty, "Default", true, 1)
                                }, new SortingIndexComparer())
                        },
                        ValidationResult = new EditorValidationResult
                        {
                            SectionNames = new Dictionary<Guid, string>
                            {
                                { key, "Section name must not be empty" }
                            }
                        }
                    }
                };
            }

            public void SetupExpectedStateWithDifferentSectionNameValidationError()
            {
                var key = Guid.NewGuid();
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Store = ExpectedState.Editor.Store! with
                        {
                            Sections = new SortedSet<EditedSection>(
                                new[]
                                {
                                    new EditedSection(Guid.NewGuid(), Guid.Empty, "Default", true, 0),
                                    new EditedSection(key, Guid.Empty, string.Empty, false, 1)
                                }, new SortingIndexComparer())
                        },
                        ValidationResult = new EditorValidationResult
                        {
                            SectionNames = new Dictionary<Guid, string>
                            {
                                { key, "Section name must not be empty" }
                            }
                        }
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
        private readonly OnDefaultSectionChangedFixture _fixture = new();

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
        private readonly OnSectionAddedFixture _fixture = new();

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
        private readonly OnDeleteStoreButtonClickedFixture _fixture = new();

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
        private readonly OnDeleteStoreAbortedFixture _fixture = new();

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

    public class OnDeleteStoreStarted
    {
        private readonly OnDeleteStoreStartedFixture _fixture = new();

        [Fact]
        public void OnDeleteStoreStarted_WithNotDeleting_ShouldSetDeleting()
        {
            // Arrange
            _fixture.SetupInitialStateNotDeleting();
            _fixture.SetupExpectedState();

            // Act
            var result = StoreEditorReducer.OnDeleteStoreStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnDeleteStoreStarted_WithDeleting_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateDeleting();
            _fixture.SetupExpectedState();

            // Act
            var result = StoreEditorReducer.OnDeleteStoreStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnDeleteStoreStartedFixture : StoreEditorReducerFixture
        {
            public void SetupInitialStateDeleting()
            {
                SetupInitialState(true);
            }

            public void SetupInitialStateNotDeleting()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isDeleting)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleting = isDeleting
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleting = true
                    }
                };
            }
        }
    }

    public class OnDeleteStoreFinished
    {
        private readonly OnDeleteStoreFinishedFixture _fixture = new();

        [Fact]
        public void OnDeleteStoreFinished_WithDeleting_ShouldSetNotDeleting()
        {
            // Arrange
            _fixture.SetupInitialStateDeleting();
            _fixture.SetupExpectedState();

            // Act
            var result = StoreEditorReducer.OnDeleteStoreFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnDeleteStoreFinished_WithNotDeleting_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateNotDeleting();
            _fixture.SetupExpectedState();

            // Act
            var result = StoreEditorReducer.OnDeleteStoreFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnDeleteStoreFinishedFixture : StoreEditorReducerFixture
        {
            public void SetupInitialStateDeleting()
            {
                SetupInitialState(true);
            }

            public void SetupInitialStateNotDeleting()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isDeleting)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleting = isDeleting
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleting = false
                    }
                };
            }
        }
    }

    public class OnCloseDeleteStoreDialog
    {
        private readonly OnCloseDeleteStoreDialogFixture _fixture = new();

        [Fact]
        public void OnCloseDeleteStoreDialog_WithDeletionNoticeNotShowing_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateNotShowingDeletionNotice();
            _fixture.SetupExpectedState();

            // Act
            var result = StoreEditorReducer.OnCloseDeleteStoreDialog(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCloseDeleteStoreDialog_WithDeletionNoticeShowing_ShouldHideDeletionNotice()
        {
            // Arrange
            _fixture.SetupInitialStateShowingDeletionNotice();
            _fixture.SetupExpectedState();

            // Act
            var result = StoreEditorReducer.OnCloseDeleteStoreDialog(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnCloseDeleteStoreDialogFixture : StoreEditorReducerFixture
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
        protected StoreEditorReducerFixture()
        {
            var rawExpectedState = new DomainTestBuilder<StoreState>().Create();
            ExpectedState = rawExpectedState with
            {
                Editor = rawExpectedState.Editor with
                {
                    ValidationResult = new EditorValidationResult()
                }
            };
        }

        public StoreState ExpectedState { get; protected set; }
        public StoreState InitialState { get; protected set; } = new DomainTestBuilder<StoreState>().Create();
    }
}