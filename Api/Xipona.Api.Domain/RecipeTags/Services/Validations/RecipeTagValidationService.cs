using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Ports;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.RecipeTags.Services.Validations;

public class RecipeTagValidationService : IRecipeTagValidationService
{
    private readonly IRecipeTagRepository _recipeTagRepository;

    public RecipeTagValidationService(IRecipeTagRepository recipeTagRepository)
    {
        _recipeTagRepository = recipeTagRepository;
    }

    public async Task ValidateAsync(IEnumerable<RecipeTagId> recipeTags)
    {
        var recipeTagIds = recipeTags.ToList();
        var nonExistingTagIds = (await _recipeTagRepository.FindNonExistingInAsync(recipeTagIds)).ToList();
        if (nonExistingTagIds.Any())
        {
            throw new DomainException(new InvalidRecipeTagIdsReason(nonExistingTagIds));
        }
    }
}