using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;

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

    public async Task<IEnumerable<ItemCategoryReadModel>> GetAsync(string searchInput)
    {
        var itemCategoryModels = await _itemCategoryRepository.FindByAsync(searchInput, _cancellationToken);

        _cancellationToken.ThrowIfCancellationRequested();

        return itemCategoryModels.Select(model => new ItemCategoryReadModel(model));
    }
}