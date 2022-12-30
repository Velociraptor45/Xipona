using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ShoppingList.Frontend.Redux.ShoppingList.States;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Stores.ToDomain
{
    public class ShoppingListStoreConverter : IToDomainConverter<ActiveStoreContract, ShoppingListStore>
    {
        public ShoppingListStore ToDomain(ActiveStoreContract contract)
        {
            var sections = contract.Sections
                .Select(s => new ShoppingListStoreSection(s.Id, s.Name, s.IsDefaultSection, s.SortingIndex))
                .ToList();
            return new ShoppingListStore(contract.Id, contract.Name, sections);
        }
    }
}