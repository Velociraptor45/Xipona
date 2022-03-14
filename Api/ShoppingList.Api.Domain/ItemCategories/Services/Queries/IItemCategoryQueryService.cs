using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Queries;

public interface IItemCategoryQueryService
{
    Task<IEnumerable<ItemCategoryReadModel>> GetAsync(string searchInput);

    Task<IEnumerable<ItemCategoryReadModel>> GetAllActive();
}