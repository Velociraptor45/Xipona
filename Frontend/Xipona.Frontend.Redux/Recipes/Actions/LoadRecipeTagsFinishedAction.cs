using Xipona.Frontend.Redux.Recipes.States;

namespace Xipona.Frontend.Redux.Recipes.Actions;
public record LoadRecipeTagsFinishedAction(IReadOnlyCollection<RecipeTag> RecipeTags);