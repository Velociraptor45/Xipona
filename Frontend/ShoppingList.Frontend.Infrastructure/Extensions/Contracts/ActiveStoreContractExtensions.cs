using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
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