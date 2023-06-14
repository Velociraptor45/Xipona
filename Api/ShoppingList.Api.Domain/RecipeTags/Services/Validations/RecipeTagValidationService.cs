using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Ports;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Services.Validations;
public class RecipeTagValidationService : IRecipeTagValidationService
{
    private readonly IRecipeTagRepository _recipeTagRepository;

    public RecipeTagValidationService(
        Func<CancellationToken, IRecipeTagRepository> recipeTagRepositoryDelegate,
        CancellationToken cancellationToken)
    {
        _recipeTagRepository = recipeTagRepositoryDelegate(cancellationToken);
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
