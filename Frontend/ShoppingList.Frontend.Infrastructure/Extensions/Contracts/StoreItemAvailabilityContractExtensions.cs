using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ShoppingList.Frontend.Models.Items;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class StoreItemAvailabilityContractExtensions
    {
        public static StoreItemAvailability ToModel(this StoreItemAvailabilityContract contract)
        {
            return new StoreItemAvailability(contract.StoreId, contract.Price);
        }
    }
}