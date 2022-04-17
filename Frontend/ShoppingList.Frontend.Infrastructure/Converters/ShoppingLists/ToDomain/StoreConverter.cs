using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToDomain
{
    public class StoreConverter : IToDomainConverter<ShoppingListStoreContract, Store>
    {
        public Store ToDomain(ShoppingListStoreContract source)
        {
            return new Store(
                source.Id,
                source.Name,
                Enumerable.Empty<Section>()); //todo #109
        }
    }
}