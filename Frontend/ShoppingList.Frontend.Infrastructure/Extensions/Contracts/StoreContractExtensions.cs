using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ShoppingList.Frontend.Models;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class StoreContractExtensions
    {
        public static Store ToModel(this StoreContract contract)
        {
            return new Store(contract.Id, contract.Name);
        }
    }
}