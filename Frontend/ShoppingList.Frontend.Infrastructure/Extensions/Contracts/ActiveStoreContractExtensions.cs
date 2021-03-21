using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Frontend.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class ActiveStoreContractExtensions
    {
        public static Store ToModel(this ActiveStoreContract contract)
        {
            return new Store(contract.Id, contract.Name, contract.Sections.Select(s => s.ToModel()));
        }
    }
}