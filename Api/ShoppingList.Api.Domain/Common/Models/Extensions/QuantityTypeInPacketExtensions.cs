using ProjectHermes.ShoppingList.Api.Core.Attributes;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypesInPacket;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;

public static class QuantityTypeInPacketExtensions
{
    public static QuantityTypeInPacketReadModel ToReadModel(this QuantityTypeInPacket quantityTypeInPacket)
    {
        return new QuantityTypeInPacketReadModel(
            (int)quantityTypeInPacket,
            quantityTypeInPacket.ToString(),
            quantityTypeInPacket.GetAttribute<QuantityLabelAttribute>().QuantityLabel);
    }
}