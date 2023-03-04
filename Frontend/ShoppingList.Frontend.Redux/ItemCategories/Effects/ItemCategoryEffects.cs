using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Constants;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using RestEase;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.Effects;

public class ItemCategoryEffects
{
    private readonly IApiClient _client;
    private readonly NavigationManager _navigationManager;

    public ItemCategoryEffects(IApiClient client, NavigationManager navigationManager)
    {
        _client = client;
        _navigationManager = navigationManager;
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

        IEnumerable<ItemCategorySearchResult> result;

        try
        {
            result = await _client.GetItemCategorySearchResultsAsync(action.SearchInput);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading search results failed", e));
            return;
        }

        var finishAction = new SearchItemCategoriesFinishedAction(result.ToList());
        dispatcher.Dispatch(finishAction);
    }

    [EffectMethod]
    public Task HandleEditItemCategoryAction(EditItemCategoryAction action, IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo($"{PageRoutes.ItemCategories}/{action.Id}");
        return Task.CompletedTask;
    }
}