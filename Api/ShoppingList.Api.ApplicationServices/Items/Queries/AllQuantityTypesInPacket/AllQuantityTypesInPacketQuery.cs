using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries.Quantities;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Queries.AllQuantityTypesInPacket;

public class AllQuantityTypesInPacketQuery : IQuery<IEnumerable<QuantityTypeInPacketReadModel>>
{
}