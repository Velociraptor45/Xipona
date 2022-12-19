using ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ShoppingList.Frontend.Redux.ShoppingList.Actions;
public record SaveTemporaryItemFinishedAction(ShoppingListItem Item, ShoppingListStoreSection Section);