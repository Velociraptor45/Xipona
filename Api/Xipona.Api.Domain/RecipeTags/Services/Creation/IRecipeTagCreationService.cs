using Xipona.Api.Domain.RecipeTags.Models;

namespace Xipona.Api.Domain.RecipeTags.Services.Creation;

public interface IRecipeTagCreationService
{
    Task<IRecipeTag> CreateAsync(string name);
}