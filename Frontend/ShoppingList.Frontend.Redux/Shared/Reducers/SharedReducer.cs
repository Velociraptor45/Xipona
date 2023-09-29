using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.Reducers;

public static class SharedReducer
{
    [ReducerMethod]
    public static SharedState OnApplicationInitialized(SharedState state, ApplicationInitializedAction action)
    {
        return state with { IsMobile = action.IsMobile };
    }
}