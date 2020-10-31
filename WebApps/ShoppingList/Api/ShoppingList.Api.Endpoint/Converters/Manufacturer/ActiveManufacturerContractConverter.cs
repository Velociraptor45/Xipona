using ShoppingList.Api.Contracts.Queries.AllActiveManufacturers;
using ShoppingList.Api.Domain.Queries.SharedModels;

namespace ShoppingList.Api.Endpoint.Converters.Manufacturer
{
    public static class ActiveManufacturerContractConverter
    {
        public static ActiveManufacturerContract ToActiveContract(this ManufacturerReadModel readModel)
        {
            return new ActiveManufacturerContract(readModel.Id.Value, readModel.Name);
        }
    }
}