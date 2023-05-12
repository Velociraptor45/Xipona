using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Services.Creation;

public interface IRecipeTagCreationService
{
    Task<IRecipeTag> CreateAsync(string name);
}