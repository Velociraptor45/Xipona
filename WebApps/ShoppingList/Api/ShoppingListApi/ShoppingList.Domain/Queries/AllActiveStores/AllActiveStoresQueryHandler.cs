using ShoppingList.Domain.Converters;
using ShoppingList.Domain.Ports;
using ShoppingList.Domain.Queries.SharedModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Queries.AllActiveStores
{
    public class AllActiveStoresQueryHandler : IQueryHandler<AllActiveStoresQuery, IEnumerable<StoreReadModel>>
    {
        private readonly IShoppingListRepository shoppingListRepository;

        public AllActiveStoresQueryHandler(IShoppingListRepository shoppingListRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
        }

        public async Task<IEnumerable<StoreReadModel>> HandleAsync(AllActiveStoresQuery query, CancellationToken cancellationToken)
        {
            var activeStores = await shoppingListRepository.FindActiveStoresAsync();

            return activeStores.Select(store => store.ToReadModel());
        }
    }
}