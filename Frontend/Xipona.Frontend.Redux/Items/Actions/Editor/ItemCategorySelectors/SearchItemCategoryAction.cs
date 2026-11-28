using Xipona.Frontend.Redux.ItemCategories.States;

namespace Xipona.Frontend.Redux.Items.Actions.Editor.ItemCategorySelectors;
public record SearchItemCategoryAction;
public record SearchItemCategoryFinishedAction(IReadOnlyCollection<ItemCategorySearchResult> SearchResults);