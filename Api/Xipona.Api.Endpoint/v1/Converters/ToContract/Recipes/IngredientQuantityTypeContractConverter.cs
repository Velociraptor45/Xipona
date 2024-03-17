using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.AllIngredientQuantityTypes;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries.Quantities;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Recipes;

public class IngredientQuantityTypeContractConverter :
    IToContractConverter<IngredientQuantityTypeReadModel, IngredientQuantityTypeContract>
{
    public IngredientQuantityTypeContract ToContract(IngredientQuantityTypeReadModel source)
    {
        return new IngredientQuantityTypeContract(source.Id, source.QuantityLabel);
    }
}