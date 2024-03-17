using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.Xipona.Api.Domain.RecipeTags.Services.Validations;

public interface IRecipeTagValidationService
{
    Task ValidateAsync(IEnumerable<RecipeTagId> recipeTags);
}