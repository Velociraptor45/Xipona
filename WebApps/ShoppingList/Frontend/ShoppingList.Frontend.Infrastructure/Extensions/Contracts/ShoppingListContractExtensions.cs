using ShoppingList.Api.Contracts.SharedContracts;
using ShoppingList.Frontend.Models;
using System.Linq;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class ShoppingListContractExtensions
    {
        public static ShoppingListRoot ToModel(this ShoppingListContract contract)
        {
            return new ShoppingListRoot(contract.Id, contract.CompletionDate, contract.Store.ToModel(),
                contract.Items.Select(item => item.ToModel()));
        }
    }
}