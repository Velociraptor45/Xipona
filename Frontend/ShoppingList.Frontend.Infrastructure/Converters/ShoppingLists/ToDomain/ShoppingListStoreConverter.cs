using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.GetActiveStoresForShopping;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ShoppingList.Frontend.Redux.ShoppingList.States;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToDomain
{
    public class ShoppingListStoreConverter : IToDomainConverter<StoreForShoppingContract, ShoppingListStore>
    {
        public ShoppingListStore ToDomain(StoreForShoppingContract contract)
        {
            var sections = contract.Sections
                .Select(s => new ShoppingListStoreSection(s.Id, s.Name, s.IsDefaultSection, s.SortingIndex))
                .ToList();
            return new ShoppingListStore(contract.Id, contract.Name, sections);
        }
    }
}