using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions;
public record SearchRecipeFinishedAction(IReadOnlyCollection<RecipeSearchResult> SearchResults, SearchType SearchType);