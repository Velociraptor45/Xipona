using ProjectHermes.ShoppingList.Api.Contracts.Manufacturer.Queries.AllActiveManufacturers;
using ProjectHermes.ShoppingList.Frontend.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class ActiveManufacturerContractExtensions
    {
        public static Manufacturer ToModel(this ActiveManufacturerContract contract)
        {
            return new Manufacturer(contract.Id, contract.Name);
        }
    }
}