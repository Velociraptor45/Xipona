using Xipona.Frontend.Redux.ItemCategories.States;

namespace Xipona.Frontend.Redux.Items.States.Filters;

public record ItemCategoryFilter(IReadOnlyList<ItemCategorySearchResult> ItemCategories, string Input,
    ItemCategorySearchResult? SelectedItemCategory);