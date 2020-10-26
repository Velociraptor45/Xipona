using ShoppingList.Api.Contracts.SharedContracts;
using ShoppingList.Frontend.Models;

namespace ShoppingList.Frontend.WebApp.Extensions.Contracts
{
    public static class StoreContractExtensions
    {
        public static Store ToModel(this StoreContract contract)
        {
            return new Store(contract.Id, contract.Name);
        }
    }
}