﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.StoreQueries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Queries.AllActiveStores;

public class AllActiveStoresQueryHandler : IQueryHandler<AllActiveStoresQuery, IEnumerable<StoreReadModel>>
{
    private readonly Func<CancellationToken, IStoreQueryService> _storeQueryServiceDelegate;

    public AllActiveStoresQueryHandler(Func<CancellationToken, IStoreQueryService> storeQueryServiceDelegate)
    {
        _storeQueryServiceDelegate = storeQueryServiceDelegate;
    }

    public async Task<IEnumerable<StoreReadModel>> HandleAsync(AllActiveStoresQuery query,
        CancellationToken cancellationToken)
    {
        var service = _storeQueryServiceDelegate(cancellationToken);
        return await service.GetActiveAsync();
    }
}