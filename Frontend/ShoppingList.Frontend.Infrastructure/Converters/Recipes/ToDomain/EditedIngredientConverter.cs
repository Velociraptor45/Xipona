using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class EditedIngredientConverter : IToDomainConverter<IngredientContract, EditedIngredient>
{
    public EditedIngredient ToDomain(IngredientContract source)
    {
        return new EditedIngredient(
            Guid.NewGuid(),
            source.Id,
            source.ItemCategoryId,
            source.QuantityType,
            source.Quantity,
            source.DefaultItemId,
            source.DefaultItemTypeId,
            new ItemCategorySelector(
                new List<ItemCategorySearchResult>(0),
                string.Empty),
            new ItemSelector(
                new List<SearchItemByItemCategoryResult>(0)));
    }
}