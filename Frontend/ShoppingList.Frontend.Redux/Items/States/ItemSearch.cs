namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
public record ItemSearch(
    bool IsLoadingSearchResults,
    IList<ItemSearchResult> SearchResults);