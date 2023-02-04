using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients.ItemCategorySelectors;
public record LoadInitialItemCategoryFinishedAction(Guid IngredientKey, ItemCategorySearchResult Result);