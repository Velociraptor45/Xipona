using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;

public class RecipeCreationService : IRecipeCreationService
{
    private readonly ILogger<RecipeCreationService> _logger;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IRecipeFactory _recipeFactory;
    private readonly IRecipeConversionService _recipeConversionService;

    public RecipeCreationService(
        Func<CancellationToken, IRecipeRepository> recipeRepositoryDelegate,
        Func<CancellationToken, IRecipeFactory> recipeFactoryDelegate,
        Func<CancellationToken, IRecipeConversionService> recipeConversionServiceDelegate,
        ILogger<RecipeCreationService> logger,
        CancellationToken cancellationToken)
    {
        _logger = logger;
        _recipeRepository = recipeRepositoryDelegate(cancellationToken);
        _recipeFactory = recipeFactoryDelegate(cancellationToken);
        _recipeConversionService = recipeConversionServiceDelegate(cancellationToken);
    }

    public async Task<RecipeReadModel> CreateAsync(RecipeCreation creation)
    {
        var recipe = await _recipeFactory.CreateNewAsync(creation);
        var storedRecipe = await _recipeRepository.StoreAsync(recipe);

        _logger.LogInformation(() => $"Created recipe {storedRecipe.Name.Value}");

        return await _recipeConversionService.ToReadModelAsync(storedRecipe);
    }
}