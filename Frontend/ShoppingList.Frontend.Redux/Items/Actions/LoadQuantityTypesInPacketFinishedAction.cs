using ShoppingList.Frontend.Redux.Shared.States;

namespace ShoppingList.Frontend.Redux.Items.Actions;
public record LoadQuantityTypesInPacketFinishedAction(IReadOnlyCollection<QuantityTypeInPacket> QuantityTypesInPacket);