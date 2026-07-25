using Xipona.Frontend.Redux.Shared.States;

namespace Xipona.Frontend.Redux.ShoppingList.Actions;
public record LoadQuantityTypesInPacketAction;
public record LoadQuantityTypesInPacketFinishedAction(IReadOnlyCollection<QuantityTypeInPacket> QuantityTypesInPacket);