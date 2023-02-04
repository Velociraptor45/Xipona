using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.TemporaryItemCreator;
public record SaveTemporaryItemFinishedAction(ShoppingListItem Item, ShoppingListStoreSection Section);