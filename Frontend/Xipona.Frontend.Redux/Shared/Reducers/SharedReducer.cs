using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Processing;

namespace ProjectHermes.Xipona.Frontend.Redux.Shared.Reducers;

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
            IsOnline = false,
            IsRetryOngoing = true
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

    [ReducerMethod(typeof(QueueProcessedAction))]
    public static SharedState OnQueueProcessed(SharedState state)
    {
        return state with
        {
            IsRetryOngoing = false
        };
    }
}