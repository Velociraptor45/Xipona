using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Frontend.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class ManufacturerContractExtensions
    {
        public static Manufacturer ToModel(this ManufacturerContract contract)
        {
            return new Manufacturer(contract.Id, contract.Name);
        }
    }
}