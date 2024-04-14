﻿using ProjectHermes.Xipona.Api.Domain.Recipes.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries.Quantities;

public class QuantitiesQueryService : IQuantitiesQueryService
{
    public IEnumerable<IngredientQuantityTypeReadModel> GetAllIngredientQuantityTypes()
    {
        var readModels = Enum.GetValues(typeof(IngredientQuantityType))
            .Cast<IngredientQuantityType>()
            .Select(v => new IngredientQuantityTypeReadModel(v))
            .ToArray();

        return readModels;
    }
}