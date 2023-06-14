using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients.ItemCategorySelectors;
public record ItemCategoryInputChangedAction(EditedIngredient Ingredient, string Input);