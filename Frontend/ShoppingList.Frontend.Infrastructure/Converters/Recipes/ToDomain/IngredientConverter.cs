using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Recipes.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class IngredientConverter : IToDomainConverter<IngredientContract, Ingredient>
{
    public Ingredient ToDomain(IngredientContract source)
    {
        return new Ingredient(
            source.Id,
            source.ItemCategoryId,
            source.QuantityType,
            source.Quantity,
            source.DefaultItemId,
            source.DefaultItemTypeId);
    }
}