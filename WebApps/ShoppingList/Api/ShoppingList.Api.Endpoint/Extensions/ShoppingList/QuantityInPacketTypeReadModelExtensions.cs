using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityInPacketTypes;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.ShoppingList
{
    public static class QuantityInPacketTypeReadModelExtensions
    {
        public static QuantityInPacketTypeContract ToContract(this QuantityInPacketTypeReadModel readModel)
        {
            return new QuantityInPacketTypeContract(readModel.Id, readModel.Name, readModel.QuantityLabel);
        }
    }
}