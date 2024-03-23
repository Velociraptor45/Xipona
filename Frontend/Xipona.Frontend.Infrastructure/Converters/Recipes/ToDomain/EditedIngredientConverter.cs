using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class EditedIngredientConverter : IToDomainConverter<IngredientContract, EditedIngredient>
{
    public EditedIngredient ToDomain(IngredientContract source)
    {
        return new EditedIngredient(
            Guid.NewGuid(),
            source.Id,
            source.Name,
            source.ItemCategoryId,
            source.QuantityType,
            source.Quantity,
            source.DefaultItemId,
            source.DefaultItemTypeId,
            source.DefaultStoreId,
            source.AddToShoppingListByDefault,
            new ItemCategorySelector(
                new List<ItemCategorySearchResult>(0),
                string.Empty),
            new ItemSelector(
                new List<SearchItemByItemCategoryResult>(0)));
    }
}