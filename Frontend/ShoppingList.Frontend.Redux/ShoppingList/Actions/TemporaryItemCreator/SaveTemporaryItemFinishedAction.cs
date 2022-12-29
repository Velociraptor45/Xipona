using ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ShoppingList.Frontend.Redux.ShoppingList.Actions.TemporaryItemCreator;
public record SaveTemporaryItemFinishedAction(ShoppingListItem Item, ShoppingListStoreSection Section);