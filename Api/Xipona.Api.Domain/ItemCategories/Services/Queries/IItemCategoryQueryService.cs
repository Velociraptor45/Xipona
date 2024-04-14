using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Shared;

namespace ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Queries;

public interface IItemCategoryQueryService
{
    Task<IEnumerable<ItemCategorySearchResultReadModel>> GetAsync(string searchInput, bool includeDeleted);

    Task<IItemCategory> GetAsync(ItemCategoryId itemCategoryId);

    Task<IEnumerable<ItemCategoryReadModel>> GetAllActive();
}