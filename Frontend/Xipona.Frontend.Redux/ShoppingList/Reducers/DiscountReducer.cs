using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Discounts;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Reducers;
public static class DiscountReducer
{
    [ReducerMethod]
    public static ShoppingListState OnOpenDiscountDialog(ShoppingListState state, OpenDiscountDialogAction action)
    {
        return state with
        {
            DiscountDialog = state.DiscountDialog with
            {
                Item = action.Item,
                Discount = action.Item.PricePerQuantity,
                IsOpen = true,
                IsSaving = false
            }
        };
    }

    [ReducerMethod(typeof(SaveDiscountStartedAction))]
    public static ShoppingListState OnSaveDiscountStarted(ShoppingListState state)
    {
        return state with
        {
            DiscountDialog = state.DiscountDialog with
            {
                IsSaving = true
            }
        };
    }

    [ReducerMethod(typeof(SaveDiscountFinishedAction))]
    public static ShoppingListState OnSaveDiscountFinished(ShoppingListState state)
    {
        return state with
        {
            DiscountDialog = state.DiscountDialog with
            {
                IsSaving = false
            }
        };
    }

    [ReducerMethod(typeof(CloseDiscountDialogAction))]
    public static ShoppingListState OnCloseDiscountDialog(ShoppingListState state)
    {
        return state with
        {
            DiscountDialog = state.DiscountDialog with
            {
                Item = null,
                Discount = 0m,
                IsOpen = false,
                IsSaving = false
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnDiscountChanged(ShoppingListState state, DiscountChangedAction action)
    {
        return state with
        {
            DiscountDialog = state.DiscountDialog with
            {
                Discount = action.NewDiscount
            }
        };
    }
}
