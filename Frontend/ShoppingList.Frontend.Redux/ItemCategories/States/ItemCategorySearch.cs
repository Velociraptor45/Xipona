namespace ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
public record ItemCategorySearch(
    bool IsLoadingSearchResults,
    IList<ItemCategorySearchResult> SearchResults);