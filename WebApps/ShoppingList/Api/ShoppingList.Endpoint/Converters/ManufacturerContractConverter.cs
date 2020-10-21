using ShoppingList.Contracts.SharedContracts;
using ShoppingList.Domain.Queries.SharedModels;

namespace ShoppingList.Endpoint.Converters
{
    public static class ManufacturerContractConverter
    {
        public static ManufacturerContract ToContract(this ManufacturerReadModel readModel)
        {
            return new ManufacturerContract(readModel.Id.Value, readModel.Name, readModel.IsDeleted);
        }
    }
}