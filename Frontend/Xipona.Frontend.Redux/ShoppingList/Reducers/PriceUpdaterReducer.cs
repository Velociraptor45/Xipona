using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.PriceUpdater;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States.Comparer;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Reducers;

public static class PriceUpdaterReducer
{
    [ReducerMethod]
    public static ShoppingListState OnOpenPriceUpdater(ShoppingListState state, OpenPriceUpdaterAction action)
    {
        return state with
        {
            PriceUpdate = state.PriceUpdate with
            {
                Item = action.Item,
                Price = action.Item.PricePerQuantity,
                UpdatePriceForAllTypes = true,
                IsOpen = true,
                IsSaving = false
            }
        };
    }

    [ReducerMethod(typeof(ClosePriceUpdaterAction))]
    public static ShoppingListState OnClosePriceUpdater(ShoppingListState state)
    {
        return state with
        {
            PriceUpdate = state.PriceUpdate with
            {
                Item = null,
                IsOpen = false,
                IsSaving = false
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnPriceOnPriceUpdaterChanged(ShoppingListState state,
        PriceOnPriceUpdaterChangedAction action)
    {
        return state with
        {
            PriceUpdate = state.PriceUpdate with
            {
                Price = action.Price
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnUpdatePriceForAllTypesOnPriceUpdaterChanged(ShoppingListState state,
        UpdatePriceForAllTypesOnPriceUpdaterChangedAction action)
    {
        return state with
        {
            PriceUpdate = state.PriceUpdate with
            {
                UpdatePriceForAllTypes = action.UpdatePriceForAllTypes
            }
        };
    }

    [ReducerMethod(typeof(SavePriceUpdateStartedAction))]
    public static ShoppingListState OnSavePriceUpdateStarted(ShoppingListState state)
    {
        return state with
        {
            PriceUpdate = state.PriceUpdate with
            {
                IsSaving = true
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnSavePriceUpdateFinished(ShoppingListState state,
        SavePriceUpdateFinishedAction action)
    {
        var sections = state.ShoppingList!.Sections.ToList();
        for (int i = 0; i < sections.Count; i++)
        {
            var section = sections[i];
            var items = section.Items.ToList();
            for (int ii = 0; ii < items.Count; ii++)
            {
                var item = items[ii];
                if (item.Id.ActualId == action.ItemId && (action.ItemTypeId is null || item.TypeId == action.ItemTypeId))
                {
                    items[ii] = item with
                    {
                        PricePerQuantity = action.Price
                    };
                }
            }

            sections[i] = section with
            {
                Items = items
            };
        }

        return state with
        {
            ShoppingList = state.ShoppingList with
            {
                Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
            },
            PriceUpdate = state.PriceUpdate with
            {
                IsSaving = false
            }
        };
    }
}