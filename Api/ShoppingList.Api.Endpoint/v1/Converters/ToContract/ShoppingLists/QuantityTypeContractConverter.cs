using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypes;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.ShoppingLists;

public class QuantityTypeContractConverter : IToContractConverter<QuantityTypeReadModel, QuantityTypeContract>
{
    public QuantityTypeContract ToContract(QuantityTypeReadModel source)
    {
        if (source is null)
            throw new System.ArgumentNullException(nameof(source));

        return new QuantityTypeContract(
            source.Id,
            source.Name,
            source.DefaultQuantity,
            source.PriceLabel,
            source.QuantityLabel,
            source.QuantityNormalizer);
    }
}