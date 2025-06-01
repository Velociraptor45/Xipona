using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.ShoppingListDiscounts;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Reducers;

public static class ShoppingListDiscountReducer
{
    [ReducerMethod]
    public static ShoppingListState OnOpenDiscountDialog(ShoppingListState state, OpenDiscountDialogAction action)
    {
        return state with
        {
            ShoppingListDiscountDialog = ShoppingListDiscountDialog.Default() with
            {
                IsOpen = true
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnCloseDiscountDialog(ShoppingListState state, CloseDiscountDialogAction action)
    {
        return state with
        {
            ShoppingListDiscountDialog = state.ShoppingListDiscountDialog with
            {
                IsOpen = false
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnSaveDiscountStarted(ShoppingListState state, SaveDiscountStartedAction action)
    {
        return state with
        {
            ShoppingListDiscountDialog = state.ShoppingListDiscountDialog with
            {
                IsSaving = true
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnSaveDiscountFinished(ShoppingListState state, SaveDiscountFinishedAction action)
    {
        return state with
        {
            ShoppingListDiscountDialog = state.ShoppingListDiscountDialog with
            {
                IsSaving = false
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnDiscountValueChanged(ShoppingListState state, DiscountValueChangedAction action)
    {
        return state with
        {
            ShoppingListDiscountDialog = state.ShoppingListDiscountDialog with
            {
                DiscountValue = action.Discount
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnDiscountTypeChanged(ShoppingListState state, DiscountTypeChangedAction action)
    {
        return state with
        {
            ShoppingListDiscountDialog = state.ShoppingListDiscountDialog with
            {
                Type = action.Type
            }
        };
    }
}
