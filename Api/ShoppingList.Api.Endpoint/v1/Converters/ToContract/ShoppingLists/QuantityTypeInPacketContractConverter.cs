using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries.Quantities;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.ShoppingLists;

public class QuantityTypeInPacketContractConverter :
    IToContractConverter<QuantityTypeInPacketReadModel, QuantityTypeInPacketContract>
{
    public QuantityTypeInPacketContract ToContract(QuantityTypeInPacketReadModel source)
    {
        return new QuantityTypeInPacketContract(source.Id, source.Name, source.QuantityLabel);
    }
}