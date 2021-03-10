using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Frontend.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class StoreContractExtensions
    {
        public static Store ToModel(this ShoppingListStoreContract contract)
        {
            return new Store(contract.Id, contract.Name, Enumerable.Empty<StoreSection>());
        }
    }
}