using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.SharedModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId
{
    public class ActiveShoppingListByStoreIdQueryHandler
        : IQueryHandler<ActiveShoppingListByStoreIdQuery, ShoppingListReadModel>
    {
        private readonly IShoppingListRepository shoppingListRepository;

        public ActiveShoppingListByStoreIdQueryHandler(IShoppingListRepository shoppingListRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
        }

        public async Task<ShoppingListReadModel> HandleAsync(ActiveShoppingListByStoreIdQuery query,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var listModel = await shoppingListRepository.FindActiveByAsync(query.StoreId, cancellationToken);

            if (listModel == null)
                throw new ArgumentException($"No shopping list for store {query.StoreId.Value} found.");

            return listModel.ToReadModel();
        }
    }
}