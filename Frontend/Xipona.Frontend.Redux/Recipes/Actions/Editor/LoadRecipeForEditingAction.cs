using Xipona.Frontend.Redux.Recipes.States;

namespace Xipona.Frontend.Redux.Recipes.Actions.Editor;
public record LoadRecipeForEditingAction(Guid RecipeId);
public record LoadRecipeForEditingFinishedAction(EditedRecipe Recipe);