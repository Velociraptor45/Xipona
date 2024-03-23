using ProjectHermes.Xipona.Frontend.Redux.Stores.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Stores.Actions;
public record LoadStoresOverviewFinishedAction(IReadOnlyCollection<StoreSearchResult> SearchResults);