using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Frontend.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class StoreContractExtensions
    {
        public static Store ToModel(this StoreContract contract)
        {
            return new Store(contract.Id, contract.Name);
        }
    }
}