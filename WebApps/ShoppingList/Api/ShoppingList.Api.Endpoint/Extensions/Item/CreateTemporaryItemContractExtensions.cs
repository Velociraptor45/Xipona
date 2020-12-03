using ShoppingList.Api.Contracts.Commands.CreateTemporaryItem;
using ShoppingList.Api.Domain.Commands.CreateTemporaryItem;

namespace ShoppingList.Api.Endpoint.Extensions.Item
{
    public static class CreateTemporaryItemContractExtensions
    {
        public static TemporaryItemCreation ToDomain(this CreateTemporaryItemContract contract)
        {
            return new TemporaryItemCreation(contract.ClientSideId, contract.Name, contract.Availability.ToDomain());
        }
    }
}