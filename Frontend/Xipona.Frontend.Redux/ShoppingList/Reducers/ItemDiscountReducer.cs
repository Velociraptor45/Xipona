using Fluxor;
using Xipona.Frontend.Redux.ShoppingList.Actions.ItemDiscounts;
using Xipona.Frontend.Redux.ShoppingList.States;

namespace Xipona.Frontend.Redux.ShoppingList.Reducers;
public static class ItemDiscountReducer
{
    [ReducerMethod]
    public static ShoppingListState OnOpenDiscountDialog(ShoppingListState state, OpenDiscountDialogAction action)
    {
        return state with
        {
            ItemDiscountDialog = state.ItemDiscountDialog with
            {
                Item = action.Item,
                Discount = action.Item.PricePerQuantity,
                IsOpen = true,
                IsSaving = false,
                IsRemoving = false
            }
        };
    }

    [ReducerMethod(typeof(SaveDiscountStartedAction))]
    public static ShoppingListState OnSaveDiscountStarted(ShoppingListState state)
    {
        return state with
        {
            ItemDiscountDialog = state.ItemDiscountDialog with
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
            ItemDiscountDialog = state.ItemDiscountDialog with
            {
                IsSaving = false
            }
        };
    }

    [ReducerMethod(typeof(RemoveDiscountStartedAction))]
    public static ShoppingListState OnRemoveDiscountStarted(ShoppingListState state)
    {
        return state with
        {
            ItemDiscountDialog = state.ItemDiscountDialog with
            {
                IsRemoving = true
            }
        };
    }

    [ReducerMethod(typeof(RemoveDiscountFinishedAction))]
    public static ShoppingListState OnRemoveDiscountFinished(ShoppingListState state)
    {
        return state with
        {
            ItemDiscountDialog = state.ItemDiscountDialog with
            {
                IsRemoving = false
            }
        };
    }

    [ReducerMethod(typeof(CloseDiscountDialogAction))]
    public static ShoppingListState OnCloseDiscountDialog(ShoppingListState state)
    {
        return state with
        {
            ItemDiscountDialog = state.ItemDiscountDialog with
            {
                Item = null,
                IsOpen = false,
                IsSaving = false,
                IsRemoving = false
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnDiscountChanged(ShoppingListState state, DiscountChangedAction action)
    {
        return state with
        {
            ItemDiscountDialog = state.ItemDiscountDialog with
            {
                Discount = action.NewDiscount
            }
        };
    }
}
