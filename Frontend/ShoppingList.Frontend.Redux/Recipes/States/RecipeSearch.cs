namespace ShoppingList.Frontend.Redux.Recipes.States;
public record RecipeSearch(bool IsLoadingSearchResults, IReadOnlyCollection<RecipeSearchResult> SearchResults);