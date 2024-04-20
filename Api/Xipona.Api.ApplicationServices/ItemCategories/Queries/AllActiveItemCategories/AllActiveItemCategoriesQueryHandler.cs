using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Shared;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Queries.AllActiveItemCategories;

public class AllActiveItemCategoriesQueryHandler
    : IQueryHandler<AllActiveItemCategoriesQuery, IEnumerable<ItemCategoryReadModel>>
{
    private readonly Func<CancellationToken, IItemCategoryQueryService> _itemCategoryQueryServiceDelegate;

    public AllActiveItemCategoriesQueryHandler(
        Func<CancellationToken, IItemCategoryQueryService> itemCategoryQueryServiceDelegate)
    {
        _itemCategoryQueryServiceDelegate = itemCategoryQueryServiceDelegate;
    }

    public async Task<IEnumerable<ItemCategoryReadModel>> HandleAsync(AllActiveItemCategoriesQuery query, CancellationToken cancellationToken)
    {
        var service = _itemCategoryQueryServiceDelegate(cancellationToken);
        return await service.GetAllActive();
    }
}