using ShoppingList.Api.Contracts.Commands.CreateStore;
using ShoppingList.Api.Domain.Models;
using Models = ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Endpoint.Converters
{
    public static class CreateStoreContractConverter
    {
        public static Models.Store ToDomain(this CreateStoreContract contract)
        {
            return new Models.Store(new StoreId(0),
                contract.Name,
                false);
        }
    }
}