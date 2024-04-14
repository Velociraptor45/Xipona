﻿using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.ItemById;

public class ItemByIdQueryHandler : IQueryHandler<ItemByIdQuery, ItemReadModel>
{
    private readonly Func<CancellationToken, IItemQueryService> _itemQueryServiceDelegate;

    public ItemByIdQueryHandler(Func<CancellationToken, IItemQueryService> itemQueryServiceDelegate)
    {
        _itemQueryServiceDelegate = itemQueryServiceDelegate;
    }

    public async Task<ItemReadModel> HandleAsync(ItemByIdQuery query, CancellationToken cancellationToken)
    {
        var service = _itemQueryServiceDelegate(cancellationToken);
        return await service.GetAsync(query.ItemId);
    }
}