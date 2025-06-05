using Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Items.Services.Queries.Quantities;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.ShoppingLists;

public class QuantityTypeContractConverter : IToContractConverter<QuantityTypeReadModel, QuantityTypeContract>
{
    public QuantityTypeContract ToContract(QuantityTypeReadModel source)
    {
        return new QuantityTypeContract(
            source.Id,
            source.Name,
            source.DefaultQuantity,
            source.PriceLabel,
            source.QuantityLabel,
            source.QuantityNormalizer);
    }
}