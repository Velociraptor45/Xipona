using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemFilterResults;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.SearchItems
{
    public class SearchItemQueryHandler : IQueryHandler<SearchItemQuery, IEnumerable<ItemFilterResultReadModel>>
    {
        private readonly Func<CancellationToken, IItemQueryService> _itemQueryServiceDelegate;

        public SearchItemQueryHandler(Func<CancellationToken, IItemQueryService> itemQueryServiceDelegate)
        {
            _itemQueryServiceDelegate = itemQueryServiceDelegate;
        }

        public async Task<IEnumerable<ItemFilterResultReadModel>> HandleAsync(SearchItemQuery query,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query);

            var itemQueryService = _itemQueryServiceDelegate(cancellationToken);
            return await itemQueryService.SearchAsync(query.SearchInput);
        }
    }
}