using ShoppingList.Api.Contracts.Queries.AllQuantityTypes;
using ShoppingList.Frontend.Models;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class QuantityTypeContractExtensions
    {
        public static QuantityType ToModel(this QuantityTypeContract contract)
        {
            return new QuantityType(contract.Id, contract.Name, contract.DefaultQuantity, contract.Pricelabel);
        }
    }
}