using Xipona.Frontend.Redux.Shared.States;

namespace Xipona.Frontend.Redux.ShoppingList.Actions;

public record LoadQuantityTypesFinishedAction(IReadOnlyCollection<QuantityType> QuantityTypes);