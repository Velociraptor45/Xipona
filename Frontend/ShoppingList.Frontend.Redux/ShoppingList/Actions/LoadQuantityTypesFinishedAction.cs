using ShoppingList.Frontend.Redux.Shared.States;

namespace ShoppingList.Frontend.Redux.ShoppingList.Actions;

public record LoadQuantityTypesFinishedAction(IEnumerable<QuantityType> QuantityTypes);