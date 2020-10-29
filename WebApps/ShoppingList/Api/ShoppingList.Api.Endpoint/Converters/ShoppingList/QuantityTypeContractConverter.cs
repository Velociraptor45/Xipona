using ShoppingList.Api.Contracts.Queries.AllQuantityTypes;
using ShoppingList.Api.Domain.Queries.AllQuantityTypes;

namespace ShoppingList.Api.Endpoint.Converters.ShoppingList
{
    public static class QuantityTypeContractConverter
    {
        public static QuantityTypeContract ToContract(this QuantityTypeReadModel readModel)
        {
            return new QuantityTypeContract(readModel.Id, readModel.Name);
        }
    }
}