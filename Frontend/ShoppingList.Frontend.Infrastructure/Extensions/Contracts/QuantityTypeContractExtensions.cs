using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Frontend.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class QuantityTypeContractExtensions
    {
        public static QuantityType ToModel(this QuantityTypeContract contract)
        {
            return new QuantityType(contract.Id, contract.Name, contract.DefaultQuantity, contract.Pricelabel);
        }
    }
}