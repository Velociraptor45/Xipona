namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
public record RecipeSearch(
    string Input,
    bool IsLoadingSearchResults,
    bool TriggeredAtLeastOnce,
    IReadOnlyCollection<RecipeSearchResult> SearchResults,
    IReadOnlyCollection<Guid> SelectedRecipeTagIds);