using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;
public record LoadItemsForItemCategoryAction(EditedIngredient Ingredient, Guid ItemCategoryId);