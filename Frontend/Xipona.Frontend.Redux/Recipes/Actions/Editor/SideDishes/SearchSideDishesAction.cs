using Xipona.Frontend.Redux.Recipes.States;

namespace Xipona.Frontend.Redux.Recipes.Actions.Editor.SideDishes;
public record SearchSideDishesAction;
public record SearchSideDishesFinishedAction(IReadOnlyCollection<RecipeSearchResult> SideDishes);