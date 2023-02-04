using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions;

public record LoadQuantityTypesFinishedAction(IEnumerable<QuantityType> QuantityTypes);