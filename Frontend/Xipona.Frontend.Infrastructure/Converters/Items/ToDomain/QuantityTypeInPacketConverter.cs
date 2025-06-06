using Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Shared.States;

namespace Xipona.Frontend.Infrastructure.Converters.Items.ToDomain;

public class QuantityTypeInPacketConverter :
    IToDomainConverter<QuantityTypeInPacketContract, QuantityTypeInPacket>
{
    public QuantityTypeInPacket ToDomain(QuantityTypeInPacketContract source)
    {
        return new QuantityTypeInPacket(source.Id, source.Name, source.QuantityLabel);
    }
}