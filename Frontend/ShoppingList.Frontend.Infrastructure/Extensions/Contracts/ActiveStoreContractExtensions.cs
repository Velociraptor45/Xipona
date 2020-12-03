using ShoppingList.Api.Contracts.Queries.AllActiveStores;
using ShoppingList.Frontend.Models;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class ActiveStoreContractExtensions
    {
        public static Store ToModel(this ActiveStoreContract contract)
        {
            return new Store(contract.Id, contract.Name);
        }
    }
}