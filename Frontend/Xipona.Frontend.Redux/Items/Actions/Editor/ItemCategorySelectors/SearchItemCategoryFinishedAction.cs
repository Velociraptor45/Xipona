using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Editor.ItemCategorySelectors;
public record SearchItemCategoryFinishedAction(IReadOnlyCollection<ItemCategorySearchResult> SearchResults);