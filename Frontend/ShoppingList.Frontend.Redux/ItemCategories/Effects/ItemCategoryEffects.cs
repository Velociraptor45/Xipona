using Fluxor;
using Microsoft.AspNetCore.Components;
using ShoppingList.Frontend.Redux.ItemCategories.Actions;
using ShoppingList.Frontend.Redux.ItemCategories.States;
using ShoppingList.Frontend.Redux.Shared.Ports;

namespace ShoppingList.Frontend.Redux.ItemCategories.Effects;

public class ItemCategoryEffects
{
    private const string ItemCategoryPageName = "item-categories";

    private readonly IApiClient _client;
    private readonly NavigationManager _navigationManager;
    private readonly IState<ItemCategoryState> _state;

    public ItemCategoryEffects(IApiClient client, NavigationManager navigationManager, IState<ItemCategoryState> state)
    {
        _client = client;
        _navigationManager = navigationManager;
        _state = state;
    }

    [EffectMethod]
    public async Task HandleSearchItemCategoriesAction(SearchItemCategoriesAction action, IDispatcher dispatcher)
    {
        if (string.IsNullOrWhiteSpace(action.SearchInput))
        {
            dispatcher.Dispatch(new SearchItemCategoriesFinishedAction(new List<ItemCategorySearchResult>()));
            return;
        }

        dispatcher.Dispatch(new SearchItemCategoriesStartedAction());

        var result = await _client.GetItemCategorySearchResultsAsync(action.SearchInput);

        var finishAction = new SearchItemCategoriesFinishedAction(result.ToList());
        dispatcher.Dispatch(finishAction);
    }

    [EffectMethod]
    public Task HandleEditItemCategoryAction(EditItemCategoryAction action, IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo($"{ItemCategoryPageName}/{action.Id}");
        return Task.CompletedTask;
    }
}