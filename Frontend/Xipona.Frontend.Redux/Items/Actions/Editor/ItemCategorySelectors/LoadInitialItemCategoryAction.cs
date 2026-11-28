using Xipona.Frontend.Redux.ItemCategories.States;

namespace Xipona.Frontend.Redux.Items.Actions.Editor.ItemCategorySelectors;
public record LoadInitialItemCategoryAction;
public record LoadInitialItemCategoryFinishedAction(ItemCategorySearchResult ItemCategory);