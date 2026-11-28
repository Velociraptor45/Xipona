using Xipona.Frontend.Redux.Recipes.States;

namespace Xipona.Frontend.Redux.Recipes.Actions.Editor;
public record InitializeRecipeAction(Guid RecipeId);
public record LoadIngredientQuantityTypesFinishedAction(IReadOnlyCollection<IngredientQuantityType> QuantityTypes);