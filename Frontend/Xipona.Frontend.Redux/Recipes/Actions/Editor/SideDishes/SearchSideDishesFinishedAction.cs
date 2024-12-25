using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions.Editor.SideDishes;
public record SearchSideDishesFinishedAction(IReadOnlyCollection<RecipeSearchResult> SideDishes);