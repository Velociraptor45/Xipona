using Xipona.Frontend.Redux.Shared.Actions;

namespace Xipona.Frontend.Redux.Items.Actions.Editor;
public record EditItemAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}