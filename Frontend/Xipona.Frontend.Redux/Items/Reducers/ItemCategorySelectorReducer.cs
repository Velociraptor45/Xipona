using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;
using ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Editor.ItemCategorySelectors;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Items.Reducers;

public static class ItemCategorySelectorReducer
{
    [ReducerMethod]
    public static ItemState OnLoadInitialItemCategoryFinished(ItemState state,
        LoadInitialItemCategoryFinishedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                ItemCategorySelector = state.Editor.ItemCategorySelector with
                {
                    ItemCategories = new List<ItemCategorySearchResult> { action.ItemCategory }
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnCreateNewItemCategoryFinished(ItemState state,
        CreateNewItemCategoryFinishedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item! with
                {
                    ItemCategoryId = action.ItemCategory.Id
                },
                ItemCategorySelector = state.Editor.ItemCategorySelector with
                {
                    ItemCategories = new List<ItemCategorySearchResult> { action.ItemCategory },
                    Input = string.Empty
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnItemCategoryInputChanged(ItemState state, ItemCategoryInputChangedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                ItemCategorySelector = state.Editor.ItemCategorySelector with
                {
                    Input = action.Input
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnSearchItemCategoryFinished(ItemState state, SearchItemCategoryFinishedAction action)
    {
        var results = action.SearchResults
            .OrderBy(r => r.Name)
            .ToList();

        var currentlySelected = state.Editor.ItemCategorySelector.ItemCategories
                .FirstOrDefault(cat => cat.Id == state.Editor.Item!.ItemCategoryId);

        if (currentlySelected != null && results.All(r => r.Id != currentlySelected.Id))
            results.Insert(0, currentlySelected);

        return state with
        {
            Editor = state.Editor with
            {
                ItemCategorySelector = state.Editor.ItemCategorySelector with
                {
                    ItemCategories = results
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnSelectedItemCategoryChanged(ItemState state, SelectedItemCategoryChangedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item! with
                {
                    ItemCategoryId = action.ItemCategoryId
                },
                ValidationResult = state.Editor.ValidationResult with
                {
                    ItemCategory = null
                }
            }
        };
    }
}