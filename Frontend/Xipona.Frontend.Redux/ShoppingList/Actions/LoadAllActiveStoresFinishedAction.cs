using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions;
public record LoadAllActiveStoresFinishedAction(IReadOnlyCollection<ShoppingListStore> Stores);