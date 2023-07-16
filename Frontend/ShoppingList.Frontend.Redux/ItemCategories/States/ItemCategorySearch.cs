namespace ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
public record ItemCategorySearch(
    bool IsLoadingSearchResults,
    bool TriggeredAtLeastOnce,
    IList<ItemCategorySearchResult> SearchResults);