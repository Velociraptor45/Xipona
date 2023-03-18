using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions;
public record LoadQuantityTypesFinishedAction(IReadOnlyCollection<QuantityType> QuantityTypes);