using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;

namespace ShoppingList.Frontend.Redux.Items.States;
public record ItemCategorySelector(IReadOnlyCollection<ItemCategorySearchResult> ItemCategories, string Input);