using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;
using ShoppingList.Frontend.Redux.ItemCategories.States;
using ShoppingList.Frontend.Redux.Recipes.States;
using ShoppingList.Frontend.Redux.Shared.Constants;
using ShoppingList.Frontend.Redux.Shared.Ports;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Effects;

public class RecipeEditorEffects
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

    [EffectMethod(typeof(LoadIngredientQuantityTypesAction))]
    public async Task HandleLoadIngredientQuantityTypesAction(IDispatcher dispatcher)
    {
        var results = await _client.GetAllIngredientQuantityTypes();
        dispatcher.Dispatch(new LoadIngredientQuantityTypesFinishedAction(results.ToList()));
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

    [EffectMethod]
    public async Task HandleLoadInitialItemCategoryAction(LoadInitialItemCategoryAction action, IDispatcher dispatcher)
    {
        var itemCategory = await _client.GetItemCategoryByIdAsync(action.Ingredient.ItemCategoryId);
        var result = new ItemCategorySearchResult(itemCategory.Id, itemCategory.Name);
        dispatcher.Dispatch(new LoadInitialItemCategoryFinishedAction(action.Ingredient.Id, result));
    }

    [EffectMethod]
    public Task HandleLoadInitialItemsAction(LoadInitialItemsAction action, IDispatcher dispatcher)
    {
        if (action.Ingredient.ItemCategoryId != Guid.Empty)
            dispatcher.Dispatch(
                new LoadItemsForItemCategoryAction(action.Ingredient, action.Ingredient.ItemCategoryId));

        return Task.CompletedTask;
    }

    [EffectMethod]
    public Task HandleSelectedItemCategoryChangedAction(SelectedItemCategoryChangedAction action,
        IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new LoadItemsForItemCategoryAction(action.Ingredient, action.ItemCategoryId));
        return Task.CompletedTask;
    }

    [EffectMethod]
    public async Task HandleLoadItemsForItemCategoryAction(LoadItemsForItemCategoryAction action,
        IDispatcher dispatcher)
    {
        var result = await _client.SearchItemByItemCategoryAsync(action.ItemCategoryId);

        dispatcher.Dispatch(new LoadItemsForItemCategoryFinishedAction(result.ToList(), action.Ingredient.Id));
    }
}