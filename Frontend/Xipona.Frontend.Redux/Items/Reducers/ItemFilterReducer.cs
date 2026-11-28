using Fluxor;
using Xipona.Frontend.Redux.Items.Actions.Filter;
using Xipona.Frontend.Redux.Items.States;

namespace Xipona.Frontend.Redux.Items.Reducers;

public static class ItemFilterReducer
{
    [ReducerMethod]
    public static ItemState OnSelectedStoreChanged(ItemState state, SelectedStoreChangedAction action)
    {
        return state with
        {
            Search = state.Search with
            {
                Filter = state.Search.Filter with
                {
                    SelectedStore = action.Store,
                    IsLoadButtonActive = true
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnLoadFilteredItemsFinishedAction(ItemState state, LoadFilteredItemsFinishedAction action)
    {
        return state with
        {
            Search = state.Search with
            {
                SearchResults = action.Items,
                TriggeredAtLeastOnce = true
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnSearchItemCategoriesFinishedAction(ItemState state, SearchItemCategoriesFinishedAction action)
    {
        return state with
        {
            Search = state.Search with
            {
                Filter = state.Search.Filter with
                {
                    ItemCategoryFilter = state.Search.Filter.ItemCategoryFilter with
                    {
                        ItemCategories = action.ItemCategories
                    }
                }
            }
        };
    }
    
    [ReducerMethod]
    public static ItemState OnSelectedItemCategoriesChangedAction(ItemState state,
        SelectedItemCategoryChangedAction action)
    {
        return state with
        {
            Search = state.Search with
            {
                Filter = state.Search.Filter with
                {
                    ItemCategoryFilter = state.Search.Filter.ItemCategoryFilter with
                    {
                        SelectedItemCategory = action.ItemCategory
                    }
                }
            }
        };
    }
    
    [ReducerMethod]
    public static ItemState OnItemCategoryInputChanged(ItemState state, ItemCategoryInputChangedAction action)
    {
        return state with
        {
            Search = state.Search with
            {
                Filter = state.Search.Filter with
                {
                    ItemCategoryFilter = state.Search.Filter.ItemCategoryFilter with
                    {
                        Input = action.Input
                    }
                }
            }
        };
    }

    [ReducerMethod(typeof(ItemCategoryDropdownClosedAction))]
    public static ItemState OnItemCategoryDropdownClosed(ItemState state)
    {
        var selectedItemCategory = state.Search.Filter.ItemCategoryFilter.SelectedItemCategory;
        return state with
        {
            Search = state.Search with
            {
                Filter = state.Search.Filter with
                {
                    ItemCategoryFilter = state.Search.Filter.ItemCategoryFilter with
                    {
                        Input = string.Empty,
                        ItemCategories = selectedItemCategory is null ? [] : [selectedItemCategory]
                    }
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnSearchManufacturersFinishedAction(ItemState state, SearchManufacturersFinishedAction action)
    {
        return state with
        {
            Search = state.Search with
            {
                Filter = state.Search.Filter with
                {
                    ManufacturerFilter = state.Search.Filter.ManufacturerFilter with
                    {
                        Manufacturers = action.Manufacturers
                    }
                }
            }
        };
    }
    
    [ReducerMethod]
    public static ItemState OnSelectedManufacturersChangedAction(ItemState state,
        SelectedManufacturerChangedAction action)
    {
        return state with
        {
            Search = state.Search with
            {
                Filter = state.Search.Filter with
                {
                    ManufacturerFilter = state.Search.Filter.ManufacturerFilter with
                    {
                        SelectedManufacturer = action.Manufacturer
                    }
                }
            }
        };
    }
    
    [ReducerMethod]
    public static ItemState OnManufacturerInputChanged(ItemState state, ManufacturerInputChangedAction action)
    {
        return state with
        {
            Search = state.Search with
            {
                Filter = state.Search.Filter with
                {
                    ManufacturerFilter = state.Search.Filter.ManufacturerFilter with
                    {
                        Input = action.Input
                    }
                }
            }
        };
    }

    [ReducerMethod(typeof(ManufacturerDropdownClosedAction))]
    public static ItemState OnManufacturerDropdownClosed(ItemState state)
    {
        var selectedManufacturer = state.Search.Filter.ManufacturerFilter.SelectedManufacturer;
        return state with
        {
            Search = state.Search with
            {
                Filter = state.Search.Filter with
                {
                    ManufacturerFilter = state.Search.Filter.ManufacturerFilter with
                    {
                        Input = string.Empty,
                        Manufacturers = selectedManufacturer is null ? [] : [selectedManufacturer]
                    }
                }
            }
        };
    }
}