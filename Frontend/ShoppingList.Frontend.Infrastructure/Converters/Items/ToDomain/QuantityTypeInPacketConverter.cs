using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ShoppingList.Frontend.Redux.Shared.States;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
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