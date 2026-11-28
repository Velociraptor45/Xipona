using Xipona.Api.Contracts.Recipes.Queries.AllIngredientQuantityTypes;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Recipes.States;

namespace Xipona.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class IngredientQuantityTypeConverter : IToDomainConverter<IngredientQuantityTypeContract, IngredientQuantityType>
{
    public IngredientQuantityType ToDomain(IngredientQuantityTypeContract source)
    {
        return new IngredientQuantityType(source.Id, source.QuantityLabel);
    }
}