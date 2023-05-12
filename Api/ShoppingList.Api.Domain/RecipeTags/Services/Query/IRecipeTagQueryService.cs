using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Services.Query;

public interface IRecipeTagQueryService
{
    Task<IEnumerable<IRecipeTag>> GetAllAsync();
}