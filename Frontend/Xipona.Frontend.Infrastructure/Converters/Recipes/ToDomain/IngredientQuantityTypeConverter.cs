using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.AllIngredientQuantityTypes;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class IngredientQuantityTypeConverter : IToDomainConverter<IngredientQuantityTypeContract, IngredientQuantityType>
{
    public IngredientQuantityType ToDomain(IngredientQuantityTypeContract source)
    {
        return new IngredientQuantityType(source.Id, source.QuantityLabel);
    }
}