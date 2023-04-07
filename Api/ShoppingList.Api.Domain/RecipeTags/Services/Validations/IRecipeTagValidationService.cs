using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Services.Validations;

public interface IRecipeTagValidationService
{
    Task ValidateAsync(IEnumerable<RecipeTagId> recipeTags);
}