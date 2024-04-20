using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions;

namespace ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Editor;
public record EditItemAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}