using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;
using ShoppingList.Frontend.Redux.ItemCategories.States;
using ShoppingList.Frontend.Redux.Recipes.States;
using ShoppingList.Frontend.Redux.Shared.Constants;
using ShoppingList.Frontend.Redux.Shared.Ports;
using Timer = System.Timers.Timer;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Effects;

public sealed class RecipeEditorEffects : IDisposable
{
    private readonly IApiClient _client;
    private readonly IState<RecipeState> _state;
    private readonly NavigationManager _navigationManager;

    private Timer? _startSearchTimer;

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
    public async Task HandleSearchItemCategoriesAction(SearchItemCategoriesAction action, IDispatcher dispatcher)
    {
        var ingredient = _state.Value.Editor.Recipe.Ingredients.FirstOrDefault(i => i.Id == action.IngredientId);
        if (ingredient is null)
            return;

        var results = await _client.GetItemCategorySearchResultsAsync(ingredient.ItemCategorySelector.Input);
        dispatcher.Dispatch(new SearchItemCategoriesFinishedAction(results.ToList(), action.IngredientId));
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

    [EffectMethod]
    public Task HandleItemCategoryInputChangedAction(ItemCategoryInputChangedAction action, IDispatcher dispatcher)
    {
        if (_startSearchTimer is not null)
        {
            _startSearchTimer.Stop();
            _startSearchTimer.Dispose();
        }

        if (string.IsNullOrWhiteSpace(action.Input))
        {
            dispatcher.Dispatch(new SearchItemCategoriesFinishedAction(new List<ItemCategorySearchResult>(),
                action.Ingredient.Id));
            return Task.CompletedTask;
        }

        _startSearchTimer = new(300d);
        _startSearchTimer.AutoReset = false;
        _startSearchTimer.Elapsed += (_, _) => dispatcher.Dispatch(new SearchItemCategoriesAction(action.Ingredient.Id));
        _startSearchTimer.Start();

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _startSearchTimer?.Dispose();
    }
}