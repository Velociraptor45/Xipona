using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Frontend.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class QuantityTypeInPacketContractExtensions
    {
        public static QuantityTypeInPacket ToModel(this QuantityTypeInPacketContract contract)
        {
            return new QuantityTypeInPacket(contract.Id, contract.Name, contract.QuantityLabel);
        }
    }
}