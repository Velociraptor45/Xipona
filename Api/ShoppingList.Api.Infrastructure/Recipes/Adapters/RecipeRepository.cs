﻿using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Ports;
using ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Contexts;
using Recipe = ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Entities.Recipe;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Adapters;

public class RecipeRepository : IRecipeRepository
{
    private readonly RecipeContext _dbContext;
    private readonly IToDomainConverter<Recipe, IRecipe> _toModelConverter;
    private readonly IToContractConverter<IRecipe, Recipe> _toEntityConverter;
    private readonly CancellationToken _cancellationToken;

    public RecipeRepository(
        RecipeContext dbContext,
        IToDomainConverter<Recipe, IRecipe> toModelConverter,
        IToContractConverter<IRecipe, Recipe> toEntityConverter,
        CancellationToken cancellationToken)
    {
        _dbContext = dbContext;
        _toModelConverter = toModelConverter;
        _toEntityConverter = toEntityConverter;
        _cancellationToken = cancellationToken;
    }

    public async Task<IRecipe> StoreAsync(IRecipe recipe)
    {
        var existingEntity = await FindTrackedEntityBy(recipe.Id);

        if (existingEntity is null)
        {
            var newEntity = _toEntityConverter.ToContract(recipe);
            _dbContext.Add(newEntity);
        }
        else
        {
            var updatedEntity = _toEntityConverter.ToContract(recipe);
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
            _dbContext.Entry(existingEntity).State = EntityState.Modified;

            UpdateOrAddIngredients(existingEntity, updatedEntity);
            DeleteIngredients(existingEntity, updatedEntity);
            UpdateOrAddPreparationSteps(existingEntity, updatedEntity);
            DeletePreparationSteps(existingEntity, updatedEntity);
        }

        await _dbContext.SaveChangesAsync(_cancellationToken);
        var entity = await GetRecipeQuery().FirstAsync(r => r.Id == recipe.Id.Value, _cancellationToken);
        return _toModelConverter.ToDomain(entity);
    }

    private async Task<Recipe?> FindTrackedEntityBy(RecipeId id)
    {
        return await _dbContext.Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.PreparationSteps)
            .FirstOrDefaultAsync(r => r.Id == id.Value, _cancellationToken);
    }

    private IQueryable<Recipe> GetRecipeQuery()
    {
        return _dbContext.Recipes.AsNoTracking()
            .Include(r => r.Ingredients)
            .Include(r => r.PreparationSteps);
    }

    private void UpdateOrAddIngredients(Recipe existing, Recipe updated)
    {
        foreach (var updatedIngredient in updated.Ingredients)
        {
            var existingIngredient = existing.Ingredients
                .FirstOrDefault(t => t.Id == updatedIngredient.Id);

            if (existingIngredient == null)
            {
                existing.Ingredients.Add(updatedIngredient);
            }
            else
            {
                _dbContext.Entry(existingIngredient).CurrentValues.SetValues(updatedIngredient);
            }
        }
    }

    private void DeleteIngredients(Recipe existing, Recipe updated)
    {
        foreach (var type in existing.Ingredients)
        {
            bool hasExistingIngredient = updated.Ingredients.Any(t => t.Id == type.Id);
            if (!hasExistingIngredient)
            {
                _dbContext.Remove(type);
            }
        }
    }

    private void UpdateOrAddPreparationSteps(Recipe existing, Recipe updated)
    {
        foreach (var updatedStep in updated.PreparationSteps)
        {
            var existingStep = existing.PreparationSteps
                .FirstOrDefault(t => t.Id == updatedStep.Id);

            if (existingStep == null)
            {
                existing.PreparationSteps.Add(updatedStep);
            }
            else
            {
                _dbContext.Entry(existingStep).CurrentValues.SetValues(updatedStep);
            }
        }
    }

    private void DeletePreparationSteps(Recipe existing, Recipe updated)
    {
        foreach (var type in existing.PreparationSteps)
        {
            bool hasExistingStep = updated.PreparationSteps.Any(t => t.Id == type.Id);
            if (!hasExistingStep)
            {
                _dbContext.Remove(type);
            }
        }
    }
}