using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Models.Items;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class StoreItemAvailabilityContractExtensions
    {
        public static StoreItemAvailability ToModel(this StoreItemAvailabilityContract contract)
        {
            return new StoreItemAvailability(contract.Store.ToModel(), contract.Price,
                contract.DefaultSection.ToStoreItemSectionModel());
        }
    }
}