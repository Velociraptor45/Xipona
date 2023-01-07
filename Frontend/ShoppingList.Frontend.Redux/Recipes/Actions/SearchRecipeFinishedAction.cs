using ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions;
public record SearchRecipeFinishedAction(IReadOnlyCollection<RecipeSearchResult> SearchResults);