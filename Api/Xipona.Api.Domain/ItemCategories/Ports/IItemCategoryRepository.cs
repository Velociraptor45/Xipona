using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.Xipona.Api.Domain.ItemCategories.Ports;

public interface IItemCategoryRepository
{
    Task<IEnumerable<IItemCategory>> FindByAsync(string searchInput, bool includeDeleted,
        int? limit);

    Task<IItemCategory?> FindByAsync(ItemCategoryId id);

    Task<IEnumerable<IItemCategory>> FindByAsync(IEnumerable<ItemCategoryId> ids);

    Task<IItemCategory?> FindActiveByAsync(ItemCategoryId id);

    Task<IEnumerable<IItemCategory>> FindActiveByAsync();

    Task<IItemCategory> StoreAsync(IItemCategory model);
}