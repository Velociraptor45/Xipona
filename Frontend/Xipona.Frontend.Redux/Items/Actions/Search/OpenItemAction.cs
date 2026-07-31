using Xipona.Frontend.Redux.Shared.Actions;

namespace Xipona.Frontend.Redux.Items.Actions.Search;

public record OpenItemAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}