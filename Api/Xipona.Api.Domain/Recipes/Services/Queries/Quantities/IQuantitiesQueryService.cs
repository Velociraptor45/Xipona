﻿namespace ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries.Quantities;

public interface IQuantitiesQueryService
{
    IEnumerable<IngredientQuantityTypeReadModel> GetAllIngredientQuantityTypes();
}