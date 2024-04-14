using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions;

namespace ProjectHermes.Xipona.Frontend.Redux.ItemCategories.Actions;

public record EditItemCategoryAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}
