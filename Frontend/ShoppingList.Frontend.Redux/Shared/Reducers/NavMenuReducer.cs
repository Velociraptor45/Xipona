using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.Reducers;

public static class NavMenuReducer
{
    [ReducerMethod(typeof(ToggleMobileNavMenuExpansionAction))]
    public static SharedState OnToggleMobileNavMenuExpansion(SharedState state)
    {
        return state with
        {
            IsMobileNavMenuExpanded = !state.IsMobileNavMenuExpanded
        };
    }

    [ReducerMethod(typeof(NavMenuItemClickedAction))]
    public static SharedState OnNavMenuItemClicked(SharedState state)
    {
        return state with
        {
            IsMobileNavMenuExpanded = false
        };
    }
}