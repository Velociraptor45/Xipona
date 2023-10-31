using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Ports;
using ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Contexts;
using RecipeTag = ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Entities.RecipeTag;

namespace ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Adapters;

public class RecipeTagRepository : IRecipeTagRepository
{
    private readonly RecipeTagContext _dbContext;
    private readonly IToDomainConverter<RecipeTag, IRecipeTag> _toDomainConverter;
    private readonly IToContractConverter<IRecipeTag, RecipeTag> _toContractConverter;
    private readonly ILogger<RecipeTagRepository> _logger;
    private readonly CancellationToken _cancellationToken;

    public RecipeTagRepository(
        RecipeTagContext dbContext,
        IToDomainConverter<RecipeTag, IRecipeTag> toDomainConverter,
        IToContractConverter<IRecipeTag, RecipeTag> toContractConverter,
        ILogger<RecipeTagRepository> logger,
        CancellationToken cancellationToken)
    {
        _dbContext = dbContext;
        _toDomainConverter = toDomainConverter;
        _toContractConverter = toContractConverter;
        _logger = logger;
        _cancellationToken = cancellationToken;
    }

    public async Task<IEnumerable<RecipeTagId>> FindNonExistingInAsync(IEnumerable<RecipeTagId> recipeTagIds)
    {
        var rawTagIds = recipeTagIds.Select(r => r.Value).ToList();

        var existingRecipeTagIds = await _dbContext.RecipeTags
            .Where(r => rawTagIds.Contains(r.Id))
            .Select(r => r.Id)
            .ToListAsync(_cancellationToken);

        return rawTagIds
            .Except(existingRecipeTagIds)
            .Select(t => new RecipeTagId(t));
    }

    public async Task<IEnumerable<IRecipeTag>> FindAllAsync()
    {
        var entities = await _dbContext.RecipeTags.ToListAsync(_cancellationToken);
        return _toDomainConverter.ToDomain(entities);
    }

    public async Task<IRecipeTag> StoreAsync(IRecipeTag recipeTag)
    {
        var existingEntity = await FindTrackedEntityBy(recipeTag.Id);

        if (existingEntity is null)
        {
            var newEntity = _toContractConverter.ToContract(recipeTag);
            await _dbContext.RecipeTags.AddAsync(newEntity, _cancellationToken);
        }
        else
        {
            var updatedEntity = _toContractConverter.ToContract(recipeTag);

            var existingRowVersion = existingEntity.RowVersion;

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
            _dbContext.Entry(existingEntity).Property(r => r.RowVersion).OriginalValue = existingRowVersion;
        }

        try
        {
            await _dbContext.SaveChangesAsync(_cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogInformation(ex, () => "Saving recipe tag '{RecipeTagId}' failed due to concurrency violation",
                recipeTag.Id.Value);
            throw new DomainException(new ModelOutOfDateReason());
        }

        var entity = await _dbContext.RecipeTags.FirstAsync(r => r.Id == recipeTag.Id, _cancellationToken);
        return _toDomainConverter.ToDomain(entity);
    }

    private async Task<RecipeTag?> FindTrackedEntityBy(RecipeTagId id)
    {
        return await _dbContext.RecipeTags
            .FirstOrDefaultAsync(rt => rt.Id == id, _cancellationToken);
    }
}