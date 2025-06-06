using Xipona.Api.Domain.RecipeTags.Models;

namespace Xipona.Api.Domain.RecipeTags.Services.Query;

public interface IRecipeTagQueryService
{
    Task<IEnumerable<IRecipeTag>> GetAllAsync();
}