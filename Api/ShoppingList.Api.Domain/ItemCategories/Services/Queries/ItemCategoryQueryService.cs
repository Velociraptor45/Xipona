using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Queries;

public class ItemCategoryQueryService : IItemCategoryQueryService
{
    private readonly IItemCategoryRepository _itemCategoryRepository;
    private readonly CancellationToken _cancellationToken;

    public ItemCategoryQueryService(IItemCategoryRepository itemCategoryRepository,
        CancellationToken cancellationToken)
    {
        _itemCategoryRepository = itemCategoryRepository;
        _cancellationToken = cancellationToken;
    }

    public async Task<IEnumerable<ItemCategorySearchResultReadModel>> GetAsync(string searchInput, bool includeDeleted)
    {
        if (string.IsNullOrWhiteSpace(searchInput))
            return Enumerable.Empty<ItemCategorySearchResultReadModel>();

        var itemCategoryModels =
            await _itemCategoryRepository.FindByAsync(searchInput, includeDeleted, _cancellationToken);

        _cancellationToken.ThrowIfCancellationRequested();

        return itemCategoryModels.Select(model => new ItemCategorySearchResultReadModel(model.Id, model.Name));
    }

    public async Task<IItemCategory> GetAsync(ItemCategoryId itemCategoryId)
    {
        var itemCategory = await _itemCategoryRepository.FindByAsync(itemCategoryId, _cancellationToken);
        if (itemCategory is null)
            throw new DomainException(new ItemCategoryNotFoundReason(itemCategoryId));

        return itemCategory;
    }

    public async Task<IEnumerable<ItemCategoryReadModel>> GetAllActive()
    {
        var results = await _itemCategoryRepository.FindActiveByAsync(_cancellationToken);

        return results.Select(r => new ItemCategoryReadModel(r));
    }
}