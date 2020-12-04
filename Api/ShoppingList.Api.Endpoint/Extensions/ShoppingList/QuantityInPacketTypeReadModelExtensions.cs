using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypesInPacket;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.ShoppingList
{
    public static class QuantityTypeInPacketReadModelExtensions
    {
        public static QuantityTypeInPacketContract ToContract(this QuantityTypeInPacketReadModel readModel)
        {
            return new QuantityTypeInPacketContract(readModel.Id, readModel.Name, readModel.QuantityLabel);
        }
    }
}