using ShoppingList.Contracts.Commands.CreateStore;
using ShoppingList.Domain.Models;

using Models = ShoppingList.Domain.Models;

namespace ShoppingList.Endpoint.Converters
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