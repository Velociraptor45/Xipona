using Xipona.Frontend.Redux.Shared.States;

namespace Xipona.Frontend.Redux.Items.Actions;
public record LoadQuantityTypesInPacketFinishedAction(IReadOnlyCollection<QuantityTypeInPacket> QuantityTypesInPacket);