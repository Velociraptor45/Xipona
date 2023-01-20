using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients.ItemCategorySelectors;
using ShoppingList.Frontend.Redux.ItemCategories.States;
using ShoppingList.Frontend.Redux.Recipes.States;
using ShoppingList.Frontend.Redux.Shared.Ports;
using Timer = System.Timers.Timer;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Effects;

public sealed class ItemCategorySelectorEffects : IDisposable
{
    private readonly IApiClient _client;
    private readonly IState<RecipeState> _state;

    private Timer? _startSearchTimer;

    public ItemCategorySelectorEffects(IApiClient client, IState<RecipeState> state)
    {
        _client = client;
        _state = state;
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
    public Task HandleSelectedItemCategoryChangedAction(SelectedItemCategoryChangedAction action,
        IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new LoadItemsForItemCategoryAction(action.IngredientId, action.ItemCategoryId));
        return Task.CompletedTask;
    }

    [EffectMethod]
    public async Task HandleLoadItemsForItemCategoryAction(LoadItemsForItemCategoryAction action,
        IDispatcher dispatcher)
    {
        var result = await _client.SearchItemByItemCategoryAsync(action.ItemCategoryId);

        dispatcher.Dispatch(new LoadItemsForItemCategoryFinishedAction(result.ToList(), action.IngredientId));
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

    [EffectMethod]
    public async Task HandleCreateNewItemCategoryAction(CreateNewItemCategoryAction action, IDispatcher dispatcher)
    {
        var name = _state.Value.Editor.Recipe.Ingredients
            .FirstOrDefault(i => i.Id == action.IngredientId)?
            .ItemCategorySelector.Input;

        if (name == null)
            return;

        var itemCategory = await _client.CreateItemCategoryAsync(name);
        var searchResult = new ItemCategorySearchResult(itemCategory.Id, itemCategory.Name);
        dispatcher.Dispatch(new CreateNewItemCategoryFinishedAction(action.IngredientId, searchResult));
        dispatcher.Dispatch(new LoadItemsForItemCategoryAction(action.IngredientId, searchResult.Id));
    }

    public void Dispose()
    {
        _startSearchTimer?.Dispose();
    }
}