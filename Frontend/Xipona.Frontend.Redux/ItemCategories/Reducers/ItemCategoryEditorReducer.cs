using Fluxor;
using Xipona.Frontend.Redux.ItemCategories.Actions;
using Xipona.Frontend.Redux.ItemCategories.States;

namespace Xipona.Frontend.Redux.ItemCategories.Reducers;

public static class ItemCategoryEditorReducer
{
    [ReducerMethod(typeof(LoadItemCategoryForEditingStartedAction))]
    public static ItemCategoryState OnLoadItemCategoryForEditingStarted(ItemCategoryState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                ReadOnlyItemCategory = null,
                ItemCategory = null,
                IsInEditMode = false,
                IsLoadingEditedItemCategory = true,
                IsDeleteDialogOpen = false
            }
        };
    }

    [ReducerMethod]
    public static ItemCategoryState OnLoadItemCategoryForEditingFinished(ItemCategoryState state,
        LoadItemCategoryForEditingFinishedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsLoadingEditedItemCategory = false,
                ReadOnlyItemCategory = action.ItemCategory,
                ItemCategory = action.ItemCategory,
                IsInEditMode = false
            }
        };
    }

    [ReducerMethod(typeof(EnterItemCategoryEditModeAction))]
    public static ItemCategoryState OnEnterItemCategoryEditMode(ItemCategoryState state)
    {
        if (state.Editor.ReadOnlyItemCategory is null)
            return state;

        return state with
        {
            Editor = state.Editor with
            {
                ItemCategory = state.Editor.ReadOnlyItemCategory,
                IsInEditMode = true
            }
        };
    }

    [ReducerMethod(typeof(CancelItemCategoryEditingAction))]
    public static ItemCategoryState OnCancelItemCategoryEditing(ItemCategoryState state)
    {
        if (state.Editor.ReadOnlyItemCategory is null)
            return state;

        return state with
        {
            Editor = state.Editor with
            {
                ItemCategory = state.Editor.ReadOnlyItemCategory,
                IsInEditMode = false
            }
        };
    }

    [ReducerMethod]
    public static ItemCategoryState OnEditedItemCategoryNameChanged(ItemCategoryState state,
        EditedItemCategoryNameChangedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                ItemCategory = state.Editor.ItemCategory! with
                {
                    Name = action.Name
                }
            }
        };
    }

    [ReducerMethod(typeof(LeaveItemCategoryEditorAction))]
    public static ItemCategoryState OnLeaveItemCategoryEditor(ItemCategoryState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                ReadOnlyItemCategory = null,
                ItemCategory = null,
                IsInEditMode = false,
                IsLoadingEditedItemCategory = false,
                IsDeleteDialogOpen = false
            }
        };
    }

    [ReducerMethod(typeof(SaveItemCategoryStartedAction))]
    public static ItemCategoryState OnSavingItemCategoryStarted(ItemCategoryState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsSaving = true
            }
        };
    }

    [ReducerMethod(typeof(SaveItemCategoryFinishedAction))]
    public static ItemCategoryState OnSavingItemCategoryFinished(ItemCategoryState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsSaving = false
            }
        };
    }

    [ReducerMethod(typeof(DeleteItemCategoryStartedAction))]
    public static ItemCategoryState OnDeletingItemCategoryStarted(ItemCategoryState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsDeleting = true
            }
        };
    }

    [ReducerMethod(typeof(DeleteItemCategoryFinishedAction))]
    public static ItemCategoryState OnDeletingItemCategoryFinished(ItemCategoryState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsDeleting = false
            }
        };
    }

    [ReducerMethod(typeof(SetNewItemCategoryAction))]
    public static ItemCategoryState OnSetNewItemCategory(ItemCategoryState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                ReadOnlyItemCategory = null,
                ItemCategory = new EditedItemCategory(Guid.Empty, ""),
                IsInEditMode = true,
                IsLoadingEditedItemCategory = false,
                IsDeleteDialogOpen = false
            }
        };
    }

    [ReducerMethod]
    public static ItemCategoryState OnUpdateSearchResultsAfterSave(ItemCategoryState state,
        UpdateItemCategorySearchResultsAfterSaveAction action)
    {
        var manufacturer = state.Search.SearchResults.FirstOrDefault(r => r.Id == action.ItemCategoryId);
        if (manufacturer == null)
            return state;

        var index = state.Search.SearchResults.IndexOf(manufacturer);
        var results = new List<ItemCategorySearchResult>(state.Search.SearchResults)
        {
            [index] = new(action.ItemCategoryId, action.Name)
        };

        return state with
        {
            Search = state.Search with
            {
                SearchResults = results
            }
        };
    }

    [ReducerMethod(typeof(OpenDeleteItemCategoryDialogAction))]
    public static ItemCategoryState OnOpenDeleteItemCategoryDialog(ItemCategoryState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsDeleteDialogOpen = true
            }
        };
    }

    [ReducerMethod(typeof(CloseDeleteItemCategoryDialogAction))]
    public static ItemCategoryState OnCloseDeleteItemCategoryDialog(ItemCategoryState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsDeleteDialogOpen = false
            }
        };
    }
}