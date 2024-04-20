﻿using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Recipes;

public class RecipeContractConverter : IToContractConverter<RecipeReadModel, RecipeContract>
{
    public RecipeContract ToContract(RecipeReadModel source)
    {
        var steps = source.PreparationSteps.Select(s => new PreparationStepContract(
            s.Id,
            s.Instruction.Value,
            s.SortingIndex));

        var ingredients = source.Ingredients.Select(i => new IngredientContract(
            i.Id,
            i.Name,
            i.ItemCategoryId,
            i.QuantityType.ToInt(),
            i.Quantity.Value,
            i.ShoppingListProperties?.DefaultItemId,
            i.ShoppingListProperties?.DefaultItemTypeId,
            i.ShoppingListProperties?.DefaultStoreId,
            i.ShoppingListProperties?.AddToShoppingListByDefault));

        return new RecipeContract(
            source.Id,
            source.Name,
            source.NumberOfServings,
            ingredients,
            steps,
            source.Tags.Select(t => t.Value));
    }
}