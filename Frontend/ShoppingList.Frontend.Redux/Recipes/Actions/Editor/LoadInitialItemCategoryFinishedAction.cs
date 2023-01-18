using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ShoppingList.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;
public record LoadInitialItemCategoryFinishedAction(EditedIngredient Ingredient, ItemCategorySearchResult Result);