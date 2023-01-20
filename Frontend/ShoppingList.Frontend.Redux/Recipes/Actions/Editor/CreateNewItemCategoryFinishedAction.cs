using ShoppingList.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;
public record CreateNewItemCategoryFinishedAction(Guid IngredientId, ItemCategorySearchResult SearchResult);