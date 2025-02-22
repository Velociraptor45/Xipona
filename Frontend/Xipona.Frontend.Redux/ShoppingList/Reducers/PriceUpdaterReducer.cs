using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.PriceUpdater;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

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
                IsSaving = false,
                OtherItemTypePrices = []
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

    [ReducerMethod(typeof(SavePriceUpdateFinishedAction))]
    public static ShoppingListState OnSavePriceUpdateFinished(ShoppingListState state)
    {
        return state with
        {
            PriceUpdate = state.PriceUpdate with
            {
                IsSaving = false
            }
        };
    }
}