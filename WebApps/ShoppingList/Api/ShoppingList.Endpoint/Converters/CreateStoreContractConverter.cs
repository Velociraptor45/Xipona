using ShoppingList.Contracts.Commands.CreateStore;
using ShoppingList.Domain.Models;

namespace ShoppingList.Endpoint.Converters
{
    public static class CreateStoreContractConverter
    {
        public static Store ToDomain(this CreateStoreContract contract)
        {
            return new Store(new StoreId(0),
                contract.Name,
                false);
        }
    }
}