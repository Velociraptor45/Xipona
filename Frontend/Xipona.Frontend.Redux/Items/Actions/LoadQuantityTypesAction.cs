using Xipona.Frontend.Redux.Shared.States;

namespace Xipona.Frontend.Redux.Items.Actions;
public record LoadQuantityTypesAction;
public record LoadQuantityTypesFinishedAction(IReadOnlyCollection<QuantityType> QuantityTypes);