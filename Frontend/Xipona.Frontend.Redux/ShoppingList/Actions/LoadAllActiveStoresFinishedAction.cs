using Xipona.Frontend.Redux.ShoppingList.States;

namespace Xipona.Frontend.Redux.ShoppingList.Actions;
public record LoadAllActiveStoresFinishedAction(IReadOnlyCollection<ShoppingListStore> Stores);