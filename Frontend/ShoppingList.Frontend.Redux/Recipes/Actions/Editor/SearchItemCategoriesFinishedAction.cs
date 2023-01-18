using ShoppingList.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;
public record SearchItemCategoriesFinishedAction(IReadOnlyCollection<ItemCategorySearchResult> ItemCategories,
    Guid IngredientId);