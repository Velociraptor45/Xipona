using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients;

public record SelectedItemChangedAction(EditedIngredient Ingredient, SearchItemByItemCategoryResult? Item);