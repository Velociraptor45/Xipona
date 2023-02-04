using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Constants;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;

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

        dispatcher.Dispatch(new LeaveRecipeEditorAction());
    }

    [EffectMethod(typeof(CreateRecipeAction))]
    public async Task HandleCreateRecipeAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new CreateRecipeStartedAction());
        await _client.CreateRecipeAsync(_state.Value.Editor.Recipe!);
        dispatcher.Dispatch(new CreateRecipeFinishedAction());

        dispatcher.Dispatch(new LeaveRecipeEditorAction());
    }
}