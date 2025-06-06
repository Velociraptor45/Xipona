using Xipona.Frontend.Redux.Shared.Actions;

namespace Xipona.Frontend.Redux.Stores.Actions;
public record EditStoreAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}