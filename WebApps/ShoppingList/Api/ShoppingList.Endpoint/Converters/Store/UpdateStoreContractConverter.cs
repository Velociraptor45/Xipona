using ShoppingList.Contracts.Commands.UpdateStore;
using Models = ShoppingList.Domain.Models;

namespace ShoppingList.Endpoint.Converters.Store
{
    public static class UpdateStoreContractConverter
    {
        public static Models.Store ToDomain(this UpdateStoreContract contract)
        {
            return new Models.Store(
                new Models.StoreId(contract.Id),
                contract.Name,
                contract.IsDeleted);
        }
    }
}