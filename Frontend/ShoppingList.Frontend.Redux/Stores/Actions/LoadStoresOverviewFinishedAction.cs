using ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Stores.Actions;
public record LoadStoresOverviewFinishedAction(IReadOnlyCollection<StoreSearchResult> SearchResults);