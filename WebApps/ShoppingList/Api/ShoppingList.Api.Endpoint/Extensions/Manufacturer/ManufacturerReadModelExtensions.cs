using ShoppingList.Api.Contracts.Queries.AllActiveManufacturers;
using ShoppingList.Api.Contracts.SharedContracts;
using ShoppingList.Api.Domain.Queries.SharedModels;

namespace ShoppingList.Api.Endpoint.Extensions.Manufacturer
{
    public static class ManufacturerReadModelExtensions
    {
        public static ActiveManufacturerContract ToActiveContract(this ManufacturerReadModel readModel)
        {
            return new ActiveManufacturerContract(readModel.Id.Value, readModel.Name);
        }

        public static ManufacturerContract ToContract(this ManufacturerReadModel readModel)
        {
            return new ManufacturerContract(readModel.Id.Value, readModel.Name, readModel.IsDeleted);
        }
    }
}