namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
public record ItemSearch(
    bool IsLoadingSearchResults,
    bool TriggeredAtLeastOnce,
    IList<ItemSearchResult> SearchResults);