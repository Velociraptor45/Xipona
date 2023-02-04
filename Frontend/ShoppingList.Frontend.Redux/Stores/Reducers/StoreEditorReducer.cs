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
}