using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
public record ItemCategorySelector(IReadOnlyCollection<ItemCategorySearchResult> ItemCategories, string Input);