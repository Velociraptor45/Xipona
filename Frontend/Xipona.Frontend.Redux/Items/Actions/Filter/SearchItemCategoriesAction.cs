using Xipona.Frontend.Redux.ItemCategories.States;

namespace Xipona.Frontend.Redux.Items.Actions.Filter;

public record SearchItemCategoriesAction;

public record SearchItemCategoriesFinishedAction(IReadOnlyList<ItemCategorySearchResult> ItemCategories);