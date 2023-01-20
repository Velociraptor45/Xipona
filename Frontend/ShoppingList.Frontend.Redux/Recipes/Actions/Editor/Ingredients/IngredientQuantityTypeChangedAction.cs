using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
public record IngredientQuantityTypeChangedAction(EditedIngredient Ingredient, IngredientQuantityType QuantityType);