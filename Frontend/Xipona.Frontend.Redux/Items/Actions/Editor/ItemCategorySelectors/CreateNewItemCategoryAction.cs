using Xipona.Frontend.Redux.ItemCategories.States;

namespace Xipona.Frontend.Redux.Items.Actions.Editor.ItemCategorySelectors;
public record CreateNewItemCategoryAction;
public record CreateNewItemCategoryFinishedAction(ItemCategorySearchResult ItemCategory);