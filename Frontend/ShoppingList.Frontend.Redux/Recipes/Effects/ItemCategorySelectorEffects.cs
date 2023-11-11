using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients.ItemCategorySelectors;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using RestEase;
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

        var ingredient = _state.Value.GetIngredientByKey(action.Ingredient.Key);
        if (ingredient is null || ingredient.ItemCategorySelector.ItemCategories.Any())
            return;

        EditedItemCategory itemCategory;
        try
        {
            itemCategory = await _client.GetItemCategoryByIdAsync(action.Ingredient.ItemCategoryId);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading item category failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Loading item category failed", e.Message));
            return;
        }

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

        IEnumerable<ItemCategorySearchResult> results;
        try
        {
            results = await _client.GetItemCategorySearchResultsAsync(ingredient.ItemCategorySelector.Input);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Searching for item categories failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Searching for item categories failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new SearchItemCategoriesFinishedAction(results.ToList(), action.IngredientKey));
    }

    [EffectMethod]
    public Task HandleSelectedItemCategoryChangedAction(SelectedItemCategoryChangedAction action,
        IDispatcher dispatcher)
    {
        var ingredient = _state.Value.GetIngredientByKey(action.IngredientKey);
        if (ingredient is null || ingredient.ItemCategoryId == action.ItemCategoryId)
            return Task.CompletedTask;

        dispatcher.Dispatch(new ItemCategoryChangedAction(action.IngredientKey, action.ItemCategoryId));
        dispatcher.Dispatch(new LoadItemsForItemCategoryAction(action.IngredientKey, action.ItemCategoryId));
        return Task.CompletedTask;
    }

    [EffectMethod]
    public async Task HandleLoadItemsForItemCategoryAction(LoadItemsForItemCategoryAction action,
        IDispatcher dispatcher)
    {
        IEnumerable<SearchItemByItemCategoryResult> result;
        try
        {
            result = await _client.SearchItemByItemCategoryAsync(action.ItemCategoryId);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading items for item category failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Loading items for item category failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new LoadItemsForItemCategoryFinishedAction(action.IngredientKey, result.ToList()));
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

        EditedItemCategory itemCategory;
        try
        {
            itemCategory = await _client.CreateItemCategoryAsync(name);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Creating item category failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Creating item category failed", e.Message));
            return;
        }

        var searchResult = new ItemCategorySearchResult(itemCategory.Id, itemCategory.Name);
        dispatcher.Dispatch(new CreateNewItemCategoryFinishedAction(action.IngredientKey, searchResult));
        dispatcher.Dispatch(new LoadItemsForItemCategoryAction(action.IngredientKey, searchResult.Id));
    }

    public void Dispose()
    {
        _startSearchTimer?.Dispose();
    }
}