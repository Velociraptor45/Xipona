using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Item
{
    public static class ItemAvailabilityContractExtensions
    {
        public static ShortAvailability ToDomain(this ItemAvailabilityContract contract)
        {
            return new ShortAvailability(new StoreId(contract.StoreId), contract.Price,
                new StoreItemSectionId(contract.DefaultSectionId));
        }
    }
}