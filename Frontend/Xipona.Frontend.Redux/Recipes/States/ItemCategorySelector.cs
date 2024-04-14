using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
public record ItemCategorySelector(IReadOnlyCollection<ItemCategorySearchResult> ItemCategories, string Input);