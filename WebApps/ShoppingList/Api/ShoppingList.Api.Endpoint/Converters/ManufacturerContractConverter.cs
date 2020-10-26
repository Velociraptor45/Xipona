using ShoppingList.Api.Contracts.SharedContracts;
using ShoppingList.Api.Domain.Queries.SharedModels;

namespace ShoppingList.Api.Endpoint.Converters
{
    public static class ManufacturerContractConverter
    {
        public static ManufacturerContract ToContract(this ManufacturerReadModel readModel)
        {
            return new ManufacturerContract(readModel.Id.Value, readModel.Name, readModel.IsDeleted);
        }
    }
}