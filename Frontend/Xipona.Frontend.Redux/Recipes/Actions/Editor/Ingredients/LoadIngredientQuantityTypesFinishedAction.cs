using Xipona.Frontend.Redux.Recipes.States;

namespace Xipona.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
public record LoadIngredientQuantityTypesFinishedAction(IReadOnlyCollection<IngredientQuantityType> QuantityTypes);