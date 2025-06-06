using Xipona.Frontend.Redux.ItemCategories.States;

namespace Xipona.Frontend.Redux.ItemCategories.Actions;
public record SearchItemCategoriesFinishedAction(IReadOnlyCollection<ItemCategorySearchResult> SearchResults);