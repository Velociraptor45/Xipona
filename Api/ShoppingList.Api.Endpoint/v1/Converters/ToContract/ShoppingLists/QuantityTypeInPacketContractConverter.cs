using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypesInPacket;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.ShoppingLists;

public class QuantityTypeInPacketContractConverter :
    IToContractConverter<QuantityTypeInPacketReadModel, QuantityTypeInPacketContract>
{
    public QuantityTypeInPacketContract ToContract(QuantityTypeInPacketReadModel source)
    {
        return new QuantityTypeInPacketContract(source.Id, source.Name, source.QuantityLabel);
    }
}