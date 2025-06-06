using Xipona.Frontend.Redux.Shared.Actions;

namespace Xipona.Frontend.Redux.ItemCategories.Actions;

public record EditItemCategoryAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}
