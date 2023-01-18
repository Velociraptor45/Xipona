using ShoppingList.Frontend.Redux.ItemCategories.States;

namespace ShoppingList.Frontend.Redux.Recipes.States;
public record ItemCategorySelector(IReadOnlyCollection<ItemCategorySearchResult> ItemCategories, string Input);