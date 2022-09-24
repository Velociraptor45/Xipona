using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.AllIngredientQuantityTypes;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries.Quantities;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Recipes;

public class IngredientQuantityTypeContractConverter :
    IToContractConverter<IngredientQuantityTypeReadModel, IngredientQuantityTypeContract>
{
    public IngredientQuantityTypeContract ToContract(IngredientQuantityTypeReadModel source)
    {
        return new IngredientQuantityTypeContract(source.Id, source.QuantityLabel);
    }
}