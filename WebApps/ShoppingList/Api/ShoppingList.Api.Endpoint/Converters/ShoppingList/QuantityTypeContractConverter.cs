using ShoppingList.Api.Contracts.Queries.AllQuantityTypes;
using ShoppingList.Api.Domain.Queries.AllQuantityTypes;

namespace ShoppingList.Api.Endpoint.Converters.ShoppingList
{
    public static class QuantityTypeContractConverter
    {
        public static QuantityTypesContract ToContract(this QuantityTypeReadModel readModel)
        {
            return new QuantityTypesContract(readModel.Id, readModel.Name);
        }
    }
}