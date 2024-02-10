namespace ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
public record ItemCategorySearch(
    string Input,
    bool IsLoadingSearchResults,
    bool TriggeredAtLeastOnce,
    IList<ItemCategorySearchResult> SearchResults);