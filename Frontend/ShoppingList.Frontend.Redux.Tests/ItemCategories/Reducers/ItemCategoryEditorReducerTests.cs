using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.ItemCategories.Reducers;

public class ItemCategoryEditorReducerTests
{
    public class OnLoadItemCategoryForEditingStarted
    {
        private readonly OnLoadItemCategoryForEditingStartedFixture _fixture;

        public OnLoadItemCategoryForEditingStarted()
        {
            _fixture = new OnLoadItemCategoryForEditingStartedFixture();
        }

        [Fact]
        public void OnLoadItemCategoryForEditingStarted_WithEditorNotLoading_ShouldSetLoading()
        {
            // Arrange
            _fixture.SetupInitialSearchNotLoading();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemCategoryEditorReducer.OnLoadItemCategoryForEditingStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnLoadItemCategoryForEditingStarted_WithEditorLoading_ShouldSetLoading()
        {
            // Arrange
            _fixture.SetupInitialSearchLoading();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemCategoryEditorReducer.OnLoadItemCategoryForEditingStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadItemCategoryForEditingStartedFixture : ItemCategoryEditorReducerFixture
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
                        IsLoadingEditedItemCategory = isLoading
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsLoadingEditedItemCategory = true
                    }
                };
            }
        }
    }

    public class OnLoadItemCategoryForEditingFinished
    {
        private readonly OnLoadItemCategoryForEditingFinishedFixture _fixture;

        public OnLoadItemCategoryForEditingFinished()
        {
            _fixture = new OnLoadItemCategoryForEditingFinishedFixture();
        }

        [Fact]
        public void OnLoadItemCategoryForEditingFinished_WithEditorNotLoading_ShouldSetNotLoadingAndItemCategory()
        {
            // Arrange
            _fixture.SetupInitialSearchNotLoading();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategoryEditorReducer.OnLoadItemCategoryForEditingFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnLoadItemCategoryForEditingFinished_WithEditorLoading_ShouldSetNotLoadingAndItemCategory()
        {
            // Arrange
            _fixture.SetupInitialSearchLoading();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategoryEditorReducer.OnLoadItemCategoryForEditingFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadItemCategoryForEditingFinishedFixture : ItemCategoryEditorReducerFixture
        {
            public LoadItemCategoryForEditingFinishedAction? Action { get; private set; }

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
                        IsLoadingEditedItemCategory = isLoading,
                        ItemCategory = new DomainTestBuilder<EditedItemCategory>().Create()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsLoadingEditedItemCategory = false,
                    }
                };
            }

            public void SetupAction()
            {
                Action = new LoadItemCategoryForEditingFinishedAction(ExpectedState.Editor.ItemCategory!);
            }
        }
    }

    public class OnEditedItemCategoryNameChanged
    {
        private readonly OnEditedItemCategoryNameChangedFixture _fixture;

        public OnEditedItemCategoryNameChanged()
        {
            _fixture = new OnEditedItemCategoryNameChangedFixture();
        }

        [Fact]
        public void OnEditedItemCategoryNameChanged_WithValidData_ShouldSetItemCategoryName()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategoryEditorReducer.OnEditedItemCategoryNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnEditedItemCategoryNameChangedFixture : ItemCategoryEditorReducerFixture
        {
            public EditedItemCategoryNameChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ItemCategory = ExpectedState.Editor.ItemCategory! with
                        {
                            Name = new DomainTestBuilder<string>().Create()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new EditedItemCategoryNameChangedAction(ExpectedState.Editor.ItemCategory!.Name);
            }
        }
    }

    public class OnLeaveItemCategoryEditor
    {
        private readonly OnLeaveItemCategoryEditorFixture _fixture;

        public OnLeaveItemCategoryEditor()
        {
            _fixture = new OnLeaveItemCategoryEditorFixture();
        }

        [Fact]
        public void OnLeaveItemCategoryEditor_WithValidData_ShouldSetItemCategoryNull()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemCategoryEditorReducer.OnLeaveItemCategoryEditor(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLeaveItemCategoryEditorFixture : ItemCategoryEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ItemCategory = new DomainTestBuilder<EditedItemCategory>().Create()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ItemCategory = null
                    }
                };
            }
        }
    }

    public class OnSavingItemCategoryStarted
    {
        private readonly OnSavingItemCategoryStartedFixture _fixture;

        public OnSavingItemCategoryStarted()
        {
            _fixture = new OnSavingItemCategoryStartedFixture();
        }

        [Fact]
        public void OnSavingItemCategoryStarted_WithEditorNotSaving_ShouldSetSaving()
        {
            // Arrange
            _fixture.SetupInitialSearchNotSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemCategoryEditorReducer.OnSavingItemCategoryStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSavingItemCategoryStarted_WithEditorSaving_ShouldSetSaving()
        {
            // Arrange
            _fixture.SetupInitialSearchSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemCategoryEditorReducer.OnSavingItemCategoryStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSavingItemCategoryStartedFixture : ItemCategoryEditorReducerFixture
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

    public class OnSavingItemCategoryFinished
    {
        private readonly OnSavingItemCategoryFinishedFixture _fixture;

        public OnSavingItemCategoryFinished()
        {
            _fixture = new OnSavingItemCategoryFinishedFixture();
        }

        [Fact]
        public void OnSavingItemCategoryFinished_WithEditorNotSaving_ShouldSetNotSaving()
        {
            // Arrange
            _fixture.SetupInitialSearchNotSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemCategoryEditorReducer.OnSavingItemCategoryFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSavingItemCategoryFinished_WithEditorSaving_ShouldSetNotSaving()
        {
            // Arrange
            _fixture.SetupInitialSearchSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemCategoryEditorReducer.OnSavingItemCategoryFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSavingItemCategoryFinishedFixture : ItemCategoryEditorReducerFixture
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

    public class OnDeletingItemCategoryStarted
    {
        private readonly OnDeletingItemCategoryStartedFixture _fixture;

        public OnDeletingItemCategoryStarted()
        {
            _fixture = new OnDeletingItemCategoryStartedFixture();
        }

        [Fact]
        public void OnDeletingItemCategoryStarted_WithEditorNotDeleting_ShouldSetDeleting()
        {
            // Arrange
            _fixture.SetupInitialSearchNotDeleting();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemCategoryEditorReducer.OnDeletingItemCategoryStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnDeletingItemCategoryStarted_WithEditorDeleting_ShouldSetDeleting()
        {
            // Arrange
            _fixture.SetupInitialSearchDeleting();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemCategoryEditorReducer.OnDeletingItemCategoryStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnDeletingItemCategoryStartedFixture : ItemCategoryEditorReducerFixture
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

    public class OnDeletingItemCategoryFinished
    {
        private readonly OnDeletingItemCategoryFinishedFixture _fixture;

        public OnDeletingItemCategoryFinished()
        {
            _fixture = new OnDeletingItemCategoryFinishedFixture();
        }

        [Fact]
        public void OnDeletingItemCategoryFinished_WithEditorNotDeleting_ShouldSetNotDeleting()
        {
            // Arrange
            _fixture.SetupInitialSearchNotDeleting();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemCategoryEditorReducer.OnDeletingItemCategoryFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnDeletingItemCategoryFinished_WithEditorDeleting_ShouldSetNotDeleting()
        {
            // Arrange
            _fixture.SetupInitialSearchDeleting();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemCategoryEditorReducer.OnDeletingItemCategoryFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnDeletingItemCategoryFinishedFixture : ItemCategoryEditorReducerFixture
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
                        IsDeleteDialogOpen = true,
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
                        IsDeleteDialogOpen = false,
                        IsDeleting = false
                    }
                };
            }
        }
    }

    public class OnSetNewItemCategory
    {
        private readonly OnSetNewItemCategoryFixture _fixture;

        public OnSetNewItemCategory()
        {
            _fixture = new OnSetNewItemCategoryFixture();
        }

        [Fact]
        public void OnSetNewItemCategory_WithValidData_ShouldSetEmptyItemCategory()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemCategoryEditorReducer.OnSetNewItemCategory(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSetNewItemCategoryFixture : ItemCategoryEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ItemCategory = new DomainTestBuilder<EditedItemCategory>().Create()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ItemCategory = new EditedItemCategory(Guid.Empty, string.Empty)
                    }
                };
            }
        }
    }

    public class OnOpenDeleteItemCategoryDialog
    {
        private readonly OnOpenDeleteItemCategoryDialogFixture _fixture;

        public OnOpenDeleteItemCategoryDialog()
        {
            _fixture = new OnOpenDeleteItemCategoryDialogFixture();
        }

        [Fact]
        public void OnOpenDeleteItemCategoryDialog_WithDialogNotOpen_ShouldOpenDialog()
        {
            // Arrange
            _fixture.SetupInitialStateWithDialogNotOpen();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemCategoryEditorReducer.OnOpenDeleteItemCategoryDialog(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnOpenDeleteItemCategoryDialog_WithDialogOpen_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateWithDialogOpen();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemCategoryEditorReducer.OnOpenDeleteItemCategoryDialog(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnOpenDeleteItemCategoryDialogFixture : ItemCategoryEditorReducerFixture
        {
            public void SetupInitialStateWithDialogOpen()
            {
                SetupInitialState(true);
            }

            public void SetupInitialStateWithDialogNotOpen()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isDeleteDialogOpen)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleteDialogOpen = isDeleteDialogOpen
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleteDialogOpen = true
                    }
                };
            }
        }
    }

    public class OnCloseDeleteItemCategoryDialog
    {
        private readonly OnCloseDeleteItemCategoryDialogFixture _fixture;

        public OnCloseDeleteItemCategoryDialog()
        {
            _fixture = new OnCloseDeleteItemCategoryDialogFixture();
        }

        [Fact]
        public void OnCloseDeleteItemCategoryDialog_WithDialogOpen_ShouldCloseDialog()
        {
            // Arrange
            _fixture.SetupInitialStateWithDialogOpen();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemCategoryEditorReducer.OnCloseDeleteItemCategoryDialog(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCloseDeleteItemCategoryDialog_WithDialogClosed_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateWithDialogNotOpen();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemCategoryEditorReducer.OnCloseDeleteItemCategoryDialog(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnCloseDeleteItemCategoryDialogFixture : ItemCategoryEditorReducerFixture
        {
            public void SetupInitialStateWithDialogOpen()
            {
                SetupInitialState(true);
            }

            public void SetupInitialStateWithDialogNotOpen()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isDeleteDialogOpen)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleteDialogOpen = isDeleteDialogOpen
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleteDialogOpen = false
                    }
                };
            }
        }
    }

    private abstract class ItemCategoryEditorReducerFixture
    {
        public ItemCategoryState ExpectedState { get; protected set; } = new DomainTestBuilder<ItemCategoryState>().Create();
        public ItemCategoryState InitialState { get; protected set; } = new DomainTestBuilder<ItemCategoryState>().Create();
    }
}