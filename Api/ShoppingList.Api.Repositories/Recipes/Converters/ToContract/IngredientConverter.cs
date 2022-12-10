using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using Ingredient = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.Ingredient;

namespace ProjectHermes.ShoppingList.Api.Repositories.Recipes.Converters.ToContract;

public class IngredientConverter : IToContractConverter<(RecipeId, IIngredient), Entities.Ingredient>
{
    public Ingredient ToContract((RecipeId, IIngredient) source)
    {
        (RecipeId recipeId, IIngredient? ingredient) = source;

        return new Ingredient
        {
            Id = ingredient.Id.Value,
            RecipeId = recipeId.Value,
            ItemCategoryId = ingredient.ItemCategoryId,
            Quantity = ingredient.Quantity.Value,
            QuantityType = ingredient.QuantityType.ToInt(),
            DefaultItemId = ingredient.DefaultItemId,
            DefaultItemTypeId = ingredient.DefaultItemTypeId
        };
    }
}