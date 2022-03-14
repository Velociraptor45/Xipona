using ProjectHermes.ShoppingList.Api.Core.Attributes;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries.Quantities;

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