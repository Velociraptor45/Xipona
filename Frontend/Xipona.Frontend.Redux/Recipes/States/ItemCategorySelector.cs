using Xipona.Frontend.Redux.ItemCategories.States;

namespace Xipona.Frontend.Redux.Recipes.States;
public record ItemCategorySelector(IReadOnlyCollection<ItemCategorySearchResult> ItemCategories, string Input);