using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Queries;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Queries.ItemCategorySearch;

public class ItemCategorySearchQueryHandler :
    IQueryHandler<ItemCategorySearchQuery, IEnumerable<ItemCategorySearchResultReadModel>>
{
    private readonly Func<CancellationToken, IItemCategoryQueryService> _itemCategoryQueryServiceDelegate;

    public ItemCategorySearchQueryHandler(
        Func<CancellationToken, IItemCategoryQueryService> itemCategoryQueryServiceDelegate)
    {
        _itemCategoryQueryServiceDelegate = itemCategoryQueryServiceDelegate;
    }

    public async Task<IEnumerable<ItemCategorySearchResultReadModel>> HandleAsync(ItemCategorySearchQuery query,
        CancellationToken cancellationToken)
    {
        var service = _itemCategoryQueryServiceDelegate(cancellationToken);
        return await service.GetAsync(query.SearchInput, query.IncludeDeleted);
    }
}