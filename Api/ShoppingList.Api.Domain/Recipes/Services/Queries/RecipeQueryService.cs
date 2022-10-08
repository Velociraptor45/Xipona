using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

public class RecipeQueryService : IRecipeQueryService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly ILogger<RecipeQueryService> _logger;

    public RecipeQueryService(
        Func<CancellationToken, IRecipeRepository> recipeRepositoryDelegate,
        ILogger<RecipeQueryService> logger,
        CancellationToken cancellationToken)
    {
        _recipeRepository = recipeRepositoryDelegate(cancellationToken);
        _logger = logger;
    }

    public async Task<IRecipe> GetAsync(RecipeId id)
    {
        var recipe = await _recipeRepository.FindByAsync(id);
        if (recipe is null)
            throw new DomainException(new RecipeNotFoundReason(id));

        return recipe;
    }

    public async Task<IEnumerable<RecipeSearchResult>> SearchByNameAsync(string searchInput)
    {
        if (string.IsNullOrWhiteSpace(searchInput))
            return Enumerable.Empty<RecipeSearchResult>();

        var results = (await _recipeRepository.SearchByAsync(searchInput)).ToList();

        _logger.LogInformation(() => $"Found {results.Count} result{(results.Count != 1 ? 's' : string.Empty)} for input '{searchInput}'");

        return results;
    }
}