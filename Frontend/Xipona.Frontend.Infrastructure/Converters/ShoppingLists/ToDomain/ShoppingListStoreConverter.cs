using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForShopping;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using System.Linq;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToDomain;

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