using ProjectHermes.Xipona.Frontend.Redux.Shared.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions;

public record LoadQuantityTypesFinishedAction(IReadOnlyCollection<QuantityType> QuantityTypes);