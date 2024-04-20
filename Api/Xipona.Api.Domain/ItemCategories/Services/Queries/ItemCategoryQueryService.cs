using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Ports;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Reasons;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Shared;

namespace ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Queries;

public class ItemCategoryQueryService : IItemCategoryQueryService
{
    private readonly IItemCategoryRepository _itemCategoryRepository;

    public ItemCategoryQueryService(IItemCategoryRepository itemCategoryRepository)
    {
        _itemCategoryRepository = itemCategoryRepository;
    }

    public async Task<IEnumerable<ItemCategorySearchResultReadModel>> GetAsync(string searchInput, bool includeDeleted)
    {
        if (string.IsNullOrWhiteSpace(searchInput))
            return Enumerable.Empty<ItemCategorySearchResultReadModel>();

        var itemCategoryModels =
            await _itemCategoryRepository.FindByAsync(searchInput, includeDeleted, null);

        return itemCategoryModels.Select(model => new ItemCategorySearchResultReadModel(model.Id, model.Name));
    }

    public async Task<IItemCategory> GetAsync(ItemCategoryId itemCategoryId)
    {
        var itemCategory = await _itemCategoryRepository.FindActiveByAsync(itemCategoryId);
        if (itemCategory is null)
            throw new DomainException(new ItemCategoryNotFoundReason(itemCategoryId));

        return itemCategory;
    }

    public async Task<IEnumerable<ItemCategoryReadModel>> GetAllActive()
    {
        var results = await _itemCategoryRepository.FindActiveByAsync();

        return results.Select(r => new ItemCategoryReadModel(r));
    }
}