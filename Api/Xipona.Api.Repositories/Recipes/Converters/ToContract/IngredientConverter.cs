using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using Ingredient = ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.Ingredient;

namespace ProjectHermes.Xipona.Api.Repositories.Recipes.Converters.ToContract;

public class IngredientConverter : IToContractConverter<(RecipeId, IIngredient), Entities.Ingredient>
{
    public Ingredient ToContract((RecipeId, IIngredient) source)
    {
        (RecipeId recipeId, IIngredient? ingredient) = source;

        return new Ingredient
        {
            Id = ingredient.Id,
            RecipeId = recipeId,
            ItemCategoryId = ingredient.ItemCategoryId,
            Quantity = ingredient.Quantity.Value,
            QuantityType = ingredient.QuantityType.ToInt(),
            DefaultItemId = ingredient.ShoppingListProperties?.DefaultItemId,
            DefaultItemTypeId = ingredient.ShoppingListProperties?.DefaultItemTypeId,
            DefaultStoreId = ingredient.ShoppingListProperties?.DefaultStoreId,
            AddToShoppingListByDefault = ingredient.ShoppingListProperties?.AddToShoppingListByDefault
        };
    }
}