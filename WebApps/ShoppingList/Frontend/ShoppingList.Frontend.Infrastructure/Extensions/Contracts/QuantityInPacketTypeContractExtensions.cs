using ShoppingList.Api.Contracts.Queries.AllQuantityTypes;
using ShoppingList.Frontend.Models;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class QuantityInPacketTypeContractExtensions
    {
        public static QuantityInPacketType ToModel(this QuantityInPacketTypeContract contract)
        {
            return new QuantityInPacketType(contract.Id, contract.Name);
        }
    }
}