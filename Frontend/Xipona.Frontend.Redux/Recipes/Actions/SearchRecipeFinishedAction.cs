using Xipona.Frontend.Redux.Recipes.States;

namespace Xipona.Frontend.Redux.Recipes.Actions;
public record SearchRecipeFinishedAction(IReadOnlyCollection<RecipeSearchResult> SearchResults, SearchType SearchType);