using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.Xipona.Api.Domain.RecipeTags.Services.Creation;

public interface IRecipeTagCreationService
{
    Task<IRecipeTag> CreateAsync(string name);
}