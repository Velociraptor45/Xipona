using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.ItemCategorySearch;

public class ItemCategorySearchQueryHandler : IQueryHandler<ItemCategorySearchQuery, IEnumerable<ItemCategoryReadModel>>
{
    private readonly IItemCategoryRepository _itemCategoryRepository;

    public ItemCategorySearchQueryHandler(IItemCategoryRepository itemCategoryRepository)
    {
        _itemCategoryRepository = itemCategoryRepository;
    }

    public async Task<IEnumerable<ItemCategoryReadModel>> HandleAsync(ItemCategorySearchQuery query,
        CancellationToken cancellationToken)
    {
        var itemCategoryModels = await _itemCategoryRepository.FindByAsync(query.SearchInput,
            cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return itemCategoryModels.Select(model => new ItemCategoryReadModel(model));
    }
}