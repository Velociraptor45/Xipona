using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Summary;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Reducers;

public static class ShoppingListSummaryReducer
{
    [ReducerMethod(typeof(OpenSummaryAction))]
    public static ShoppingListState OnOpenSummary(ShoppingListState state)
    {
        return state with
        {
            Summary = state.Summary with
            {
                IsOpen = true,
                IsSaving = false,
                FinishedAt = DateTime.Now
            }
        };
    }

    [ReducerMethod(typeof(CloseSummaryAction))]
    public static ShoppingListState OnCloseSummary(ShoppingListState state)
    {
        return state with
        {
            Summary = state.Summary with
            {
                IsOpen = false
            }
        };
    }

    [ReducerMethod(typeof(FinishShoppingListStartedAction))]
    public static ShoppingListState OnFinishShoppingListStarted(ShoppingListState state)
    {
        return state with
        {
            Summary = state.Summary with
            {
                IsSaving = true
            }
        };
    }

    [ReducerMethod(typeof(FinishShoppingListFinishedAction))]
    public static ShoppingListState OnFinishShoppingListFinished(ShoppingListState state)
    {
        return state with
        {
            Summary = state.Summary with
            {
                IsSaving = false,
                IsOpen = false
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnFinishedAtOnSummaryChanged(ShoppingListState state,
        FinishedAtOnSummaryChangedAction action)
    {
        return state with
        {
            Summary = state.Summary with
            {
                FinishedAt = action.FinishedAt
            }
        };
    }

    [ReducerMethod(typeof(EnterFinishedAtEditModeAction))]
    public static ShoppingListState OnEnterFinishedAtEditMode(ShoppingListState state)
    {
        return state with
        {
            Summary = state.Summary with
            {
                IsEditingFinishedAt = true
            }
        };
    }

    [ReducerMethod(typeof(LeaveFinishedAtEditModeAction))]
    public static ShoppingListState OnLeaveFinishedAtEditMode(ShoppingListState state)
    {
        return state with
        {
            Summary = state.Summary with
            {
                IsEditingFinishedAt = false
            }
        };
    }
}