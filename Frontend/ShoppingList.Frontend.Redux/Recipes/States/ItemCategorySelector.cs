using ShoppingList.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
public record ItemCategorySelector(IReadOnlyCollection<ItemCategorySearchResult> ItemCategories, string Input);