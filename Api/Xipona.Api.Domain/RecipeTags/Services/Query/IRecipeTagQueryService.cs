using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.Xipona.Api.Domain.RecipeTags.Services.Query;

public interface IRecipeTagQueryService
{
    Task<IEnumerable<IRecipeTag>> GetAllAsync();
}