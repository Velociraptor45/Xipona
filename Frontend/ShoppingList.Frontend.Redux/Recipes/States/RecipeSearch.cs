namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
public record RecipeSearch(
    string Input,
    bool IsLoadingSearchResults,
    bool TriggeredAtLeastOnce,
    SearchType LastSearchType,
    IReadOnlyCollection<RecipeSearchResult> SearchResults,
    IReadOnlyCollection<Guid> SelectedRecipeTagIds);