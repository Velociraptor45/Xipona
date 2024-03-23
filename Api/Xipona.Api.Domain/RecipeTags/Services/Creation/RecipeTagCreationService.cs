using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Ports;

namespace ProjectHermes.Xipona.Api.Domain.RecipeTags.Services.Creation;

public class RecipeTagCreationService : IRecipeTagCreationService
{
    private readonly IRecipeTagFactory _recipeTagFactory;
    private readonly IRecipeTagRepository _recipeTagRepository;

    public RecipeTagCreationService(IRecipeTagFactory recipeTagFactory, IRecipeTagRepository recipeTagRepository)
    {
        _recipeTagFactory = recipeTagFactory;
        _recipeTagRepository = recipeTagRepository;
    }

    public async Task<IRecipeTag> CreateAsync(string name)
    {
        var recipeTag = _recipeTagFactory.CreateNew(name);
        recipeTag = await _recipeTagRepository.StoreAsync(recipeTag);
        return recipeTag;
    }
}