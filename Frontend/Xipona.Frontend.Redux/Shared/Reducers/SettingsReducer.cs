using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions.Settings;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Shared.Reducers;

public static class SettingsReducer
{
    [ReducerMethod(typeof(CloseSettingsAction))]
    public static SharedState OnCloseSettings(SharedState state)
    {
        return state with
        {
            Settings = state.Settings with
            {
                SettingsOpen = false,
                Editor = null
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
                Editor = new SettingsEditor(state.Settings.GeneralSettings!, action.AllCurrencies)
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
                Editor = state.Settings.Editor! with
                {
                    GeneralSettings = state.Settings.GeneralSettings! with
                    {
                        Currency = action.Currency
                    }
                },
            }
        };
    }

    [ReducerMethod(typeof(SaveSettingsStartedAction))]
    public static SharedState OnSaveSettingsStarted(SharedState state)
    {
        return state with
        {
            Settings = state.Settings with
            {
                IsSaving = true
            }
        };
    }

    [ReducerMethod(typeof(SaveSettingsFinishedAction))]
    public static SharedState OnSaveSettingsFinished(SharedState state)
    {
        return state with
        {
            Settings = state.Settings with
            {
                IsSaving = false,
                GeneralSettings = state.Settings.Editor!.GeneralSettings,
                Editor = null
            }
        };
    }

    [ReducerMethod]
    public static SharedState OnGeneralSettingsLoaded(SharedState state, GeneralSettingsLoadedAction action)
    {
        return state with
        {
            Settings = state.Settings with
            {
                GeneralSettings = action.GeneralSettings
            }
        };
    }
}
