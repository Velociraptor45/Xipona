using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.UpdateStore;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Store
{
    public static class UpdateStoreContractExtensions
    {
        public static StoreUpdate ToDomain(this UpdateStoreContract contract)
        {
            return new StoreUpdate(
                new StoreId(contract.Id),
                contract.Name);
        }
    }
}