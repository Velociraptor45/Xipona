using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ShoppingList.Frontend.Redux.ItemCategories.States;
using ShoppingList.Frontend.Redux.Recipes.States;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class EditedIngredientConverter : IToDomainConverter<IngredientContract, EditedIngredient>
{
    public EditedIngredient ToDomain(IngredientContract source)
    {
        return new EditedIngredient(
            source.Id,
            source.ItemCategoryId,
            source.QuantityType,
            source.Quantity,
            source.DefaultItemId,
            source.DefaultItemTypeId,
            new ItemCategorySelector(
                new List<ItemCategorySearchResult>(0),
                string.Empty));
    }
}