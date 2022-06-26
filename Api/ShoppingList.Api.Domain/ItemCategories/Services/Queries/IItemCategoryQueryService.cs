using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Queries;

public interface IItemCategoryQueryService
{
    Task<IEnumerable<ItemCategorySearchResultReadModel>> GetAsync(string searchInput, bool includeDeleted);

    Task<IItemCategory> GetAsync(ItemCategoryId itemCategoryId);

    Task<IEnumerable<ItemCategoryReadModel>> GetAllActive();
}