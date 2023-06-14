using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Search;
public record SearchItemsFinishedAction(IReadOnlyCollection<ItemSearchResult> SearchResults);