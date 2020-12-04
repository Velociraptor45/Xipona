using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Item
{
    public static class CreateTemporaryItemContractExtensions
    {
        public static TemporaryItemCreation ToDomain(this CreateTemporaryItemContract contract)
        {
            return new TemporaryItemCreation(contract.ClientSideId, contract.Name, contract.Availability.ToDomain());
        }
    }
}