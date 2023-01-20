using ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
public record LoadIngredientQuantityTypesFinishedAction(IReadOnlyCollection<IngredientQuantityType> QuantityTypes);