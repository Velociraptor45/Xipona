using ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Items.ToDomain
{
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
}