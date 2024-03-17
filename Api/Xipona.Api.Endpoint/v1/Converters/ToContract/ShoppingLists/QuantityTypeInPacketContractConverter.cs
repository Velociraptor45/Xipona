using ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries.Quantities;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.ShoppingLists;

public class QuantityTypeInPacketContractConverter :
    IToContractConverter<QuantityTypeInPacketReadModel, QuantityTypeInPacketContract>
{
    public QuantityTypeInPacketContract ToContract(QuantityTypeInPacketReadModel source)
    {
        return new QuantityTypeInPacketContract(source.Id, source.Name, source.QuantityLabel);
    }
}