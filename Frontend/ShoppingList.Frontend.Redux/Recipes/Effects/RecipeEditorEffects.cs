using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Constants;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using RestEase;

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
        EditedRecipe result;
        try
        {
            result = await _client.GetRecipeByIdAsync(action.RecipeId);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading recipe failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Loading recipe failed", e.Message));
            return;
        }
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
        try
        {
            await _client.ModifyRecipeAsync(_state.Value.Editor.Recipe!);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Modifying recipe failed", e));
            dispatcher.Dispatch(new ModifyRecipeFinishedAction());
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Modifying recipe failed", e.Message));
            dispatcher.Dispatch(new ModifyRecipeFinishedAction());
            return;
        }

        dispatcher.Dispatch(new ModifyRecipeFinishedAction());
        dispatcher.Dispatch(new LeaveRecipeEditorAction());
    }

    [EffectMethod(typeof(CreateRecipeAction))]
    public async Task HandleCreateRecipeAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new CreateRecipeStartedAction());
        try
        {
            await _client.CreateRecipeAsync(_state.Value.Editor.Recipe!);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Creating recipe failed", e));
            dispatcher.Dispatch(new CreateRecipeFinishedAction());
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Creating recipe failed", e.Message));
            dispatcher.Dispatch(new CreateRecipeFinishedAction());
            return;
        }

        dispatcher.Dispatch(new CreateRecipeFinishedAction());
        dispatcher.Dispatch(new LeaveRecipeEditorAction());
    }

    [EffectMethod(typeof(CreateNewRecipeTagAction))]
    public async Task HandleCreateNewRecipeTagAction(IDispatcher dispatcher)
    {
        if (string.IsNullOrWhiteSpace(_state.Value.Editor.RecipeTagCreateInput))
            return;

        RecipeTag newTag;
        try
        {
            newTag = await _client.CreateRecipeTagAsync(_state.Value.Editor.RecipeTagCreateInput);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Creating recipe tag failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Creating recipe tag failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new CreateNewRecipeTagFinishedAction(newTag));
    }

    [EffectMethod(typeof(LoadAddToShoppingListAction))]
    public async Task HandleLoadAddToShoppingListAction(IDispatcher dispatcher)
    {
        if (_state.Value.Editor.Recipe is null)
            return;

        IEnumerable<AddToShoppingListIngredient> results;
        try
        {
            results = await _client.GetItemAmountsForOneServingAsync(_state.Value.Editor.Recipe.Id);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading item amounts for one serving failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Loading item amounts for one serving failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new LoadAddToShoppingListFinishedAction(results.ToList()));
    }
}