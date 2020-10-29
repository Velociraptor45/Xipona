using ShoppingList.Api.Contracts.Queries.AllQuantityTypes;
using ShoppingList.Api.Domain.Queries.AllQuantityInPacketTypes;

namespace ShoppingList.Api.Endpoint.Converters.ShoppingList
{
    public static class QuantityTypeInPacketContractConverter
    {
        public static QuantityInPacketTypeContract ToContract(this QuantityInPacketTypeReadModel readModel)
        {
            return new QuantityInPacketTypeContract(readModel.Id, readModel.Name);
        }
    }
}