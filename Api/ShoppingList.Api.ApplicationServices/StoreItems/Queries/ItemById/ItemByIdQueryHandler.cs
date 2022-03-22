﻿using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.ItemById;

public class ItemByIdQueryHandler : IQueryHandler<ItemByIdQuery, StoreItemReadModel>
{
    private readonly Func<CancellationToken, IItemQueryService> _itemQueryServiceDelegate;

    public ItemByIdQueryHandler(Func<CancellationToken, IItemQueryService> itemQueryServiceDelegate)
    {
        _itemQueryServiceDelegate = itemQueryServiceDelegate;
    }

    public async Task<StoreItemReadModel> HandleAsync(ItemByIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var service = _itemQueryServiceDelegate(cancellationToken);
        return await service.GetAsync(query.ItemId);
    }
}