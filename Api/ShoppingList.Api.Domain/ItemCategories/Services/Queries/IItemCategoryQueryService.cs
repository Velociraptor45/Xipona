using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Queries;

public interface IItemCategoryQueryService
{
    Task<IEnumerable<ItemCategoryReadModel>> GetAsync(string searchInput);
}