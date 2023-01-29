using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;
using ShoppingList.Frontend.Redux.Recipes.States;
using ShoppingList.Frontend.Redux.Shared.Constants;
using ShoppingList.Frontend.Redux.Shared.Ports;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Effects;

public sealed class RecipeEditorEffects
{
    private readonly IApiClient _client;
    private readonly IState<RecipeState> _state;
    private readonly NavigationManager _navigationManager;

    public RecipeEditorEffects(IApiClient client, IState<RecipeState> state, NavigationManager navigationManager)
    {
        _client = client;
        _state = state;
        _navigationManager = navigationManager;
    }

    [EffectMethod]
    public async Task HandleLoadRecipeForEditing(LoadRecipeForEditingAction action, IDispatcher dispatcher)
    {
        var result = await _client.GetRecipeByIdAsync(action.RecipeId);
        dispatcher.Dispatch(new LoadRecipeForEditingFinishedAction(result));
    }

    [EffectMethod(typeof(LeaveRecipeEditorAction))]
    public Task HandleLeaveRecipeEditor(IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo(PageRoutes.Recipes);
        return Task.CompletedTask;
    }

    [EffectMethod(typeof(ModifyRecipeAction))]
    public async Task HandleModifyRecipeAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new ModifyRecipeStartedAction());
        await _client.ModifyRecipeAsync(_state.Value.Editor.Recipe!);
        dispatcher.Dispatch(new ModifyRecipeFinishedAction());
    }
}