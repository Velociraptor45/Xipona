using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.AllIngredientQuantityTypes;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class IngredientQuantityTypeConverter : IToDomainConverter<IngredientQuantityTypeContract, IngredientQuantityType>
{
    public IngredientQuantityType ToDomain(IngredientQuantityTypeContract source)
    {
        return new IngredientQuantityType(source.Id, source.QuantityLabel);
    }
}