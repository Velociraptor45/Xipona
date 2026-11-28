using Fluxor;
using Xipona.Frontend.Redux.Shared.Actions;
using Xipona.Frontend.Redux.Shared.States;
using Xipona.Frontend.Redux.ShoppingList.Actions.Processing;

namespace Xipona.Frontend.Redux.Shared.Reducers;

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

    [ReducerMethod]
    public static SharedState OnUserLoggedIn(SharedState state, UserLoggedInAction action)
    {
        return state with
        {
            User = action.User
        };
    }
}