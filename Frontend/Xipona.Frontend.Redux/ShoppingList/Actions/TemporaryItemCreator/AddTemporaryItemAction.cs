using Xipona.Frontend.Redux.ShoppingList.States;

namespace Xipona.Frontend.Redux.ShoppingList.Actions.TemporaryItemCreator;

public record AddTemporaryItemAction(ShoppingListItem Item, ShoppingListStoreSection Section);