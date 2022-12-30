using ShoppingList.Frontend.Redux.Items.States;

namespace ShoppingList.Frontend.Redux.Items.Actions.Search;
public record SearchItemsFinishedAction(IReadOnlyCollection<ItemSearchResult> SearchResults);