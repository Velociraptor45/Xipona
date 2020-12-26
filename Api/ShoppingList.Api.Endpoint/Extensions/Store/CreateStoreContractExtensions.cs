using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.CreateStore;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Store
{
    public static class CreateStoreContractExtensions
    {
        public static StoreCreationInfo ToDomain(this CreateStoreContract contract)
        {
            return new StoreCreationInfo(new StoreId(0), contract.Name);
        }
    }
}