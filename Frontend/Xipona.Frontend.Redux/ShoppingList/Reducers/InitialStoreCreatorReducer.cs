using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.InitialStoreCreator;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Reducers;

public static class InitialStoreCreatorReducer
{
    [ReducerMethod(typeof(NoStoresFoundAction))]
    public static ShoppingListState OnNoStoresFound(ShoppingListState state)
    {
        return state with
        {
            InitialStoreCreator = state.InitialStoreCreator with
            {
                IsOpen = true
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnInitialStoreNameChanged(ShoppingListState state,
        InitialStoreNameChangedAction action)
    {
        return state with
        {
            InitialStoreCreator = state.InitialStoreCreator with
            {
                Name = action.Name
            }
        };
    }

    [ReducerMethod(typeof(CreateInitialStoreStartedAction))]
    public static ShoppingListState OnCreateInitialStoreStarted(ShoppingListState state)
    {
        return state with
        {
            InitialStoreCreator = state.InitialStoreCreator with
            {
                IsSaving = true
            }
        };
    }

    [ReducerMethod(typeof(CreateInitialStoreFinishedAction))]
    public static ShoppingListState OnCreateInitialStoreFinished(ShoppingListState state)
    {
        return state with
        {
            InitialStoreCreator = state.InitialStoreCreator with
            {
                IsOpen = false,
                IsSaving = false,
                Name = string.Empty
            }
        };
    }
}