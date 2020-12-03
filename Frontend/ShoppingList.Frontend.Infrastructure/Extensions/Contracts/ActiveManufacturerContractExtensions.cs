using ShoppingList.Api.Contracts.Queries.AllActiveManufacturers;
using ShoppingList.Frontend.Models;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class ActiveManufacturerContractExtensions
    {
        public static Manufacturer ToModel(this ActiveManufacturerContract contract)
        {
            return new Manufacturer(contract.Id, contract.Name);
        }
    }
}