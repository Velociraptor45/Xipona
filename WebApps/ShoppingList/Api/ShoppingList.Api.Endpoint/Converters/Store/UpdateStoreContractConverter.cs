using ShoppingList.Api.Contracts.Commands.UpdateStore;
using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Endpoint.Converters.Store
{
    public static class UpdateStoreContractConverter
    {
        public static Domain.Models.Store ToDomain(this UpdateStoreContract contract)
        {
            return new Domain.Models.Store(
                new StoreId(contract.Id),
                contract.Name,
                contract.IsDeleted);
        }
    }
}