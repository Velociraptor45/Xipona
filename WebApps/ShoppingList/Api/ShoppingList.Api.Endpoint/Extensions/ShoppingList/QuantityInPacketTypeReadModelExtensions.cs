using ShoppingList.Api.Contracts.Queries.AllQuantityTypes;
using ShoppingList.Api.Domain.Queries.AllQuantityInPacketTypes;

namespace ShoppingList.Api.Endpoint.Extensions.ShoppingList
{
    public static class QuantityInPacketTypeReadModelExtensions
    {
        public static QuantityInPacketTypeContract ToContract(this QuantityInPacketTypeReadModel readModel)
        {
            return new QuantityInPacketTypeContract(readModel.Id, readModel.Name, readModel.PriceLabel);
        }
    }
}