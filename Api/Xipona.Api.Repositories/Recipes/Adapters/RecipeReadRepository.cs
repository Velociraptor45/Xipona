using Microsoft.EntityFrameworkCore;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Ports;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Contexts;

namespace ProjectHermes.Xipona.Api.Repositories.Recipes.Adapters;
public class RecipeReadRepository : IRecipeReadRepository
{
    private readonly RecipeContext _dbContext;
    private readonly IToDomainConverter<Entities.Recipe, SideDishReadModel> _sideDishConverter;
    private readonly CancellationToken _cancellationToken;

    public RecipeReadRepository(RecipeContext dbContext,
        IToDomainConverter<Entities.Recipe, SideDishReadModel> sideDishConverter,
        CancellationToken cancellationToken)
    {
        _dbContext = dbContext;
        _sideDishConverter = sideDishConverter;
        _cancellationToken = cancellationToken;
    }

    public async Task<SideDishReadModel?> GetSideDishAsync(RecipeId recipeId)
    {
        var entry = await _dbContext.Recipes
            .FirstOrDefaultAsync(s => s.Id == recipeId, _cancellationToken);

        if (entry is null)
            return null;

        return _sideDishConverter.ToDomain(entry);
    }
}