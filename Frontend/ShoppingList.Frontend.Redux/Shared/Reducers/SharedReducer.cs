using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.Processing;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.Reducers;

public static class SharedReducer
{
    [ReducerMethod]
    public static SharedState OnApplicationInitialized(SharedState state, ApplicationInitializedAction action)
    {
        return state with { IsMobile = action.IsMobile };
    }

    [ReducerMethod(typeof(ApiConnectionDiedAction))]
    public static SharedState OnApiConnectionDied(SharedState state)
    {
        return state with
        {
            IsOnline = false
        };
    }

    [ReducerMethod(typeof(ApiConnectionRecoveredAction))]
    public static SharedState OnApiConnectionRecovered(SharedState state)
    {
        return state with
        {
            IsOnline = true
        };
    }
}