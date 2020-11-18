using ShoppingList.Api.Contracts.Queries.AllQuantityTypes;
using ShoppingList.Api.Domain.Queries.AllQuantityTypes;

namespace ShoppingList.Api.Endpoint.Extensions.ShoppingList
{
    public static class QuantityTypeReadModelExtensions
    {
        public static QuantityTypeContract ToContract(this QuantityTypeReadModel readModel)
        {
            return new QuantityTypeContract(readModel.Id, readModel.Name, readModel.DefaultQuantity, readModel.Label);
        }
    }
}