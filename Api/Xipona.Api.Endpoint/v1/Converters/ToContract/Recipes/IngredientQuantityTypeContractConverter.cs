using Xipona.Api.Contracts.Recipes.Queries.AllIngredientQuantityTypes;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Recipes.Services.Queries.Quantities;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.Recipes;

public class IngredientQuantityTypeContractConverter :
    IToContractConverter<IngredientQuantityTypeReadModel, IngredientQuantityTypeContract>
{
    public IngredientQuantityTypeContract ToContract(IngredientQuantityTypeReadModel source)
    {
        return new IngredientQuantityTypeContract(source.Id, source.QuantityLabel);
    }
}