using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.AllActiveItemCategories;

public class AllActiveItemCategoriesQueryHandler
    : IQueryHandler<AllActiveItemCategoriesQuery, IEnumerable<ItemCategoryReadModel>>
{
    private readonly IItemCategoryRepository _itemCategoryRepository;

    public AllActiveItemCategoriesQueryHandler(IItemCategoryRepository itemCategoryRepository)
    {
        _itemCategoryRepository = itemCategoryRepository;
    }

    public async Task<IEnumerable<ItemCategoryReadModel>> HandleAsync(AllActiveItemCategoriesQuery query, CancellationToken cancellationToken)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        var results = await _itemCategoryRepository.FindActiveByAsync(cancellationToken);

        return results.Select(r => new ItemCategoryReadModel(r));
    }
}