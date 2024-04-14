using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ItemCategories.Actions;
public record SearchItemCategoriesFinishedAction(IReadOnlyCollection<ItemCategorySearchResult> SearchResults);