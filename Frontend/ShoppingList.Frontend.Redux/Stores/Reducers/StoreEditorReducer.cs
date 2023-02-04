using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Stores.Reducers;

public static class StoreEditorReducer
{
    [ReducerMethod(typeof(SetNewStoreAction))]
    public static StoreState OnSetNewStore(StoreState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Store = new EditedStore(
                    Guid.Empty,
                    string.Empty,
                    new List<EditedSection>(0))
            }
        };
    }

    [ReducerMethod]
    public static StoreState OnLoadStoreForEditingFinished(StoreState state, LoadStoreForEditingFinishedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Store = action.Store
            }
        };
    }

    [ReducerMethod]
    public static StoreState OnStoreNameChanged(StoreState state, StoreNameChangedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Store = state.Editor.Store with
                {
                    Name = action.Name
                }
            }
        };
    }

    [ReducerMethod(typeof(SaveStoreStartedAction))]
    public static StoreState OnSaveStoreStarted(StoreState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsSaving = true
            }
        };
    }

    [ReducerMethod(typeof(SaveStoreFinishedAction))]
    public static StoreState OnSaveStoreFinished(StoreState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsSaving = false
            }
        };
    }
}