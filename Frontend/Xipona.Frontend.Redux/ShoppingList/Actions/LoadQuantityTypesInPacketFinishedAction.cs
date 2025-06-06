using Xipona.Frontend.Redux.Shared.States;

namespace Xipona.Frontend.Redux.ShoppingList.Actions;

public record LoadQuantityTypesInPacketFinishedAction(IReadOnlyCollection<QuantityTypeInPacket> QuantityTypesInPacket);