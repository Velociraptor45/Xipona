using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.ShoppingListDiscounts;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Reducers;

public static class ShoppingListDiscountReducer
{
    [ReducerMethod(typeof(OpenDiscountDialogAction))]
    public static ShoppingListState OnOpenDiscountDialog(ShoppingListState state)
    {
        return state with
        {
            ShoppingListDiscountDialog = ShoppingListDiscountDialog.Default() with
            {
                IsOpen = true
            }
        };
    }

    [ReducerMethod(typeof(CloseDiscountDialogAction))]
    public static ShoppingListState OnCloseDiscountDialog(ShoppingListState state)
    {
        return state with
        {
            ShoppingListDiscountDialog = state.ShoppingListDiscountDialog with
            {
                IsOpen = false
            }
        };
    }

    [ReducerMethod(typeof(SaveDiscountStartedAction))]
    public static ShoppingListState OnSaveDiscountStarted(ShoppingListState state)
    {
        return state with
        {
            ShoppingListDiscountDialog = state.ShoppingListDiscountDialog with
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

    [ReducerMethod]
    public static ShoppingListState OnRemoveDiscountStarted(ShoppingListState state, RemoveDiscountStartedAction action)
    {
        return SetDiscountDeletingState(state, action.DiscountId, true);
    }

    [ReducerMethod]
    public static ShoppingListState OnRemoveDiscountFinished(ShoppingListState state, RemoveDiscountFinishedAction action)
    {
        if (state.ShoppingList is null)
            return state;

        var discounts = state.ShoppingList.Discounts.Where(d => d.Id != action.DiscountId).ToList();

        return state with
        {
            ShoppingList = state.ShoppingList with
            {
                Discounts = discounts
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnRemoveDiscountFailed(ShoppingListState state, RemoveDiscountFailedAction action)
    {
        return SetDiscountDeletingState(state, action.DiscountId, false);
    }

    private static ShoppingListState SetDiscountDeletingState(ShoppingListState state, Guid discountId, bool isDeleting)
    {
        if (state.ShoppingList is null)
            return state;

        var discounts = state.ShoppingList.Discounts.ToList();
        var idx = discounts.FindIndex(d => d.Id == discountId);
        if (idx < 0)
            return state;

        discounts[idx] = discounts[idx] with
        {
            IsDeleting = isDeleting
        };

        return state with
        {
            ShoppingList = state.ShoppingList with
            {
                Discounts = discounts
            }
        };

    }
}
