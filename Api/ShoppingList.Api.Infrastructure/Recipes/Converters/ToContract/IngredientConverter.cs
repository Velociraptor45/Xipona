using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using Ingredient = ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Entities.Ingredient;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Converters.ToContract;

public class IngredientConverter : IToContractConverter<(RecipeId, IIngredient), Ingredient>
{
    public Ingredient ToContract((RecipeId, IIngredient) source)
    {
        (RecipeId recipeId, IIngredient? ingredient) = source;

        return new Ingredient()
        {
            Id = ingredient.Id.Value,
            RecipeId = recipeId.Value,
            ItemCategoryId = ingredient.ItemCategoryId.Value,
            Quantity = ingredient.Quantity.Value,
            QuantityType = ingredient.QuantityType.ToInt()
        };
    }
}