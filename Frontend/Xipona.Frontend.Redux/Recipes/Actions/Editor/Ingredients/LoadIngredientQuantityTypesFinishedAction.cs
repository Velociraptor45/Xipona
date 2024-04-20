using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
public record LoadIngredientQuantityTypesFinishedAction(IReadOnlyCollection<IngredientQuantityType> QuantityTypes);