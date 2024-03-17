using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Editor.ManufacturerSelectors;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using ProjectHermes.Xipona.Frontend.Redux.Manufacturers.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Items.Reducers;

public static class ManufacturerSelectorReducer
{
    [ReducerMethod]
    public static ItemState OnLoadInitialManufacturerFinished(ItemState state,
        LoadInitialManufacturerFinishedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                ManufacturerSelector = state.Editor.ManufacturerSelector with
                {
                    Manufacturers = new List<ManufacturerSearchResult> { action.Manufacturer }
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnCreateNewManufacturerFinished(ItemState state,
        CreateNewManufacturerFinishedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item! with
                {
                    ManufacturerId = action.Manufacturer.Id
                },
                ManufacturerSelector = state.Editor.ManufacturerSelector with
                {
                    Manufacturers = new List<ManufacturerSearchResult> { action.Manufacturer }
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnManufacturerInputChanged(ItemState state, ManufacturerInputChangedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                ManufacturerSelector = state.Editor.ManufacturerSelector with
                {
                    Input = action.Input
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnSearchManufacturerFinished(ItemState state, SearchManufacturerFinishedAction action)
    {
        var results = action.SearchResults
            .OrderBy(r => r.Name)
            .ToList();

        var currentlySelected = state.Editor.ManufacturerSelector.Manufacturers
                .FirstOrDefault(cat => cat.Id == state.Editor.Item!.ManufacturerId);

        if (currentlySelected != null && results.All(r => r.Id != currentlySelected.Id))
            results.Insert(0, currentlySelected);

        return state with
        {
            Editor = state.Editor with
            {
                ManufacturerSelector = state.Editor.ManufacturerSelector with
                {
                    Manufacturers = results
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnSelectedManufacturerChanged(ItemState state, SelectedManufacturerChangedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item! with
                {
                    ManufacturerId = action.ManufacturerId
                }
            }
        };
    }

    [ReducerMethod(typeof(ClearManufacturerAction))]
    public static ItemState OnClearManufacturer(ItemState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item! with
                {
                    ManufacturerId = null
                }
            }
        };
    }
}