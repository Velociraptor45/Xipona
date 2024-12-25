using Microsoft.Extensions.Logging;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Recipes.Ports;
using ProjectHermes.Xipona.Api.Domain.Recipes.Reasons;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Shared;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Services.Creations;

public class RecipeCreationService : IRecipeCreationService
{
    private readonly ILogger<RecipeCreationService> _logger;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IRecipeFactory _recipeFactory;
    private readonly IRecipeConversionService _recipeConversionService;

    public RecipeCreationService(IRecipeRepository recipeRepository, IRecipeFactory recipeFactory,
        IRecipeConversionService recipeConversionService, ILogger<RecipeCreationService> logger)
    {
        _logger = logger;
        _recipeRepository = recipeRepository;
        _recipeFactory = recipeFactory;
        _recipeConversionService = recipeConversionService;
    }

    public async Task<RecipeReadModel> CreateAsync(RecipeCreation creation)
    {
        if (creation.SideDishId is not null)
        {
            var sideDishExists = await _recipeRepository.Exists(creation.SideDishId.Value);
            if (!sideDishExists)
                throw new DomainException(new RecipeNotFoundReason(creation.SideDishId.Value));
        }

        var recipe = await _recipeFactory.CreateNewAsync(creation);
        var storedRecipe = await _recipeRepository.StoreAsync(recipe);

        _logger.LogInformation(() => "Created recipe '{RecipeName}' with id '{RecipeId}'", storedRecipe.Name.Value,
            storedRecipe.Id.Value);

        return await _recipeConversionService.ToReadModelAsync(storedRecipe);
    }
}