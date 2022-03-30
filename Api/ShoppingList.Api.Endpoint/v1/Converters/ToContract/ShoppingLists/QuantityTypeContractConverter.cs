using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries.Quantities;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.ShoppingLists;

public class QuantityTypeContractConverter : IToContractConverter<QuantityTypeReadModel, QuantityTypeContract>
{
    public QuantityTypeContract ToContract(QuantityTypeReadModel source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new QuantityTypeContract(
            source.Id,
            source.Name,
            source.DefaultQuantity,
            source.PriceLabel,
            source.QuantityLabel,
            source.QuantityNormalizer);
    }
}