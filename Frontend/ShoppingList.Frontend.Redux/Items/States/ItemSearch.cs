namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
public record ItemSearch(
    string Input,
    bool IsLoadingSearchResults,
    bool TriggeredAtLeastOnce,
    IList<ItemSearchResult> SearchResults);