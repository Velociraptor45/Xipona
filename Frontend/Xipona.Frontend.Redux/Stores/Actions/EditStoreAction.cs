using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions;

namespace ProjectHermes.Xipona.Frontend.Redux.Stores.Actions;
public record EditStoreAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}