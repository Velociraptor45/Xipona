using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Processing;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Reducers;

public static class ShoppingListProcessingReducer
{
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