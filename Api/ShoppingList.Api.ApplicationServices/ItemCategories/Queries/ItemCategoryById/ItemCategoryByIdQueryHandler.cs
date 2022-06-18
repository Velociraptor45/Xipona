using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Queries.ItemCategoryById;

public class ItemCategoryByIdQueryHandler : IQueryHandler<ItemCategoryByIdQuery, IItemCategory>
{
    private readonly Func<CancellationToken, IItemCategoryQueryService> _itemCategoryQueryServiceDelegate;

    public ItemCategoryByIdQueryHandler(
        Func<CancellationToken, IItemCategoryQueryService> itemCategoryQueryServiceDelegate)
    {
        _itemCategoryQueryServiceDelegate = itemCategoryQueryServiceDelegate;
    }

    public async Task<IItemCategory> HandleAsync(ItemCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var service = _itemCategoryQueryServiceDelegate(cancellationToken);
        return await service.GetAsync(query.ItemCategoryId);
    }
}