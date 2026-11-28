using Xipona.Api.Domain.RecipeTags.Models;

namespace Xipona.Api.Domain.RecipeTags.Services.Validations;

public interface IRecipeTagValidationService
{
    Task ValidateAsync(IEnumerable<RecipeTagId> recipeTags);
}