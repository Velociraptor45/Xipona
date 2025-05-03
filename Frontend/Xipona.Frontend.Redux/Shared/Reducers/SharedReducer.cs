using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions.Settings;
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

    [ReducerMethod]
    public static SharedState OnUserLoggedIn(SharedState state, UserLoggedInAction action)
    {
        return state with
        {
            User = action.User
        };
    }

    [ReducerMethod(typeof(CloseSettingsAction))]
    public static SharedState OnCloseSettings(SharedState state)
    {
        return state with
        {
            Settings = state.Settings with
            {
                SettingsOpen = false
            }
        };
    }

    [ReducerMethod(typeof(OpenSettingsAction))]
    public static SharedState OnOpenSettings(SharedState state)
    {
        return state with
        {
            Settings = state.Settings with
            {
                SettingsOpen = true
            }
        };
    }

    [ReducerMethod]
    public static SharedState OnSettingsLoaded(SharedState state, SettingsLoadedAction action)
    {
        return state with
        {
            Settings = state.Settings with
            {
                GeneralSettings = action.GeneralSettings
            }
        };
    }

    [ReducerMethod]
    public static SharedState OnSelectedCurrencyChanged(SharedState state, SelectedCurrencyChangedAction action)
    {
        return state with
        {
            Settings = state.Settings with
            {
                GeneralSettings = state.Settings.GeneralSettings with
                {
                    Currency = action.Currency
                }
            }
        };
    }
}