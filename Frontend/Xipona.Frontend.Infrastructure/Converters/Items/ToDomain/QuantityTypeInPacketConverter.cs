using ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Items.ToDomain
{
    public class QuantityTypeInPacketConverter :
        IToDomainConverter<QuantityTypeInPacketContract, QuantityTypeInPacket>
    {
        public QuantityTypeInPacket ToDomain(QuantityTypeInPacketContract source)
        {
            return new QuantityTypeInPacket(source.Id, source.Name, source.QuantityLabel);
        }
    }
}