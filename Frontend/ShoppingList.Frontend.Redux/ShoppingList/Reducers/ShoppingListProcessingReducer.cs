using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.Processing;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Reducers;

public static class ShoppingListProcessingReducer
{
    [ReducerMethod(typeof(ApiRequestProcessingErrorOccurredAction))]
    public static ShoppingListState OnApiRequestProcessingErrorOccurred(ShoppingListState state)
    {
        return state with
        {
            Errors = state.Errors with
            {
                HasErrors = true
            }
        };
    }

    [ReducerMethod(typeof(ApiRequestProcessingErrorResolvedAction))]
    public static ShoppingListState OnApiRequestProcessingErrorResolved(ShoppingListState state)
    {
        return state with
        {
            Errors = state.Errors with
            {
                HasErrors = false
            }
        };
    }

    [ReducerMethod(typeof(ApiConnectionDiedAction))]
    public static ShoppingListState OnApiConnectionDied(ShoppingListState state)
    {
        if (!state.Errors.IsDebug)
            return state;

        var stack = state.Errors.Stack.ToList();
        stack.Add("Connection failed");

        return state with
        {
            Errors = state.Errors with
            {
                Stack = stack
            }
        };
    }

    [ReducerMethod(typeof(QueueProcessedAction))]
    public static ShoppingListState OnQueueProcessed(ShoppingListState state)
    {
        if (!state.Errors.IsDebug)
            return state;

        var stack = state.Errors.Stack.ToList();
        stack.Add("Queue processed");

        return state with
        {
            Errors = state.Errors with
            {
                Stack = stack
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnLog(ShoppingListState state, LogAction action)
    {
        if (!state.Errors.IsDebug)
            return state;

        var stack = state.Errors.Stack.ToList();
        stack.Add(action.Entry);

        return state with
        {
            Errors = state.Errors with
            {
                Stack = stack
            }
        };
    }

    [ReducerMethod(typeof(ToggleDebugAction))]
    public static ShoppingListState OnToggleDebug(ShoppingListState state)
    {
        return state with
        {
            Errors = state.Errors with
            {
                IsDebug = !state.Errors.IsDebug
            }
        };
    }
}