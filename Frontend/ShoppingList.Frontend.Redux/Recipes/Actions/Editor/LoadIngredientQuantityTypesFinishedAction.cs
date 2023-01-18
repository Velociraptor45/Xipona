using ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;
public record LoadIngredientQuantityTypesFinishedAction(IReadOnlyCollection<IngredientQuantityType> QuantityTypes);