using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Queries;

public interface IItemCategoryQueryService
{
    Task<IEnumerable<ItemCategoryReadModel>> GetAsync(string searchInput);

    Task<IItemCategory> GetAsync(ItemCategoryId itemCategoryId);

    Task<IEnumerable<ItemCategoryReadModel>> GetAllActive();
}