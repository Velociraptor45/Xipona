using ShoppingList.Domain.Converters;
using ShoppingList.Domain.Ports;
using ShoppingList.Domain.Queries.SharedModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Queries.ActiveShoppingListByStoreId
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
            var listModel = await shoppingListRepository.FindActiveByStoreIdAsync(query.StoreId, cancellationToken);

            if (listModel == null)
                throw new ArgumentException($"No shopping list for store {query.StoreId.Value} found.");

            return listModel.ToReadModel();
        }
    }
}