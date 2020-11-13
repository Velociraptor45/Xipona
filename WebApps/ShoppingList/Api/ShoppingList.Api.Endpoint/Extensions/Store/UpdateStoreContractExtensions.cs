using ShoppingList.Api.Contracts.Commands.UpdateStore;
using ShoppingList.Api.Domain.Commands.UpdateStore;
using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Endpoint.Extensions.Store
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