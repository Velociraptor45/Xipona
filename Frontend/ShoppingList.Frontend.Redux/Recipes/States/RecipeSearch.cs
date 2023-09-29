namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
public record RecipeSearch(
    bool IsLoadingSearchResults,
    bool TriggeredAtLeastOnce,
    IReadOnlyCollection<RecipeSearchResult> SearchResults,
    IReadOnlyCollection<Guid> SelectedRecipeTagIds);