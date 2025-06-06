using Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Shared.States;

namespace Xipona.Frontend.Infrastructure.Converters.Items.ToDomain;

public class QuantityTypeConverter : IToDomainConverter<QuantityTypeContract, QuantityType>
{
    public QuantityType ToDomain(QuantityTypeContract source)
    {
        return new QuantityType(
            source.Id,
            source.Name,
            source.DefaultQuantity,
            source.PriceLabel,
            source.QuantityLabel,
            source.QuantityNormalizer);
    }
}