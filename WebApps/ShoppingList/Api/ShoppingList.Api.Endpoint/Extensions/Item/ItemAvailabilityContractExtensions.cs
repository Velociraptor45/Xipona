using ShoppingList.Api.Contracts.Commands.SharedContracts;
using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Endpoint.Extensions.Item
{
    public static class ItemAvailabilityContractExtensions
    {
        public static StoreItemAvailability ToDomain(this ItemAvailabilityContract contract)
        {
            return new StoreItemAvailability(new StoreId(contract.StoreId), contract.Price);
        }
    }
}