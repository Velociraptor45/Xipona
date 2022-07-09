using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToDomain
{
    public class ShoppingListStoreConverter : IToDomainConverter<ShoppingListStoreContract, ShoppingListStore>
    {
        public ShoppingListStore ToDomain(ShoppingListStoreContract source)
        {
            return new ShoppingListStore(
                source.Id,
                source.Name);
        }
    }
}