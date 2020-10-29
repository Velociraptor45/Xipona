using ShoppingList.Api.Contracts.Queries.AllQuantityTypes;
using ShoppingList.Api.Domain.Queries.AllQuantityTypes;

namespace ShoppingList.Api.Endpoint.Converters.ShoppingList
{
    public static class QuantityTypesConverter
    {
        public static QuantityTypesContract ToContract(this QuantityTypesReadModel readModel)
        {
            return new QuantityTypesContract(readModel.QuantityTypes);
        }
    }
}