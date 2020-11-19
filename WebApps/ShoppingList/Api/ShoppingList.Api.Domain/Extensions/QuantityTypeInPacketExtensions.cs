using ShoppingList.Api.Core.Attributes;
using ShoppingList.Api.Core.Extensions;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.AllQuantityInPacketTypes;

namespace ShoppingList.Api.Domain.Extensions
{
    public static class QuantityTypeInPacketExtensions
    {
        public static QuantityInPacketTypeReadModel ToReadModel(this QuantityTypeInPacket quantityTypeInPacket)
        {
            return new QuantityInPacketTypeReadModel(
                (int)quantityTypeInPacket,
                quantityTypeInPacket.ToString(),
                quantityTypeInPacket.GetAttribute<QuantityLabelAttribute>().QuantityLabel);
        }
    }
}