using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Manufacturers.Reducers;

public class ManufacturerReducerTests
{
    public class OnSearchManufacturersStarted
    {
        private readonly OnSearchManufacturersStartedFixture _fixture;

        public OnSearchManufacturersStarted()
        {
            _fixture = new OnSearchManufacturersStartedFixture();
        }

        [Fact]
        public void OnSearchManufacturersStarted_WithSearchNotLoading_ShouldSetLoading()
        {
            // Arrange
            _fixture.SetupInitialSearchNotLoading();
            _fixture.SetupExpectedState();

            // Act
            var result = ManufacturerReducer.OnSearchManufacturersStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSearchManufacturersStarted_WithSearchLoading_ShouldSetLoading()
        {
            // Arrange
            _fixture.SetupInitialSearchLoading();
            _fixture.SetupExpectedState();

            // Act
            var result = ManufacturerReducer.OnSearchManufacturersStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSearchManufacturersStartedFixture : ManufacturerReducerFixture
        {
            public void SetupInitialSearchLoading()
            {
                SetupInitialState(true);
            }

            public void SetupInitialSearchNotLoading()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isLoading)
            {
                InitialState = ExpectedState with
                {
                    Search = ExpectedState.Search with
                    {
                        IsLoadingSearchResults = isLoading
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Search = ExpectedState.Search with
                    {
                        IsLoadingSearchResults = true
                    }
                };
            }
        }
    }

    public class OnSearchManufacturersFinished
    {
        private readonly OnSearchManufacturersFinishedFixture _fixture;

        public OnSearchManufacturersFinished()
        {
            _fixture = new OnSearchManufacturersFinishedFixture();
        }

        [Fact]
        public void OnSearchManufacturersFinished_WithSearchNotLoading_ShouldSetNotLoadingAndSortResults()
        {
            // Arrange
            _fixture.SetupInitialSearchNotLoading();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ManufacturerReducer.OnSearchManufacturersFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public void OnSearchManufacturersFinished_WithSearchLoading_ShouldSetNotLoadingAndSortResults()
        {
            // Arrange
            _fixture.SetupInitialSearchLoading();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ManufacturerReducer.OnSearchManufacturersFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState, opt => opt.WithStrictOrdering());
        }

        private sealed class OnSearchManufacturersFinishedFixture : ManufacturerReducerFixture
        {
            public SearchManufacturersFinishedAction? Action { get; private set; }

            public void SetupInitialSearchLoading()
            {
                SetupInitialState(true);
            }

            public void SetupInitialSearchNotLoading()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isLoading)
            {
                InitialState = ExpectedState with
                {
                    Search = ExpectedState.Search with
                    {
                        IsLoadingSearchResults = isLoading,
                        TriggeredAtLeastOnce = false,
                        SearchResults = new DomainTestBuilder<ManufacturerSearchResult>().CreateMany(2).ToList()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Search = ExpectedState.Search with
                    {
                        IsLoadingSearchResults = false,
                        TriggeredAtLeastOnce = true,
                        SearchResults = new List<ManufacturerSearchResult>
                        {
                            new DomainTestBuilder<ManufacturerSearchResult>()
                                .FillPropertyWith(r => r.Name, $"A{new DomainTestBuilder<string>().Create()}")
                                .Create(),
                            new DomainTestBuilder<ManufacturerSearchResult>()
                                .FillPropertyWith(r => r.Name, $"B{new DomainTestBuilder<string>().Create()}")
                                .Create(),
                            new DomainTestBuilder<ManufacturerSearchResult>()
                                .FillPropertyWith(r => r.Name, $"Z{new DomainTestBuilder<string>().Create()}")
                                .Create()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new SearchManufacturersFinishedAction(ExpectedState.Search.SearchResults.Reverse().ToList());
            }
        }
    }

    public class OnLoadManufacturerForEditingStarted
    {
        private readonly OnLoadManufacturerForEditingStartedFixture _fixture;

        public OnLoadManufacturerForEditingStarted()
        {
            _fixture = new OnLoadManufacturerForEditingStartedFixture();
        }

        [Fact]
        public void OnLoadManufacturerForEditingStarted_WithEditorNotLoading_ShouldSetLoading()
        {
            // Arrange
            _fixture.SetupInitialSearchNotLoading();
            _fixture.SetupExpectedState();

            // Act
            var result = ManufacturerReducer.OnLoadManufacturerForEditingStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnLoadManufacturerForEditingStarted_WithEditorLoading_ShouldSetLoading()
        {
            // Arrange
            _fixture.SetupInitialSearchLoading();
            _fixture.SetupExpectedState();

            // Act
            var result = ManufacturerReducer.OnLoadManufacturerForEditingStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadManufacturerForEditingStartedFixture : ManufacturerReducerFixture
        {
            public void SetupInitialSearchLoading()
            {
                SetupInitialState(true);
            }

            public void SetupInitialSearchNotLoading()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isLoading)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsLoadingEditedManufacturer = isLoading
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsLoadingEditedManufacturer = true
                    }
                };
            }
        }
    }

    public class OnLoadManufacturerForEditingFinished
    {
        private readonly OnLoadManufacturerForEditingFinishedFixture _fixture;

        public OnLoadManufacturerForEditingFinished()
        {
            _fixture = new OnLoadManufacturerForEditingFinishedFixture();
        }

        [Fact]
        public void OnLoadManufacturerForEditingFinished_WithEditorNotLoading_ShouldSetNotLoadingAndManufacturer()
        {
            // Arrange
            _fixture.SetupInitialSearchNotLoading();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ManufacturerReducer.OnLoadManufacturerForEditingFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnLoadManufacturerForEditingFinished_WithEditorLoading_ShouldSetNotLoadingAndManufacturer()
        {
            // Arrange
            _fixture.SetupInitialSearchLoading();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ManufacturerReducer.OnLoadManufacturerForEditingFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadManufacturerForEditingFinishedFixture : ManufacturerReducerFixture
        {
            public LoadManufacturerForEditingFinishedAction? Action { get; private set; }

            public void SetupInitialSearchLoading()
            {
                SetupInitialState(true);
            }

            public void SetupInitialSearchNotLoading()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isLoading)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsLoadingEditedManufacturer = isLoading,
                        Manufacturer = new DomainTestBuilder<EditedManufacturer>().Create()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsLoadingEditedManufacturer = false,
                    }
                };
            }

            public void SetupAction()
            {
                Action = new LoadManufacturerForEditingFinishedAction(ExpectedState.Editor.Manufacturer!);
            }
        }
    }

    public class OnEditedManufacturerNameChanged
    {
        private readonly OnEditedManufacturerNameChangedFixture _fixture;

        public OnEditedManufacturerNameChanged()
        {
            _fixture = new OnEditedManufacturerNameChangedFixture();
        }

        [Fact]
        public void OnEditedManufacturerNameChanged_WithValidData_ShouldSetManufacturerName()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ManufacturerReducer.OnEditedManufacturerNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnEditedManufacturerNameChangedFixture : ManufacturerReducerFixture
        {
            public EditedManufacturerNameChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Manufacturer = ExpectedState.Editor.Manufacturer! with
                        {
                            Name = new DomainTestBuilder<string>().Create()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new EditedManufacturerNameChangedAction(ExpectedState.Editor.Manufacturer!.Name);
            }
        }
    }

    public class OnLeaveManufacturerEditor
    {
        private readonly OnLeaveManufacturerEditorFixture _fixture;

        public OnLeaveManufacturerEditor()
        {
            _fixture = new OnLeaveManufacturerEditorFixture();
        }

        [Fact]
        public void OnLeaveManufacturerEditor_WithValidData_ShouldSetManufacturerNull()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ManufacturerReducer.OnLeaveManufacturerEditor(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLeaveManufacturerEditorFixture : ManufacturerReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Manufacturer = new DomainTestBuilder<EditedManufacturer>().Create()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Manufacturer = null
                    }
                };
            }
        }
    }

    public class OnSavingManufacturerStarted
    {
        private readonly OnSavingManufacturerStartedFixture _fixture;

        public OnSavingManufacturerStarted()
        {
            _fixture = new OnSavingManufacturerStartedFixture();
        }

        [Fact]
        public void OnSavingManufacturerStarted_WithEditorNotSaving_ShouldSetSaving()
        {
            // Arrange
            _fixture.SetupInitialSearchNotSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = ManufacturerReducer.OnSavingManufacturerStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSavingManufacturerStarted_WithEditorSaving_ShouldSetSaving()
        {
            // Arrange
            _fixture.SetupInitialSearchSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = ManufacturerReducer.OnSavingManufacturerStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSavingManufacturerStartedFixture : ManufacturerReducerFixture
        {
            public void SetupInitialSearchSaving()
            {
                SetupInitialState(true);
            }

            public void SetupInitialSearchNotSaving()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isSaving)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = isSaving
                    }
                };
            }

            public void SetupExpectedState()
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

    public class OnSavingManufacturerFinished
    {
        private readonly OnSavingManufacturerFinishedFixture _fixture;

        public OnSavingManufacturerFinished()
        {
            _fixture = new OnSavingManufacturerFinishedFixture();
        }

        [Fact]
        public void OnSavingManufacturerFinished_WithEditorNotSaving_ShouldSetNotSaving()
        {
            // Arrange
            _fixture.SetupInitialSearchNotSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = ManufacturerReducer.OnSavingManufacturerFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSavingManufacturerFinished_WithEditorSaving_ShouldSetNotSaving()
        {
            // Arrange
            _fixture.SetupInitialSearchSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = ManufacturerReducer.OnSavingManufacturerFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSavingManufacturerFinishedFixture : ManufacturerReducerFixture
        {
            public void SetupInitialSearchSaving()
            {
                SetupInitialState(true);
            }

            public void SetupInitialSearchNotSaving()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isSaving)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = isSaving
                    }
                };
            }

            public void SetupExpectedState()
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

    public class OnDeletingManufacturerStarted
    {
        private readonly OnDeletingManufacturerStartedFixture _fixture;

        public OnDeletingManufacturerStarted()
        {
            _fixture = new OnDeletingManufacturerStartedFixture();
        }

        [Fact]
        public void OnDeletingManufacturerStarted_WithEditorNotDeleting_ShouldSetDeleting()
        {
            // Arrange
            _fixture.SetupInitialSearchNotDeleting();
            _fixture.SetupExpectedState();

            // Act
            var result = ManufacturerReducer.OnDeletingManufacturerStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnDeletingManufacturerStarted_WithEditorDeleting_ShouldSetDeleting()
        {
            // Arrange
            _fixture.SetupInitialSearchDeleting();
            _fixture.SetupExpectedState();

            // Act
            var result = ManufacturerReducer.OnDeletingManufacturerStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnDeletingManufacturerStartedFixture : ManufacturerReducerFixture
        {
            public void SetupInitialSearchDeleting()
            {
                SetupInitialState(true);
            }

            public void SetupInitialSearchNotDeleting()
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

    public class OnDeletingManufacturerFinished
    {
        private readonly OnDeletingManufacturerFinishedFixture _fixture;

        public OnDeletingManufacturerFinished()
        {
            _fixture = new OnDeletingManufacturerFinishedFixture();
        }

        [Fact]
        public void OnDeletingManufacturerFinished_WithEditorNotDeleting_ShouldSetNotDeleting()
        {
            // Arrange
            _fixture.SetupInitialSearchNotDeleting();
            _fixture.SetupExpectedState();

            // Act
            var result = ManufacturerReducer.OnDeletingManufacturerFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnDeletingManufacturerFinished_WithEditorDeleting_ShouldSetNotDeleting()
        {
            // Arrange
            _fixture.SetupInitialSearchDeleting();
            _fixture.SetupExpectedState();

            // Act
            var result = ManufacturerReducer.OnDeletingManufacturerFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnDeletingManufacturerFinishedFixture : ManufacturerReducerFixture
        {
            public void SetupInitialSearchDeleting()
            {
                SetupInitialState(true);
            }

            public void SetupInitialSearchNotDeleting()
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

    public class OnSetNewManufacturer
    {
        private readonly OnSetNewManufacturerFixture _fixture;

        public OnSetNewManufacturer()
        {
            _fixture = new OnSetNewManufacturerFixture();
        }

        [Fact]
        public void OnSetNewManufacturer_WithValidData_ShouldSetEmptyManufacturer()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ManufacturerReducer.OnSetNewManufacturer(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSetNewManufacturerFixture : ManufacturerReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Manufacturer = new DomainTestBuilder<EditedManufacturer>().Create()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Manufacturer = new EditedManufacturer(Guid.Empty, string.Empty)
                    }
                };
            }
        }
    }

    private abstract class ManufacturerReducerFixture
    {
        public ManufacturerState ExpectedState { get; protected set; } = new DomainTestBuilder<ManufacturerState>().Create();
        public ManufacturerState InitialState { get; protected set; } = new DomainTestBuilder<ManufacturerState>().Create();
    }
}