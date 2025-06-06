using Xipona.Frontend.Redux.Stores.States;

namespace Xipona.Frontend.Redux.Stores.Actions;
public record LoadStoresOverviewFinishedAction(IReadOnlyCollection<StoreSearchResult> SearchResults);