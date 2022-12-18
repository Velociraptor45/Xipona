using Fluxor;
using ShoppingList.Frontend.Redux.Manufacturers.Actions;
using ShoppingList.Frontend.Redux.Manufacturers.States;

namespace ShoppingList.Frontend.Redux.Manufacturers.Reducers;

public static class ManufacturerReducer
{
    [ReducerMethod(typeof(SearchManufacturersStartedAction))]
    public static ManufacturerState OnSearchManufacturersStarted(ManufacturerState state)
    {
        return state with
        {
            Search = state.Search with
            {
                IsLoadingSearchResults = true
            }
        };
    }

    [ReducerMethod]
    public static ManufacturerState OnSearchManufacturersFinished(ManufacturerState state,
        SearchManufacturersFinishedAction action)
    {
        return state with
        {
            Search = state.Search with
            {
                IsLoadingSearchResults = false,
                SearchResults = action.SearchResults
            }
        };
    }

    [ReducerMethod(typeof(LoadManufacturerForEditingStartedAction))]
    public static ManufacturerState OnLoadManufacturerForEditingStarted(ManufacturerState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsLoadingEditedManufacturer = true
            }
        };
    }

    [ReducerMethod]
    public static ManufacturerState OnLoadManufacturerForEditingFinished(ManufacturerState state,
        LoadManufacturerForEditingFinishedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsLoadingEditedManufacturer = false,
                Manufacturer = action.Manufacturer
            }
        };
    }

    [ReducerMethod]
    public static ManufacturerState OnEditedManufacturerNameChanged(ManufacturerState state,
        EditedManufacturerNameChangedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Manufacturer = state.Editor.Manufacturer! with
                {
                    Name = action.Name
                }
            }
        };
    }

    [ReducerMethod(typeof(LeaveManufacturerEditorAction))]
    public static ManufacturerState OnLeaveManufacturerEditor(ManufacturerState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Manufacturer = null
            }
        };
    }

    [ReducerMethod(typeof(SavingManufacturerStartedAction))]
    public static ManufacturerState OnSavingManufacturerStarted(ManufacturerState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsSaving = true
            }
        };
    }

    [ReducerMethod(typeof(SavingManufacturerFinishedAction))]
    public static ManufacturerState OnSavingManufacturerFinished(ManufacturerState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsSaving = false
            }
        };
    }

    [ReducerMethod(typeof(DeletingManufacturerStartedAction))]
    public static ManufacturerState OnDeletingManufacturerStarted(ManufacturerState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsDeleting = true
            }
        };
    }

    [ReducerMethod(typeof(DeletingManufacturerFinishedAction))]
    public static ManufacturerState OnDeletingManufacturerFinished(ManufacturerState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsDeleting = false
            }
        };
    }

    [ReducerMethod(typeof(SetNewManufacturerAction))]
    public static ManufacturerState OnSetNewManufacturer(ManufacturerState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Manufacturer = new EditedManufacturer(Guid.Empty, "")
            }
        };
    }

    [ReducerMethod]
    public static ManufacturerState OnUpdateSearchResultsAfterSave(ManufacturerState state,
        UpdateSearchResultsAfterSaveAction action)
    {
        var manufacturer = state.Search.SearchResults.FirstOrDefault(r => r.Id == action.Id);
        if (manufacturer == null)
            return state;

        var index = state.Search.SearchResults.IndexOf(manufacturer);
        var results = new List<ManufacturerSearchResult>(state.Search.SearchResults);
        results[index] = new ManufacturerSearchResult(action.Id, action.Name);

        return state with
        {
            Search = state.Search with
            {
                SearchResults = results
            }
        };
    }
}