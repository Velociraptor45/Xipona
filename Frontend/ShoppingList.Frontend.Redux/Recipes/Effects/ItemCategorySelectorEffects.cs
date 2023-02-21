using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients.ItemCategorySelectors;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
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
        if (action.Ingredient.ItemCategoryId == Guid.Empty)
            return;

        var itemCategory = await _client.GetItemCategoryByIdAsync(action.Ingredient.ItemCategoryId);
        var result = new ItemCategorySearchResult(itemCategory.Id, itemCategory.Name);
        dispatcher.Dispatch(new LoadInitialItemCategoryFinishedAction(action.Ingredient.Key, result));
    }

    [EffectMethod]
    public async Task HandleSearchItemCategoriesAction(SearchItemCategoriesAction action, IDispatcher dispatcher)
    {
        if (_state.Value.Editor.Recipe is null)
            return;

        var ingredient = _state.Value.Editor.Recipe.Ingredients.FirstOrDefault(i => i.Key == action.IngredientKey);
        if (ingredient is null || string.IsNullOrWhiteSpace(ingredient.ItemCategorySelector.Input))
            return;

        var results = await _client.GetItemCategorySearchResultsAsync(ingredient.ItemCategorySelector.Input);
        dispatcher.Dispatch(new SearchItemCategoriesFinishedAction(results.ToList(), action.IngredientKey));
    }

    [EffectMethod]
    public Task HandleSelectedItemCategoryChangedAction(SelectedItemCategoryChangedAction action,
        IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new LoadItemsForItemCategoryAction(action.IngredientKey, action.ItemCategoryId));
        return Task.CompletedTask;
    }

    [EffectMethod]
    public async Task HandleLoadItemsForItemCategoryAction(LoadItemsForItemCategoryAction action,
        IDispatcher dispatcher)
    {
        var result = await _client.SearchItemByItemCategoryAsync(action.ItemCategoryId);

        dispatcher.Dispatch(new LoadItemsForItemCategoryFinishedAction(result.ToList(), action.IngredientKey));
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
                action.Ingredient.Key));
            return Task.CompletedTask;
        }

        _startSearchTimer = new(300d);
        _startSearchTimer.AutoReset = false;
        _startSearchTimer.Elapsed += (_, _) => dispatcher.Dispatch(new SearchItemCategoriesAction(action.Ingredient.Key));
        _startSearchTimer.Start();

        return Task.CompletedTask;
    }

    [EffectMethod]
    public async Task HandleCreateNewItemCategoryAction(CreateNewItemCategoryAction action, IDispatcher dispatcher)
    {
        if (_state.Value.Editor.Recipe is null)
            return;

        var name = _state.Value.Editor.Recipe.Ingredients
            .FirstOrDefault(i => i.Key == action.IngredientKey)?
            .ItemCategorySelector.Input;

        if (string.IsNullOrWhiteSpace(name))
            return;

        var itemCategory = await _client.CreateItemCategoryAsync(name);
        var searchResult = new ItemCategorySearchResult(itemCategory.Id, itemCategory.Name);
        dispatcher.Dispatch(new CreateNewItemCategoryFinishedAction(action.IngredientKey, searchResult));
        dispatcher.Dispatch(new LoadItemsForItemCategoryAction(action.IngredientKey, searchResult.Id));
    }

    public void Dispose()
    {
        _startSearchTimer?.Dispose();
    }
}