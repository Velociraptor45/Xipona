using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities;
using Recipe = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.Recipe;

namespace ProjectHermes.ShoppingList.Api.Repositories.Recipes.Adapters;

public class RecipeRepository : IRecipeRepository
{
    private readonly RecipeContext _dbContext;
    private readonly IToDomainConverter<Recipe, RecipeSearchResult> _searchToModelConverter;
    private readonly IToDomainConverter<Recipe, IRecipe> _toModelConverter;
    private readonly IToContractConverter<IRecipe, Recipe> _toEntityConverter;
    private readonly ILogger<RecipeRepository> _logger;
    private readonly CancellationToken _cancellationToken;

    public RecipeRepository(
        RecipeContext dbContext,
        IToDomainConverter<Recipe, RecipeSearchResult> searchToModelConverter,
        IToDomainConverter<Recipe, IRecipe> toModelConverter,
        IToContractConverter<IRecipe, Recipe> toEntityConverter,
        ILogger<RecipeRepository> logger,
        CancellationToken cancellationToken)
    {
        _dbContext = dbContext;
        _searchToModelConverter = searchToModelConverter;
        _toModelConverter = toModelConverter;
        _toEntityConverter = toEntityConverter;
        _logger = logger;
        _cancellationToken = cancellationToken;
    }

    public async Task<IEnumerable<RecipeSearchResult>> SearchByAsync(string searchInput)
    {
        var entities = await _dbContext.Recipes.AsNoTracking()
            .Where(r => r.Name.Contains(searchInput))
            .ToListAsync(_cancellationToken);

        return _searchToModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IRecipe>> FindByContainingAllAsync(IEnumerable<RecipeTagId> recipeTagIds)
    {
        var rawRecipeTagIds = recipeTagIds.Select(t => t.Value).ToList();

        var query = GetRecipeQuery();

        foreach (Guid tagId in rawRecipeTagIds)
        {
            query = query.Where(r => r.Tags.Any(t => t.RecipeTagId == tagId));
        }

        var entities = await query.ToListAsync(_cancellationToken);

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IRecipe?> FindByAsync(RecipeId recipeId)
    {
        var entity = await GetRecipeQuery()
            .FirstOrDefaultAsync(r => r.Id == recipeId, _cancellationToken);

        return entity is null ? null : _toModelConverter.ToDomain(entity);
    }

    public async Task<IEnumerable<IRecipe>> FindByAsync(ItemId defaultItemId)
    {
        var entities = await GetRecipeQuery()
            .Where(r => r.Ingredients.Any(i => i.DefaultItemId == defaultItemId))
            .ToListAsync(_cancellationToken);

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IRecipe>> FindByAsync(ItemId defaultItemId, ItemTypeId? defaultItemTypeId, 
        StoreId defaultStoreId)
    {
        var entities = await GetRecipeQuery()
            .Where(r => r.Ingredients.Any(i => i.DefaultItemId == defaultItemId
                && i.DefaultItemTypeId == defaultItemTypeId
                && i.DefaultStoreId == defaultStoreId))
            .ToListAsync(_cancellationToken);

        return _toModelConverter.ToDomain(entities);
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

            var existingRowVersion = existingEntity.RowVersion;
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
            _dbContext.Entry(existingEntity).Property(r => r.RowVersion).OriginalValue = existingRowVersion;
            _dbContext.Entry(existingEntity).State = EntityState.Modified;

            UpdateOrAddIngredients(existingEntity, updatedEntity);
            DeleteIngredients(existingEntity, updatedEntity);
            UpdateOrAddPreparationSteps(existingEntity, updatedEntity);
            DeletePreparationSteps(existingEntity, updatedEntity);
            UpdateOrAddTags(existingEntity, updatedEntity);
            DeleteTags(existingEntity, updatedEntity);
        }

        try
        {
            await _dbContext.SaveChangesAsync(_cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogInformation(ex, () => $"Saving recipe {recipe.Id.Value} failed due to concurrency violation");
            throw new DomainException(new ModelOutOfDateReason());
        }

        var entity = await GetRecipeQuery().FirstAsync(r => r.Id == recipe.Id, _cancellationToken);
        return _toModelConverter.ToDomain(entity);
    }

    private async Task<Recipe?> FindTrackedEntityBy(RecipeId id)
    {
        return await _dbContext.Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.PreparationSteps)
            .Include(r => r.Tags)
            .FirstOrDefaultAsync(r => r.Id == id, _cancellationToken);
    }

    private IQueryable<Recipe> GetRecipeQuery()
    {
        return _dbContext.Recipes.AsNoTracking()
            .Include(r => r.Ingredients)
            .Include(r => r.PreparationSteps)
            .Include(r => r.Tags);
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

    private void UpdateOrAddTags(Recipe existing, Recipe updated)
    {
        foreach (var updatedTag in updated.Tags)
        {
            var existingTag = existing.Tags.FirstOrDefault(t => MatchesKey(t, updatedTag));

            if (existingTag == null)
            {
                existing.Tags.Add(updatedTag);
            }
            else
            {
                _dbContext.Entry(existingTag).CurrentValues.SetValues(updatedTag);
            }
        }
    }

    private void DeleteTags(Recipe existing, Recipe updated)
    {
        foreach (var type in existing.Tags)
        {
            bool hasExistingTag = updated.Tags.Any(t => MatchesKey(t, type));
            if (!hasExistingTag)
            {
                _dbContext.Remove(type);
            }
        }
    }

    private bool MatchesKey(TagsForRecipe left, TagsForRecipe right)
    {
        return left.RecipeId == right.RecipeId && left.RecipeTagId == right.RecipeTagId;
    }
}