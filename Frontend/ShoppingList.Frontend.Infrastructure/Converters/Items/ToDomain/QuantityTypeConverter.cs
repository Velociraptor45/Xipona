using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
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