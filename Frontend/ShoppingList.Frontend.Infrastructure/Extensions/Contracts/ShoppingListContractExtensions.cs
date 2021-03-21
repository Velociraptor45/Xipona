using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Frontend.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class ShoppingListContractExtensions
    {
        public static ShoppingListRoot ToModel(this ShoppingListContract contract)
        {
            return new ShoppingListRoot(contract.Id, contract.CompletionDate, contract.Store.ToModel(),
                contract.Sections.Select(section => section.ToModel()));
        }
    }
}